namespace Probability.Tests.Samples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// A sample formulation of the birthday problem: the probability that, in a set 
    /// of n randomly chosen people, some pair of them will have the same birthday.
    /// </summary>
    [TestClass]
    public class Birthdays : TestBase
    {
        private const int MAXGROUPSIZE = 100;

        [Sample]
        [TestMethod]
        public void MinimumGroupSizeWhereProbabilityOf2SharedBirthdaysExceedsBound()
        {
            var probOfSharedBirthdays = ComputeProbabilitiesOfSharedBirthdays();

            Assert.AreEqual(15, IndexOfProbabilityExceedingBound(probOfSharedBirthdays, Probability.FromDecimal(.25M)));
            Assert.AreEqual(23, IndexOfProbabilityExceedingBound(probOfSharedBirthdays, Probability.FromDecimal(.5M)));
            Assert.AreEqual(32, IndexOfProbabilityExceedingBound(probOfSharedBirthdays, Probability.FromDecimal(.75M)));
            Assert.AreEqual(57, IndexOfProbabilityExceedingBound(probOfSharedBirthdays, Probability.FromDecimal(.99M)));
            Assert.AreEqual(70, IndexOfProbabilityExceedingBound(probOfSharedBirthdays, Probability.FromDecimal(.999M)));
        }

        private static Dist<int> NextBirthday(int distinctBirthdays)
        {
            var probabilityOfSharedBirthday = Probability.FromDecimal(Math.Min(distinctBirthdays, 365) / 365M);
            return Distribution.OneOf<int>(distinctBirthdays, distinctBirthdays + 1, probabilityOfSharedBirthday);
        }

        private static int IndexOfProbabilityExceedingBound(IEnumerable<Probability> probabilities, Probability lowerBound)
        {
            return probabilities
                .Select((p, k) => new KeyValuePair<int, Probability>(k, p))
                .Where(item => item.Value > lowerBound)
                .Min(item => item.Key);
        }

        private static Probability[] ComputeProbabilitiesOfSharedBirthdays()
        {
            var probOfSharedBirthdays = new Probability[MAXGROUPSIZE];

            var numberOfDistinctBirthdays = Distribution.Certainly<int>(0);
            int groupSize = 0;
            do
            {
                var probabilityOfNoSharedBirthday = numberOfDistinctBirthdays.ProbabilityOf(k => k == groupSize);
                probOfSharedBirthdays[groupSize] = !probabilityOfNoSharedBirthday;
                numberOfDistinctBirthdays = numberOfDistinctBirthdays.SelectMany(NextBirthday);
                groupSize++;
            } while (groupSize < MAXGROUPSIZE);

            return probOfSharedBirthdays;
        }
    }
}
