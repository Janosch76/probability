namespace Probability
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// (Sub-) Distributions
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Probability.PDist{T}}" />
    public class Dist<T> : IEnumerable<PValue<T>>
    {
        /// <summary>
        /// The empty distribution over type <see cref="Dist{T}"/>
        /// </summary>
        public static readonly Dist<T> Zero = new Dist<T>(new PValue<T>[0]);

        private List<PValue<T>> values;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dist{T}"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        internal Dist(IEnumerable<PValue<T>> values)
            : this(values, EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dist{T}"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        internal Dist(IEnumerable<PValue<T>> values, IEqualityComparer<T> equalityComparer)
        {
            var p = values.Sum(v => v.Probability.ToDecimal());
            if (decimal.Round(p, 20) > 1M)
            {
                throw new ArgumentOutOfRangeException(nameof(values), $"Given probabilities {p} exceed 1.0");
            }

            this.values = Normalize(values, equalityComparer);
        }

        public static Dist<T> operator *(Probability p, Dist<T> distribution)
        {
            return new Dist<T>(distribution.values.Select(v => p * v));
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
            var p = this.values
                .Where(v => @event(v.Value))
                .Sum(v => v.Probability);
            return new Probability(p);
        }

        public Dist<S> Select<S>(Func<T, S> selector)
        {
            return Dist<T>.Map<S>(this, selector);
        }

        public Dist<S> SelectMany<S>(Func<T, Dist<S>> selector)
        {
            return SelectMany(selector, (v, u) => u);
        }

        public Dist<R> SelectMany<S, R>(Func<T, Dist<S>> selector, Func<T, S, R> resultSelector)
        {
            return Dist<T>.Bind(this, v => Dist<S>.Map(selector(v), u => resultSelector(v, u)));
        }

        public Dist<T> Where(Predicate<T> @event)
        {
            var p = this.ProbabilityOf(@event);
            var values = this.values
                .Where(v => @event(v.Value))
                .Select(v => new PValue<T>(v.Value, new Probability(1M / p * v.Probability)));
            return new Dist<T>(values);
        }

        public bool Any()
        {
            return Any(v => true);
        }

        public bool Any(Predicate<T> @event)
        {
            return this.ProbabilityOf(@event) > 0;
        }

        public Dist<Tuple<T, S>> Prod<S>(Dist<S> distribution)
        {
            return this.JoinWith(distribution, (v1, v2) => Tuple.Create<T, S>(v1, v2));
        }

        // TODO
        //public Dist<IEnumerable<T>> Repeat(int n)
        //{
        //    if (n < 0)
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(n));
        //    }

        //    var dists = Enumerable.Repeat(this, n);
        //    var d = dists.Aggregate<Dist<T>, Dist<IEnumerable<T>>>(Certainly(new T[0]), )
        //}

        public Dist<R> JoinWith<S, R>(Dist<S> distribution, Func<T, S, R> f)
        {
            return Dist<T>.Join<S, R>(this, distribution, f);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Join(
                Environment.NewLine,
                this.values
                    .OrderByDescending(v => v.Probability)
                    .Take(3));
        }

        public static Dist<T> Unit(T value)
        {
            return new Dist<T>(new[] { new PValue<T>(value, new Probability(1)) });
        }

        private static Dist<S> Bind<S>(Dist<T> distribution, Func<T, Dist<S>> f)
        {
            return new Dist<S>(distribution.values.SelectMany(v => v.Probability * f(v.Value)));
        }

        private static Dist<S> Map<S>(Dist<T> distribution, Func<T, S> f)
        {
            return new Dist<S>(distribution.values.Select(v => PValue<T>.Map(f, v)));
        }

        private static Dist<R> Join<S, R>(Dist<T> distribution1, Dist<S> distribution2, Func<T, S, R> f)
        {
            return new Dist<R>(distribution1.SelectMany(v1 => distribution2.Select(v2 => v1.JoinWith(v2, f))));
        }

        private List<PValue<T>> Normalize(IEnumerable<PValue<T>> values, IEqualityComparer<T> comparer)
        {
            return values
                .Where(v => v.Probability > 0)
                .GroupBy(v => v.Value, comparer)
                .Select(g => new PValue<T>(g.Key, new Probability(g.Sum(v => v.Probability))))
                .ToList();
        }
    }
}
