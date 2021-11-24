using common.Controllers;
using common.Enumerations;
using common.Models;
using Highsoft.Web.Mvc.Charts;
using Highsoft.Web.Mvc.Charts.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using visualize.Models;

namespace visualize.Controllers
{
    public class ChartsController : BaseController
    {
        /// <summary>
        /// The houses names
        /// </summary>
        private readonly static string[] HOUSES_NAMES = { "Gryffindor", "Hufflepuff", "Slytherin", "Ravenclaw" };

        public IActionResult Index()
        {
            if (mainDataset != null && mainDataset.IsValidDataset())
            {
                return View(new ChartsIndexViewModel() { Courses = mainDataset.Features?.Select(f => f.FeatureName).Distinct().ToList() });
            }

            return RedirectToAction("Index", "Home", routeValues: new HomeIndexViewModel() { Error = true, ErrorMessage = "An error occurred." });
        }

        [HttpPost]
        public IActionResult Index(IFormFile datasetFile)
        {
            try
            {
                DatasetModel dataset = TryParseDatasetFromFile(datasetFile);
                mainDataset = dataset;
                trainingResults = null;
                predictionResults = null;
                return View(new ChartsIndexViewModel() { Courses = dataset.Features?.Select(f => f.FeatureName).Distinct().ToList() });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index", "Home", routeValues: new HomeIndexViewModel() { Error = true, ErrorMessage = "An error occurred ! " + e.Message });
            }
        }

        public IActionResult PairPlot()
        {
            return View();
        }

