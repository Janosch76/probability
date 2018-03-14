namespace Probability
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Probabilistic state transitions
    /// </summary>
    public static class TransitionExtensions
    {
        public static Func<T, Dist<T>> Then<T>(this Func<T, Dist<T>> transition1, Func<T, Dist<T>> transition2)
        {
            return v => transition1(v).SelectMany(transition2);
        }

        public static Dist<T> From<T>(this Func<T, Dist<T>> transition, T initialState)
        {
            return transition(initialState);
        }
    }
}
