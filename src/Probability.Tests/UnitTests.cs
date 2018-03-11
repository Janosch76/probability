﻿namespace Probability.Tests
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
 
    /// <summary>
    /// A collection of sample unit tests.
    /// </summary>
    [TestClass]
    public class UnitTests : TestBase
    {
        [UnitTest]
        [TestMethod]
        public void ProbabilityComparison1()
        {
            var p1 = new Probability(.5M);
            var p2 = new Probability(.75M);

            Assert.AreEqual(true, p1 < p2);
            Assert.AreEqual(true, p1 <= p2);
            Assert.AreEqual(false, p2 < p1);
            Assert.AreEqual(false, p2 <= p1);
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod1()
        {
            var rng = new Random();

            var heads = 0;
            for (int i = 0; i < 100000; i++)
            {
                if (rng.CoinToss(Coin.Head, Coin.Tail) == Coin.Head)
                {
                    heads++;
                }
            }

            Assert.AreEqual(heads / 100000.0, 0.5, 0.01);
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod2()
        {
            var coin = Dist<Coin>.Uniform(Coin.Head, Coin.Tail);
            var p = coin.ProbabilityOf(v => v.In(Coin.Head));

            Assert.AreEqual(.5, (double)(decimal)p, .001);
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod3()
        {
            var d = Dist<int>.Certainly(0);

            Assert.AreEqual(1, (double)(decimal)d.ProbabilityOf(v => v == 0));
            Assert.AreEqual(0, (double)(decimal)d.ProbabilityOf(v => v != 0));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod4()
        {
            var d = Dist<int>.Impossible;

            Assert.AreEqual(0, (double)(decimal)d.ProbabilityOf(v => true));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod5()
        {
            var d1 = Dist<int>.Uniform(0, 1, 2, 3);
            var d2 = d1.Select(v => v > 0 ? Coin.Head : Coin.Tail);
            Assert.AreEqual(.75, (double)(decimal)d2.ProbabilityOf(v => v == Coin.Head));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod6()
        {
            var d1 = Dist<int>.Uniform(0, 1);
            var d2 = d1.Where(v => v > 0);
            Assert.AreEqual(1, (double)(decimal)d2.ProbabilityOf(v => true));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod7()
        {
            Assert.AreEqual(true, Dist<int>.Uniform(1,2,3).Any());
            Assert.AreEqual(true, Dist<int>.Uniform(1,2,3).Any(v => v > 1));
            Assert.AreEqual(false, Dist<int>.Uniform(1,2,3).Any(v => v < 1));
            Assert.AreEqual(false, Dist<int>.Impossible.Any());
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod8()
        {
            var d = Dist<Coin>.Uniform(Coin.Head, Coin.Tail)
                .Prod(Dist<Coin>.Uniform(Coin.Head, Coin.Tail));
            Assert.AreEqual(.25, (double)(decimal)d.ProbabilityOf(v => v.Item1 == Coin.Head && v.Item2 == Coin.Head));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod9()
        {
            var d = Dist<int>.OneOf(1, 2, 0.9M)
                .Prod(Dist<int>.OneOf(1, 2, 0.9M));
            Assert.AreEqual(.99, (double)(decimal)d.ProbabilityOf(v => v.Item1 == 1 || v.Item2 == 1));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod10()
        {
            var d =
                from v1 in Dist<int>.OneOf(1, 2)
                from v2 in Dist<int>.OneOf(1, 2)
                       select v1 + v2;

            Assert.AreEqual(.5, (double)(decimal)d.ProbabilityOf(v => v == 3));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod11()
        {
            var d = Dist<int>.Uniform(1, 2, 3, 4);
            Assert.AreEqual(.25, (double)(decimal)d.ProbabilityOf(v => v == 4));

            var d2 = d.Where(v => v > 2);
            Assert.AreEqual(.5, (double)(decimal)d2.ProbabilityOf(v => v == 4));
        }
    }
}
