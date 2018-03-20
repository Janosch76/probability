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
    public class DistributionTests : TestBase
    {
        [UnitTest]
        [TestMethod]
        public void TestMethod4()
        {
            var d = Distribution.Impossible<int>();

            Assert.AreEqual(0, (double)(decimal)d.ProbabilityOf(v => true));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod5()
        {
            var d1 = Distribution.Uniform(0, 1, 2, 3);
            var d2 = d1.Select(v => v > 0 ? Coin.Head : Coin.Tail);
            Assert.AreEqual(.75, (double)(decimal)d2.ProbabilityOf(v => v == Coin.Head));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod6()
        {
            var d1 = Distribution.Uniform(0, 1);
            var d2 = d1.Where(v => v > 0);
            Assert.AreEqual(1, (double)(decimal)d2.ProbabilityOf(v => true));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod7()
        {
            Assert.AreEqual(true, Distribution.Uniform(1,2,3).Any());
            Assert.AreEqual(true, Distribution.Uniform(1,2,3).Any(v => v > 1));
            Assert.AreEqual(false, Distribution.Uniform(1,2,3).Any(v => v < 1));
            Assert.AreEqual(false, Distribution.Impossible<int>().Any());
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod8()
        {
            var d = Distribution.Uniform(Coin.Head, Coin.Tail)
                .Prod(Distribution.Uniform(Coin.Head, Coin.Tail));
            Assert.AreEqual(.25, (double)(decimal)d.ProbabilityOf(v => v.Item1 == Coin.Head && v.Item2 == Coin.Head));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod9()
        {
            var d = Distribution.OneOf(1, 2, new Probability(0.9M))
                .Prod(Distribution.OneOf(1, 2, new Probability(0.9M)));
            Assert.AreEqual(.99, (double)(decimal)d.ProbabilityOf(v => v.Item1 == 1 || v.Item2 == 1));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod11()
        {
            var d = Distribution.Uniform(1, 2, 3, 4);
            Assert.AreEqual(.25, (double)(decimal)d.ProbabilityOf(v => v == 4));

            var d2 = d.Where(v => v > 2);
            Assert.AreEqual(.5, (double)(decimal)d2.ProbabilityOf(v => v == 4));
        }
    }
}
