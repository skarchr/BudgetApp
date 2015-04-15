using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions
{
    public class Dropdownlist
    {
        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }


        public static List<GroupedSelectListItem> GroupedCategories(string selectedItem, bool useCategoryId = false)
        {
            var result = new List<GroupedSelectListItem>();
            
            foreach (var dict in Categories.Grouped)
            {
                foreach (var category in dict.Value)
                {
                    result.Add(new GroupedSelectListItem
                    {
                        GroupKey = dict.Key,
                        GroupName = dict.Key,
                        Text = GetEnumDescription(category),
                        Value = category.ToString(),
                        Selected = category.ToString() == selectedItem
                    });
                }

                
            }

            return result.OrderBy(s => s.GroupName).ToList();
        }

    }
}