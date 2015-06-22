using System;
using System.Collections.Generic;
using BudgetApp.Extensions;
using BudgetApp.Models;
using FluentAssertions;
using NUnit.Framework;

namespace BudgetApp.Tests
{
    [TestFixture]
    public class SavingHelperTests
    {
        [Test]
        public void TestSavingHelper_No_Transactions()
        {
            var result = SavingHelper.CreateSavingsList(new List<Transaction>(), null);

            result.Count.Should().Be(0);
        }

        [Test]
        public void TestSavingHelper_One_Month_No_goal()
        {
            var trans = new List<Transaction>
            {
                new Transaction
                {
                    Amount = 1000.0,
                    Date = new DateTime(2014,1,1),
                    Category = Category.Saving
                },
                new Transaction
                {
                    Amount = 300.0,
                    Date = new DateTime(2014,1,23),
                    Category = Category.Saving
                },
                new Transaction
                {
                    Amount = 100.0,
                    Date = new DateTime(2014,1,30),
                    Category = Category.OtherFood
                }
            };

            var result = SavingHelper.CreateSavingsList(trans,null);

            result.Count.Should().Be(1);
            result[0].Date.Should().Be(new DateTime(2014, 1, 1));
            result[0].Saved.Should().Be(1300.0);
            result[0].Percentage.Should().Be(0.0);
        }

        [Test]
        public void TestSavingHelper_One_Month_goal()
        {
            var trans = new List<Transaction>
            {
                new Transaction
                {
                    Amount = 1000.0,
                    Date = new DateTime(2014,1,1),
                    Category = Category.Saving
                },
                new Transaction
                {
                    Amount = 1000.0,
                    Date = new DateTime(2014,1,23),
                    Category = Category.Saving
                },
                new Transaction
                {
                    Amount = 100.0,
                    Date = new DateTime(2014,1,30),
                    Category = Category.OtherFood
                }
            };

            var result = SavingHelper.CreateSavingsList(trans, 4000);

            result.Count.Should().Be(1);
            result[0].Date.Should().Be(new DateTime(2014, 1, 1));
            result[0].Saved.Should().Be(2000.0);
            result[0].Percentage.Should().Be(50.0);
        }

    }
}
