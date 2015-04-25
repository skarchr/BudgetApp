using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class ImportViewModel
    {
        public List<Transaction> Transactions { get; set; }
        public Mapping Mapping { get; set; }
    }
}