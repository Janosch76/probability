namespace Probability.Tests.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the uniform distribution.
    /// </summary>
    [TestClass]
    public class UniformDistributionTests : TestBase
    {
        private enum Coin : int
        {
            Head,
            Tail
        }

        [UnitTest]
        [TestMethod]
        public void UniformDistributionOverEnum()
        {
            var coinToss = Distribution.Uniform(Coin.Head, Coin.Tail);

            AssertEqualProbabilities(.5, coinToss.ProbabilityOf(v => v == Coin.Head));
            AssertEqualProbabilities(.5, coinToss.ProbabilityOf(v => v == Coin.Tail));
        }

        [UnitTest]
        [TestMethod]
        public void UniformDistributionOverInt()
        {
            var d = Distribution.Uniform(0, 1, 2, 3);

            AssertEqualProbabilities(.25, d.ProbabilityOf(v => v == 0));
            AssertEqualProbabilities(.25, d.ProbabilityOf(v => v == 1));
            AssertEqualProbabilities(.25, d.ProbabilityOf(v => v == 2));
            AssertEqualProbabilities(.25, d.ProbabilityOf(v => v == 3));
        }
    }
}
