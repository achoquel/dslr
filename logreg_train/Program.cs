using common.Controllers;
using common.Enumerations;
using common.Models;
using logreg_train.Controllers;
using System;

namespace logreg_train
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    DatasetModel dataset = DatasetParsingController.ParseDatasetFromFile(args[0], ExecutionModeEnum.TRAINING);
                    LogregController.Train(dataset).Export();
                    Console.Write("Done !\nWeights have successfully been exported.");
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
    }
}
