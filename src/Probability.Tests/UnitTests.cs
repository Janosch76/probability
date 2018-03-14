namespace Probability.Tests
{
    using System;
    using System.IO;
    using System.Linq;
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
            var coin = Spread.Uniform(Coin.Head, Coin.Tail);
            var p = coin.ProbabilityOf(v => v.In(Coin.Head));

            Assert.AreEqual(.5, (double)(decimal)p, .001);
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod3()
        {
            var d = Spread.Certainly(0);

            Assert.AreEqual(1, (double)(decimal)d.ProbabilityOf(v => v == 0));
            Assert.AreEqual(0, (double)(decimal)d.ProbabilityOf(v => v != 0));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod4()
        {
            var d = Spread.Impossible<int>();

            Assert.AreEqual(0, (double)(decimal)d.ProbabilityOf(v => true));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod5()
        {
            var d1 = Spread.Uniform(0, 1, 2, 3);
            var d2 = d1.Select(v => v > 0 ? Coin.Head : Coin.Tail);
            Assert.AreEqual(.75, (double)(decimal)d2.ProbabilityOf(v => v == Coin.Head));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod6()
        {
            var d1 = Spread.Uniform(0, 1);
            var d2 = d1.Where(v => v > 0);
            Assert.AreEqual(1, (double)(decimal)d2.ProbabilityOf(v => true));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod7()
        {
            Assert.AreEqual(true, Spread.Uniform(1,2,3).Any());
            Assert.AreEqual(true, Spread.Uniform(1,2,3).Any(v => v > 1));
            Assert.AreEqual(false, Spread.Uniform(1,2,3).Any(v => v < 1));
            Assert.AreEqual(false, Spread.Impossible<int>().Any());
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod8()
        {
            var d = Spread.Uniform(Coin.Head, Coin.Tail)
                .Prod(Spread.Uniform(Coin.Head, Coin.Tail));
            Assert.AreEqual(.25, (double)(decimal)d.ProbabilityOf(v => v.Item1 == Coin.Head && v.Item2 == Coin.Head));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod9()
        {
            var d = Spread.OneOf(1, 2, new Probability(0.9M))
                .Prod(Spread.OneOf(1, 2, new Probability(0.9M)));
            Assert.AreEqual(.99, (double)(decimal)d.ProbabilityOf(v => v.Item1 == 1 || v.Item2 == 1));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod10()
        {
            var d =
                from v1 in Spread.OneOf(1, 2)
                from v2 in Spread.OneOf(1, 2)
                       select v1 + v2;

            Assert.AreEqual(.5, (double)(decimal)d.ProbabilityOf(v => v == 3));
        }

        [UnitTest]
        [TestMethod]
        public void TestMethod11()
        {
            var d = Spread.Uniform(1, 2, 3, 4);
            Assert.AreEqual(.25, (double)(decimal)d.ProbabilityOf(v => v == 4));

            var d2 = d.Where(v => v > 2);
            Assert.AreEqual(.5, (double)(decimal)d2.ProbabilityOf(v => v == 4));
        }

        private enum Doors { A, B, C }
        private struct State
        {
            public Doors? Prize;
            public Doors? Selected;
            public Doors? Opened;
            public override bool Equals(object obj)
            {
                return obj is State && this.Prize.Equals(((State)obj).Prize) && this.Selected.Equals(((State)obj).Selected) && this.Opened.Equals(((State)obj).Opened);
            }
            public override int GetHashCode()
            {
                return this.Prize.GetHashCode() ^ this.Selected.GetHashCode() ^ this.Opened.GetHashCode();
            }
            public override string ToString()
            {
                return $"[Prize:{Prize.ToString() ?? "-"}|Selected:{Selected.ToString() ?? "-"}|Opened:{Opened.ToString() ?? "-"}]";
            }
        }

        [UnitTest]
        [TestMethod]
        public void MontyHall()
        {
            var doors = new[] { Doors.A, Doors.B, Doors.C };
            Func<State, Dist<State>> hidePrize = s =>
                Spread.Uniform(doors)
                .Select(d => new State() { Prize = d, Selected = s.Selected, Opened = s.Opened });
            Func<State, Dist<State>> chooseDoor = s =>
                Spread.Uniform(doors)
                .Select(d => new State() { Prize = s.Prize, Selected = d, Opened = s.Opened });
            Func<State, Dist<State>> revealEmpty = s =>
                Spread.Uniform(doors.Where(d => d != s.Prize).ToArray())
                .Select(d => new State() { Prize = s.Prize, Selected = s.Selected, Opened = d });

            Func<State, Dist<State>> keepSelected = s =>
                Spread.Certainly(s);
            Func<State, Dist<State>> switchSelected = s =>
                Spread.Uniform(doors.Where(d => !d .In(s.Opened, s.Selected)).ToArray())
                    .Select(d => new State() { Prize = s.Prize, Selected = d, Opened = s.Opened });

            Predicate<State> Win = s => s.Selected == s.Prize;

            var state = Spread.Certainly(new State());
            state = state.SelectMany(hidePrize);
            state = state.SelectMany(chooseDoor);
            state = state.SelectMany(revealEmpty);
            state = state.SelectMany(keepSelected);

            var p = state.ProbabilityOf(Win);

            Func<Func<State, Dist<State>>, Dist<State>> game = strategy =>
                hidePrize
                .Then(chooseDoor)
                .Then(revealEmpty)
                .Then(strategy)
                .From(new State());

            var p1 = game(keepSelected).ProbabilityOf(Win);
            var p2 = game(switchSelected).ProbabilityOf(Win);
            Assert.IsTrue(p1 < p2);

        }

    }
}
