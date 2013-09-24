using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Expect.Test
{
    [TestClass]
    public class LocalAppProcessHandlerTest
    {
        [TestMethod]
        public void CtorTest()
        {
            LocalAppProcessHandler t = new LocalAppProcessHandler("test");
        }
    }
}
