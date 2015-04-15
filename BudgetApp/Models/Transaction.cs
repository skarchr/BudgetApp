using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string UserName { get; set; }

        public double Amount { get; set; }
        public Category? Category { get; set; }
        public string Description { get; set; }
        
        public DateTime TransactionDate { get; set; }
        public DateTime Created { get; set; }
    }
}