using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Extensions;
using BudgetApp.Extensions.Graphs;

namespace BudgetApp.Models
{
    public class DashboardViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public double Income { get; set; }
        public double Expenses { get; set; }
        public double Balance { get; set; }

        public List<Treemap> TreemapChart { get; set; }
        public Highchart TransactionGraph { get; set; }
        public Highchart BalanceGraph { get; set; }
    }
}