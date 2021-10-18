using System;
using common.Controllers;
using common.Models;
using logreg_train.Controllers;

namespace logreg_train
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0 )
                {
                    Console.WriteLine(args[0]);
                    DatasetModel dataset = DatasetParsingController.ParseDatasetFromFile(args[0]);
                    LogregController.Train(dataset);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }
    }
}
