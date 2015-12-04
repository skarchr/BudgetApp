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
            DescriptionCol = 2;
            AmountCols = new List<int> { 3 };
            DateFormat = "dd.MM.yyyy";
        }

        public HttpPostedFileBase File { get; set; }

        public string DateFormat { get; set; }
        public int DateCol { get; set; }
        public int DescriptionCol { get; set; }
        public List<int> AmountCols { get; set; }
    }
}