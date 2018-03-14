namespace Probability
{
    using System;
    using System.Linq;

    /// <summary>
    /// General purpose extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Determines whether the value is between the specified lower and upper bounds (inclusive).
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="lower">The lower bound.</param>
        /// <param name="upper">The upper bound.</param>
        /// <returns>
        ///   <c>true</c> if the value is between the specified lower and upper bounds; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBetween<T>(this T value, T lower, T upper) where T : IComparable<T>
        {
            return value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0;
        }

        /// <summary>
        /// Determines whether a sequence contains a specified element 
        /// by using the default equality comparer.
        /// </summary>
        /// <typeparam name="T">An enumeration type.</typeparam>
        /// <param name="value">The value to find.</param>
        /// <param name="values">A sequence in which to locate a value.</param>
        /// <returns>True if the sequence contains an element that has the specified value; otherwise, false.</returns>
        public static bool In<T>(this T value, params T[] values)
            where T : struct
        {
            return values.Contains(value);
        }

        /// <summary>
        /// Determines whether a sequence contains a specified element 
        /// by using the default equality comparer.
        /// </summary>
        /// <param name="value">The value to find.</param>
        /// <param name="values">A sequence in which to locate a value.</param>
        /// <returns>True if the sequence contains an element that has the specified value; otherwise, false.</returns>
        public static bool In(this object value, params object[] values)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return values.Contains(value);
        }
    }
}