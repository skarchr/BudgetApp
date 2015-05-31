using System.Collections.Generic;

namespace BudgetApp.Models
{
    public class Highchart
    {
        public List<string> Categories { get; set; }
        public List<Series> Series { get; set; }
        public Drilldown Drilldown { get; set; }
        public Title Title { get; set; }
        public string Currency { get; set; }
    }

    public class Data
    {
        public Data()
        {
            DataLabels = new DataLabels();
        }

        public double Y { get; set; }
        public double X { get; set; }
        public string Color { get; set; }
        public string Drilldown { get; set; }
        public string Name { get; set; }
        public DataLabels DataLabels { get; set; }
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

    public class Style
    {
        public string Color { get; set; }
        public string FontSize { get; set; }
        public string FontWeight { get; set; }
        public string TextShadow { get; set; }
    }

    public class DataLabels
    {
        public DataLabels()
        {
            Enabled = true;
            Format = "{point.y:,.1f}";
        }
        public bool Enabled { get; set; }
        public string Format { get; set; }
    }
}