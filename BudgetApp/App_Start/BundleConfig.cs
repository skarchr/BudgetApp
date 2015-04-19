using System.Web.Optimization;

namespace BudgetApp
{
   public class BundleConfig
   {
      // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
      public static void RegisterBundles(BundleCollection bundles)
      {
         bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                     "~/Scripts/jquery-{version}.js"));

         bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                     "~/Scripts/jquery.validate*"));

         // Use the development version of Modernizr to develop with and learn from. Then, when you're
         // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
         bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                     "~/Scripts/modernizr-*"));

         bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                   "~/Scripts/bootstrap.js",
                   //"~/Scripts/bootstrap-datepicker.js",
                   "~/Scripts/respond.js"));

         bundles.Add(new ScriptBundle("~/bundles/bower").Include(
                   "~/bower_components/velocity/velocity.min.js",
                   "~/bower_components/velocity/velocity.ui.min.js",
                   "~/bower_components/highstock-release/highstock.js",
                   "~/bower_components/highstock-release/highcharts-more.js",
                   "~/bower_components/highstock-release/modules/solid-gauge.js",
                   "~/bower_components/highstock-release/modules/drilldown.js",
                   "~/bower_components/angular/angular.min.js",
                   "~/bower_components/angular-animate/angular-animate.js",
                   "~/bower_components/angular-gravatar/build/angular-gravatar.min.js",
                   "~/bower_components/angular-bootstrap/ui-bootstrap.min.js",
                   "~/bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js"));

         bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                   "~/js/app/BudgetApp.js",
                   "~/js/directives/Directives.js",
                   "~/js/filters/Filters.js",
                   "~/js/controllers/TransactionController.js",
                   "~/js/controllers/LoginController.js"));

         bundles.Add(new StyleBundle("~/Content/css").Include(
                   "~/Content/bootstrap.css",
                   "~/bower_components/animate.css/animate.css",
                   "~/Content/site.css"));

         // Set EnableOptimizations to false for debugging. For more information,
         // visit http://go.microsoft.com/fwlink/?LinkId=301862
         BundleTable.EnableOptimizations = false;
      }
   }
}
