using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class HomeViewModel
    {
        public List<Transaction> Transactions { get; set; } 

        public string DailyExpensesGraph { get; set; }
    }
}