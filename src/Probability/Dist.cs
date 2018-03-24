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
    public class Dist<T> : IEnumerable<ProbValue<T>>
    {
        /// <summary>
        /// The empty distribution over type <see cref="Dist{T}"/>
        /// </summary>
        public static readonly Dist<T> Zero = new Dist<T>(new ProbValue<T>[0]);

        private List<ProbValue<T>> values;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dist{T}"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        internal Dist(IEnumerable<ProbValue<T>> values)
            : this(values, EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dist{T}"/> class.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        internal Dist(IEnumerable<ProbValue<T>> values, IEqualityComparer<T> equalityComparer)
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

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ProbValue<T>> GetEnumerator()
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
                .Select(v => new ProbValue<T>(v.Value, new Probability(1M / p * v.Probability)));
            return new Dist<T>(values);
        }

        public decimal Average(Func<T, decimal> selector)
        {
            var avg = 0M;
            foreach (var pv in this.values)
            {
                avg += pv.Probability * selector(pv.Value);
            }

            return avg;
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
            var mostSignificantValues = this.values
                    .OrderByDescending(v => v.Probability)
                    .Take(3);
            return string.Join(Environment.NewLine, mostSignificantValues);
        }

        public static Dist<T> Unit(T value)
        {
            return new Dist<T>(new[] { new ProbValue<T>(value, new Probability(1)) });
        }

        private static Dist<S> Bind<S>(Dist<T> distribution, Func<T, Dist<S>> f)
        {
            return new Dist<S>(distribution.values.SelectMany(v => v.Probability * f(v.Value)));
        }

        private static Dist<S> Map<S>(Dist<T> distribution, Func<T, S> f)
        {
            return new Dist<S>(distribution.values.Select(v => ProbValue<T>.Map(f, v)));
        }

        private static Dist<R> Join<S, R>(Dist<T> distribution1, Dist<S> distribution2, Func<T, S, R> f)
        {
            return new Dist<R>(distribution1.SelectMany(v1 => distribution2.Select(v2 => v1.JoinWith(v2, f))));
        }

        private List<ProbValue<T>> Normalize(IEnumerable<ProbValue<T>> values, IEqualityComparer<T> comparer)
        {
            return values
                .Where(v => v.Probability > 0)
                .GroupBy(v => v.Value, comparer)
                .Select(g => new ProbValue<T>(g.Key, new Probability(g.Sum(v => v.Probability))))
                .ToList();
        }
    }
}
