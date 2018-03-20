namespace Probability
{
    using System;

    /// <summary>
    /// Probabilistic state transitions
    /// </summary>
    public static class Transitions
    {
        /// <summary>
        /// Lets us view a given deterministic transition function as a probabilistic state transition.
        /// </summary>
        /// <typeparam name="T">The state type.</typeparam>
        /// <param name="transition">The transition.</param>
        /// <returns>The state transition, returning a point distribution on the unique state reached.</returns>
        public static Func<T, Dist<T>> Certainly<T>(Func<T, T> transition)
        {
            return v => Distribution.Certainly(transition(v));
        }

        /// <summary>
        /// Sequential composition of state transitions.
        /// </summary>
        /// <typeparam name="T">The state type</typeparam>
        /// <param name="transition1">The initial transition.</param>
        /// <param name="transition2">The second transition.</param>
        /// <returns>The composed state transition.</returns>
        public static Func<T, Dist<T>> Then<T>(this Func<T, Dist<T>> transition1, Func<T, Dist<T>> transition2)
        {
            return v => transition1(v).SelectMany(transition2);
        }

        /// <summary>
        /// Evaluates the state transition on a given initial state.
        /// </summary>
        /// <typeparam name="T">The state type</typeparam>
        /// <param name="transition">The state transition.</param>
        /// <param name="initialState">The initial state.</param>
        /// <returns>The distribution over the states reachable by the transition.</returns>
        public static Dist<T> From<T>(this Func<T, Dist<T>> transition, T initialState)
        {
            return transition(initialState);
        }
    }
}
