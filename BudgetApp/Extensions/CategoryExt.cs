using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions
{
    public static class CategoryExt
    {
        public static List<string> GetCategoryNames()
        {
            return Enum.GetNames(typeof(Category)).ToList();
        }

        public static string CamelCaseToNormal(string input)
        {
            return Regex.Replace(input, "(\\B[A-Z])", " $1");
        }

        public static string GetMainCategory(Category category, bool normal = false)
        {
            if (normal)
                return
                    CamelCaseToNormal(
                        (from key in Categories.Grouped where key.Value.Any(val => val == category) select key.Key)
                            .FirstOrDefault());

            return (from key in Categories.Grouped where key.Value.Any(val => val == category) select key.Key).FirstOrDefault();
        }

        public static int GetCategoryColor(Category? category)
        {
            if (category != null)
            {
                return GetCategoryColor(GetMainCategory(category.Value));
            }
            return 0;
        }

        public static int GetCategoryColor(string mainCategory)
        {
            return GetColorIndex(mainCategory);
        }

        private static int GetColorIndex(string mainCategory)
        {

            switch (mainCategory)
            {
                case Categories.Fixed:
                    return 0;
                case Categories.Food:
                    return 1;
                case Categories.Income:
                    return 2;
                case Categories.Personal:
                    return 3;
                case Categories.Saving:
                    return 4;
                case Categories.Shelter:
                    return 5;
                case Categories.Transport:
                    return 6;
                default:
                    return 0;
            }
        }
    }
}