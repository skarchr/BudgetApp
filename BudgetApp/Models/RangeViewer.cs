using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public enum Range
    {
        Annual, Month, Week
    }

    public class RangeViewer
    {
        public int Year { get; set; }
        public string Title { get; set; }
        public Range Range { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<Transaction> Transactions { get; set; }
        public string Graph { get; set; }
    }
}