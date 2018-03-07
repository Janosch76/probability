namespace Probability
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// <see cref="Random"/> extension methods
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// A (fair) coin toss.
        /// </summary>
        /// <param name="rng">The random number generator instance.</param>
        /// <param name="successProbability">The success probability.</param>
        /// <returns>A boolean chosen randomly according to the given success probability.</returns>
        public static bool CoinToss(this Random rng, double successProbability = .5)
        {
            if (!successProbability.IsBetween(0, 1))
            {
                throw new ArgumentOutOfRangeException(nameof(successProbability));
            }

            return rng.NextDouble() < successProbability;
        }

        public static T CoinToss<T>(this Random rng, T value1, T value2, double successPropability = .5)
        {
            return rng.CoinToss(successPropability) ? value1 : value2;
        }

        /// <summary>
        /// Chooses one of the given values uniformly at random.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="rng">The random number generator.</param>
        /// <param name="values">The values to choose from.</param>
        /// <returns>One of the given values, chosen uniformly at random.</returns>
        public static T OneOf<T>(this Random rng, IList<T> values)
        {
            return values[rng.Next(values.Count)];
        }

        /// <summary>
        /// Chooses one of the given values uniformly at random.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="rng">The random number generator.</param>
        /// <param name="values">The values to choose from.</param>
        /// <returns>One of the given values, chosen uniformly at random.</returns>
        public static T OneOf<T>(this Random rng, params T[] values)
        {
            return rng.OneOf<T>(values.ToList());
        }
    }
}
