namespace Probability
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// (Sub-) Distributions
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Probability.PDist{T}}" />
    public class Dist<T> : IEnumerable<PValue<T>>
    {
        public static readonly Dist<T> Impossible = Dist<T>.Zero();

        private List<PValue<T>> values;

        private Dist(IEnumerable<PValue<T>> values)
        {
            this.values = Normalize(values);
        }

        public static Dist<T> operator *(Probability p, Dist<T> distribution)
        {
            return new Dist<T>(distribution.values.Select(v => p * v));
        }

        public static Dist<T> Certainly(T value)
        {
            return Dist<T>.Unit(value);
        }

        public static Dist<T> Uniform(params T[] values)
        {
            return Dist<T>.Uniform(values.ToList());
        }

        public static Dist<T> Uniform(IList<T> values)
        {
            var count = values.Count;
            return new Dist<T>(values.Select(v => new PValue<T>(v, new Probability(1.0M / count))));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<PValue<T>> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }

        public Probability ProbabilityOf(Predicate<T> @event)
        {
            return this.values
                .Where(v => @event(v.Value))
                .Sum(v => (decimal)(v.Probability));
        }

        public Dist<S> Select<S>(Func<T,S> f)
        {
            return Dist<T>.Map<S>(this, f);
        }

        public Dist<S> SelectMany<S>(Func<T, Dist<S>> f)
        {
            return Dist<T>.Bind(this, f);
        }

        public Dist<T> Where(Predicate<T> predicate)
        {
            return new Dist<T>(this.values.Where(v => predicate(v.Value)));
        }

        public bool Any()
        {
            return Any(v => true);
        }

        public bool Any(Predicate<T> predicate)
        {
            return this.ProbabilityOf(predicate) > 0;
        }

        private static Dist<T> Unit(T value)
        {
            return new Dist<T>(new[] { new PValue<T>(value, 1) });
        }

        private static Dist<T> Zero()
        {
            return new Dist<T>(new PValue<T>[0]);
        }

        private static Dist<S> Bind<S>(Dist<T> distribution, Func<T, Dist<S>> f)
        {
            return new Dist<S>(distribution.values.SelectMany(v => v.Probability * f(v.Value)));
        }

        private static Dist<S> Map<S>(Dist<T> distribution, Func<T, S> f)
        {
            return new Dist<S>(distribution.values.Select(v => PValue<T>.Map(f, v)));
        }

        private List<PValue<T>> Normalize(IEnumerable<PValue<T>> values)
        {
            return values
                .Where(v => v.Probability > 0)
                .GroupBy(v => v.Value)
                .Select(g => new PValue<T>(g.Key, g.Sum(v => (decimal)(v.Probability))))
                .ToList();
        }
    }
}
