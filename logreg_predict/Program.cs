using common.Controllers;
using common.Enumerations;
using common.Models;
using logreg_predict.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace logreg_predict
{
    class Program
    {
        private static readonly string EXPORT_PATH = "./houses.csv";

        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    DatasetModel dataset = DatasetParsingController.ParseDatasetFromFile(args[0], ExecutionModeEnum.PREDICTION);
                    (float[] gW, float[] hW, float[] sW, float[] rW) = LogRegTrainingResultsModel.Import();
                    (int, string)[] results = new (int, string)[dataset.Entries.Count];
                    Console.Write("Predicting houses... ");
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
                        results[i] = (i, PredictionController.RunPredictionOnEntry(features, gW, hW, sW, rW));
                    });
                    ExportResults(results.ToList());
                    Console.Write("Done !\nResults were saved in " + EXPORT_PATH);
                }
                else
                {
                    throw new Exception("No dataset provided !");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }

        private static void ExportResults(List<(int, string)> results)
        {
            try
            {
                using (var sw = File.CreateText(EXPORT_PATH))
                {
                    sw.WriteLine("Index,Hogwarts House");
                    foreach (var line in results)
                    {
                        sw.WriteLine($"{line.Item1},{line.Item2}");
                    }

                    sw.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
