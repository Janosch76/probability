namespace Probability.Tests.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for Hypergeometric distribution.
    /// </summary>
    [TestClass]
    public class HypergeometricDistributionTests : TestBase
    {
        [UnitTest]
        [TestMethod]
        public void HypergeometricDistributionOnEqualProbAnd0Experiments()
        {
            var d = Distribution.Hypergeometric(0, 2, 1);

            AssertEqualProbabilities(1.0, d.ProbabilityOf(k => k == 0));
        }

        [UnitTest]
        [TestMethod]
        public void HypergeometricDistributionOnEqualProbAnd1Experiments()
        {
            var d = Distribution.Hypergeometric(1, 2, 1);

            AssertEqualProbabilities(.5, d.ProbabilityOf(k => k == 0));
            AssertEqualProbabilities(.5, d.ProbabilityOf(k => k == 1));
        }

        [UnitTest]
        [TestMethod]
        public void HypergeometricDistributionOnEqualProbAnd2Experiments()
        {
            var d = Distribution.Hypergeometric(2, 2, 1);

            AssertEqualProbabilities(0.0, d.ProbabilityOf(k => k == 0));
            AssertEqualProbabilities(1.0, d.ProbabilityOf(k => k == 1));
            AssertEqualProbabilities(0.0, d.ProbabilityOf(k => k == 2));
        }

        [UnitTest]
        [TestMethod]
        public void HypergeometricDistributionOn2Experiments()
        {
            var d = Distribution.Hypergeometric(2, 5, 2);

            AssertEqualProbabilities(0.3, d.ProbabilityOf(k => k == 0));
            AssertEqualProbabilities(0.6, d.ProbabilityOf(k => k == 1));
            AssertEqualProbabilities(0.1, d.ProbabilityOf(k => k == 2));
        }
    }
}
