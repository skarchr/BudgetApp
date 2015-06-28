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
            var result = CategoryExt.GetMainCategory(Category.Car);
            result.Should().Be("Transport");
        }

        [Test]
        public void TestCamelCaseToNormal()
        {
            var result = CategoryExt.CamelCaseToNormal(Category.OtherShelter.ToString());

            result.Should().Be("Other Shelter");
        }
    }
}
