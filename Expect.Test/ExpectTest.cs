using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expect;
using Moq;

namespace Expect.Test
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

            Assert.IsInstanceOfType(session, typeof(ISession));
            Assert.IsNotNull(session);
        }
    }
}
