using System.Collections.Generic;

namespace visualize.Models
{
    public class ChartAjaxModel
    {
        /// <summary>
        /// The HTML Id of the chart
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The title of the chart
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The subtitle of the chart
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// The name of the X-Axis
        /// </summary>
        public string XAxisName { get; set; }

        /// <summary>
        /// The name of the Y-Axis
        /// </summary>
        public string YAxisName { get; set; }

        /// <summary>
        /// The data series to be displayed on the chart
        /// </summary>
        public List<CustomChartSerie> Series { get; set; } = new List<CustomChartSerie>();
    }

    public class CustomChartSerie
    {
        /// <summary>
        /// The name of the Serie
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The data of the serie
        /// </summary>
        public List<SerieValues> Data { get; set; } = new List<SerieValues>();
    }

    public class SerieValues
    {
        /// <summary>
        /// X-Axis value
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y-Axis value
        /// </summary>
        public float Y { get; set; }
    }
}
