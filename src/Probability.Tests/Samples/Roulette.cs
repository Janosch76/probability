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
            AssertEqualProbabilities(18.0 / 37, Play.ProbabilityOf(p => p.Manque), .0001);
            AssertEqualProbabilities(18.0 / 37, Play.ProbabilityOf(p => p.Passe), .0001);

            // Red or Black
            AssertEqualProbabilities(18.0 / 37, Play.ProbabilityOf(p => p.Rouge), .0001);
            AssertEqualProbabilities(18.0 / 37, Play.ProbabilityOf(p => p.Noir), .0001);

            // Even or Odd
            AssertEqualProbabilities(18.0 / 37, Play.ProbabilityOf(p => p.Pair), .0001);
            AssertEqualProbabilities(18.0 / 37, Play.ProbabilityOf(p => p.Impair), .0001);

            // Dozen bets
            AssertEqualProbabilities(12.0 / 37, Play.ProbabilityOf(p => p.PremiereDouzaine), .0001);
            AssertEqualProbabilities(12.0 / 37, Play.ProbabilityOf(p => p.MoyenneDouzaine), .0001);
            AssertEqualProbabilities(12.0 / 37, Play.ProbabilityOf(p => p.DerniereDouzaine), .0001);

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
            // Straight bet on a single number
            throw new NotImplementedException();
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

            public bool Pair
            {
                get { return this.number > 0 && this.number % 2 == 0; }
            }

            public bool Impair
            {
                get { return this.number > 0 && this.number % 2 == 1; }
            }

            public bool Green
            {
                get { return this.number == 0; }
            }

            public bool Rouge
            {
                get { return this.number.In(1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36); }
            }

            public bool Noir
            {
                get { return this.number.In(2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35); }
            }

            public bool Passe
            {
                get { return this.number.IsBetween(19, 36); }
            }

            public bool Manque
            {
                get { return this.number.IsBetween(1, 18); }
            }

            public bool PremiereDouzaine
            {
                get { return this.number.IsBetween(1, 12); }
            }

            public bool MoyenneDouzaine
            {
                get { return this.number.IsBetween(13, 24); }
            }

            public bool DerniereDouzaine
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
