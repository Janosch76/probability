namespace Probability.Tests.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// A collection of sample unit tests.
    /// </summary>
    [TestClass]
    public class DistributionCombinatorsTests : TestBase
    {
        private enum Coin : int
        {
            Head,
            Tail
        }

        [UnitTest]
        [TestMethod]
        public void DistributionMap()
        {
            var d = Distribution.Uniform(0, 1, 2, 3)
                .Select(v => v > 0 ? Coin.Head : Coin.Tail);

            AssertEqualProbabilities(.75, d.ProbabilityOf(v => v == Coin.Head));
        }

        [UnitTest]
        [TestMethod]
        public void DistributionProduct()
        {
            var d = Distribution.Uniform(Coin.Head, Coin.Tail)
                .Prod(Distribution.Uniform(Coin.Head, Coin.Tail));

            AssertEqualProbabilities(.25, d.ProbabilityOf(v => v.Item1 == Coin.Head && v.Item2 == Coin.Head));
        }

        [UnitTest]
        [TestMethod]
        public void DistributionExists()
        {
            var d = Distribution.Uniform(1, 2, 3);

            Assert.AreEqual(true, d.Any());
            Assert.AreEqual(true, d.Any(v => v > 1));
            Assert.AreEqual(false, d.Any(v => v < 1));
        }

        [UnitTest]
        [TestMethod]
        public void DistributionFilteringRescales()
        {
            var d = Distribution.Uniform(0, 1)
                .Where(v => v > 0);

            AssertEqualProbabilities(1.0, d.ProbabilityOf(v => true));
        }

        [UnitTest]
        [TestMethod]
        public void DistributionFiltering()
        {
            var d = Distribution.Uniform(1, 2, 3, 4)
                .Where(v => v > 2);

            AssertEqualProbabilities(.5, d.ProbabilityOf(v => v == 4));
        }
    }
}
