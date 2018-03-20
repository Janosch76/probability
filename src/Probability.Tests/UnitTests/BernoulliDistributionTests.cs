namespace Probability.Tests.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for Bernoulli distribution.
    /// </summary>
    [TestClass]
    public class BernoulliDistributionTests : TestBase
    {
        private enum Coin : int
        {
            Head,
            Tail
        }

        [UnitTest]
        [TestMethod]
        public void BernoulliDistribution()
        {
            var d = Distribution.Bernoulli(Probability.FromDecimal(.3M));

            AssertEqualProbabilities(.7, d.ProbabilityOf(k => k == 0));
            AssertEqualProbabilities(.3, d.ProbabilityOf(k => k == 1));
        }

        [UnitTest]
        [TestMethod]
        public void BernoulliDistributionOverEnum()
        {
            var d = Distribution.OneOf(Coin.Head, Coin.Tail, Probability.FromDecimal(.3M));

            AssertEqualProbabilities(.3, d.ProbabilityOf(v => v == Coin.Head));
            AssertEqualProbabilities(.7, d.ProbabilityOf(v => v == Coin.Tail));
        }
    }
}
