namespace Probability.Tests.Samples
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// A sample formulation of the Monty Hall game.
    /// </summary>
    [TestClass]
    public class MontyHall : TestBase
    {
        private enum Doors
        {
            A, B, C
        }

        private struct State
        {
            public Doors? Prize;

            public Doors? Selected;

            public Doors? Opened;

            public override bool Equals(object obj)
            {
                return obj is State && this.Prize.Equals(((State)obj).Prize) && this.Selected.Equals(((State)obj).Selected) && this.Opened.Equals(((State)obj).Opened);
            }

            public override int GetHashCode()
            {
                return this.Prize.GetHashCode() ^ this.Selected.GetHashCode() ^ this.Opened.GetHashCode();
            }

            public override string ToString()
            {
                return $"[Prize:{Prize.ToString() ?? "-"}|Selected:{Selected.ToString() ?? "-"}|Opened:{Opened.ToString() ?? "-"}]";
            }
        }

        [Sample]
        [TestMethod]
        public void AnalyzeStrategy()
        {
            var doors = new[] { Doors.A, Doors.B, Doors.C };
            Func<State, Dist<State>> hidePrize = s =>
                Spread.Uniform(doors)
                .Select(d => new State() { Prize = d, Selected = s.Selected, Opened = s.Opened });
            Func<State, Dist<State>> chooseDoor = s =>
                Spread.Uniform(doors)
                .Select(d => new State() { Prize = s.Prize, Selected = d, Opened = s.Opened });
            Func<State, Dist<State>> revealEmpty = s =>
                Spread.Uniform(doors.Where(d => d != s.Prize).ToArray())
                .Select(d => new State() { Prize = s.Prize, Selected = s.Selected, Opened = d });

            Func<State, Dist<State>> keepSelected = Transitions.Certainly<State>(s => s);
            Func<State, Dist<State>> switchSelected = s =>
                Spread.Uniform(doors.Where(d => !d.In(s.Opened, s.Selected)).ToArray())
                    .Select(d => new State() { Prize = s.Prize, Selected = d, Opened = s.Opened });

            Predicate<State> Win = s => s.Selected == s.Prize;

            var state = Spread.Certainly(new State());
            state = state.SelectMany(hidePrize);
            state = state.SelectMany(chooseDoor);
            state = state.SelectMany(revealEmpty);
            state = state.SelectMany(keepSelected);

            var p = state.ProbabilityOf(Win);

            Func<Func<State, Dist<State>>, Dist<State>> game = strategy =>
                hidePrize
                .Then(chooseDoor)
                .Then(revealEmpty)
                .Then(strategy)
                .From(new State());

            var p1 = game(keepSelected).ProbabilityOf(Win);
            var p2 = game(switchSelected).ProbabilityOf(Win);
            Assert.IsTrue(p1 < p2);

        }

    }
}
