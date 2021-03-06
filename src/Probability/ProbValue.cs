﻿namespace Probability
{
    using System;

    /// <summary>
    /// Represents a value with its associated likelyhood.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    public struct ProbValue<T>
    {
        private readonly T value;
        private readonly Probability probability;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProbValue{T}"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="probability">The probability.</param>
        internal ProbValue(T value, Probability probability)
        {
            this.value = value;
            this.probability = probability;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Gets the probability.
        /// </summary>
        public Probability Probability
        {
            get { return this.probability; }
        }

        /// <summary>
        /// Scales the likelyhood of a value by a given probability.
        /// </summary>
        /// <param name="p">The probability.</param>
        /// <param name="v">The value with its associated likelyhood.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static ProbValue<T> operator *(Probability p, ProbValue<T> v)
        {
            return new ProbValue<T>(v.Value, p * v.Probability);
        }

        /// <summary>
        /// Applies a specified map to a value with its associated likelyhood.
        /// </summary>
        /// <typeparam name="S">The result type</typeparam>
        /// <param name="f">The map.</param>
        /// <param name="v">The value.</param>
        /// <returns>The mapped value with its associated likelyhood.</returns>
        public static ProbValue<S> Map<S>(Func<T, S> f, ProbValue<T> v)
        {
            return v.Map(f);
        }

        /// <summary>
        /// Joins this instance with another instance, by applying a selection function.
        /// </summary>
        /// <typeparam name="S">The element type.</typeparam>
        /// <typeparam name="R">The result type.</typeparam>
        /// <param name="v1">The first value.</param>
        /// <param name="v2">The second value.</param>
        /// <param name="selector">The selection map.</param>
        /// <returns>The combined value, with the joint probability.</returns>
        public static ProbValue<R> Join<S, R>(ProbValue<T> v1, ProbValue<S> v2, Func<T, S, R> selector)
        {
            return v1.JoinWith(v2, selector);
        }

        /// <summary>
        /// Applies a specified map to a value with its associated likelyhood.
        /// </summary>
        /// <typeparam name="S">The result type</typeparam>
        /// <param name="f">The map.</param>
        /// <returns>The mapped value with its associated likelyhood.</returns>
        public ProbValue<S> Map<S>(Func<T, S> f)
        {
            return new ProbValue<S>(f(this.value), this.probability);
        }

        /// <summary>
        /// Joins this instance with another instance, by applying a selection function.
        /// </summary>
        /// <typeparam name="S">The element type.</typeparam>
        /// <typeparam name="R">The result type.</typeparam>
        /// <param name="other">The second.</param>
        /// <param name="selector">The selection map.</param>
        /// <returns>The combined value, with the joint probability.</returns>
        public ProbValue<R> JoinWith<S, R>(ProbValue<S> other, Func<T, S, R> selector)
        {
            return new ProbValue<R>(selector(this.value, other.value), this.probability * other.probability);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.value}:{this.probability}";
        }
    }
}
