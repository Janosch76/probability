namespace Probability
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Factory methods to create specific distributions
    /// </summary>
    public static class Spread
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
            return Spread.Uniform<T>(values.ToList());
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
            return new Dist<T>(values.Select(v => new PValue<T>(v, new Probability(1.0M / count))));
        }

        /// <summary>
        /// A coin toss, assigning equal probabilities to two given values.
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A uniform distribution.</returns>
        public static Dist<T> OneOf<T>(T value1, T value2)
        {
            return Spread.OneOf(value1, value2, new Probability(0.5M));
        }

        /// <summary>
        /// A coin toss, assigning probabilities to two given values according to a given bias.
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="bias">The bias.</param>
        /// <returns>A biased distribution.</returns>
        public static Dist<T> OneOf<T>(T value1, T value2, Probability bias)
        {
            return new Dist<T>(new[] { new PValue<T>(value1, bias), new PValue<T>(value2, new Probability(1M - bias)) });
        }

        public static Dist<double> Normal()
        {
            throw new NotImplementedException();
        }
    }
}
