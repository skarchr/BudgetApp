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
            var income = SumCategory(transactions, "Income");
            var personal = SumCategory(transactions, "Personal");
            var shelter = SumCategory(transactions, "Shelter");
            var trans = SumCategory(transactions, "Transportation");


            return String.Format("[[\"Fixed\",{0}],[\"Food\",{1}],[\"Income\",{2}],[\"Personal\",{3}],[\"Shelter\",{4}],[\"Transportation\",{5}]]", fix, food, income, personal, shelter, trans);
        }

        private static string SumCategory(IEnumerable<Transaction> transactions, string mainCategory)
        {
            return transactions.Where(s => s.Category != null && Categories.GetMainCategory(s.Category.Value) == mainCategory).Sum(s => s.Amount).ToString(new CultureInfo("en-US"));
        }
    }
}