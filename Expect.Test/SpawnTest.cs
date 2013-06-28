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
        public void BasicExpectTest()
        {
            var proc = new Mock<Process>();
            Task<string> task = new Task<string>(() => "test expected string test");
            task.Start();
            proc.Setup(p => p.readAsync()).Returns(task);
            Spawn spawn = new Spawn(proc.Object);
            bool funcCalled = false;

            spawn.expect("expected string", () => funcCalled = true);

            Assert.IsTrue(funcCalled);
        }

        [TestMethod]
        public void SplitResultExpectTest()
        {
            var proc = new Mock<Process>();
            int i = 0;
            Task<string>[] tasks = {new Task<string>(() => "test expected "), 
                                     new Task<string>(() => "string test")};
            tasks[0].Start();
            tasks[1].Start();

            proc.Setup(p => p.readAsync()).Callback(() => Thread.Sleep(100)).Returns(() => tasks[i]).Callback(() => i++);
            Spawn spawn = new Spawn(proc.Object);
            bool funcCalled = false;

            spawn.expect("expected string", () => funcCalled = true);

            Assert.IsTrue(funcCalled);
            Assert.AreEqual(2, i);
        }

        [TestMethod]
        public void TimeoutThrownExpectTest()
        {
            var proc = new Mock<Process>();
            Task<string> task = new Task<string>(() => "test expected string test");
            task.Start();
            proc.Setup(p => p.readAsync()).Callback(() => Thread.Sleep(1200)).Returns(task);
            Spawn spawn = new Spawn(proc.Object);
            Exception exc = null;
            bool funcCalled = false;
            
            try
            {
                spawn.expect("expected string", () => funcCalled = true);
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
