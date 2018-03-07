namespace Probability
{
    using System;

    public struct PValue<T>
    {
        private readonly T value;
        private readonly Probability probability;

        internal PValue(T value, Probability probability)
        {
            this.value = value;
            this.probability = probability;
        }

        public T Value
        {
            get { return this.value; }
        }

        public Probability Probability
        {
            get { return this.probability; }
        }

        public static PValue<T> operator *(Probability p, PValue<T> v)
        {
            return new PValue<T>(v.Value, p * v.Probability);
        }

        public static PValue<S> Map<S>(Func<T, S> f, PValue<T> v)
        {
            return new PValue<S>(f(v.Value), v.Probability);
        }
    }
}
