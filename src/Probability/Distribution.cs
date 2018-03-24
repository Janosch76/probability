namespace Probability
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods and static methods to create specific distributions
    /// </summary>
    public static class Distribution
    {
        /// <summary>
        /// Empty distribution on a given space.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <returns>An empty distribution on the given type.</returns>
        public static Dist<T> Impossible<T>()
        {
            return Dist<T>.Zero;
        }

        /// <summary>
        /// A point distribution.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>A point distribution on the given value.</returns>
        public static Dist<T> Certainly<T>(T value)
        {
            return Dist<T>.Unit(value);
        }

        /// <summary>
        /// A uniform distribution, assigning equal probabilities to a collection of values.
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="values">The values.</param>
        /// <returns>A uniform distribution.</returns>
        public static Dist<T> Uniform<T>(params T[] values)
        {
            return Distribution.Uniform<T>(values.ToList());
        }

        /// <summary>
        /// A uniform distribution, assigning equal probabilities to a collection of values.
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="values">The values.</param>
        /// <returns>A uniform distribution.</returns>
        public static Dist<T> Uniform<T>(IList<T> values)
        {
            var count = values.Count;
            return new Dist<T>(values.Select(v => new ProbValue<T>(v, new Probability(1.0M / count))));
        }

        /// <summary>
        /// Bernoulli distribution, which takes value 1 with probability p and value 0 with probability 1 − p.
        /// </summary>
        /// <param name="p">The probability.</param>
        /// <returns>Bernoulli distribution with the specified probability.</returns>
        public static Dist<int> Bernoulli(Probability p)
        {
            return Distribution.OneOf(1, 0, p);
        }

        /// <summary>
        /// Rademacher distribution, which takes values 1 and -1 with probability 0.5 each.
        /// </summary>
        /// <returns>Rademacher distribution.</returns>
        public static Dist<int> Rademacher()
        {
            return Distribution.OneOf(1, -1);
        }

        /// <summary>
        /// Bernoulli distribution, assigning equal probabilities to two given values.
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A uniform distribution.</returns>
        public static Dist<T> OneOf<T>(T value1, T value2)
        {
            return Distribution.OneOf(value1, value2, new Probability(0.5M));
        }

        /// <summary>
        /// Bernoulli distribution, assigning probabilities to two given values according to a given bias.
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="bias">The bias.</param>
        /// <returns>A biased distribution.</returns>
        public static Dist<T> OneOf<T>(T value1, T value2, Probability bias)
        {
            return new Dist<T>(new[] { new ProbValue<T>(value1, bias), new ProbValue<T>(value2, new Probability(1M - bias)) });
        }

        /// <summary>
        /// Binomial distribution with (discrete probability distribution of the number of successes in a sequence of independent yes/no experiments).
        /// </summary>
        /// <param name="experiments">The number of experiments.</param>
        /// <returns>Binomial distribution for the given parameters.</returns>
        public static Dist<int> Binomial(int experiments)
        {
            return Distribution.Binomial(experiments, new Probability(.5M));
        }

        /// <summary>
        /// Binomial distribution (discrete probability distribution of 
        /// the number of successes in a sequence of independent yes/no experiments).
        /// </summary>
        /// <param name="experiments">The number of experiments.</param>
        /// <param name="prob">The probability of a successful experiment.</param>
        /// <returns>Binomial distribution for the given parameters.</returns>
        public static Dist<int> Binomial(int experiments, Probability prob)
        {
            if (experiments < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(experiments));
            }

            var binomials = new int[experiments + 1];
            binomials[0] = binomials[experiments] = 1;
            for (int k = 1; k < (experiments / 2) + 1; k++)
            {
                binomials[k] = binomials[experiments - k] = binomials[k - 1] * (experiments + 1 - k) / k;
            }

            var p = new Probability[experiments + 1];
            p[0] = new Probability(1M);
            for (int k = 1; k <= experiments; k++)
            {
                p[k] = p[k - 1] * prob;
            }

            var q = new Probability[experiments + 1];
            q[0] = new Probability(1M);
            for (int k = 1; k <= experiments; k++)
            {
                q[k] = q[k - 1] * prob;
            }

            var values = Enumerable.Range(0, experiments + 1)
                .Select(k => new Probability(binomials[k] * p[k] * q[experiments - k]))
                .Select((probability, k) => new ProbValue<int>(k, probability));
            return new Dist<int>(values);
        }

        /// <summary>
        /// Hypergeometric distribution (probability of a particular number of successes
        /// within a specified number of experiments, where each experiment consists of selecting 
        /// a distinct object from a finite population that contains a known number of objects 
        /// with the desired property.
        /// </summary>
        /// <param name="experiments">The number of experiments.</param>
        /// <param name="populationSize">The size of the population.</param>
        /// <param name="successPopulationSize">The number of success states in the population.</param>
        /// <returns>The hypergeometric distribution for the given parameters.</returns>
        public static Dist<int> Hypergeometric(int experiments, int populationSize, int successPopulationSize)
        {
            if (experiments < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(experiments));
            }

            if (populationSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(populationSize));
            }

            if (successPopulationSize < 0 || successPopulationSize > populationSize)
            {
                throw new ArgumentOutOfRangeException(nameof(successPopulationSize));
            }

            var binomial = new int[populationSize + 1, populationSize + 1];
            binomial[0, 0] = 1;
            for (int n = 1; n <= populationSize; n++)
            {
                binomial[n, 0] = binomial[n, n] = 1;
                for (int k = 1; k < (n / 2) + 1; k++)
                {
                    binomial[n, k] = binomial[n, n - k] = binomial[n - 1, k - 1] + binomial[n - 1, k];
                }
            }

            var minObservableSuccesses = Math.Max(0, experiments + successPopulationSize - populationSize);
            var maxObservableSuccesses = Math.Min(experiments, successPopulationSize);
            var permutations = binomial[populationSize, experiments];
            var values = Enumerable.Range(minObservableSuccesses, maxObservableSuccesses - minObservableSuccesses + 1)
                .Select(k => new Probability((decimal)(binomial[successPopulationSize, k] * binomial[populationSize - successPopulationSize, experiments - k]) / (decimal)permutations))
                .Select((probability, k) => new ProbValue<int>(minObservableSuccesses + k, probability));
            return new Dist<int>(values);
        }

        /// <summary>
        /// Averages the specified distribution.
        /// </summary>
        /// <param name="distribution">The distribution.</param>
        /// <returns>The expected value for the given distribution.</returns>
        public static decimal Average(this Dist<int> distribution)
        {
            return distribution.Average(v => v);
        }

        /// <summary>
        /// Averages the specified distribution.
        /// </summary>
        /// <param name="distribution">The distribution.</param>
        /// <returns>The expected value for the given distribution.</returns>
        public static decimal Average(this Dist<decimal> distribution)
        {
            return distribution.Average(v => v);
        }
    }
}
