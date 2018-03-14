namespace Probability.Tests.UnitTests
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Attribute to mark a <see cref="Microsoft.VisualStudio.TestTools.UnitTesting"/> method as unit test
    /// </summary>
    /// <seealso cref="Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryBaseAttribute" />
    public class UnitTestAttribute : TestCategoryBaseAttribute
    {
        /// <summary>
        /// Gets the test category name.
        /// </summary>
        public override IList<string> TestCategories
        {
            get
            {
                return new List<string>() { "UnitTest" };
            }
        }
    }
}
