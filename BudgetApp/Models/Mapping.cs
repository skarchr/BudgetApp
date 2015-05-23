using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BudgetApp.Models
{
    public class Mapping
    {
        public int MappingId { get; set; }
        public string UserName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Category? Category { get; set; }
        public string TextDescription { get; set; }
        public DateTime Created { get; set; }
    }
}