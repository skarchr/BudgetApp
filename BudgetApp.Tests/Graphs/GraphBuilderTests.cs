using System;
using System.Collections.Generic;
using System.Linq;
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

            var result = GraphBuilder.TransactionDrilldownGraph(CreateTransactions(), "NOK");

            result.Currency.Should().Be("NOK");
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
        public void TestDrilldownIncomeGraph()
        {

            var trans = CreateTransactions();

            trans.Add(new Transaction
            {
                Amount = 1000,
                Category = Category.OtherIncome,
                Date = new DateTime(2014,1,1)
            });

            var result = GraphBuilder.IncomeDrilldownGraph(trans, "NOK");

            result.Currency.Should().Be("NOK");

            result.Series[0].Data.Count.Should().Be(1);
            result.Series[0].Data[0].X.Should().Be(0);
            result.Series[0].Data[0].Y.Should().Be(1600);
            result.Series[0].Data[0].Drilldown.Should().Be("income");
            
            result.Drilldown.Series[0].Data[0].Name.Should().Be("OtherIncome");
            result.Drilldown.Series[0].Data[0].X.Should().Be(0);
            result.Drilldown.Series[0].Data[0].Y.Should().Be(1000);

            result.Drilldown.Series[0].Data[1].Name.Should().Be("Salary");
            result.Drilldown.Series[0].Data[1].X.Should().Be(1);
            result.Drilldown.Series[0].Data[1].Y.Should().Be(600);

        }

        [Test]
        public void TestExpensesGraph_No_Transactions()
        {
            var result = GraphBuilder.DailyExpensesGraph(new List<Transaction>(), "NOK");

            result.Currency.Should().Be("NOK");
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

        [Test]
        public void TestOverviewGraph_Annual()
        {

            var model = new List<RangeViewer>
            {
                new RangeViewer
                {
                    Range = Range.Annual,
                    Transactions = CreateTransactions(),
                    StartDate = new DateTime(2014,1,1),
                    EndDate = new DateTime(2014,12,31),
                    Title = "2014",
                    Year = 2014
                }
            };

            var result = GraphBuilder.OverviewGraph(model, new ApplicationUser { Currency = "NOK" });

            result.Categories[0].Should().Be("2014");

            result.Series[0].Name.Should().Be("Expenses");
            result.Series[0].Data[0].X.Should().Be(0);
            result.Series[0].Data[0].Y.Should().Be(300);

            result.Series[1].Name.Should().Be("Income");
            result.Series[1].Data[0].X.Should().Be(0);
            result.Series[1].Data[0].Y.Should().Be(600);
        }

        [Test]
        public void TestOverviewGraph_Monthly()
        {

            var model = new List<RangeViewer>
            {
                new RangeViewer
                {
                    Range = Range.Month,
                    Transactions = CreateTransactions(),
                    StartDate = new DateTime(2014,1,1),
                    EndDate = new DateTime(2014,1,31),
                    Title = "1",
                    Year = 2014
                }
            };

            var result = GraphBuilder.OverviewGraph(model, new ApplicationUser { Currency = "NOK" });

            result.Categories[0].Should().Be("Jan");

            result.Series[0].Name.Should().Be("Expenses");
            result.Series[0].Data[0].X.Should().Be(0);
            result.Series[0].Data[0].Y.Should().Be(300);

            result.Series[1].Name.Should().Be("Income");
            result.Series[1].Data[0].X.Should().Be(0);
            result.Series[1].Data[0].Y.Should().Be(600);

            result.Series[2].Name.Should().Be("Balance");
            result.Series[2].Data[0].X.Should().Be(0);
            result.Series[2].Data[0].Y.Should().Be(300);
        }

        [Test]
        public void TestOverviewGraph_Plotlines_Year()
        {
            var model = new List<RangeViewer>
            {
                new RangeViewer
                {
                    Range = Range.Month,
                    Transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            Date = new DateTime(2013,12,30),
                            Category = Category.Dental
                        }
                    },
                    StartDate = new DateTime(2013,12,1),
                    EndDate = new DateTime(2013,12,31),
                    Title = "12",
                    Year = 2013
                },
                new RangeViewer
                {
                    Range = Range.Month,
                    Transactions = CreateTransactions(),
                    StartDate = new DateTime(2014,1,1),
                    EndDate = new DateTime(2014,1,31),
                    Title = "1",
                    Year = 2014
                }    
            };

            var result = GraphBuilder.OverviewGraph(model, new ApplicationUser{Currency = "NOK"});

            result.PlotLinesX[0].Value.Should().Be(0.5);
            result.PlotLinesX[0].Label.Text.Should().Be("2014");

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
                },                
                new Transaction
                {
                    Amount = 600.0,
                    Category = Category.Salary,
                    Date = new DateTime(2014,2,2)
                }
            };
        } 

    }
}
