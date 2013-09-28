using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Moq;

namespace Expect.Test
{
    [TestClass]
    public class LocalAppProcessHandlerTest
    {
        [TestMethod]
        public void CtorEmptyFileNameTest()
        {
            var pw = new Mock<ProcessAdapter>();
            
            ProcessStartInfo si = new ProcessStartInfo("");
            pw.Setup(proc => proc.StartInfo).Returns(si);
            Exception exc = null;

            try
            {
                LocalAppProcessHandler t = new LocalAppProcessHandler(pw.Object);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNotNull(exc);
            Assert.IsInstanceOfType(exc, typeof(ArgumentException));
            Assert.AreEqual("process.StartInfo.FileName", (exc as ArgumentException).ParamName);
        }

        [TestMethod]
        public void CtorNullFileNameTest()
        {
            var pw = new Mock<ProcessAdapter>();
            ProcessStartInfo si = new ProcessStartInfo(null);
            pw.Setup(proc => proc.StartInfo).Returns(si);

            Exception exc = null;

            try
            {
                LocalAppProcessHandler t = new LocalAppProcessHandler(pw.Object);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNotNull(exc);
            Assert.IsInstanceOfType(exc, typeof(ArgumentException));
            Assert.AreEqual("process.StartInfo.FileName", (exc as ArgumentException).ParamName);
        }

        [TestMethod]
        public void CtorArgSetTest()
        {
            var pw = new Mock<ProcessAdapter>();
            ProcessStartInfo si = new ProcessStartInfo("test");
            pw.Setup(proc => proc.StartInfo).Returns(si);

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);

            Assert.IsNotNull(t);
            Assert.AreSame(pw.Object, t.ProcessAdapter);
        }

        [TestMethod]
        public void CtorFileNameNotChangedTest()
        {
            var pw = new Mock<ProcessAdapter>();
            ProcessStartInfo si = new ProcessStartInfo("test");
            pw.Setup(proc => proc.StartInfo).Returns(si);

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);
            Assert.IsNotNull(t);
            Assert.AreEqual("test", t.ProcessAdapter.StartInfo.FileName);
        }

        [TestMethod]
        public void CtorArgumentsNotChangedTest()
        {
            var pw = new Mock<ProcessAdapter>();
            ProcessStartInfo si = new ProcessStartInfo("test", "argtest");
            pw.Setup(proc => proc.StartInfo).Returns(si);
            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);

            Assert.IsNotNull(t);
            Assert.AreEqual("argtest", t.ProcessAdapter.StartInfo.Arguments);
        }

        [TestMethod]
        public void CtorSetRedirectStdErrTest()
        {
            var pw = new Mock<ProcessAdapter>();
            ProcessStartInfo si = new ProcessStartInfo("test");
            pw.Setup(proc => proc.StartInfo).Returns(si);
            si.RedirectStandardError = false;

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);
            Assert.IsNotNull(t);
            Assert.IsTrue(t.ProcessAdapter.StartInfo.RedirectStandardError);
        }

        [TestMethod]
        public void CtorSetRedirectStdOutTest()
        {
            var pw = new Mock<ProcessAdapter>();
            ProcessStartInfo si = new ProcessStartInfo("test");
            si.RedirectStandardOutput = false;
            pw.Setup(proc => proc.StartInfo).Returns(si);

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);
            Assert.IsNotNull(t);
            Assert.IsTrue(t.ProcessAdapter.StartInfo.RedirectStandardOutput);
        }

        [TestMethod]
        public void CtorSetRedirectStdInTest()
        {
            var pw = new Mock<ProcessAdapter>();
            ProcessStartInfo si = new ProcessStartInfo("test");
            si.RedirectStandardInput = false;
            pw.Setup(proc => proc.StartInfo).Returns(si);
            
            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);
            Assert.IsNotNull(t);
            Assert.IsTrue(t.ProcessAdapter.StartInfo.RedirectStandardInput);
        }

        [TestMethod]
        public void CtorSetUseShellExecuteTest()
        {
            var pw = new Mock<ProcessAdapter>();
            ProcessStartInfo si = new ProcessStartInfo("test");
            si.UseShellExecute = true;
            pw.Setup(proc => proc.StartInfo).Returns(si);
            
            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);
            Assert.IsNotNull(t);
            Assert.IsFalse(t.ProcessAdapter.StartInfo.UseShellExecute);
        }

        [TestMethod]
        public void CtorStartCalledTest()
        {
            var pw = new Mock<ProcessAdapter>();
            ProcessStartInfo si = new ProcessStartInfo("test");
            pw.Setup(proc => proc.StartInfo).Returns(si);

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);
            Assert.IsNotNull(t);
            pw.Verify(proc => proc.Start(), Times.Once());
        }

    }

}


