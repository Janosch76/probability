namespace Probability.Tests.Samples
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// A sample formulation of a Binomial distribution and Bayes rule using Linq syntax
    /// </summary>
    [TestClass]
    public class LinqSyntax : TestBase
    {
        [Sample]
        [TestMethod]
        public void BinomialDistribution()
        {
            var binomial =
                from v1 in Distribution.OneOf(0, 1)
                from v2 in Distribution.OneOf(0, 1)
                select v1 + v2;

            AssertEqualProbabilities(.5, binomial.ProbabilityOf(k => k == 1));
        }

        [Sample]
        [TestMethod]
        public void BayesRule()
        {
            var d =
                from v in Distribution.Uniform(1, 2, 3)
                where v != 1
                select v;

            AssertEqualProbabilities(.5, d.ProbabilityOf(v => v == 2));
            AssertEqualProbabilities(.5, d.ProbabilityOf(v => v == 3));
        }
    }
}
