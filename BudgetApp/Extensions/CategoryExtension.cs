using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Models;

namespace BudgetApp.Extensions
{
    public static class CategoryExtension
    {
        public static List<string> GetCategoryNames()
        {
            return Enum.GetNames(typeof(Category)).ToList();
        } 

    }
}