using System;
using System.Collections.Generic;
using BudgetApp.Constants;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;
using FluentAssertions;
using NUnit.Framework;

namespace BudgetApp.Tests.Graphs
{
    [TestFixture]
    public class BalanceGraphTests
    {
        [Test]
        public void No_Transactions()
        {
            var result = Balance.CreateChart(new List<Transaction>());

            result.Series.Count.Should().Be(0);
        }

        [Test]
        public void One_Month_Returns_One_Data()
        {
            var date = new DateTime(2014, 1, 1);

            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    Amount = 500,
                    Category = Category.Appearance,
                    Date = date
                },
                new Transaction
                {
                    Amount = 140,
                    Category = Category.Phone,
                    Date = date
                },
                new Transaction
                {
                    Amount = 300,
                    Category = Category.Phone,
                    Date = date.AddDays(1)
                },
                new Transaction
                {
                    Amount = 1000,
                    Category = Category.Salary,
                    Date = new DateTime(2015,1,1)
                }
            };

            var result = Balance.CreateChart(transactions);

            result.Series.Count.Should().Be(2);
            result.Series[1].Data.Count.Should().Be(2);
            result.Series[1].Data[0].X.Should().Be(HighchartUtilities.ConvertToMilliseconds(date));
            result.Series[1].Data[0].Y.Should().Be(640);

            result.Series[0].Data[0].X.Should().Be(HighchartUtilities.ConvertToMilliseconds(new DateTime(2015, 1, 1)));
            result.Series[0].Data[0].Y.Should().Be(1000);
        }
    }
}
