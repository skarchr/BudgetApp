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

            result.Series.Count.Should().Be(1);
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

            result.Series.Count.Should().Be(1);


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

        [Test]
        public void BadTrend()
        {
            var startDate = new DateTime(2014, 1, 1);

            var trans = CreateBadTrends(startDate);

            var result = Spc.CreateChart(trans, "NOK");
            result.Series[0].Data.Count.Should().Be(25);

            result.Series[0].Data[24].Y.Should().Be(2100);
            result.XAxis[0].Categories[24].Should().Be("1.jan");

            result.Series.Count.Should().Be(2);
            result.Series[1].Data.Count.Should().Be(8);
            result.Series[1].Data[7].Y.Should().Be(2100);
            result.Series[1].Data[7].X.Should().Be(24);
        }

        [Test]
        public void BadTrend_Multiple()
        {
            var startDate = new DateTime(2014, 1, 1);

            var trans = CreateBadTrends(startDate);

            trans.AddRange(CreateBadTrends(startDate.AddDays(-9)));

            var result = Spc.CreateChart(trans, "NOK");
            result.Series[0].Data.Count.Should().Be(25);

            result.Series[0].Data[24].Y.Should().Be(2100);
            result.XAxis[0].Categories[24].Should().Be("1.jan");

            result.Series.Count.Should().Be(2);
            result.Series[1].Data.Count.Should().Be(16);

            result.Series[1].Data[15].Y.Should().Be(2100);
            result.Series[1].Data[15].X.Should().Be(24);
        }

        private static List<Transaction> CreateBadTrends(DateTime startDate)
        {
            var trans = new List<Transaction>
            {
                new Transaction
                {
                    Amount = 1500,
                    Date = startDate.AddDays(-7),
                    Category = Category.Appearance
                },
                new Transaction
                {
                    Amount = 1600,
                    Date = startDate.AddDays(-6),
                    Category = Category.Appearance
                },
                new Transaction
                {
                    Amount = 1700,
                    Date = startDate.AddDays(-5),
                    Category = Category.Appearance
                },
                new Transaction
                {
                    Amount = 1700,
                    Date = startDate.AddDays(-4),
                    Category = Category.Appearance
                },
                new Transaction
                {
                    Amount = 1800,
                    Date = startDate.AddDays(-3),
                    Category = Category.Appearance
                },
                new Transaction
                {
                    Amount = 1900,
                    Date = startDate.AddDays(-2),
                    Category = Category.Appearance
                },
                new Transaction
                {
                    Amount = 2000,
                    Date = startDate.AddDays(-1),
                    Category = Category.Appearance
                },
                new Transaction
                {
                    Amount = 2100,
                    Date = startDate,
                    Category = Category.Appearance
                }
            };
            return trans;
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
