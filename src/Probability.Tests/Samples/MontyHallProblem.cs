namespace Probability.Tests.Samples
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// A sample formulation of the Monty Hall problem.
    /// </summary>
    [TestClass]
    public class MontyHallProblem : TestBase
    {
        private enum Door
        {
            A, B, C
        }

        private Dist<State> Play(Func<State, Dist<State>> strategy)
        {
            return hidePrize
                .Then(chooseDoor)
                .Then(revealEmptyDoor)
                .Then(strategy)
                .From(State.Initial);
        }

        private static Func<State, Dist<State>> hidePrize = (State s) =>
              Distribution.Uniform(Doors)
                 .Select(d => s.HidePrize(d));

        private static Func<State, Dist<State>> chooseDoor = (State s) =>
            Distribution.Uniform(Doors)
                .Select(d => s.Choose(d));

        private static Func<State, Dist<State>> revealEmptyDoor = (State s) =>
             Distribution.Uniform(Doors.Where(d => d != s.Prize).ToArray())
                 .Select(d => s.Open(d));

        private static Func<State, Dist<State>> keepSelected = (State s) =>
            Distribution.Certainly(s);

        private static Func<State, Dist<State>> switchSelected = (State s) =>
                Distribution.Uniform(Doors.Where(d => !d.In(s.Opened, s.Chosen)).ToArray())
                    .Select(d => s.Choose(d));

        private static Door[] Doors
        {
            get { return new[] { Door.A, Door.B, Door.C }; }
        }

        [Sample]
        [TestMethod]
        public void MontyHallStrategies()
        {
            var probStay = Play(keepSelected).ProbabilityOf(s => s.IsWinningState);
            AssertEqualProbabilities(.33, probStay, .01);

            var probSwitch = Play(switchSelected).ProbabilityOf(s => s.IsWinningState);
            AssertEqualProbabilities(.5, probSwitch, .001);
        }

        private struct State
        {
            public Door? Prize;

            public Door? Chosen;

            public Door? Opened;

            public static State Initial
            {
                get { return new State(); }
            }

            public bool IsWinningState
            {
                get { return this.Prize.HasValue && this.Chosen == this.Prize; }
            }

            public State HidePrize(Door d)
            {
                return new State() { Prize = d, Opened = this.Opened, Chosen = this.Chosen };
            }

            public State Choose(Door d)
            {
                return new State() { Prize = this.Prize, Opened = this.Opened, Chosen = d };
            }

            public State Open(Door d)
            {
                return new State() { Prize = this.Prize, Opened = d, Chosen = this.Chosen };
            }

            public override bool Equals(object obj)
            {
                return obj is State && this.Prize.Equals(((State)obj).Prize) && this.Chosen.Equals(((State)obj).Chosen) && this.Opened.Equals(((State)obj).Opened);
            }

            public override int GetHashCode()
            {
                return this.Prize.GetHashCode() ^ this.Chosen.GetHashCode() ^ this.Opened.GetHashCode();
            }

            public override string ToString()
            {
                return $"[Prize:{Prize.ToString() ?? "-"}|Chosen:{Chosen.ToString() ?? "-"}|Opened:{Opened.ToString() ?? "-"}]";
            }
        }
    }
}
