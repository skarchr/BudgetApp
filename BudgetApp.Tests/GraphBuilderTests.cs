using System.Collections.Generic;
using BudgetApp.Extensions;
using BudgetApp.Models;
using FluentAssertions;
using NUnit.Framework;

namespace BudgetApp.Tests
{
    [TestFixture]
    public class GraphBuilderTests
    {
        [Test]
        public void TestDrilldownGraph()
        {
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    Amount = 200,
                    Category = Category.Insurance
                },

                new Transaction
                {
                    Amount = 40,
                    Category = Category.CollectiveTransport
                },
                new Transaction
                {
                    Amount = 60,
                    Category = Category.Car
                }
            };

            var result = GraphBuilder.TransactionDrilldownGraph(transactions);

            result.Series[0].Data[0].X.Should().Be(0);
            result.Series[0].Data[0].Y.Should().Be(200);
            result.Series[0].Data[0].Drilldown.Should().Be("fixed");

            result.Series[0].Data[4].X.Should().Be(4);
            result.Series[0].Data[4].Y.Should().Be(100);
            result.Series[0].Data[4].Drilldown.Should().Be("transport");

            result.Drilldown.Series[0].Id.Should().Be("fixed");
            result.Drilldown.Series[0].Data[2].Name.Should().Be("Insurance");
            result.Drilldown.Series[0].Data[2].X.Should().Be(2);
            result.Drilldown.Series[0].Data[2].Y.Should().Be(200);

            result.Drilldown.Series[4].Id.Should().Be("transport");
            result.Drilldown.Series[4].Data[0].Name.Should().Be("Car");
            result.Drilldown.Series[4].Data[0].X.Should().Be(0);
            result.Drilldown.Series[4].Data[0].Y.Should().Be(60);

            result.Drilldown.Series[4].Data[1].Name.Should().Be("CollectiveTransport");
            result.Drilldown.Series[4].Data[1].X.Should().Be(1);
            result.Drilldown.Series[4].Data[1].Y.Should().Be(40);

        }


        [Test]
        public void TestGraph()
        {
            var transactions = new List<Transaction>
            {
                new Transaction
                {
                    Amount = 40,
                    Category = Category.Mortgage
                },
                new Transaction
                {
                    Amount = 60,
                    Category = Category.Car
                }
            };

            var result = GraphBuilder.TransactionGraph(transactions);

            result.Should().Be("[[\"Fixed\",0],[\"Food\",0],[\"Personal\",0],[\"Shelter\",40],[\"Transport\",60]]");
        }

    }
}
