using System;
using System.Collections.Generic;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;
using FluentAssertions;
using NUnit.Framework;

namespace BudgetApp.Tests.Graphs
{
    [TestFixture]
    public class GraphBuilderTests
    {
        [Test]
        public void TestDrilldownGraph()
        {

            var result = GraphBuilder.TransactionDrilldownGraph(CreateTransactions());

            result.Series[0].Data[0].X.Should().Be(0);
            result.Series[0].Data[0].Y.Should().Be(200);
            result.Series[0].Data[0].Drilldown.Should().Be("fixed");

            result.Series[0].Data[5].X.Should().Be(5);
            result.Series[0].Data[5].Y.Should().Be(100);
            result.Series[0].Data[5].Drilldown.Should().Be("transport");

            result.Drilldown.Series[0].Id.Should().Be("fixed");
            result.Drilldown.Series[0].Data[2].Name.Should().Be("Insurance");
            result.Drilldown.Series[0].Data[2].X.Should().Be(2);
            result.Drilldown.Series[0].Data[2].Y.Should().Be(200);

            result.Drilldown.Series[5].Id.Should().Be("transport");
            result.Drilldown.Series[5].Data[0].Name.Should().Be("Car");
            result.Drilldown.Series[5].Data[0].X.Should().Be(0);
            result.Drilldown.Series[5].Data[0].Y.Should().Be(60);

            result.Drilldown.Series[5].Data[1].Name.Should().Be("CollectiveTransport");
            result.Drilldown.Series[5].Data[1].X.Should().Be(1);
            result.Drilldown.Series[5].Data[1].Y.Should().Be(40);

        }


        [Test]
        public void TestExpensesGraph_No_Transactions()
        {
            var result = GraphBuilder.DailyExpensesGraph(new List<Transaction>());


            result.Series[0].Data.Count.Should().Be(0);

        }

        [Test]
        public void TestExpensesGraph_With_Transactions()
        {
            var result = GraphBuilder.DailyExpensesGraph(CreateTransactions());


            result.Series[0].Data[0].X.Should().Be(1388530800000);
            result.Series[0].Data[0].Y.Should().Be(240.0);

            result.Series[0].Data[1].X.Should().Be(1388617200000);
            result.Series[0].Data[1].Y.Should().Be(60.0);

        }

        private static List<Transaction> CreateTransactions()
        {
            return new List<Transaction>
            {
                new Transaction
                {
                    Amount = 200,
                    Category = Category.Insurance,
                    Date = new DateTime(2014,1,1)
                },

                new Transaction
                {
                    Amount = 40,
                    Category = Category.CollectiveTransport,
                    Date = new DateTime(2014,1,1)
                },
                new Transaction
                {
                    Amount = 60,
                    Category = Category.Car,
                    Date = new DateTime(2014,1,2)
                }
            };
        } 

    }
}
