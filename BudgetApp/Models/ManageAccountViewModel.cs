using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class ManageAccountViewModel
    {
        public string UserName { get; set; }

        public string Currency { get; set; }

        public string Country { get; set; }

        public int AccessFailedCount { get; set; }
    }

}