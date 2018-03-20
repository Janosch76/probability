namespace Probability.Tests.Samples
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// A sample formulation of a Bernoulli distribution using Linq syntax
    /// </summary>
    [TestClass]
    public class LinqSyntax : TestBase
    {
        [Sample]
        [TestMethod]
        public void BernoulliDistribution()
        {
            var bernoulli =
                from v1 in Distribution.OneOf(0, 1)
                from v2 in Distribution.OneOf(0, 1)
                select v1 + v2;

            AssertEqualProbabilities(.5, bernoulli.ProbabilityOf(k => k == 1));
        }
    }
}
