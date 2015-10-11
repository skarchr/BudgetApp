using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;
using FluentAssertions;
using NUnit.Framework;

namespace BudgetApp.Tests.Graphs
{
    [TestFixture]
    public class BurnRateTests
    {
        [Test]
        public void NoExpensesForCurrentMonth()
        {
            var result = BurnRate.CreateChart(new List<Transaction>
            {
                new Transaction
                {
                    Amount = 500,
                    Category = Category.Saving,
                    Date = new DateTime(2014,1,1)
                }
            }, DateTime.Now, null,"NOK");

            result.Series.Count.Should().Be(0);
        }

        [Test]
        public void FirstdayinmonthWithExpense()
        {
            var date1 = new DateTime(2014, 1, 1);

            var result = BurnRate.CreateChart(new List<Transaction>
            {
                new Transaction
                {
                    Amount = 500,
                    Category = Category.Saving,
                    Date = date1
                }
            }, date1, 25000, "NOK");

            result.Series.Count.Should().Be(3);

            result.Series[0].Data.Count.Should().Be(2);
            result.Series[0].Data[0].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1.AddMinutes(-1)));
            result.Series[0].Data[0].Y.Should().Be(25000);

            result.Series[0].Data[1].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1));
            result.Series[0].Data[1].Y.Should().Be(24500);

            result.Series[1].Data[0].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1));
            result.Series[1].Data[0].Y.Should().Be(24500);
        }

        [Test]
        public void OneExpenseForCurrentMonth()
        {
            var date1 = new DateTime(2014,1,1);

            var result = BurnRate.CreateChart(new List<Transaction>
            {
                new Transaction
                {
                    Amount = 500,
                    Category = Category.Saving,
                    Date = date1
                }
            }, date1.AddDays(1), 25000, "NOK");

            result.Series.Count.Should().Be(3);
            result.Series[0].Data[0].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1.AddMinutes(-1)));
            result.Series[0].Data[0].Y.Should().Be(25000);

            result.Series[0].Data[1].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1));
            result.Series[0].Data[1].Y.Should().Be(24500);
        }

        [Test]
        public void ManyExpensesForCurrentMonth()
        {
            var date1 = new DateTime(2014, 1, 1);

            var result = BurnRate.CreateChart(new List<Transaction>
            {
                new Transaction
                {
                    Amount = 500,
                    Category = Category.Saving,
                    Date = date1
                },
                new Transaction
                {
                    Amount = 500,
                    Category = Category.Saving,
                    Date = date1.AddDays(3)
                },
                new Transaction
                {
                    Amount = 1500,
                    Category = Category.Salary,
                    Date = date1.AddDays(3)
                },
                new Transaction
                {
                    Amount = 500,
                    Category = Category.Saving,
                    Date = date1.AddDays(5)
                }
            }, date1.AddDays(5), 25000, "NOK");

            result.Series.Count.Should().Be(3);
            result.Series[0].Data.Count.Should().Be(7);

            result.Series[0].Data[0].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1.AddMinutes(-1)));
            result.Series[0].Data[0].Y.Should().Be(25000);

            result.Series[0].Data[1].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1));
            result.Series[0].Data[1].Y.Should().Be(24500);

            result.Series[0].Data[2].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1.AddDays(1)));
            result.Series[0].Data[2].Y.Should().Be(24500);

            result.Series[0].Data[3].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1.AddDays(2)));
            result.Series[0].Data[3].Y.Should().Be(24500);

            result.Series[0].Data[4].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1.AddDays(3)));
            result.Series[0].Data[4].Y.Should().Be(24000);

            result.Series[0].Data[5].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1.AddDays(4)));
            result.Series[0].Data[5].Y.Should().Be(24000);

            result.Series[0].Data[6].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1.AddDays(5)));
            result.Series[0].Data[6].Y.Should().Be(23500);

            result.Series[1].Data[0].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1.AddDays(5)));
            result.Series[1].Data[0].Y.Should().Be(23500);
            result.Series[1].Data[25].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1.AddDays(30)));
            result.Series[1].Data[25].Y.Should().Be(23500);

            result.Series[2].Data[0].X.Should().Be(GraphBuilder.ConvertDateToMilliSeconds(date1.AddDays(5)));
            result.Series[2].Data[0].Y.Should().Be(23500);
            result.Series[2].Data.Count.Should().Be(1);
        }

    }
}
