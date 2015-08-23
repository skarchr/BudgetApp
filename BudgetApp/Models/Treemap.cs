using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class Treemap
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public string Parent { get; set; }

        public string Color { get; set; }

    }
}