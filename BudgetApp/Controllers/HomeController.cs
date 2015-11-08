using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BudgetApp.Constants;
using BudgetApp.Extensions;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace BudgetApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (ConfigurationManager.AppSettings["debugmode"] == "true")
                ViewBag.Link = TempData["ViewBagLink"];

            ViewBag.Success = TempData["Success"];

            var transactions = db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList();
            var user = db.Users.First(u => u.UserName == User.Identity.Name);

            var expenses = transactions.Where(s => (CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income)).OrderBy(s => s.Date);

            var timeSpan = 0;
            
            if(expenses.Count() > 1)
                timeSpan = (expenses.Last().Date - expenses.First().Date).Days;

            var range = timeSpan >= 700 ?
                ChartRange.Monthly :
                    timeSpan >= 175 ?
                    ChartRange.Weekly :
                    ChartRange.Daily;

            var model = new HomeViewModel
            {
                ChartRange = range,
                Transactions = transactions,
                DailyExpensesGraph = GraphBuilder.DailyExpensesGraph(transactions, user.Currency).ToJson(),
                TransactionDrilldownGraph = GraphBuilder.TransactionDrilldownGraph(transactions, user.Currency, "bar", true).ToJson(),
                DrilldownGraphTot = GraphBuilder.DrilldownGraph(transactions, user.Currency, "column", true).ToJson(),
                DrilldownGraphYtd = GraphBuilder.DrilldownGraph(transactions.Where(s => s.Date.Year == DateTime.Now.Year).ToList(), user.Currency, "column", true).ToJson(),
                ScpExpensesChart = GraphBuilder.SpcGraph(transactions, user.Currency, range).ToJson(),
                ExpensesGoal = user.MonthlyExpensesGoal,
                SavingGoal = user.MonthlySavingGoal,
                Currency = user.Currency
            };

            return View("Index", model);
        }

        [HttpGet]
        public ActionResult ChangeRange(ChartRange range)
        {
            if (ConfigurationManager.AppSettings["debugmode"] == "true")
                ViewBag.Link = TempData["ViewBagLink"];

            ViewBag.Success = TempData["Success"];

            var transactions = db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList();
            var user = db.Users.First(u => u.UserName == User.Identity.Name);

            var model = new HomeViewModel
            {
                ChartRange = range,
                Transactions = transactions,
                DailyExpensesGraph = GraphBuilder.DailyExpensesGraph(transactions, user.Currency).ToJson(),
                TransactionDrilldownGraph = GraphBuilder.TransactionDrilldownGraph(transactions, user.Currency, "bar", true).ToJson(),
                DrilldownGraphTot = GraphBuilder.DrilldownGraph(transactions, user.Currency, "column", true).ToJson(),
                DrilldownGraphYtd = GraphBuilder.DrilldownGraph(transactions.Where(s => s.Date.Year == DateTime.Now.Year).ToList(), user.Currency, "column", true).ToJson(),
                ScpExpensesChart = GraphBuilder.SpcGraph(transactions, user.Currency, range).ToJson(),
                ExpensesGoal = user.MonthlyExpensesGoal,
                SavingGoal = user.MonthlySavingGoal,
                Currency = user.Currency
            };

            return View("Index", model);
        }


        public ActionResult DownloadCurrentYear()
        {

            var ytdTrans = db.Transactions.Where(s => s.UserName == User.Identity.Name && s.Date.Year == DateTime.Now.Year).OrderBy(t => t.Date).ToList();

            var fileName = string.Format("test_budget_app_transactions_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmm"));

            var outputDir = Server.MapPath("~/App_Data/downloads/");
            var file = new FileInfo(outputDir + fileName);
           
            using (var package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now.Year.ToString());

                // Add some formatting to the worksheet
                worksheet.DefaultRowHeight = 12;
                worksheet.HeaderFooter.FirstFooter.LeftAlignedText = string.Format("Generated: {0}", DateTime.Now.ToShortDateString());
                worksheet.Row(1).Height = 20;
                worksheet.Column(1).Style.Numberformat.Format = "dd.MM.yyyy";

                // Start adding the header
                // First of all the first row
                worksheet.Cells[1, 1].Value = "Date";
                worksheet.Cells[1, 2].Value = "Description";
                worksheet.Cells[1, 3].Value = "Amount";
                worksheet.Cells[1, 4].Value = "Category";

                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.MediumBlue);
                    range.Style.ShrinkToFit = false;
                    range.AutoFitColumns();
                }

                var rowNumber = 2;

                foreach (var trans in ytdTrans)
                {
                    worksheet.Cells[rowNumber, 1].Value = trans.Date;
                    worksheet.Cells[rowNumber, 2].Value = trans.Description;
                    worksheet.Cells[rowNumber, 3].Value = trans.Amount;
                    worksheet.Cells[rowNumber, 4].Value = CategoryExt.CamelCaseToNormal(trans.Category.ToString());
                    
                    rowNumber++;
                }

                var stream = new MemoryStream();

                package.SaveAs(stream);
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

            }
        }
    }
}