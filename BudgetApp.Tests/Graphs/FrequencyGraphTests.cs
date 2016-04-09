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
    public class FrequencyGraphTests
    {
        [Test]
        public void No_Transactions()
        {
            var result = Frequency.CreateGraph(new List<Transaction>());

            result.Series.Count.Should().Be(0);
        }

        [Test]
        public void With_One_Transaction()
        {
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    Category = Category.Rent,
                    Amount = 500
                }
            };

            var result = Frequency.CreateGraph(transactions);

            result.Series.Count.Should().Be(1);
            result.Series[0].Data.Count.Should().Be(1);
            result.Series[0].Data[0].X.Should().Be(0);
            result.Series[0].Data[0].Y.Should().Be(500.0);
            result.Series[0].Data[0].Z.Should().Be(1);
            result.Series[0].Data[0].Color.Should().Be("#F38630");

            result.XAxis[0].Categories[0].Should().Be("Shelter");
        }

        [Test]
        public void With_Mulitple_Transactions()
        {
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    Category = Category.Rent,
                    Amount = 500
                },
                new Transaction
                {
                    Category = Category.ATM,
                    Amount = 100
                },
                new Transaction
                {
                    Category = Category.ATM,
                    Amount = 200
                }
            };

            var result = Frequency.CreateGraph(transactions);

            result.Series.Count.Should().Be(1);
            result.Series[0].Data.Count.Should().Be(2);

            result.Series[0].Data[0].X.Should().Be(0);
            result.Series[0].Data[0].Y.Should().Be(300.0);
            result.Series[0].Data[0].Z.Should().Be(2);
            result.Series[0].Data[0].Color.Should().Be("#48DDb8");

            result.XAxis[0].Categories[0].Should().Be("ATM");

            result.Series[0].Data[1].X.Should().Be(1);
            result.Series[0].Data[1].Y.Should().Be(500.0);
            result.Series[0].Data[1].Z.Should().Be(1);
            result.Series[0].Data[1].Color.Should().Be("#F38630");

            result.XAxis[0].Categories[1].Should().Be("Shelter");
        }

    }
}
