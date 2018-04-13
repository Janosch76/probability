namespace Probability.Tests.Samples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Roulette : TestBase
    {
        private static readonly Pocket[] Pockets = Enumerable.Range(0, 37).Select(i => new Pocket(i)).ToArray();
        private static readonly Dist<Pocket> Play = Distribution.Uniform(Pockets);

        [Sample]
        [TestMethod]
        public void OutsideBetsProbabilities()
        {
            // Low or High
            AssertEqualProbabilities(18.0 / 37, Play.ProbabilityOf(p => p.Low), .0001);
            AssertEqualProbabilities(18.0 / 37, Play.ProbabilityOf(p => p.High), .0001);

            // Red or Black
            AssertEqualProbabilities(18.0 / 37, Play.ProbabilityOf(p => p.Red), .0001);
            AssertEqualProbabilities(18.0 / 37, Play.ProbabilityOf(p => p.Black), .0001);

            // Even or Odd
            AssertEqualProbabilities(18.0 / 37, Play.ProbabilityOf(p => p.Even), .0001);
            AssertEqualProbabilities(18.0 / 37, Play.ProbabilityOf(p => p.Odd), .0001);

            // Dozen bets
            AssertEqualProbabilities(12.0 / 37, Play.ProbabilityOf(p => p.FirstDozen), .0001);
            AssertEqualProbabilities(12.0 / 37, Play.ProbabilityOf(p => p.MiddleDozen), .0001);
            AssertEqualProbabilities(12.0 / 37, Play.ProbabilityOf(p => p.LastDozen), .0001);

            // Column bet
            AssertEqualProbabilities(12.0 / 37, Play.ProbabilityOf(p => p.Column1), .0001);
            AssertEqualProbabilities(12.0 / 37, Play.ProbabilityOf(p => p.Column2), .0001);
            AssertEqualProbabilities(12.0 / 37, Play.ProbabilityOf(p => p.Column3), .0001);
        }

        [Sample]
        [TestMethod]
        public void InsideBets()
        {
            // Straight bet on a single number
            AssertEqualProbabilities(1.0 / 37, Play.ProbabilityOf(p => p.Number == 7), .0001);
        }

        [Sample]
        [TestMethod]
        public void Payoff()
        {
            var bet = 
                from p in Distribution.Uniform(Pockets)
                where p.Number != 0
                select Bet.SingleNumber(1M, p.Number);
            var amount = 1M;
            var payoff =
                from p in Distribution.Uniform(Pockets)
                from b in bet
                select -amount + (b.Value.Win(p.Value) ? 36M * amount : 0);

            Assert.AreEqual(-0.027, (double)payoff.Average(), 0.0001);
        }

        private class Bet
        {
            private readonly int[] numbers;

            public Bet(IEnumerable<int> numbers)
            {
                this.numbers = numbers.ToArray();
            }

            public bool Win(Pocket pocket)
            {
                return pocket.Number.In(this.numbers);
            }

            public static Bet SingleNumber(decimal amount, int number)
            {
                return new Bet(new int[] { number });
            }

            public static Bet Pair(decimal amount)
            {
                return new Bet(Pockets.Where(p => p.Even).Select(p => p.Number));
            }

            public static Bet Impair(decimal amount)
            {
                return new Bet(Pockets.Where(p => p.Odd).Select(p => p.Number));
            }

            public static Bet Rouge(decimal amount)
            {
                return new Bet(Pockets.Where(p => p.Red).Select(p => p.Number));
            }

            public static Bet Noir(decimal amount)
            {
                return new Bet(Pockets.Where(p => p.Black).Select(p => p.Number));
            }

            public static Bet Passe(decimal amount)
            {
                return new Bet(Pockets.Where(p => p.High).Select(p => p.Number));
            }

            public static Bet Manque(decimal amount)
            {
                return new Bet(Pockets.Where(p => p.Low).Select(p => p.Number));
            }

            public static Bet PremiereDouzaine(decimal amount)
            {
                return new Bet(Pockets.Where(p => p.FirstDozen).Select(p => p.Number));
            }

            public static Bet MoyenneDouzaine(decimal amount)
            {
                return new Bet(Pockets.Where(p => p.MiddleDozen).Select(p => p.Number));
            }

            public static Bet DerniereDouzaine(decimal amount)
            {
                return new Bet(Pockets.Where(p => p.LastDozen).Select(p => p.Number));
            }
        }

        private class Pocket
        {
            private readonly int number;

            public Pocket(int number)
            {
                if (!number.IsBetween(0, 36))
                {
                    throw new ArgumentOutOfRangeException(nameof(number));
                }

                this.number = number;
            }

            public int Number
            {
                get { return this.number; }
            }

            public bool Even
            {
                get { return this.number > 0 && this.number % 2 == 0; }
            }

            public bool Odd
            {
                get { return this.number > 0 && this.number % 2 == 1; }
            }

            public bool Green
            {
                get { return this.number == 0; }
            }

            public bool Red
            {
                get { return this.number.In(1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36); }
            }

            public bool Black
            {
                get { return this.number.In(2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35); }
            }

            public bool High
            {
                get { return this.number.IsBetween(19, 36); }
            }

            public bool Low
            {
                get { return this.number.IsBetween(1, 18); }
            }

            public bool FirstDozen
            {
                get { return this.number.IsBetween(1, 12); }
            }

            public bool MiddleDozen
            {
                get { return this.number.IsBetween(13, 24); }
            }

            public bool LastDozen
            {
                get { return this.number.IsBetween(25, 36); }
            }

            public bool Column1
            {
                get { return this.number > 0 && this.number % 3 == 1; }
            }

            public bool Column2
            {
                get { return this.number > 0 && this.number % 3 == 2; }
            }

            public bool Column3
            {
                get { return this.number > 0 && this.number % 3 == 0; }
            }
        }
    }
}
