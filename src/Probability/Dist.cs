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
        public static readonly Dist<T> Impossible = Dist<T>.Zero();

        private List<PValue<T>> values;

        private Dist(IEnumerable<PValue<T>> values)
        {
            if (values.Sum(v => v.Probability.ToDecimal()) > 1M)
            {
                throw new ArgumentOutOfRangeException(nameof(values), "Given probabilities exceed 1.0");
            }

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

        public static Dist<T> OneOf(T value1, T value2)
        {
            return Dist<T>.OneOf(value1, value2, new Probability(0.5M));
        }

        public static Dist<T> OneOf(T value1, T value2, Probability bias)
        {
            return new Dist<T>(new[] { new PValue<T>(value1, bias), new PValue<T>(value2, new Probability(1M - (decimal)bias)) });
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

        public Dist<S> Select<S>(Func<T,S> selector)
        {
            return Dist<T>.Map<S>(this, selector);
        }

        public Dist<S> SelectMany<S>(Func<T, Dist<S>> selector)
        {
            return SelectMany(selector, (v, u) => u);
        }

        public Dist<R> SelectMany<S,R>(Func<T, Dist<S>> selector, Func<T, S, R> resultSelector)
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

        public Dist<Tuple<T,S>> Prod<S>(Dist<S> distribution)
        {
            return this.JoinWith(distribution, (v1, v2) => Tuple.Create(v1, v2));
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

        public Dist<R> JoinWith<S,R>(Dist<S> distribution, Func<T,S,R> f)
        {
            return Dist<T>.JoinWith<S,R>(this, distribution, f);
        }
       
        private static Dist<T> Unit(T value)
        {
            return new Dist<T>(new[] { new PValue<T>(value, new Probability(1)) });
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

        private static Dist<R> JoinWith<S,R>(Dist<T> distribution1, Dist<S> distribution2, Func<T, S, R> f)
        {
            return new Dist<R>(distribution1.SelectMany(v1 => distribution2.Select(v2 => PValue<T>.JoinWith(v1, v2, f))));
        }

        private List<PValue<T>> Normalize(IEnumerable<PValue<T>> values)
        {
            return values
                .Where(v => v.Probability > 0)
                .GroupBy(v => v.Value)
                .Select(g => new PValue<T>(g.Key, new Probability(g.Sum(v => v.Probability))))
                .ToList();
        }
    }
}
