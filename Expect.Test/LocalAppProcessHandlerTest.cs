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
            Process p = new Process();
            p.StartInfo.FileName = "";

            Exception exc = null;

            try
            {
                LocalAppProcessHandler t = new LocalAppProcessHandler(p);
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
            Process p = new Process();
            p.StartInfo.FileName = null;

            Exception exc = null;

            try
            {
                LocalAppProcessHandler t = new LocalAppProcessHandler(p);
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
            Process p = new Process();
            p.StartInfo.FileName = "test";

            Exception exc = null;

            LocalAppProcessHandler t = null;
            try
            {
                t = new LocalAppProcessHandler(p);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNull(exc);
            Assert.IsNotNull(t);
            Assert.AreSame(p, t.Process);
        }

        [TestMethod]
        public void CtorFileNameNotChangedTest()
        {
            Process p = new Process();
            p.StartInfo.FileName = "test";

            Exception exc = null;

            LocalAppProcessHandler t = null;
            try
            {
                t = new LocalAppProcessHandler(p);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNull(exc);
            Assert.IsNotNull(t);
            Assert.AreEqual("test", t.Process.StartInfo.FileName);
        }

        [TestMethod]
        public void CtorArgumentsNotChangedTest()
        {
            Process p = new Process();
            p.StartInfo.FileName = "test";
            p.StartInfo.Arguments = "argtest";

            Exception exc = null;

            LocalAppProcessHandler t = null;
            try
            {
                t = new LocalAppProcessHandler(p);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNull(exc);
            Assert.IsNotNull(t);
            Assert.AreEqual("argtest", t.Process.StartInfo.Arguments);
        }

        [TestMethod]
        public void CtorSetRedirectStdErrTest()
        {
            Process p = new Process();
            p.StartInfo.FileName = "test";
            p.StartInfo.RedirectStandardError = false;

            Exception exc = null;

            LocalAppProcessHandler t = null;
            try
            {
                t = new LocalAppProcessHandler(p);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNull(exc);
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Process.StartInfo.RedirectStandardError);
        }

        [TestMethod]
        public void CtorSetRedirectStdOutTest()
        {
            Process p = new Process();
            p.StartInfo.FileName = "test";
            p.StartInfo.RedirectStandardOutput = false;

            Exception exc = null;

            LocalAppProcessHandler t = null;
            try
            {
                t = new LocalAppProcessHandler(p);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNull(exc);
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Process.StartInfo.RedirectStandardOutput);
        }

        [TestMethod]
        public void CtorSetRedirectStdInTest()
        {
            Process p = new Process();
            p.StartInfo.FileName = "test";
            p.StartInfo.RedirectStandardInput = false;

            Exception exc = null;

            LocalAppProcessHandler t = null;
            try
            {
                t = new LocalAppProcessHandler(p);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNull(exc);
            Assert.IsNotNull(t);
            Assert.IsTrue(t.Process.StartInfo.RedirectStandardInput);
        }

        [TestMethod]
        public void CtorSetUseShellExecuteTest()
        {
            Process p = new Process();
            p.StartInfo.FileName = "test";
            p.StartInfo.UseShellExecute = true;

            Exception exc = null;

            LocalAppProcessHandler t = null;
            try
            {
                t = new LocalAppProcessHandler(p);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNull(exc);
            Assert.IsNotNull(t);
            Assert.IsFalse(t.Process.StartInfo.UseShellExecute);
        }

        [TestMethod]
        public void CtorStartCalledTest()
        {
            var proc = new Mock<Process>();
            ProcessStartInfo si = new ProcessStartInfo("test");
            proc.Setup(p => p.StartInfo).Returns(si);
            Exception exc = null;

            LocalAppProcessHandler t = null;
            try
            {
                t = new LocalAppProcessHandler(proc.Object);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNull(exc);
            Assert.IsNotNull(t);
            proc.Verify(p => p.Start(), Times.Once());
        }
    }
}
