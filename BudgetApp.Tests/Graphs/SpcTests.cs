using System;
using System.Collections.Generic;
using BudgetApp.Extensions;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;
using FluentAssertions;
using NUnit.Framework;

namespace BudgetApp.Tests.Graphs
{
    [TestFixture]
    public class SpcTests
    {
        //TODO make tests for median, ucl and lcl

        [Test]
        public void Last_Weeks_No_Transactions()
        {
            var result = Spc.CreateChart(new List<Transaction>(), "NOK");

            result.Series.Count.Should().Be(0);
        }

        [Test]
        public void Less_Than_140_Days()
        {
            var trans = createTransactions(new DateTime(2014,1,1), new DateTime(2014,1,4));
            var result = Spc.CreateChart(trans, "NOK");
            result.Series[0].Data.Count.Should().Be(25);

            result.Series[0].Data[0].Y.Should().Be(0);
            result.Series[0].Data[21].Y.Should().Be(100);
            result.Series[0].Data[24].Y.Should().Be(1500);
            result.XAxis[0].Categories[24].Should().Be("4.jan");
        }

        [Test]
        public void More_Than_175_Days()
        {
            var date1 = new DateTime(2014, 1, 1);

            var trans = createTransactions(date1, date1.AddDays(175));
            var result = Spc.CreateChart(trans, "NOK", ChartRange.Weekly);

            result.Series[0].Data.Count.Should().Be(25);

            result.Series[0].Data[0].Y.Should().Be(0);
            result.Categories[0].Should().Be(DateHelper.GetWeekNumber(date1.AddDays(7)).ToString());

            result.Series[0].Data[24].Y.Should().Be(1500);
            result.Categories[24].Should().Be(DateHelper.GetWeekNumber(date1.AddDays(175)).ToString());

        }

        [Test]
        public void More_Than_700_Days()
        {
            var date1 = DateTime.Now;

            var trans = createTransactions(date1.AddMonths(-25), date1);
            var result = Spc.CreateChart(trans, "NOK", ChartRange.Monthly);

            result.Series[0].Data.Count.Should().Be(25);

            result.Series[0].Data[0].Y.Should().Be(100);
            result.Categories[0].Should().Be(DateHelper.GetMonthText(date1.AddMonths(-25), true));

            result.Series[0].Data[24].Y.Should().Be(0);
            result.Categories[24].Should().Be(DateHelper.GetMonthText(date1.AddMonths(-1), true));

        }

        private List<Transaction> createTransactions(DateTime start, DateTime end)
        {
            return new List<Transaction>
            {
                new Transaction
                {
                    Amount = 100,
                    Date = start,
                    Category = Category.Travel
                },
                new Transaction
                {
                    Amount = 500,
                    Date = end,
                    Category = Category.Dental
                },
                new Transaction
                {
                    Amount = 1000,
                    Date = end,
                    Category = Category.Fuel
                }
            };
        } 

    }
}
