using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using BudgetApp.Extensions;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;
using OfficeOpenXml;

namespace BudgetApp.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public const string BoxPlot = "_BoxPlot";
        public const string SPC = "_SPC";
        public const string Treemap = "_Treemap";
        public const string Drilldown = "_Drilldown";

        public class ReportViewModel
        {
            public string Name { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public bool IsDrilldown { get; set; }
            public ChartRange Range { get; set; }
            public string ChartType { get; set; }
            public bool ByYear { get; set; }
            public string SearchString { get; set; }
            public List<string> Categories { get; set; }
            
            public List<string> Charts
            {
                get
                {
                    return new List<string>
                    {
                        BoxPlot,
                        SPC,
                        Drilldown,
                        Treemap
                    };
                }
            }
        }

        // GET: Report
        public ActionResult Index()
        {
            var transactions = db.Transactions.Where(s => s.UserName == User.Identity.Name).OrderBy(s => s.Date).ToList();

            var categories = CreateCategoryList();

            return View(new ReportViewModel
            {
                FromDate = transactions.FirstOrDefault() != null ? transactions.First().Date : DateTime.Now,
                ToDate = transactions.LastOrDefault() != null ? transactions.Last().Date : DateTime.Now,
                Categories = categories,
                Name = BoxPlot
            });
        }

        private List<string> CreateCategoryList()
        {
            var categories = new List<string>();

            foreach (var hovedCat in Constants.Categories.Grouped)
            {
                categories.AddRange(hovedCat.Value.Select(category => category.ToString()));
            }
            return categories;
        }

        [HttpPost]
        public ActionResult CreateChart(ReportViewModel model)
        {
            var transactions = FilterTransactions(db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList(), model);

            return PartialView(model.Name, FindChart(model, transactions));
        }

        [HttpPost]
        public JsonResult SearchTransactions(ReportViewModel model)
        {

            var transactions = FilterTransactions(db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList(), model);

            if (!string.IsNullOrEmpty(model.SearchString))
            {
                transactions = transactions.Where(s => s.Description.ToLower().Contains(model.SearchString.ToLower())).OrderByDescending(s => s.Date).ToList();
            }

            return new JsonResult {Data = transactions.ToJson()};
        }

        private List<Transaction> FilterTransactions(List<Transaction> transactions, ReportViewModel model)
        {
            var result = DateFilter(transactions, model);

            return CategoryFilter(result, model);
        }

        public List<Transaction> DateFilter(List<Transaction> transactions, ReportViewModel model)
        {
            return transactions.Where(s => s.Date >= model.FromDate && s.Date <= model.ToDate).ToList();
        }

        public List<Transaction> CategoryFilter(List<Transaction> transactions, ReportViewModel model)
        {
            return transactions.Where(t => model.Categories.Contains(t.Category.Value.ToString())).ToList();
        } 

        private string FindChart(ReportViewModel model, List<Transaction> transactions)
        {
            var currency = db.Users.First(s => s.UserName == User.Identity.Name).Currency;

            switch (model.Name)
            {
                case BoxPlot:
                    return GraphBuilder.BoxPlotGraph(transactions, currency, model.Range).ToJson();
                case SPC:
                    return GraphBuilder.SpcGraph(transactions, currency, model.Range).ToJson();
                case Treemap:
                    return TreemapGenerator.CreateChart(transactions).ToJson();
                case Drilldown:
                    return GraphBuilder.DrilldownGraph(transactions, currency, model.ChartType ?? model.ChartType, true).ToJson();
                default:
                    throw new ArgumentException("No such graph exists!");
            }
        }

        [HttpPost]
        public ActionResult DownloadExcelFile(ReportViewModel model)
        {

            var transactions = FilterTransactions(db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList(), model);

            var fileName = string.Format("budget_app_transactions_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmm"));

            var outputDir = Server.MapPath("~/App_Data/downloads/");
            var file = new FileInfo(outputDir + fileName);

            using (var package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook


                if (model.ByYear)
                {
                    foreach (var transaction in transactions.GroupBy(s => s.Date.Year))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(transaction.Key.ToString());

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

                        foreach (var trans in transaction)
                        {
                            worksheet.Cells[rowNumber, 1].Value = trans.Date;
                            worksheet.Cells[rowNumber, 2].Value = trans.Description;
                            worksheet.Cells[rowNumber, 3].Value = trans.Amount;
                            worksheet.Cells[rowNumber, 4].Value = CategoryExt.CamelCaseToNormal(trans.Category.ToString());

                            rowNumber++;
                        }

                    }
                }
                else
                {
                    foreach (var transaction in transactions.GroupBy(s => s.Category))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(CategoryExt.CamelCaseToNormal(transaction.Key.ToString()));

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

                        using (var range = worksheet.Cells[1, 1, 1, 4])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Font.Color.SetColor(Color.MediumBlue);
                            range.Style.ShrinkToFit = false;
                            range.AutoFitColumns();
                        }

                        var rowNumber = 2;

                        foreach (var trans in transaction)
                        {
                            worksheet.Cells[rowNumber, 1].Value = trans.Date;
                            worksheet.Cells[rowNumber, 2].Value = trans.Description;
                            worksheet.Cells[rowNumber, 3].Value = trans.Amount;

                            rowNumber++;
                        }

                    }
                }


                

                var stream = new MemoryStream();

                package.SaveAs(stream);
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

            }
        }



    }
}