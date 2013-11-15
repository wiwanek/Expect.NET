using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Expect;
using System.Threading.Tasks;
using System.Threading;

namespace Expect.Test
{
    [TestClass]
    public class SpawnTest
    {
        [TestMethod]
        public void SendTest()
        {
            var backend = new Mock<IBackend>();
            var bf = new Mock<IBackendFactory>();
            bf.Setup<IBackend>(foo => foo.createBackend()).Returns(backend.Object);
            Spawn spawn = new Spawn(bf.Object);
            string command = "test command";

            spawn.send(command);

            backend.Verify(p => p.write(command));
        }

        [TestMethod]
        public async Task BasicExpectTest()
        {
            var backend = new Mock<IBackend>();
            backend.Setup(p => p.readAsync()).Returns(ReturnStringAfterDelay("test expected string test", 10));
            var bf = new Mock<IBackendFactory>();
            bf.Setup<IBackend>(foo => foo.createBackend()).Returns(backend.Object);
            Spawn spawn = new Spawn(bf.Object);
            bool funcCalled = false;

            await spawn.expect("expected string", () => funcCalled = true);

            Assert.IsTrue(funcCalled);
        }

        [TestMethod]
        public async Task BasicExpectWithOutputTest()
        {
            var backend = new Mock<IBackend>();
            backend.Setup(p => p.readAsync()).Returns(ReturnStringAfterDelay("test expected string test", 10));
            var bf = new Mock<IBackendFactory>();
            bf.Setup<IBackend>(foo => foo.createBackend()).Returns(backend.Object);
            Spawn spawn = new Spawn(bf.Object);
            bool funcCalled = false;

            string output = "";
            await spawn.expect("expected string", (s) => { funcCalled = true; output = s; });

            Assert.IsTrue(funcCalled);
            Assert.AreEqual("test expected string test", output);
        }

        [TestMethod]
        public async Task SplitResultExpectTest()
        {
            var backend = new Mock<IBackend>();
            int i = 0;
            Task<string>[] tasks = {ReturnStringAfterDelay("test expected ", 100), 
                                     ReturnStringAfterDelay("string test", 150)};
            backend.Setup(p => p.readAsync()).Returns(() => tasks[i]).Callback(() => i++);
            var bf = new Mock<IBackendFactory>();
            bf.Setup<IBackend>(foo => foo.createBackend()).Returns(backend.Object);
            Spawn spawn = new Spawn(bf.Object);
            bool funcCalled = false;
            
            await spawn.expect("expected string", () => funcCalled = true);

            Assert.IsTrue(funcCalled);
            Assert.AreEqual(2, i);
        }

        [TestMethod]
        public async Task SplitResultExpectWitOutputTest()
        {
            var backend = new Mock<IBackend>();
            int i = 0;
            Task<string>[] tasks = {ReturnStringAfterDelay("test expected ", 100), 
                                     ReturnStringAfterDelay("string test", 150)};
            backend.Setup(p => p.readAsync()).Returns(() => tasks[i]).Callback(() => i++);
            var bf = new Mock<IBackendFactory>();
            bf.Setup<IBackend>(foo => foo.createBackend()).Returns(backend.Object);
            Spawn spawn = new Spawn(bf.Object);
            bool funcCalled = false;
            string output = "";

            await spawn.expect("expected string", (s) => { funcCalled = true; output = s; });

            Assert.IsTrue(funcCalled);
            Assert.AreEqual(2, i);
            Assert.AreEqual("test expected string test", output);
        }

        [TestMethod]
        public async Task SendResetOutputTest()
        {
            var backend = new Mock<IBackend>();
            int i = 0;
            Task<string>[] tasks = {ReturnStringAfterDelay("test expected ", 100), 
                                     ReturnStringAfterDelay("string test", 150),
                                   ReturnStringAfterDelay("next expected string", 100)};
            backend.Setup(p => p.readAsync()).Returns(() => tasks[i]).Callback(() => i++);
            var bf = new Mock<IBackendFactory>();
            bf.Setup<IBackend>(foo => foo.createBackend()).Returns(backend.Object);
            Spawn spawn = new Spawn(bf.Object);
            string output = "";

            await spawn.expect("expected string", (s) => { spawn.send("test"); });
            await spawn.expect("next expected", (s) => { output = s; });
            Assert.AreEqual("next expected string", output);
        }

        private async Task<string> ReturnStringAfterDelay(string s, int delayInMs)
        {
            await Task.Delay(delayInMs);
            return s;
        }

        [TestMethod]
        public async Task TimeoutThrownExpectTest()
        {
            var backend = new Mock<IBackend>();
            backend.Setup(p => p.readAsync()).Returns(ReturnStringAfterDelay("test expected string test", 1200));
            var bf = new Mock<IBackendFactory>();
            bf.Setup<IBackend>(foo => foo.createBackend()).Returns(backend.Object);
            Spawn spawn = new Spawn(bf.Object);
            spawn.setTimeout(500);
            Exception exc = null;
            bool funcCalled = false;

            try
            {
                await spawn.expect("expected string", () => funcCalled = true);
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
        public async Task TimeoutNotThrownExpectTest()
        {
            var backend = new Mock<IBackend>();
            backend.Setup(p => p.readAsync()).Returns(ReturnStringAfterDelay("test expected string test", 1200));
            var bf = new Mock<IBackendFactory>();
            bf.Setup<IBackend>(foo => foo.createBackend()).Returns(backend.Object);
            Spawn spawn = new Spawn(bf.Object);
            spawn.setTimeout(2400);
            Exception exc = null;
            bool funcCalled = false;

            try
            {
                await spawn.expect("expected string", () => funcCalled = true);
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
            var backend = new Mock<IBackend>();
            var bf = new Mock<IBackendFactory>();
            bf.Setup<IBackend>(foo => foo.createBackend()).Returns(backend.Object);
            Spawn spawn = new Spawn(bf.Object);
            spawn.setTimeout(2400);
            Assert.AreEqual(2400, spawn.getTimeout());
        }

        [TestMethod]
        public void SetGetTimeout200Test()
        {
            var backend = new Mock<IBackend>();
            var bf = new Mock<IBackendFactory>();
            bf.Setup<IBackend>(foo => foo.createBackend()).Returns(backend.Object);
            Spawn spawn = new Spawn(bf.Object);
            spawn.setTimeout(200);
            Assert.AreEqual(200, spawn.getTimeout());
        }

        [TestMethod]
        public void SetGetTimeoutIncorrectValueTest()
        {
            var backend = new Mock<IBackend>();
            var bf = new Mock<IBackendFactory>();
            bf.Setup<IBackend>(foo => foo.createBackend()).Returns(backend.Object);
            Spawn spawn = new Spawn(bf.Object);
            Exception exc = null;
            ArgumentOutOfRangeException aoorexc = null;
            try
            {
                spawn.setTimeout(-1);
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                aoorexc = aoore;
            }
            catch (Exception e)
            {
                exc = e;
            }

            Assert.IsNull(exc);
            Assert.IsNotNull(aoorexc);
            Assert.AreEqual("timeout", aoorexc.ParamName);
        }
    }
}
