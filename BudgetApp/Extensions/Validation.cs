using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BudgetApp.Extensions
{
    public static class Validation
    {
        public static MvcHtmlString MyValidationSummary(this HtmlHelper helper, string validationMessage = "")
        {
            string retVal = "";
            if (helper.ViewData.ModelState.IsValid)
                return new MvcHtmlString("<div class='placeholder-text'><span>*</span></div>");
            retVal += "<div class='notification-warnings text-danger'><span>";
            if (!String.IsNullOrEmpty(validationMessage))
                retVal += helper.Encode(validationMessage);
            retVal += "</span>";
            retVal += "<div class='text'>";
            retVal = helper.ViewData.ModelState.Keys.SelectMany(key => helper.ViewData.ModelState[key].Errors).Aggregate(retVal, (current, err) => current + ("<p>" + helper.Encode(err.ErrorMessage) + "</p>"));
            retVal += "</div></div>";
            return new MvcHtmlString(retVal);
        }
    }
}