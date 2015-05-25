using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}