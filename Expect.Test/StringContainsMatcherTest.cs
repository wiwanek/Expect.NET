using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpectNet.NET.Test
{
    [TestClass]
    public class StringContainsMatcherTest
    {
        [TestMethod]
        public void CtorOk()
        {
            Exception exception = null;
            IMatcher matcher = null;
            try
            {
                matcher = new StringContainsMatcher("string");
            }
            catch (Exception e)
            {
                exception = e;
            }
            Assert.IsNull(exception);
            Assert.IsNotNull(matcher);
        }

        [TestMethod]
        public void CtorNullArg()
        {
            Exception exception = null;
            IMatcher matcher = null;
            try
            {
                matcher = new StringContainsMatcher(null);
            }
            catch (Exception e)
            {
                exception = e;
            }
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
            Assert.AreEqual("query", (exception as ArgumentNullException).ParamName);
            Assert.IsNull(matcher);
        }

        [TestMethod]
        public void IsMatchTrue1()
        {
            IMatcher matcher = new StringContainsMatcher("test");
        
            bool result = matcher.IsMatch("testing if string matches");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchTrue2()
        {
            IMatcher matcher = new StringContainsMatcher("t of s");

            bool result = matcher.IsMatch("It's a Test of string");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatchFalse1()
        {
            IMatcher matcher = new StringContainsMatcher("[Tt]est.*string");

            bool result = matcher.IsMatch("string testing in progress");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsMatchFalse2()
        {
            IMatcher matcher = new StringContainsMatcher("string");

            bool result = matcher.IsMatch("It's a Test of String");

            Assert.IsFalse(result);
        }

    }
}
