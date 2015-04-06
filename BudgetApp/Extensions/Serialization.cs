using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace BudgetApp.Extensions
{
    public static class Serialization
    {
        public static string ToJson(this object obj)
        {
            var js = JsonSerializer.Create(new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.IsoDateFormat });
            js.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            js.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            var jw = new StringWriter();
            js.Serialize(jw, obj);
            var result = jw.ToString();
            return result;
        }
    }
}