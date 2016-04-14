using System;
using System.Collections.Generic;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;
using FluentAssertions;
using NUnit.Framework;

namespace BudgetApp.Tests.Graphs
{
    [TestFixture]
    public class GraphGeneratorTests
    {
        private GraphModel _graphModel;

        [SetUp]
        public void SetUp()
        {
            _graphModel = new GraphModel
            {
                Color = "red",
                Name = "Test",
                Type = "column"
            };
        }

        [Test]
        public void Test_Expenses_Chart_Two_Transactions()
        {
            var date = new DateTime(2013, 12, 1);
            var trans = new List<Transaction>
            {
                new Transaction
                {
                    Category = Category.Appearance,
                    Amount = 500,
                    Date = date
                },
                new Transaction
                {
                    Category = Category.Car,
                    Amount = 1000,
                    Date = new DateTime(2014,4,1)
                }
            };

            var result = GraphGenerator.CreateMonthlyGraph(trans, "column");

            result.Series.Count.Should().Be(2);

        }

        [Test]
        public void Test_Expenses_Chart_Income_Expenses()
        {
            var date = new DateTime(2013, 12, 1);
            var trans = new List<Transaction>
            {
                new Transaction
                {
                    Category = Category.Appearance,
                    Amount = 500,
                    Date = date
                },
                new Transaction
                {
                    Category = Category.Car,
                    Amount = 1000,
                    Date = new DateTime(2014,4,1)
                },
                new Transaction
                {
                    Category = Category.Salary,
                    Amount = 1000,
                    Date = new DateTime(2014,4,1)
                }
            };

            var result = GraphGenerator.CreateMonthlyGraph(trans, "column",true);

            result.Series.Count.Should().Be(2);


        }

        [Test]
        public void Test_Monthly_Series_No_Transactions()
        {
            var result = GraphGenerator.CreateMontlySeries(new List<Transaction>(), null, _graphModel);

            result.Should().Be(null);
        }

        [Test]
        public void Test_Monthly_Series_Two_Different_Transactions()
        {
            var date = new DateTime(2013, 12, 1);
            var trans = new List<Transaction>
            {
                new Transaction
                {
                    Category = Category.Appearance,
                    Amount = 500,
                    Date = date
                },
                new Transaction
                {
                    Category = Category.Car,
                    Amount = 1000,
                    Date = new DateTime(2014,4,1)
                }
            };
            var result = GraphGenerator.CreateMontlySeries(trans, date, _graphModel);

            result.Name.Should().Be(_graphModel.Name);
            result.Data.Count.Should().Be(2);

            result.Data[0].X.Should().Be(0);
            result.Data[1].X.Should().Be(4);

        }

        [Test]
        public void Test_Categories_No_Transaction()
        {
            var trans = new List<Transaction>();

            var plotLines = new List<PlotLine>();
            var result = GraphGenerator.CreateMontlyCategories(trans, out plotLines);

            result.Count.Should().Be(0);

            plotLines.Count.Should().Be(0);

        }

        [Test]
        public void Test_Categories_Two_Transactions()
        {
            var date = new DateTime(2013, 12, 1);
            var trans = new List<Transaction>
            {
                new Transaction
                {
                    Category = Category.Appearance,
                    Amount = 500,
                    Date = date
                },
                new Transaction
                {
                    Category = Category.Car,
                    Amount = 1000,
                    Date = new DateTime(2014,4,1)
                }
            };

            var plotLines = new List<PlotLine>();
            var result = GraphGenerator.CreateMontlyCategories(trans, out plotLines);

            result.Count.Should().Be(5);
            result[0].Should().Be("Dec");
            result[1].Should().Be("Jan");
            result[2].Should().Be("Feb");
            result[3].Should().Be("Mar");
            result[4].Should().Be("Apr");

            plotLines.Count.Should().Be(1);
            plotLines[0].Value.Should().Be(0.5);
            plotLines[0].Label.Text.Should().Be("2014");

        }

    }
}
