namespace Probability.Tests.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for Binomial distribution.
    /// </summary>
    [TestClass]
    public class BinomialDistributionTests : TestBase
    {
        [UnitTest]
        [TestMethod]
        public void BinomialDistributionOn1Experiments()
        {
            var d = Distribution.Binomial(1, Probability.FromDecimal(.5M));

            AssertEqualProbabilities(.5, d.ProbabilityOf(k => k == 0));
            AssertEqualProbabilities(.5, d.ProbabilityOf(k => k == 1));
        }

        [UnitTest]
        [TestMethod]
        public void BinomialDistributionOn2Experiments()
        {
            var d = Distribution.Binomial(2, Probability.FromDecimal(.5M));

            AssertEqualProbabilities(.25, d.ProbabilityOf(k => k == 0));
            AssertEqualProbabilities(.5, d.ProbabilityOf(k => k == 1));
            AssertEqualProbabilities(.25, d.ProbabilityOf(k => k == 2));
        }

        [UnitTest]
        [TestMethod]
        public void BinomialDistributionOn0Experiments()
        {
            var d = Distribution.Binomial(0, Probability.FromDecimal(.5M));

            AssertEqualProbabilities(1.0, d.ProbabilityOf(k => k == 0));
        }
    }
}
