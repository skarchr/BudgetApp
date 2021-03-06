﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BudgetApp.Controllers;
using BudgetApp.Models;
using Excel;

namespace BudgetApp.Importer
{
    public static class ExcelReader
    {
        private static readonly ApplicationDbContext Db = new ApplicationDbContext();

        public static List<Transaction> ReadFile(string filePath, string userName, ExcelColumns excelColumns, out int found)
        {
            found = 0;

            var existingTransactions = Db.Transactions.ToList();

            var transactions = new List<Transaction>();
            var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read);
            // Want to be able to handle .xls or .xlsx file formats
            var excelReader = filePath.Contains(".xlsx")
                                  ? ExcelReaderFactory.CreateOpenXmlReader(stream)
                                  : ExcelReaderFactory.CreateBinaryReader(stream);

            excelReader.IsFirstRowAsColumnNames = true;
            excelReader.Read(); //skip first row
            while (excelReader.Read())
            {
                
                var date = DateTime.Now;

                var provider = CultureInfo.InvariantCulture;

                try
                {
                    date = DateTime.ParseExact(excelReader.GetString(excelColumns.DateCol), excelColumns.DateFormat, provider);
                        //Convert.ToDateTime(excelReader.GetString(excelColumns.DateCol), new CultureInfo("nb-NO"));
                }
                catch (Exception)
                {
                    date = excelReader.GetDateTime(excelColumns.DateCol); 
                    // Convert.ToDateTime(excelReader.GetDateTime(excelColumns.DateCol), new CultureInfo("nb-NO"));
                }

                var desc = excelReader.GetString(excelColumns.DescriptionCol);

                var amount = FindAmount(excelReader, excelColumns);
                var cat = FindCategory(excelReader.GetString(excelColumns.DescriptionCol), userName);

                if (amount != null)
                {
                    var newTransaction = new Transaction
                    {
                        Import = true,
                        Date = date,
                        Description = desc,
                        Amount = amount.Value > 0 ? amount.Value : amount.Value * -1,
                        Category = cat,
                        Created = DateTime.Now,
                        UserName = userName
                    };

                    if (!ExistsInDb(newTransaction, existingTransactions))
                    {
                        transactions.Add(newTransaction);
                        found++;
                    }
                }
            }

            excelReader.Close();

            return transactions;
        }

        private static double? FindAmount(IExcelDataReader excelReader, ExcelColumns excelColumns)
        {
            foreach (var i in excelColumns.AmountCols)
            {
                try
                {
                    var result = excelReader.GetDouble(i);

                    if (excelReader[i].GetType().Name == "Double")
                    {
                        return result;
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        public static bool ExistsInDb(Transaction transaction, List<Transaction> database)
        {
            if (database.Any(db => db.Amount == transaction.Amount && db.Date == transaction.Date && db.Description == transaction.Description && db.UserName == transaction.UserName))
            {
                return true;
            }

            return false;
        }

        public static Category? FindCategory(string descriptions, string userName)
        {
            var mappings = Db.Mappings.Where(s => s.UserName == userName).ToList();

            foreach (var mapping in mappings)
            {

                if (descriptions != null && descriptions.Contains(mapping.TextDescription))
                {
                    return mapping.Category;
                }
            }
            return null;
        }
    }
}