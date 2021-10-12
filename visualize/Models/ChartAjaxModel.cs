using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace visualize.Models
{
    public class ChartAjaxModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string XAxisName { get; set; }

        public string YAxisName { get; set; }

        public List<CustomChartSerie> Series { get; set; } = new List<CustomChartSerie>();
    }

    public class CustomChartSerie
    {
        public string Name { get; set; }

        public List<SerieValues> Data { get; set; } = new List<SerieValues>();
    }

    public class SerieValues
    {
        public float X { get; set; }

        public float Y { get; set; }
    }
}
