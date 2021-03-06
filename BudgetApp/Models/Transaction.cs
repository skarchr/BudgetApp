﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BudgetApp.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string UserName { get; set; }

        public double Amount { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Category? Category { get; set; }
        public string Description { get; set; }

        public DateTime Date { get; set; }
        public DateTime Created { get; set; }
        public bool Import { get; set; }

        public string CategoryText
        {
            get
            {
                return Category != null ? CategoryExt.CamelCaseToNormal(Category.Value.ToString()) : null;
            }
        }

        public string MainCategory
        {
            get
            {
                return Category != null ? CategoryExt.GetMainCategory(Category.Value) : null;
            }
        }

        public string Color
        {
            get
            {
                return HighchartUtilities.Colors[Extensions.CategoryExt.GetCategoryColor(Category)];
            }
        }

        public string CategoryType
        {
            get
            {
                return Category != null ? CategoryExt.GetMainCategory(Category.Value) == "Income" ? "Income" : "Expenses" : null;
            }
        }
    }
}