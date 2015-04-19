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
        public int TransactionsDisplayed { get; set; }
        public int TotalTransactions { get; set; }
        public Filter Filter { get; set; }
        public List<RangeViewer> RangeViewers { get; set; } 
    }

    public class Filter
    {
        public Range Range { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}