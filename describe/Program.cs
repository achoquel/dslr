using common.Controllers;
using common.Enumerations;
using common.Models;
using System;

namespace describe
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    DatasetModel dataset = DatasetParsingController.ParseDatasetFromFile(args[0], ExecutionModeEnum.DESCRIBE);
                    dataset.Describe();
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
