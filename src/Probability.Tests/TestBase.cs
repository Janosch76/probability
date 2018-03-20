namespace Probability.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Base class for test classes, providing some assertions and other utility methods.
    /// </summary>
    [TestClass]
    public class TestBase
    {
        /// <summary>
        /// Asserts that a specified code block throws an exception.
        /// </summary>
        /// <typeparam name="T">The exception type.</typeparam>
        /// <param name="action">The code block.</param>
        /// <returns>The exception that was thrown.</returns>
        public static T AssertThrows<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T e)
            {
                return e;
            }
            catch (Exception e)
            {
                Assert.Fail(
                    "Expected exception of type {0}, but an exception of type {1} was thrown.",
                    typeof(T).Name,
                    e.GetType().Name);
                return null;
            }

            Assert.Fail(
                "Expected exception of type {0}, but no exception was thrown.",
                typeof(T).Name);
            return null;
        }

        public static void AssertEqualProbabilities(Probability expected, Probability actual)
        {
            TestBase.AssertEqualProbabilities(expected.ToDecimal(), actual);
        }

        public static void AssertEqualProbabilities(decimal expected, Probability actual)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void AssertEqualProbabilities(Probability expected, Probability actual, double delta)
        {
            TestBase.AssertEqualProbabilities(expected.ToDouble(), actual, delta);
        }

        public static void AssertEqualProbabilities(double expected, Probability actual)
        {
            AssertEqualProbabilities(expected, actual, 0);
        }

        public static void AssertEqualProbabilities(double expected, Probability actual, double delta)
        {
            Assert.AreEqual(expected, actual.ToDouble(), delta);
        }
    }
}
