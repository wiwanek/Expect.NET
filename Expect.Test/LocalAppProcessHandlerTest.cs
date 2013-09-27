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
            var pw = new Mock<ProcessWrapper>();
            Process p = new Process();
            p.StartInfo.FileName = "";
            pw.Setup(proc => proc.Process).Returns(p);
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
            var pw = new Mock<ProcessWrapper>();
            Process p = new Process();
            p.StartInfo.FileName = "";
            pw.Setup(proc => proc.Process).Returns(p);

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
            var pw = new Mock<ProcessWrapper>();
            Process p = new Process();
            p.StartInfo.FileName = "test";
            pw.Setup(proc => proc.Process).Returns(p);

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);

            Assert.IsNotNull(t);
            Assert.AreSame(p, t.ProcessWrapper.Process);
        }

        [TestMethod]
        public void CtorFileNameNotChangedTest()
        {
            var pw = new Mock<ProcessWrapper>();
            Process p = new Process();
            p.StartInfo.FileName = "test";
            pw.Setup(proc => proc.Process).Returns(p);

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);
            Assert.IsNotNull(t);
            Assert.AreEqual("test", t.ProcessWrapper.Process.StartInfo.FileName);
        }

        [TestMethod]
        public void CtorArgumentsNotChangedTest()
        {
            var pw = new Mock<ProcessWrapper>();
            Process p = new Process();
            p.StartInfo.FileName = "test";
            p.StartInfo.Arguments = "argtest";
            pw.Setup(proc => proc.Process).Returns(p);

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);

            Assert.IsNotNull(t);
            Assert.AreEqual("argtest", t.ProcessWrapper.Process.StartInfo.Arguments);
        }

        [TestMethod]
        public void CtorSetRedirectStdErrTest()
        {
            var pw = new Mock<ProcessWrapper>();
            Process p = new Process();
            p.StartInfo.FileName = "test";
            p.StartInfo.RedirectStandardError = false;
            pw.Setup(proc => proc.Process).Returns(p);

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);
            Assert.IsNotNull(t);
            Assert.IsTrue(t.ProcessWrapper.Process.StartInfo.RedirectStandardError);
        }

        [TestMethod]
        public void CtorSetRedirectStdOutTest()
        {
            var pw = new Mock<ProcessWrapper>();
            Process p = new Process();
            p.StartInfo.FileName = "test";
            p.StartInfo.RedirectStandardOutput = false;
            pw.Setup(proc => proc.Process).Returns(p);

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);
            Assert.IsNotNull(t);
            Assert.IsTrue(t.ProcessWrapper.Process.StartInfo.RedirectStandardOutput);
        }

        [TestMethod]
        public void CtorSetRedirectStdInTest()
        {
            var pw = new Mock<ProcessWrapper>();
            Process p = new Process();
            p.StartInfo.FileName = "test";
            p.StartInfo.RedirectStandardInput = false;
            pw.Setup(proc => proc.Process).Returns(p);

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);
            Assert.IsNotNull(t);
            Assert.IsTrue(t.ProcessWrapper.Process.StartInfo.RedirectStandardInput);
        }

        [TestMethod]
        public void CtorSetUseShellExecuteTest()
        {
            var pw = new Mock<ProcessWrapper>();
            Process p = new Process();
            p.StartInfo.FileName = "test";
            p.StartInfo.UseShellExecute = true;
            pw.Setup(proc => proc.Process).Returns(p);

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);
            Assert.IsNotNull(t);
            Assert.IsFalse(t.ProcessWrapper.Process.StartInfo.UseShellExecute);
        }

        [TestMethod]
        public void CtorStartCalledTest()
        {
            var pw = new Mock<ProcessWrapper>();
            Process p = new Process();
            p.StartInfo.FileName = "test";
            pw.Setup(proc => proc.Process).Returns(p);

            LocalAppProcessHandler t = null;
            t = new LocalAppProcessHandler(pw.Object);
            Assert.IsNotNull(t);
            pw.Verify(proc => proc.Start(), Times.Once());
        }

    }

}


