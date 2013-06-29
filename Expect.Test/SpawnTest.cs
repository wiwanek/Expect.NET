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
            var proc = new Mock<Process>();
            Spawn spawn = new Spawn(proc.Object);
            string command = "test command";

            spawn.send(command);

            proc.Verify(p => p.write(command));
        }

        [TestMethod]
        public async Task BasicExpectTest()
        {
            var proc = new Mock<Process>();
            proc.Setup(p => p.readAsync()).Returns(ReturnStringAfterDelay("test expected string test", 10));
            Spawn spawn = new Spawn(proc.Object);
            bool funcCalled = false;

            await spawn.expect("expected string", () => funcCalled = true);

            Assert.IsTrue(funcCalled);
        }

        [TestMethod]
        public async Task SplitResultExpectTest()
        {
            var proc = new Mock<Process>();
            int i = 0;
            Task<string>[] tasks = {ReturnStringAfterDelay("test expected ", 100), 
                                     ReturnStringAfterDelay("string test", 150)};
            proc.Setup(p => p.readAsync()).Returns(() => tasks[i]).Callback(() => i++);
            Spawn spawn = new Spawn(proc.Object);
            bool funcCalled = false;

            await spawn.expect("expected string", () => funcCalled = true);

            Assert.IsTrue(funcCalled);
            Assert.AreEqual(2, i);
        }

        private async Task<string> ReturnStringAfterDelay(string s, int delayInMs)
        {
            await Task.Delay(delayInMs);
            return s;
        }

        [TestMethod]
        public async Task TimeoutThrownExpectTest()
        {
            var proc = new Mock<Process>();
            proc.Setup(p => p.readAsync()).Returns(ReturnStringAfterDelay("test expected string test", 1200));
            Spawn spawn = new Spawn(proc.Object);
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

    }
}
