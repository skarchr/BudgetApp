using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using BudgetApp.Models;

namespace BudgetApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.Link = TempData["ViewBagLink"];



            return View("Index", db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList());
        }

    }
}