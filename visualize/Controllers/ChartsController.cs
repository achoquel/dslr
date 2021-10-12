using common.Controllers;
using common.Models;
using Highsoft.Web.Mvc.Charts;
using Highsoft.Web.Mvc.Charts.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using visualize.Models;

namespace visualize.Controllers
{
    [Route("/[controller]/[action]")]
    public class ChartsController : Controller
    {
        private static string[] HOUSES_NAMES = { "Gryffindor", "Hufflepuff", "Slytherin", "Ravenclaw" };

        private static DatasetModel _dataset;

        [HttpPost]
        public IActionResult Index(IFormFile datasetFile)
        {
            try
            {
                DatasetModel dataset = TryParseDatasetFromFile(datasetFile);
                _dataset = dataset;
                return View(new ChartsIndexViewModel() { Courses = dataset.Features?.Select(f => f.FeatureName).Distinct().ToList() });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("Home", "Index");
        }

        public IActionResult Index()
        {
            if (_dataset != null && _dataset.Features != null)
                return View(new ChartsIndexViewModel() { Courses = _dataset.Features?.Select(f => f.FeatureName).Distinct().ToList() });
            return RedirectToAction("Index", "Home");
        }

        public IActionResult PairPlot()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateHistogramDataPost(string courseName)
        {
            if (_dataset != null && _dataset.Features != null)
            {
                List<string> availableCourses = _dataset.Features.Select(f => f.FeatureName).Distinct().ToList();
                if (availableCourses.Contains(courseName))
                {
                    var scatterModel = new ChartAjaxModel()
                    {
                        Title = $"Repartition of the notes between houses for {courseName} course",
                        XAxisName = $"{courseName} notes",
                        YAxisName = "Students",
                    };
                    scatterModel.Series = GenerateHistogramSeries(courseName);
                    return Json(scatterModel);
                }
            }
            return Json(false);
        }

        [HttpPost]
        public IActionResult GenerateScatterPlotDataPost(string courseName1, string courseName2)
        {
            if (_dataset != null && _dataset.Features != null)
            {
                List<string> availableCourses = _dataset.Features.Select(f => f.FeatureName).Distinct().ToList();
                if (availableCourses.Contains(courseName1) && availableCourses.Contains(courseName2))
                {
                    var scatterModel = new ChartAjaxModel()
                    {
                        Title = $"Repartition of the notes between {courseName1} and {courseName2}",
                        XAxisName = courseName1,
                        YAxisName = courseName2,
                    };
                    scatterModel.Series = GenerateScatterSeries(courseName1, courseName2);
                    return Json(scatterModel);
                }  
            }
            return Json(false);
        }

        [HttpPost]
        public IActionResult GeneratePairPlotData()
        {
            if (_dataset != null && _dataset.Features != null)
            {
                List<ChartAjaxModel> charts = new List<ChartAjaxModel>();
                List<string> availableCourses = _dataset.Features.Select(f => f.FeatureName).Distinct().ToList();
                for (int y = 0; y < availableCourses.Count(); ++y)
                {
                    for (int x = 0; x < availableCourses.Count(); ++x)
                    {
                        var chartModel = new ChartAjaxModel()
                        {
                            Id = $"chart{x}-{y}",
                            XAxisName = y == 12 ? availableCourses[x] : "",
                            YAxisName = x == 0 ? availableCourses[y] : ""
                        };
                        if (availableCourses[x] != availableCourses[y])
                            chartModel.Series = GenerateScatterSeries(availableCourses[x], availableCourses[y]);
                        else
                            chartModel.Series = GenerateHistogramSeries(availableCourses[x]);
                        charts.Add(chartModel);
                    }
                }
                return Json(charts);
            }
            return Json(false);
        }

        private List<CustomChartSerie> GenerateScatterSeries(string courseNameX, string courseNameY)
        {
            List<CustomChartSerie> series = new List<CustomChartSerie>();
            foreach (string house in HOUSES_NAMES)
            {
                var serie = new CustomChartSerie() { Name = house };
                var feature1 = _dataset.Features.Where(f => f.FeatureName == courseNameX).First().Values.Where(v => v.House == house).Select(v => v.Value).ToList();
                var feature2 = _dataset.Features.Where(f => f.FeatureName == courseNameY).First().Values.Where(v => v.House == house).Select(v => v.Value).ToList();
                for (int i = 0; i < feature1.Count() && i < feature2.Count(); ++i)
                {
                    serie.Data.Add(new SerieValues() { X = feature1[i], Y = feature2[i] });
                }
                series.Add(serie);
            }
            return series;
        }

        private List<CustomChartSerie> GenerateHistogramSeries(string courseName)
        {
            List<CustomChartSerie> series = new List<CustomChartSerie>();
            foreach (string house in HOUSES_NAMES)
            {
                var serie = new CustomChartSerie() { Name = house };
                var feature = _dataset.Features.Where(f => f.FeatureName == courseName).First().Values.Where(v => v.House == house).Select(v => v.Value).ToList();
                for (int i = 0; i < feature.Count(); ++i)
                {
                    serie.Data.Add(new SerieValues() { X = 0, Y = feature[i] });
                }
                series.Add(serie);
            }
            return series;
        }

        private static DatasetModel TryParseDatasetFromFile(IFormFile datasetFile)
        {
            try
            {
                if (datasetFile != null && datasetFile.Length > 0 && Path.GetExtension(datasetFile.FileName) == ".csv")
                {
                    string fileContents;
                    using (var stream = datasetFile.OpenReadStream())
                    using (var reader = new StreamReader(stream))
                    {
                        fileContents = reader.ReadToEnd();
                    }
                    DatasetModel dataset = DatasetParsingController.ParseDatasetFromString(fileContents);
                    return dataset;
                }
                throw new Exception("File is empty or is not a .csv.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //private static List<HighchartsRenderer> CreateHistogramCharts(List<NumericalFeatureModel> features)
        //{
        //    List<HighchartsRenderer> charts = new List<HighchartsRenderer>();
        //    foreach (NumericalFeatureModel feature in features)
        //    {
        //        List<ColumnSeries> series = new List<ColumnSeries>();
                
        //        var chartOptions = new Highcharts()
        //        {
        //            Title = new Title() { Text = $"{feature.FeatureName} marks repartition between houses" },
        //            XAxis = new List<XAxis>()
        //            {
        //                new XAxis()
        //                {
        //                    Title = new XAxisTitle(){ Text = "Marks"},
        //                }
        //            },
        //            YAxis = new List<YAxis>()
        //            {
        //                new YAxis()
        //                {
        //                    Title = new YAxisTitle(){ Text = "Number of Students"}
        //                }
        //            },
        //            ID = $"{feature.FeatureName.Replace(' ', '_')}HistogramChart"
        //        };
        //        //Need to do the computation for the histograms
        //        charts.Add(new HighchartsRenderer(chartOptions));
        //    }
        //    return charts;
        //}

        //private static List<HighchartsRenderer> CreateScatterPlotCharts(List<NumericalFeatureModel> features)
        //{
        //    List<HighchartsRenderer> charts = new List<HighchartsRenderer>();

        //    foreach(NumericalFeatureModel feature in features)
        //    {
        //        foreach(NumericalFeatureModel feature2 in features)
        //        {
        //            List<ScatterSeries> series = new List<ScatterSeries>();

        //            var chartOptions = new Highcharts()
        //            {
        //                //Title = new Title() { Text = $"{feature.FeatureName} marks" },
        //                XAxis = new List<XAxis>()
        //            {
        //                new XAxis()
        //                {
        //                    Title = new XAxisTitle(){ Text = feature.FeatureName},
        //                }
        //            },
        //                YAxis = new List<YAxis>()
        //            {
        //                new YAxis()
        //                {
        //                    Title = new YAxisTitle(){ Text = feature2.FeatureName}
        //                }
        //            },
        //                ID = $"{feature.FeatureName.Replace(' ', '_')}{feature2.FeatureName.Replace(' ', '_')}ScatterPlotChart"
        //            };
        //            foreach (string hName in HOUSES_NAMES)
        //            {
        //                var dataX = feature.Values.Where(v => v.House == hName).OrderBy(v => v).ToList();
        //                var dataY = feature2.Values.Where(v => v.House == hName).OrderBy(v => v).ToList();
        //                var serie = new ScatterSeries();
        //                for (int i = 0; i < MathUtils.Count(dataX) && i < MathUtils.Count(dataY); ++i)
        //                {
        //                    serie.Data.Add(new ScatterSeriesData() { X = dataX[i].Value, Y = dataY[i].Value });
        //                }
        //                chartOptions.Series.Add(serie);
        //            }
        //            charts.Add(new HighchartsRenderer(chartOptions));
        //        }
                
        //    }
        //    return charts;
        //}

        //private static HighchartsRenderer GenerateChart(ChartModel graphData)
        //{
        //    var scatterPlotChartOptions = new Highcharts
        //    {
        //        Title = new Title
        //        {
        //            Text = graphData.ChartTitle
        //        },
        //        Subtitle = new Subtitle
        //        {
        //            Text = graphData.ChartSubtitle
        //        },
        //        XAxis = new List<XAxis>
        //        {
        //            new XAxis
        //            {
        //                Title = new XAxisTitle(){ Text = graphData.XAxisName }
        //            }
        //        },
        //        YAxis = new List<YAxis>
        //        {
        //            new YAxis
        //            {
        //                Title = new YAxisTitle(){ Text = graphData.YAxisName }
        //            }
        //        },
        //        Credits = new Credits
        //        {
        //            Enabled = false
        //        },
        //        Series = graphData.Series,
        //        ID = graphData.ChartID
        //    };

        //    return new HighchartsRenderer(scatterPlotChartOptions);
        //}
    }
}
