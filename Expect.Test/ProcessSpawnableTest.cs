using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using Moq;

namespace ExpectNet.Test
{
    [TestClass]
    public class ProcessSpawnableTest
    {
        [TestMethod]
        public void CtorFilenameTest()
        {
            string filename = "testFileName";
            ProcessSpawnable proc = new ProcessSpawnable(filename);

            Assert.IsNotNull(proc.Process);
            Assert.AreEqual(filename, proc.Process.StartInfo.FileName);
            Assert.AreEqual("", proc.Process.StartInfo.Arguments);
        }

        [TestMethod]
        public void CtorFilenameArgumentsTest()
        {
            string filename = "testFileName";
            string args = "arg1 arg2";
            ProcessSpawnable proc = new ProcessSpawnable(filename, args);

            Assert.IsNotNull(proc.Process);
            Assert.AreEqual(filename, proc.Process.StartInfo.FileName);
            Assert.AreEqual(args, proc.Process.StartInfo.Arguments);
        }

        [TestMethod]
        public void CtorProcessTest()
        {
            string filename = "testFileName";
            string args = "arg1 arg2";
            Process p = new Process();
            p.StartInfo.FileName = filename;
            p.StartInfo.Arguments = args;
            ProcessSpawnable proc = new ProcessSpawnable(p);

            Assert.IsNotNull(proc.Process);
            Assert.AreSame(p.StartInfo, proc.Process.StartInfo);
        }

        [TestMethod]
        public void NullFilenameTest()
        {
            var proc = new Mock<IProcess>();
            ProcessStartInfo psi = new ProcessStartInfo(null);
            proc.Setup(p => p.StartInfo).Returns(psi);
            Exception caughtException = null;

            try
            {
                ProcessSpawnable process = new ProcessSpawnable(proc.Object);
                process.Init();
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(ArgumentException));
            Assert.AreEqual("_process.StartInfo.FileName", (caughtException as ArgumentException).ParamName);

        }

        [TestMethod]
        public void EmptyFilenameTest()
        {
            var proc = new Mock<IProcess>();
            ProcessStartInfo psi = new ProcessStartInfo(null);
            proc.Setup(p => p.StartInfo).Returns(psi);
            Exception caughtException = null;

            try
            {
                ProcessSpawnable process = new ProcessSpawnable(proc.Object);
                process.Init();
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(ArgumentException));
            Assert.AreEqual("_process.StartInfo.FileName", (caughtException as ArgumentException).ParamName);

        }

        [TestMethod]
        public void InitProcessTest()
        {
            var proc = new Mock<IProcess>();
            ProcessStartInfo psi = new ProcessStartInfo(null);
            psi.FileName = "filename";
            psi.RedirectStandardError = false;
            psi.RedirectStandardInput = false;
            psi.RedirectStandardOutput = false;
            psi.UseShellExecute = true;
            proc.Setup(p => p.StartInfo).Returns(psi);

            ProcessSpawnable ps = null;
            Exception caughtException = null;
            try
            {
                ps = new ProcessSpawnable(proc.Object);
                ps.Init();
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            Assert.IsNull(caughtException);
            Assert.IsTrue(ps.Process.StartInfo.RedirectStandardError);
            Assert.IsTrue(ps.Process.StartInfo.RedirectStandardInput);
            Assert.IsTrue(ps.Process.StartInfo.RedirectStandardOutput);
            Assert.IsFalse(ps.Process.StartInfo.UseShellExecute);
            proc.Verify(p => p.Start());
        }

        [TestMethod]
        public void WriteTest()
        {
            //Arrange
            string testText = "This is text to write";
            Stream so = new MemoryStream();
            so.WriteByte(1);
            so.Seek(0, SeekOrigin.Begin);
            StreamReader output = new StreamReader(so);
            Assert.IsFalse(output.EndOfStream);

            Stream se = new MemoryStream();
            se.WriteByte(2);
            se.Seek(0, SeekOrigin.Begin);
            StreamReader error = new StreamReader(se);
            Assert.IsFalse(error.EndOfStream);

            Stream si = new MemoryStream();
            StreamWriter input = new StreamWriter(si);

            var proc = new Mock<IProcess>();
            ProcessStartInfo psi = new ProcessStartInfo("filename");
            proc.SetupGet<ProcessStartInfo>(p => p.StartInfo).Returns(psi);
            proc.SetupGet<StreamReader>(p => p.StandardError).Returns(error);
            proc.SetupGet<StreamReader>(p => p.StandardOutput).Returns(output);
            proc.SetupGet<StreamWriter>(p => p.StandardInput).Returns(input);
            ProcessSpawnable ps = new ProcessSpawnable(proc.Object);
            ps.Init();

            //Act
            ps.Write(testText);

            //Assert
            ps.Process.StandardInput.Flush();
            int maxSize = 4096;
            byte[] tmp = new byte[maxSize];
            si.Seek(0, SeekOrigin.Begin);
            int n = si.Read(tmp, 0, maxSize);
            string writtenText = System.Text.Encoding.Default.GetString(tmp, 0, n);

            Assert.IsTrue(output.EndOfStream);
            Assert.IsTrue(error.EndOfStream);
            Assert.AreEqual(testText, writtenText);
        }
    }
}
