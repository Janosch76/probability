namespace Probability.Tests.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the point distribution.
    /// </summary>
    [TestClass]
    public class PointDistributionTests : TestBase
    {
        private enum Coin : int
        {
            Head,
            Tail
        }

        [UnitTest]
        [TestMethod]
        public void PointDistributionOverEnum()
        {
            var d = Distribution.Certainly(Coin.Head);

            AssertEqualProbabilities(1.0, d.ProbabilityOf(v => v == Coin.Head));
            AssertEqualProbabilities(0.0, d.ProbabilityOf(v => v == Coin.Tail));
        }
    }
}
