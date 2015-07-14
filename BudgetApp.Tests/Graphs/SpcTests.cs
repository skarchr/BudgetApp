using System;
using System.Collections.Generic;
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
        public void Last_Weeks_Last_Days()
        {
            var trans = createTransactions();

            var result = Spc.CreateChart(trans, "NOK");

            result.Series[0].Data.Count.Should().Be(4);

            result.Series[0].Data[0].Y.Should().Be(100);
            result.Series[0].Data[0].X.Should().Be(0);

            result.Series[0].Data[1].Y.Should().Be(700);
            result.Series[0].Data[1].X.Should().Be(1);

            result.Series[0].Data[2].Y.Should().Be(0);
            result.Series[0].Data[2].X.Should().Be(2);

            result.Series[0].Data[3].Y.Should().Be(150);
            result.Series[0].Data[3].X.Should().Be(3);

            result.PlotLinesY[0].Value.Should().Be(125);
            Math.Round(result.PlotLinesY[1].Value, 1).Should().Be(1068.7);
        }

        [Test]
        public void Last_Weeks()
        {
            var trans = createTransactions();

            trans.Add(new Transaction {Amount = 200, Date = new DateTime(2014, 1, 6), Category = Category.Restaurant});

            var result = Spc.CreateChart(trans, "NOK", Range.Week);

            result.Series[0].Data.Count.Should().Be(2);

            result.Series[0].Data[0].Y.Should().Be(950);
            result.Series[0].Data[0].X.Should().Be(0);
            result.Series[0].Data[0].Year.Should().Be(2014);
            result.Categories[0].Should().Be("1");

            result.Series[0].Data[1].Y.Should().Be(200);
            result.Series[0].Data[1].X.Should().Be(1);
            result.Categories[1].Should().Be("2");
        }

        [Test]
        public void Last_Months()
        {
            var trans = createTransactions();

            trans.Add(new Transaction { Amount = 200, Date = new DateTime(2013, 12, 31), Category = Category.Restaurant });

            var result = Spc.CreateChart(trans, "NOK", Range.Month);

            result.Series[0].Data.Count.Should().Be(2);

            result.Series[0].Data[0].Y.Should().Be(200);
            result.Series[0].Data[0].X.Should().Be(0);
            result.Series[0].Data[0].Year.Should().Be(2013);
            result.Categories[0].Should().Be("Dec");

            result.Series[0].Data[1].Y.Should().Be(950);
            result.Series[0].Data[1].X.Should().Be(1);
            result.Categories[1].Should().Be("Jan");
            result.Series[0].Data[1].Year.Should().Be(2014);

            result.PlotLinesX[0].Value.Should().Be(0.5);
        }


        private List<Transaction> createTransactions()
        {
            return new List<Transaction>
            {
                new Transaction
                {
                    Amount = 100,
                    Date = new DateTime(2014,1,1),
                    Category = Category.Travel
                },
                new Transaction
                {
                    Amount = 200,
                    Date = new DateTime(2014,1,2),
                    Category = Category.Travel
                },
                new Transaction
                {
                    Amount = 500,
                    Date = new DateTime(2014,1,2),
                    Category = Category.Travel
                },
                new Transaction
                {
                    Amount = 150,
                    Date = new DateTime(2014,1,4),
                    Category = Category.Travel
                },
                new Transaction
                {
                    Amount = 1500,
                    Date = new DateTime(2014,1,3),
                    Category = Category.Salary
                }
            };
        } 

    }
}
