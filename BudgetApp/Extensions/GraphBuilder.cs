using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions
{
    public class GraphBuilder
    {
        public static string TransactionGraph(List<Transaction> transactions)
        {

            var fix = SumCategory(transactions, "Fixed");
            var food = SumCategory(transactions, "Food");
            var personal = SumCategory(transactions, "Personal");
            var shelter = SumCategory(transactions, "Shelter");
            var trans = SumCategory(transactions, "Transportation");


            return String.Format("[[\"Fixed\",{0}],[\"Food\",{1}],[\"Personal\",{2}],[\"Shelter\",{3}],[\"Transportation\",{4}]]", fix, food, personal, shelter, trans);
        }

        private static string SumCategory(IEnumerable<Transaction> transactions, string mainCategory)
        {
            return transactions.Where(s => s.Category != null && Categories.GetMainCategory(s.Category.Value) == mainCategory).Sum(s => s.Amount).ToString(new CultureInfo("en-US"));
        }
    }
}