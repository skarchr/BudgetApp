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
        public List<PlotLines> PlotLinesX { get; set; }
        public List<PlotLines> PlotLinesY { get; set; }
        public string Type { get; set; }
    }

    public class PlotLines
    {
        public PlotLines()
        {
            ZIndex = 5;
            DashStyle = "Solid";
        }

        public string Id { get; set; }

        public Label Label { get; set; }

        public string Color { get; set; }

        public string DashStyle { get; set; }

        public double Value { get; set; }

        public int Width { get; set; }

        public int ZIndex { get; set; }
    }

    public class Style
    {
        public Style()
        {
            Color = "#333333";
            FontSize = "12px";
            FontWeight = "normal";
        }

        public Style(string size)
        {
            Color = "#333333";
            FontSize = size;
            FontWeight = "normal";
        }

        public string FontSize { get; set; }
        public string FontWeight { get; set; }
        public string Color { get; set; }
        public string TextShadow { get; set; }
    }

    public class Label
    {
        public Label()
        {
            X = 2;
            Y = 2;
            Style = new Style("12px");
        }

        public string Text { get; set; }

        public Style Style { get; set; }

        public int Rotation { get; set; }

        public string Align { get; set; }

        public string VerticalAlign { get; set; }

        public int? X { get; set; }

        public int? Y { get; set; }
    }



    public class Data
    {
        public Data()
        {
            DataLabels = new DataLabels();
        }

        public double? Y { get; set; }
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
        public Series()
        {
            Visible = true;
        }

        public bool Visible { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public List<Data> Data { get; set; }
    }

    public class Title
    {
        public string Text { get; set; }
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