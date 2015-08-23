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
    public class TreemapTests
    {
        [Test]
        public void Test_Treemap_No_Transactions()
        {            
            var result = TreemapGenerator.CreateChart(new List<Transaction>());
            result.Count.Should().Be(0);
        }

        [Test]
        public void Test_Treemap_1_Groceries()
        {
            var trans = new List<Transaction>
            {
                new Transaction
                {
                    Amount = 500,
                    Category = Category.Groceries
                }
            };

            var result = TreemapGenerator.CreateChart(trans);

            result.Count.Should().Be(2);
            result[0].Name.Should().Be("Groceries");
            result[0].Id.Should().Be("groceries");
            result[0].Value.Should().Be(500);
            result[0].Parent.Should().Be("food");

            result[1].Name.Should().Be("Food");
            result[1].Id.Should().Be("food");
            result[1].Value.Should().Be(500);
            result[1].Color.Should().Be("#A7DBD8");
        }

        [Test]
        public void Test_Treemap_Multiple_Food_And_Personal()
        {
            var trans = new List<Transaction>
            {
                new Transaction
                {
                    Amount = 900,
                    Category = Category.Restaurant
                },
                new Transaction
                {
                    Amount = 500,
                    Category = Category.Groceries
                },
                new Transaction
                {
                    Amount = 200,
                    Category = Category.Phone
                }
            };

            var result = TreemapGenerator.CreateChart(trans);

            result.Count.Should().Be(5);
            result[0].Name.Should().Be("Groceries");
            result[0].Id.Should().Be("groceries");
            result[0].Value.Should().Be(500);
            result[0].Parent.Should().Be("food");

            result[1].Name.Should().Be("Restaurant");
            result[1].Id.Should().Be("restaurant");
            result[1].Value.Should().Be(900);
            result[1].Parent.Should().Be("food");

            result[2].Name.Should().Be("Food");
            result[2].Id.Should().Be("food");
            result[2].Value.Should().Be(1400);
            result[2].Color.Should().Be("#A7DBD8");

            result[3].Name.Should().Be("Phone");
            result[3].Id.Should().Be("phone");
            result[3].Value.Should().Be(200);
            result[3].Parent.Should().Be("personal");

            result[4].Name.Should().Be("Personal");
            result[4].Id.Should().Be("personal");
            result[4].Value.Should().Be(200);
            result[4].Color.Should().Be("#E0E4CC");
        }

    }
}
