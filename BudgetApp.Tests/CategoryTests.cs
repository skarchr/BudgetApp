using BudgetApp.Constants;
using BudgetApp.Extensions;
using BudgetApp.Models;
using FluentAssertions;
using NUnit.Framework;

namespace BudgetApp.Tests
{
    [TestFixture]
    public class CategoryTests
    {
        [Test]
        public void TestMethod1()
        {
            var result = CategoryExt.GetMainCategory(Models.Category.Car);
            result.Should().Be("Transport");
        }
    }
}
