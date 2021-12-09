using common.Controllers;
using common.Enumerations;
using common.Models;
using logreg_predict.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using visualize.Models;

namespace visualize.Controllers
{
    public class PredictController : BaseController
    {
        public IActionResult Index()
        {
            if (mainDataset != null && mainDataset.IsValidDataset())
            {
                if (trainingResults != null)
                {
                    return RedirectToAction("Index", "Train");
                }
                return RedirectToAction("Index", "Charts");
            }
            return RedirectToAction("Index", "Home", new HomeIndexViewModel() { Error = true, ErrorMessage = "An error occurred." });
        }

        [HttpPost]
        public IActionResult Index(IFormFile testDataset)
        {
            try
            {
                DatasetModel dataset = TryParseDatasetFromFile(testDataset);
                predictionResults = new Tuple<int, string>[dataset.Entries.Count];
                Parallel.For(0, dataset.FilledFeatures[0].Count, (i, state) =>
                {
                    float[] features = new float[13] {
                            dataset.FilledFeatures[0].ValuesStandardized[i].Value,
                            dataset.FilledFeatures[1].ValuesStandardized[i].Value,
                            dataset.FilledFeatures[2].ValuesStandardized[i].Value,
                            dataset.FilledFeatures[3].ValuesStandardized[i].Value,
                            dataset.FilledFeatures[4].ValuesStandardized[i].Value,
                            dataset.FilledFeatures[5].ValuesStandardized[i].Value,
                            dataset.FilledFeatures[6].ValuesStandardized[i].Value,
                            dataset.FilledFeatures[7].ValuesStandardized[i].Value,
                            dataset.FilledFeatures[8].ValuesStandardized[i].Value,
                            dataset.FilledFeatures[9].ValuesStandardized[i].Value,
                            dataset.FilledFeatures[10].ValuesStandardized[i].Value,
                            dataset.FilledFeatures[11].ValuesStandardized[i].Value,
                            dataset.FilledFeatures[12].ValuesStandardized[i].Value
                        };
                    predictionResults[i] = new Tuple<int, string>(i, PredictionController.RunPredictionOnEntry(features, trainingResults.GryffindorWeights, trainingResults.HufflepuffWeights, trainingResults.SlytherinWeights, trainingResults.RavenclawWeights));
                });
                return View(predictionResults);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index", "Train", new TrainIndexViewModel()
                {
                    Error = true,
                    ErrorMessage = "An error occurred ! " + e.Message
                });
            }
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
                    DatasetModel dataset = DatasetParsingController.ParseDatasetFromString(fileContents, ExecutionModeEnum.PREDICTION);
                    return dataset;
                }
                throw new Exception("File is empty or is not a .csv.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
