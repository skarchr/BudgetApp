using System;
using System.Collections.Generic;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;
using FluentAssertions;
using NUnit.Framework;

namespace BudgetApp.Tests.Graphs
{
    [TestFixture]
    public class PrognosisGraphTests
    {
        [Test]
        public void No_Transactions()
        {
            var result = Prognosis.CreateChart(new List<Transaction>(), "NOK");

            result.Series.Count.Should().Be(0);
        }

        [Test]
        public void One_Year()
        {
            var result = Prognosis.CreateChart(CreateTransactions(), "NOK");

            result.Series.Count.Should().Be(1);

            result.Series[0].Name.Should().Be("2014");
            result.Series[0].Data[0].X.Should().Be(0);
            result.Series[0].Data[0].Y.Should().Be(300);

            result.Series[0].Data[1].X.Should().Be(1);
            result.Series[0].Data[1].Y.Should().Be(800);

            result.Series[0].Data[2].X.Should().Be(2);
            result.Series[0].Data[2].Y.Should().Be(800);

            result.Series[0].Data[3].X.Should().Be(3);
            result.Series[0].Data[3].Y.Should().Be(1000);            
        }

        private List<Transaction> CreateTransactions()
        {
            return new List<Transaction>
            {
                new Transaction
                {
                    Date = new DateTime(2014,1,1),
                    Amount = 100,
                    Category = Category.Restaurant
                },
                new Transaction
                {
                    Date = new DateTime(2014,1,2),
                    Amount = 200,
                    Category = Category.OtherFood
                },
                new Transaction
                {
                    Date = new DateTime(2014,2,2),
                    Amount = 500,
                    Category = Category.Rent
                },
                new Transaction
                {
                    Date = new DateTime(2014,4,2),
                    Amount = 200,
                    Category = Category.Travel
                }
            };
        }

        [Test]
        public void This_Year_Expenses()
        {

            var trans = new List<Transaction>
            {
                new Transaction
                {
                    Date = new DateTime(DateTime.Now.Year,1,1),
                    Amount = 1,
                    Category = Category.Travel
                }
            };

            var result = Prognosis.CreateChart(trans, "NOK");

            result.Series.Count.Should().Be(2);            

            result.Series[1].Name.Should().Be(DateTime.Now.Year.ToString());
            result.Series[1].Data[0].X.Should().Be(0);
            result.Series[1].Data[0].Y.Should().Be(1);

            result.Series[0].Name.Should().Be("Expenses (" + DateTime.Now.Year + ")");
            result.Series[0].Data[0].X.Should().Be(0);
            result.Series[0].Data[0].Y.Should().Be(1);

            result.Series[0].Data[1].X.Should().Be(1);
            result.Series[0].Data[1].Y.Should().Be(0);

        }

        [Test]
        public void This_Year_Income()
        {

            var month = DateTime.Now.Month - 1;

            var trans = new List<Transaction>
            {
                new Transaction
                {
                    Date = new DateTime(DateTime.Now.Year,DateTime.Now.Month,1),
                    Amount = 1,
                    Category = Category.Salary
                }
            };

            var result = Prognosis.CreateChart(trans, "NOK", true);

            result.Series.Count.Should().Be(2);

            result.Series[1].Name.Should().Be(DateTime.Now.Year.ToString());
            result.Series[1].Data[month].X.Should().Be(month);
            result.Series[1].Data[month].Y.Should().Be(1);

            result.Series[0].Name.Should().Be("Income (" + DateTime.Now.Year + ")");
            result.Series[0].Data[month].X.Should().Be(month);
            result.Series[0].Data[month].Y.Should().Be(1);
            result.Series[0].Data[month].Color.Should().Be("#0094f4");

        }

        [Test]
        public void Two_Years_Oldest_Disabled()
        {

            var trans = CreateTransactions();

            trans.Add(new Transaction
            {
                Amount = 200,
                Category = Category.Travel,
                Date = new DateTime(DateTime.Now.Year,1,1)
            });

            var result = Prognosis.CreateChart(trans, "NOK");

            result.Series.Count.Should().Be(3);

            result.Series[0].Visible.Should().Be(true);
            result.Series[1].Visible.Should().Be(false);
            result.Series[2].Visible.Should().Be(true);

        }

    }
}
