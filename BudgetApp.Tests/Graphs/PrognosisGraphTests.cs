﻿using System;
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

            var trans = new List<Transaction>
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

            var result = Prognosis.CreateChart(trans, "NOK");

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

        [Test]
        public void Two_Year()
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

            result.Series[0].Name.Should().Be("Predicted (" + DateTime.Now.Year + ")");
            result.Series[0].Data[0].X.Should().Be(0);
            result.Series[0].Data[0].Y.Should().Be(1);

            result.Series[0].Data[1].X.Should().Be(1);
            result.Series[0].Data[1].Y.Should().Be(2);

            result.Series[0].Data[11].X.Should().Be(11);
            result.Series[0].Data[11].Y.Should().Be(12);

        }

    }
}