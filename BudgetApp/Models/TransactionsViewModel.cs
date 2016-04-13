using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BudgetApp.Models
{
    public class TransactionsViewModel
    {
        public List<Transaction> Transactions { get; set; }
        public Transaction AddTransaction { get; set; }
    }

}