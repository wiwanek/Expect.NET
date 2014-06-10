using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using System.Threading;

namespace ExpectNet.Test
{
    [TestClass]
    public class SessionTest
    {
        private async Task<string> ReturnStringAfterDelayAsync(string s, int delayInMs)
        {
            await Task.Delay(delayInMs);
            return s;
        }

        private string ReturnStringAfterDelay(string s, int delayInMs)
        {
            Thread.Sleep(delayInMs);
            return s;
        }

        [TestMethod]
        public void SendTest()
        {
            var spawnable = new Mock<ISpawnable>();
            Session session = new Session(spawnable.Object);
            string command = "test command";

            session.Send(command);

            spawnable.Verify(p => p.Write(command));
        }

        [TestMethod]
        public void BasicExpectTest()
        {
            var spawnable = new Mock<ISpawnable>();
            spawnable.Setup(p => p.Read()).Callback(() => Thread.Sleep(1000)).Returns("test expected string test");
            Session session = new Session(spawnable.Object);
            bool funcCalled = false;

            session.Expect("expected string", () => funcCalled = true);

            Assert.IsTrue(funcCalled);
        }

        [TestMethod]
        public void BasicExpectWithOutputTest()
        {
            var spawnable = new Mock<ISpawnable>();
            spawnable.Setup(p => p.Read()).Returns(ReturnStringAfterDelay("test expected string test", 10));
            Session session = new Session(spawnable.Object);
            bool funcCalled = false;

            string output = "";
            session.Expect("expected string", (s) => { funcCalled = true; output = s; });

            Assert.IsTrue(funcCalled);
            Assert.AreEqual("test expected string test", output);
        }

        [TestMethod]
        public void SplitResultExpectTest()
        {
            var spawnable = new Mock<ISpawnable>();
            int i = 0;
            string[] strings = {ReturnStringAfterDelay("test expected ", 100), 
                                     ReturnStringAfterDelay("string test", 150)};
            spawnable.Setup(p => p.Read()).Returns(() => strings[i]).Callback(() => i++);
            Session session = new Session(spawnable.Object);
            bool funcCalled = false;

            session.Expect("expected string", () => funcCalled = true);

            Assert.IsTrue(funcCalled);
            Assert.AreEqual(2, i);
        }

        [TestMethod]
        public void SplitResultExpectWitOutputTest()
        {
            var spawnable = new Mock<ISpawnable>();
            int i = 0;
            string[] strings = {ReturnStringAfterDelay("test expected ", 100), 
                                     ReturnStringAfterDelay("string test", 150)};
            spawnable.Setup(p => p.Read()).Returns(() => strings[i]).Callback(() => i++);
            Session session = new Session(spawnable.Object);
            bool funcCalled = false;
            string output = "";

            session.Expect("expected string", (s) => { funcCalled = true; output = s; });

            Assert.IsTrue(funcCalled);
            Assert.AreEqual(2, i);
            Assert.AreEqual("test expected string test", output);
        }

        [TestMethod]
        public void SendResetOutputTest()
        {
            var spawnable = new Mock<ISpawnable>();
            int i = 0;
            string[] strings = {ReturnStringAfterDelay("test expected ", 100), 
                                     ReturnStringAfterDelay("string test", 150),
                                   ReturnStringAfterDelay("next expected string", 100)};
            spawnable.Setup(p => p.Read()).Returns(() => strings[i]).Callback(() => i++);
            Session session = new Session(spawnable.Object);
            string output = "";

            session.Expect("expected string", (s) => { session.Send("test"); });
            session.Expect("next expected", (s) => { output = s; });
            Assert.AreEqual("next expected string", output);
        }

