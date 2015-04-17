using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            result.Should().Be("[[\"Fixed\",0],[\"Food\",0],[\"Personal\",0],[\"Shelter\",40],[\"Transportation\",60]]");
        }

    }
}
