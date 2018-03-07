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

        public static PValue<R> JoinWith<S,R>(PValue<T> v1, PValue<S> v2, Func<T, S, R> f)
        {
            return new PValue<R>(f(v1.Value, v2.Value), v1.Probability * v2.Probability);
        }
    }
}