        [TestMethod]
        public void TimeoutThrownExpectTest()
        {
            var spawnable = new Mock<ISpawnable>();
            spawnable.Setup(p => p.Read()).Returns(() => ReturnStringAfterDelay("test expected string test", 1000));
            Session session = new Session(spawnable.Object);
            session.Timeout = 500;
            Exception exc = null;
            bool funcCalled = false;

            try
            {
                session.Expect("expected string", () => funcCalled = true);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNotNull(exc);
            Assert.IsInstanceOfType(exc, typeof(TimeoutException));
            Assert.IsFalse(funcCalled);
        }

        [TestMethod]
        public void TimeoutNotThrownExpectTest()
        {
            var spawnable = new Mock<ISpawnable>();
            spawnable.Setup(p => p.Read()).Returns(ReturnStringAfterDelay("test expected string test", 1200));
            Session session = new Session(spawnable.Object);
            session.Timeout = 2400;
            Exception exc = null;
            bool funcCalled = false;

            try
            {
                session.Expect("expected string", () => funcCalled = true);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNull(exc);
            Assert.IsTrue(funcCalled);
        }

        [TestMethod]
        public async Task TimeoutThrownExpectAsyncTest()
        {
            var spawnable = new Mock<ISpawnable>();
            spawnable.Setup(p => p.ReadAsync()).Returns(ReturnStringAfterDelayAsync("test expected string test", 1200));
            Session session = new Session(spawnable.Object);
            session.Timeout = 500;
            Exception exc = null;
            bool funcCalled = false;

            try
            {
                await session.ExpectAsync("expected string", () => funcCalled = true);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNotNull(exc);
            Assert.IsInstanceOfType(exc, typeof(TimeoutException));
            Assert.IsFalse(funcCalled);
        }

        [TestMethod]
        public async Task TimeoutNotThrownExpectAsyncTest()
        {
            var spawnable = new Mock<ISpawnable>();
            spawnable.Setup(p => p.ReadAsync()).Returns(ReturnStringAfterDelayAsync("test expected string test", 1200));
            Session session = new Session(spawnable.Object);
            session.Timeout = 2400;
            Exception exc = null;
            bool funcCalled = false;

            try
            {
                await session.ExpectAsync("expected string", () => funcCalled = true);
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNull(exc);
            Assert.IsTrue(funcCalled);
        }

        [TestMethod]
        public void SetGetTimeout2400Test()
        {
            var spawnable = new Mock<ISpawnable>();
            Session session = new Session(spawnable.Object);
            session.Timeout = 2400;
            Assert.AreEqual(2400, session.Timeout);
        }

        [TestMethod]
        public void SetGetTimeout200Test()
        {
            var spawnable = new Mock<ISpawnable>();
            Session session = new Session(spawnable.Object);
            session.Timeout  = 200;
            Assert.AreEqual(200, session.Timeout);
        }

        [TestMethod]
        public void SetGetTimeoutIncorrectValueTest()
        {
            var spawnable = new Mock<ISpawnable>();
            Session session = new Session(spawnable.Object);
            Exception exc = null;
            ArgumentException aoorexc = null;
            try
            {
                session.Timeout = -1;
            }
            catch (ArgumentException aoore)
            {
                aoorexc = aoore;
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNull(exc);
            Assert.IsNotNull(aoorexc);
        }

        [TestMethod]
        public void BasicAsyncExpectTest()
        {
            var spawnable = new Mock<ISpawnable>();
            spawnable.Setup(p => p.ReadAsync()).Returns(ReturnStringAfterDelayAsync("test expected string test", 10));
            Session session = new Session(spawnable.Object);
            bool funcCalled = false;

            Task task = session.ExpectAsync("expected string", () => funcCalled = true);
            task.Wait();

            Assert.IsTrue(funcCalled);
        }

        [TestMethod]
        public void BasicExpectAsyncWithOutputTest()
        {
            var spawnable = new Mock<ISpawnable>();
            spawnable.Setup(p => p.ReadAsync()).Returns(ReturnStringAfterDelayAsync("test expected string test", 10));
            Session session = new Session(spawnable.Object);
            bool funcCalled = false;

            string output = "";
            session.ExpectAsync("expected string", (s) => { funcCalled = true; output = s; }).Wait();

            Assert.IsTrue(funcCalled);
            Assert.AreEqual("test expected string test", output);
        }

        [TestMethod]
        public void SplitResultExpectAsyncTest()
        {
            var spawnable = new Mock<ISpawnable>();
            int i = 0;
            Task<string>[] tasks = {ReturnStringAfterDelayAsync("test expected ", 100), 
                                     ReturnStringAfterDelayAsync("string test", 150)};
            spawnable.Setup(p => p.ReadAsync()).Returns(() => tasks[i]).Callback(() => i++);
            Session session = new Session(spawnable.Object);
            bool funcCalled = false;

            session.ExpectAsync("expected string", () => funcCalled = true).Wait();

            Assert.IsTrue(funcCalled);
            Assert.AreEqual(2, i);
        }

        [TestMethod]
        public void SplitResultExpectAsyncWitOutputTest()
        {
            var spawnable = new Mock<ISpawnable>();
            int i = 0;
            Task<string>[] tasks = {ReturnStringAfterDelayAsync("test expected ", 100), 
                                     ReturnStringAfterDelayAsync("string test", 150)};
            spawnable.Setup(p => p.ReadAsync()).Returns(() => tasks[i]).Callback(() => i++);
            Session session = new Session(spawnable.Object);
            bool funcCalled = false;
            string output = "";

            session.ExpectAsync("expected string", (s) => { funcCalled = true; output = s; }).Wait();

            Assert.IsTrue(funcCalled);
            Assert.AreEqual(2, i);
            Assert.AreEqual("test expected string test", output);
        }

        [TestMethod]
        public void SendResetOutputAsyncTest()
        {
            var spawnable = new Mock<ISpawnable>();
            int i = 0;
            Task<string>[] tasks = {ReturnStringAfterDelayAsync("test expected ", 100), 
                                     ReturnStringAfterDelayAsync("string test", 150),
                                   ReturnStringAfterDelayAsync("next expected string", 100)};
            spawnable.Setup(p => p.ReadAsync()).Returns(() => tasks[i]).Callback(() => i++);
            Session session = new Session(spawnable.Object);
            string output = "";

            session.ExpectAsync("expected string", (s) => { session.Send("test"); }).Wait();
            session.ExpectAsync("next expected", (s) => { output = s; }).Wait();
            Assert.AreEqual("next expected string", output);
        }
    }
    
}
