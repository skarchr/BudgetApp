using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using BudgetApp.Models;
using Excel;
using System.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace BudgetApp.Importer
{
    public static class ExcelReader
    {
        private static readonly ApplicationDbContext Db = new ApplicationDbContext();

        public static List<Transaction> ReadFile(string filePath, string userName, out int found)
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

                var newTransaction = new Transaction
                {
                    Import = true,
                    Date = Convert.ToDateTime(excelReader.GetString(0), new CultureInfo("nb-NO")),
                    Description = excelReader.GetString(1),
                    Amount = Convert.ToDouble(excelReader.GetString(2) ?? excelReader.GetString(3)),
                    Category = FindCategory(excelReader.GetString(1), userName),
                    Created = DateTime.Now,
                    UserName = userName
                };


                if (!ExistsInDb(newTransaction, existingTransactions))
                {
                    transactions.Add(newTransaction);
                    found++;
                }

            }

            excelReader.Close();

            return transactions;
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
                if (descriptions.Contains(mapping.TextDescription))
                {
                    return mapping.Category;
                }
            }
            return null;
        }
    }
}