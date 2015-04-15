using System.Web.Helpers;
using System.Web.Mvc;
using BudgetApp.Models;

namespace BudgetApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Link = TempData["ViewBagLink"];



            return View("Index", new Transaction());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}