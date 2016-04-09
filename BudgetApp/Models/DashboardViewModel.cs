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

        public DateTime First { get; set; }
        
        public Highchart SpcGraph { get; set; }
        public Highchart BurnGraph { get; set; }
        public Highchart PrognosisChart { get; set; }
        public Highchart PrognosisIncomeChart { get; set; }

        public DashboardVariables Variables { get; set; }
        public DateTime Last { get; set; }
    }

    public class DashboardVariables
    {
        public double Income { get; set; }
        public double Expenses { get; set; }
        public double Balance { get; set; }

        public List<Treemap> TreemapChart { get; set; }
        public Highchart TransactionGraph { get; set; }
        public Highchart BalanceGraph { get; set; }
        public Highchart Frequency { get; set; }
        public Highchart CategoryGraph { get; set; }
    }
}