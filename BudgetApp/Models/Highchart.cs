using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Models
{
    public class Highchart
    {
        public List<string> Categories { get; set; }
        public List<Series> Series { get; set; }
        public Drilldown Drilldown { get; set; }
        public Title Title { get; set; }
    }

    public class Data
    {
        public double Y { get; set; }
        public double X { get; set; }
        public string Color { get; set; }
        public string Drilldown { get; set; }
        public string Name { get; set; }
    }

    public class Drilldown
    {
        public List<Series> Series { get; set; }
    }

    public class Series
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Data> Data { get; set; }
    }

    public class Title
    {
        public string Text { get; set; }
    }
}