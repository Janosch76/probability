namespace Probability.Tests.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
 
    /// <summary>
    /// A collection of unit tests on probabilities.
    /// </summary>
    [TestClass]
    public class ProbabilityTests : TestBase
    {
        [UnitTest]
        [TestMethod]
        public void ProbabilityInequality()
        {
            var p1 = new Probability(.5M);
            var p2 = new Probability(.75M);

            Assert.AreEqual(true, p1 < p2);
            Assert.AreEqual(true, p1 <= p2);
            Assert.AreEqual(false, p2 < p1);
            Assert.AreEqual(false, p2 <= p1);
        }

        [UnitTest]
        [TestMethod]
        public void ProbabilityEquality()
        {
            var p1 = new Probability(.5M);
            var p2 = new Probability(.5M);

            Assert.AreEqual(true, p1 == p2);
            Assert.AreEqual(false, p1 != p2);
        }
    }
}
