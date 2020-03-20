using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExpectNet;
using Moq;

namespace ExpectNet.Test
{
    [TestClass]
    public class ExpectTest
    {
        [TestMethod]
        public void SpawnInitSpawnableTest()
        {
            var spawnable = new Mock<ISpawnable>();

            Expect.Spawn(spawnable.Object);

            spawnable.Verify(s => s.Init());
        }

        [TestMethod]
        public void SpawnCreateSessionTest()
        {
            var spawnable = new Mock<ISpawnable>();

            var session = Expect.Spawn(spawnable.Object);

            Assert.IsInstanceOfType(session, typeof(Session));
            Assert.IsNotNull(session);
        }
    }
}
