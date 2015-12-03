using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class ExcelColumns
    {
        public ExcelColumns()
        {
            DateCol = 0;
            DescriptionCol = 1;
            AmountCols = new List<int> { 2,3,4 };
            DateFormat = "dd.MM.yyyy";
        }

        public HttpPostedFileBase File { get; set; }

        public string DateFormat { get; set; }
        public int DateCol { get; set; }
        public int DescriptionCol { get; set; }
        public List<int> AmountCols { get; set; }
    }
}