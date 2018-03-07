namespace Probability
{
    using System;

    public struct Probability : IEquatable<Probability>, IComparable<Probability>
    {
        private readonly decimal value;

        public Probability(decimal value)
        {
            if (!value.IsBetween(0, 1))
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "The value must be between 0.0 and 1.0");
            }

            this.value = value;
        }

        public static implicit operator Probability(decimal value)
        {
            return new Probability(value);
        }

        public static explicit operator decimal(Probability prob)
        {
            return prob.value;
        }

        public static Probability operator *(Probability p1, Probability p2)
        {
            return new Probability(p1.value * p2.value);
        }

        public static bool operator !=(Probability p1, Probability p2)
        {
            return !(p1 == p2);
        }

        public static bool operator ==(Probability p1, Probability p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator >=(Probability p1, Probability p2)
        {
            return p1.CompareTo(p2) >= 0;
        }

        public static bool operator <=(Probability p1, Probability p2)
        {
            return p1.CompareTo(p2) <= 0;
        }

        public static bool operator >(Probability p1, Probability p2)
        {
            return p1.CompareTo(p2) > 0;
        }

        public static bool operator <(Probability p1, Probability p2)
        {
            return p1.CompareTo(p2) < 0;
        }

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

        public bool Equals(Probability other)
        {
            return this.value.Equals(other.value);
        }

        public int CompareTo(Probability other)
        {
            return this.value.CompareTo(other.value);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
