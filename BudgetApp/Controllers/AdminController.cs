using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BudgetApp.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        } 

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {

            var users =  db.Users.ToList();

            return View(users);
        }


        public ActionResult DeleteProfile(string username)
        {
            var user = db.Users.FirstOrDefault(s => s.UserName == username);

            if (user != null)
            {

                foreach (var transaction in db.Transactions.Where(s => s.UserName == user.UserName).ToList())
                {
                    db.Transactions.Remove(transaction);
                }

                foreach (var mapping in db.Mappings.Where(s => s.UserName == user.UserName).ToList())
                {
                    db.Mappings.Remove(mapping);
                }

                foreach (var userLoginInfo in UserManager.GetLogins(user.Id))
                {
                    UserManager.RemoveLogin(user.Id, new UserLoginInfo(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey));
                }

                db.Users.Remove(user);
                db.SaveChanges();

                ViewBag.Success = string.Format("{0} was deleted", username);
            }
            else
            {
                ViewBag.Error = string.Format("{0} could NOT be deleted", username);
            }
            return View("Index", db.Users.ToList());
        }
    }
}