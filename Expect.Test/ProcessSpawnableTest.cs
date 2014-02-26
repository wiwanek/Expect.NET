using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace Expect.Test
{
    [TestClass]
    public class ProcessSpawnableTest
    {
        [TestMethod]
        public void CommandBackendNullTest()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = null;
            Exception caughtException = null;

            try
            {
                CommandSpawnable backend = new CommandSpawnable(proc);
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
        public void CommandBackendEmptyTest()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "";
            Exception caughtException = null;

            try
            {
                CommandSpawnable backend = new CommandSpawnable(proc);
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
        public void CommandBackendOKTest()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "filename";
            Exception caughtException = null;
            try
            {
                CommandSpawnable backend = new CommandSpawnable(proc);
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            Assert.IsNull(caughtException);
            
        }

    }
}
