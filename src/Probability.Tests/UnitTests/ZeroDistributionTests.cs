namespace Probability.Tests.UnitTests
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
 
    /// <summary>
    /// Unit tests for the empty sub-distribution.
    /// </summary>
    [TestClass]
    public class ZeroDistributionTests : TestBase
    {
        [UnitTest]
        [TestMethod]
        public void ImpossibleTotalsToZero()
        {
            var d = Distribution.Impossible<int>();

            AssertEqualProbabilities(0.0, d.ProbabilityOf(v => true));
        }
    }
}
