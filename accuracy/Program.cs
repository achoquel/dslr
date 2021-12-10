using System;
using System.IO;

namespace accuracy
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 2)
                {
                    if (File.Exists(args[0]) && File.Exists(args[1]))
                    {
                        string[] pred = File.ReadAllLines(args[0]);
                        string[] truth = File.ReadAllLines(args[1]);
                        if (pred.Length == truth.Length)
                        {
                            for (int i = 1; i < Math.Max(pred.Length, truth.Length); ++i)
                            {
                                if (pred[i] != truth[i])
                                {
                                    Console.Write($"[{i - 1}] ");
                                    Console.BackgroundColor = ConsoleColor.Red; 
                                    Console.Write($"- {pred[i].Split(',')[1]}  ");
                                    Console.BackgroundColor = ConsoleColor.Green;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.WriteLine($"+ {truth[i].Split(',')[1]}");
                                    Console.ResetColor();
                                }
                            }
                            Console.ResetColor();
                            Console.WriteLine("The accuracy of the prediciton is: " + (Accuracy(pred, truth) * 100).ToString("n2") + "%.");
                        }
                    }
                }
                else
                {
                    throw new Exception("Usage: dotnet run <prediction results file> <true results file>");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }

        /// <summary>
        /// Calculates the accuracy of the results of our predictions. Based on https://scikit-learn.org/stable/modules/model_evaluation.html#accuracy-score formula
        /// </summary>
        /// <param name="pred"></param>
        /// <param name="truth"></param>
        /// <returns></returns>
        private static float Accuracy(string[] pred, string[] truth)
        {
            float sum = 0f;
            for (int i = 0; i < pred.Length; ++i)
            {
                sum += pred[i] == truth[i] ? 1f : 0f;
            }

            return (1f / pred.Length) * sum;

        }
    }
}
