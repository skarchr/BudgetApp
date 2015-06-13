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
        public Range Range { get; set; }
        public List<RangeViewer> RangeViewers { get; set; }
        public string Currency { get; set; }
        public string OverviewGraph { get; set; }

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public double TotalExpenses { get; set; }
        public double TotalIncome { get; set; }
    }

}