        /// <summary>
        /// Generates the data to display an histogram
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GenerateHistogramDataPost(string courseName)
        {
            if (mainDataset != null && mainDataset.IsValidDataset())
            {
                List<string> availableCourses = mainDataset.FullFeatures.Select(f => f.FeatureName).Distinct().ToList();
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

        /// <summary>
        /// Generates the data to display a scatter plot
        /// </summary>
        /// <param name="courseName1"></param>
        /// <param name="courseName2"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GenerateScatterPlotDataPost(string courseName1, string courseName2)
        {
            if (mainDataset != null && mainDataset.IsValidDataset())
            {
                List<string> availableCourses = mainDataset.FullFeatures.Select(f => f.FeatureName).Distinct().ToList();
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

        /// <summary>
        /// Generates the data to display a pair plot
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GeneratePairPlotData()
        {
            if (mainDataset != null && mainDataset.IsValidDataset())
            {
                List<ChartAjaxModel> charts = new List<ChartAjaxModel>();
                List<string> availableCourses = mainDataset.FullFeatures.Select(f => f.FeatureName).Distinct().ToList();
                for (int y = 0; y < MathUtils.Count(availableCourses); ++y)
                {
                    for (int x = 0; x < MathUtils.Count(availableCourses); ++x)
                    {
                        var chartModel = new ChartAjaxModel()
                        {
                            Id = $"chart{x}-{y}",
                            Subtitle = $"r = {CalulateCorrelationCoefficient(availableCourses[x], availableCourses[y])}",
                            XAxisName = y == 12 ? availableCourses[x] : "",
                            YAxisName = x == 0 ? availableCourses[y] : ""
                        };
                        if (availableCourses[x] != availableCourses[y])
                        {
                            chartModel.Series = GenerateScatterSeries(availableCourses[x], availableCourses[y]);
                        }
                        else
                        {
                            chartModel.Series = GenerateHistogramSeries(availableCourses[x]);
                        }

                        charts.Add(chartModel);
                    }
                }
                return Json(charts);
            }
            return Json(false);
        }

        public static HighchartsRenderer GenerateLossChart()
        {
            if (trainingResults != null && trainingResults.GryffindorLossHistory.Length >= 10 && trainingResults.HufflepuffLossHistory.Length >= 10 && trainingResults.RavenclawLossHistory.Length >= 10 && trainingResults.SlytherinLossHistory.Length >= 10)
            {
                List<Series> series = new List<Series>();
                series.Add(new SplineSeries()
                {
                    Data = GenerateSplineSerie(trainingResults.GryffindorLossHistory),
                    Name = "Gryffindor",
                    TurboThreshold = trainingResults.GryffindorLossHistory.Length,
                    Color = "rgba(101,0,0,255)",
                    Opacity = 0.6D
                });
                series.Add(new SplineSeries()
                {
                    Data = GenerateSplineSerie(trainingResults.HufflepuffLossHistory),
                    Name = "Hufflepuff",
                    TurboThreshold = trainingResults.HufflepuffLossHistory.Length,
                    Color = "rgba(255,157,10,255)",
                    Opacity = 0.6D
                });
                series.Add(new SplineSeries()
                {
                    Data = GenerateSplineSerie(trainingResults.SlytherinLossHistory),
                    Name = "Slytherin",
                    TurboThreshold = trainingResults.SlytherinLossHistory.Length,
                    Color = "rgba(47,117,28,255)",
                    Opacity = 0.6D
                });
                series.Add(new SplineSeries()
                {
                    Data = GenerateSplineSerie(trainingResults.RavenclawLossHistory),
                    Name = "Ravenclaw",
                    TurboThreshold = trainingResults.SlytherinLossHistory.Length,
                    Color = "rgba(26,57,86,255)",
                    Opacity = 0.6D
                });
                var chartOptions = new Highcharts
                {
                    Title = new Title
                    {
                        Text = "Loss History"
                    },
                    Subtitle = new Subtitle
                    {
                        Text = "during model training"
                    },
                    XAxis = new List<XAxis>
                    {
                        new XAxis
                        {
                            Title = new XAxisTitle(){ Text = "Epochs" }
                        }
                    },
                    YAxis = new List<YAxis>
                    {
                        new YAxis
                        {
                            Title = new YAxisTitle(){ Text = "Loss" }
                        }
                    },
                    Credits = new Credits
                    {
                        Enabled = false
                    },
                    Series = series,
                    ID = "lossHistoryHighcharts"
                };
                return new HighchartsRenderer(chartOptions);
            }
            throw new Exception("Training results are not valid.");
        }

        #region Private Methods
        /// <summary>
        /// Generates the series of a Scatter Plot
        /// </summary>
        /// <param name="courseNameX"></param>
        /// <param name="courseNameY"></param>
        /// <returns></returns>
        private List<CustomChartSerie> GenerateScatterSeries(string courseNameX, string courseNameY)
        {
            List<CustomChartSerie> series = new List<CustomChartSerie>();
            foreach (string house in HOUSES_NAMES)
            {
                var serie = new CustomChartSerie() { Name = house };
                var feature1 = mainDataset.FullFeatures.Where(f => f.FeatureName == courseNameX).First().Values.Where(v => v.House == house).Select(v => v.Value).ToList();
                var feature2 = mainDataset.FullFeatures.Where(f => f.FeatureName == courseNameY).First().Values.Where(v => v.House == house).Select(v => v.Value).ToList();
                for (int i = 0; i < MathUtils.Count(feature1) && i < MathUtils.Count(feature2); ++i)
                {
                    serie.Data.Add(new SerieValues() { X = feature1[i], Y = feature2[i] });
                }
                series.Add(serie);
            }
            return series;
        }

        /// <summary>
        /// Generates the series of a Histogram
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns></returns>
        private List<CustomChartSerie> GenerateHistogramSeries(string courseName)
        {
            List<CustomChartSerie> series = new List<CustomChartSerie>();
            foreach (string house in HOUSES_NAMES)
            {
                var serie = new CustomChartSerie() { Name = house };
                var feature = mainDataset.FullFeatures.Where(f => f.FeatureName == courseName).First().Values.Where(v => v.House == house).Select(v => v.Value).ToList();
                for (int i = 0; i < MathUtils.Count(feature); ++i)
                {
                    serie.Data.Add(new SerieValues() { X = 0, Y = feature[i] });
                }
                series.Add(serie);
            }
            return series;
        }

        private static List<SplineSeriesData> GenerateSplineSerie(float[] data)
        {
            var serie = new List<SplineSeriesData>();
            for (int i = 0; i < data.Length; ++i)
            {
                serie.Add(new SplineSeriesData() { X = i, Y = data[i] });
            }
            return serie;
        }

        /// <summary>
        /// Parses the dataset from the Http FormFile
        /// </summary>
        /// <param name="datasetFile"></param>
        /// <returns></returns>
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
                    DatasetModel dataset = DatasetParsingController.ParseDatasetFromString(fileContents, ExecutionModeEnum.VISUALIZE);
                    return dataset;
                }
                throw new Exception("File is empty or is not a .csv.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Calculates the correlation Coefficient to display on pair plot
        /// </summary>
        /// <param name="courseNameX">The name of the course from the x axis</param>
        /// <param name="courseNameY">The name of the course from the y axis</param>
        /// <returns></returns>
        private float CalulateCorrelationCoefficient(string courseNameX, string courseNameY)
        {
            var featureX = mainDataset.FullFeatures.Where(f => f.FeatureName == courseNameX).First().Values.Select(v => v.Value).ToList();
            var featureY = mainDataset.FullFeatures.Where(f => f.FeatureName == courseNameY).First().Values.Select(v => v.Value).ToList();
            var meanX = MathUtils.Mean(featureX);
            var meanY = MathUtils.Mean(featureY);

            float topSum = 0f;
            for (int i = 0; i < MathUtils.Count(featureX) && i < MathUtils.Count(featureY); ++i)
            {
                topSum += (featureX[i] - meanX) * (featureY[i] - meanY);
            }
            float xSum = 0f;
            foreach (var x in featureX)
            {
                xSum += MathF.Pow(x - meanX, 2);
            }

            float ySum = 0f;
            foreach (var y in featureY)
            {
                ySum += MathF.Pow(y - meanY, 2);
            }

            float bottom = MathF.Sqrt(xSum * ySum);

            return topSum / bottom;
        }
        #endregion
    }
}
