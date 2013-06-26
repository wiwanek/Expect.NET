using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Expect;
using System.Threading.Tasks;

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
    }
}
