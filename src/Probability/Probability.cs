namespace Probability
{
    using System;

    /// <summary>
    /// Represents a probability by a decimal value in the interval [0,1]
    /// </summary>
    /// <seealso cref="System.IEquatable{Probability.Probability}" />
    /// <seealso cref="System.IComparable{Probability.Probability}" />
    public struct Probability : IEquatable<Probability>, IComparable<Probability>
    {
        private readonly decimal value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Probability"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentOutOfRangeException">value - The value must be between 0.0 and 1.0</exception>
        public Probability(decimal value)
        {
            if (!value.IsBetween(0, 1))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "The value must be between 0.0 and 1.0");
            }

            this.value = value;
        }

        /// <summary>
        /// Gets a probability for an event that is impossible.
        /// </summary>
        public static Probability Impossible
        {
            get { return Probability.FromDecimal(0); }
        }

        /// <summary>
        /// Gets the probability for an event that is certain.
        /// </summary>
        public static Probability Certain
        {
            get { return Probability.FromDecimal(1); }
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Decimal"/> to <see cref="Probability"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator Probability(decimal value)
        {
            return Probability.FromDecimal(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Probability"/> to <see cref="System.Decimal"/>.
        /// </summary>
        /// <param name="prob">The probability.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator decimal(Probability prob)
        {
            return prob.ToDecimal();
        }

        /// <summary>
        /// Implements the multiplication operator *.
        /// </summary>
        /// <param name="p1">The first probability.</param>
        /// <param name="p2">The second probability.</param>
        /// <returns>
        /// The product of the given probabilities.
        /// </returns>
        public static Probability operator *(Probability p1, Probability p2)
        {
            return new Probability(p1.value * p2.value);
        }

        /// <summary>
        /// Implements the inequality operator !=.
        /// </summary>
        /// <param name="p1">The first probability.</param>
        /// <param name="p2">The second probability.</param>
        /// <returns>
        /// <c>false</c>, if the given probabilities coincide, and <c>true</c> otherwise
        /// </returns>
        public static bool operator !=(Probability p1, Probability p2)
        {
            return !(p1 == p2);
        }

        /// <summary>
        /// Implements the equality operator ==.
        /// </summary>
        /// <param name="p1">The first probability.</param>
        /// <param name="p2">The second probability.</param>
        /// <returns>
        /// <c>true</c>, if the given probabilities coincide, and <c>false</c> otherwise
        /// </returns>
        public static bool operator ==(Probability p1, Probability p2)
        {
            return p1.Equals(p2);
        }

        /// <summary>
        /// Implements the greater or equal operator.
        /// </summary>
        /// <param name="p1">The first probability.</param>
        /// <param name="p2">The second probability.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >=(Probability p1, Probability p2)
        {
            return p1.CompareTo(p2) >= 0;
        }

        /// <summary>
        /// Implements the less or equal operator.
        /// </summary>
        /// <param name="p1">The first probability.</param>
        /// <param name="p2">The second probability.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <=(Probability p1, Probability p2)
        {
            return p1.CompareTo(p2) <= 0;
        }

        /// <summary>
        /// Implements the greater-than operator.
        /// </summary>
        /// <param name="p1">The first probability.</param>
        /// <param name="p2">The second probability.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(Probability p1, Probability p2)
        {
            return p1.CompareTo(p2) > 0;
        }

        /// <summary>
        /// Implements the less-than operator.
        /// </summary>
        /// <param name="p1">The first probability.</param>
        /// <param name="p2">The second probability.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(Probability p1, Probability p2)
        {
            return p1.CompareTo(p2) < 0;
        }

        /// <summary>
        /// Converts a decimal value to a probability.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new probability value corresponding to the given decimal value.</returns>
        public static Probability FromDecimal(decimal value)
        {
            return new Probability(value);
        }

        /// <summary>
        /// Converts a double value to a probability.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new probability value corresponding to the given double value.</returns>
        public static Probability FromDouble(double value)
        {
            return Probability.FromDecimal(Convert.ToDecimal(value));
        }

        /// <summary>
        /// Converts this instance to a corresponding decimal value.
        /// </summary>
        /// <returns>A decimal value representing this instance.</returns>
        public decimal ToDecimal()
        {
            return this.value;
        }

        /// <summary>
        /// Converts this instance to a corresponding double value.
        /// </summary>
        /// <returns>A double value representing this instance.</returns>
        public double ToDouble()
        {
            return (double)this.value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Probability))
            {
                return false;
            }

            return Equals((Probability)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(Probability other)
        {
            return this.value.Equals(other.value);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order.
        /// </returns>
        public int CompareTo(Probability other)
        {
            return this.value.CompareTo(other.value);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ToString("0.##%");
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance
        /// using the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            return ToString(format, System.Globalization.CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance
        /// using the specified format and culture-specific format information.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="provider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return this.value.ToString(format, provider);
        }
    }
}
