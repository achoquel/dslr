using common.Controllers;
using common.Models;
using System;
using System.Linq;

namespace describe
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    DatasetModel dataset = DatasetParsingController.ParseDatasetFromFile(args[0]);
                    dataset.Describe();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
            }
        }
    }
}
