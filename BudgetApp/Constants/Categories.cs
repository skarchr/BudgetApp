﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Models;

namespace BudgetApp.Constants
{
    public static class Categories
    {
        public static readonly Dictionary<string, List<Category>> Grouped = new Dictionary<string, List<Category>>
            {
                {
                    "Fixed", new List<Category>
                    {
                        Category.DebtReduction,
                        Category.Dental,
                        Category.Insurance,
                        Category.Medical,
                        Category.OtherFixed
                    }
                },
                {
                    "Food", new List<Category>
                    {
                        Category.Groceries,
                        Category.Restaurant,
                        Category.Treats,
                        Category.OtherFood
                    }
                },
                {
                    "Income", new List<Category>
                    {
                        Category.Salary,
                        Category.OtherIncome
                    }
                },
                {
                    "Personal", new List<Category>
                    {
                        Category.Appearance,
                        Category.Entertainment,
                        Category.Gifts,
                        Category.Hobby,
                        Category.Phone,
                        Category.Subscriptions,
                        Category.Travel,

                        Category.OtherPersonal
                    }
                },
                {
                    "Shelter", new List<Category>
                    {
                        Category.Furniture,
                        Category.Interior,
                        Category.Mortgage,
                        Category.Rent,
                        Category.Utilities,

                        Category.OtherShelter
                    }
                },
                {
                    "Transportation", new List<Category>
                    {
                        Category.Car,
                        Category.Fuel,
                        Category.Maintenance,
                        Category.Parts,
                        Category.ParkingFees,
                        Category.Repairs,

                        Category.CollectiveTransport,

                        Category.OtherTransportation
                    }
                }
            };
    }
}