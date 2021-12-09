using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace common.Models
{
    public class LogRegTrainingResultsModel
    {
        /// <summary>
        /// The path where the training results will be exported
        /// </summary>
        private static readonly string WEIGHTS_FILE_PATH = "../.logreg_weights.csv";

        /// <summary>
        /// Creates a LogRegTrainingResult and inputs training weights in it
        /// </summary>
        /// <param name="gW">Gryffindor weights</param>
        /// <param name="hW">Hufflepuff weights</param>
        /// <param name="sW">Slytherin weights</param>
        /// <param name="rW">Ravenclaw weights</param>
        public LogRegTrainingResultsModel(float[] gW, float[] hW, float[] sW, float[] rW)
        {
            GryffindorWeights = gW;
            HufflepuffWeights = hW;
            SlytherinWeights = sW;
            RavenclawWeights = rW;
        }

        /// <summary>
        /// The weights for gryffindor obtained after model training
        /// </summary>
        public float[] GryffindorWeights { get; set; }

        /// <summary>
        /// The weights for hufflepuff obtained after model training
        /// </summary>
        public float[] HufflepuffWeights { get; set; }

        /// <summary>
        /// The weights for slytherin obtained after model training
        /// </summary>
        public float[] SlytherinWeights { get; set; }

        /// <summary>
        /// The weights for ravenclaw obtained after model training
        /// </summary>
        public float[] RavenclawWeights { get; set; }

        /// <summary>
        /// The loss history for gryffindor one-vs-all training
        /// </summary>
        public float[] GryffindorLossHistory { get; set; }

        /// <summary>
        /// The loss history for hufflepuff one-vs-all training
        /// </summary>
        public float[] HufflepuffLossHistory { get; set; }

        /// <summary>
        /// The loss history for slytherin one-vs-all training
        /// </summary>
        public float[] SlytherinLossHistory { get; set; }

        /// <summary>
        /// The loss history for ravenclaw one-vs-all training
        /// </summary>
        public float[] RavenclawLossHistory { get; set; }

        #region Public Methods
        /// <summary>
        /// Exports the weights to the file specified by WEIGHTS_FILE_PATH
        /// </summary>
        public void Export()
        {
            try
            {
                if (File.Exists(WEIGHTS_FILE_PATH))
                {
                    File.Delete(WEIGHTS_FILE_PATH);
                }

                using (StreamWriter sw = File.CreateText(WEIGHTS_FILE_PATH))
                {
                    sw.WriteLine(GenerateCsvLine(GryffindorWeights));
                    sw.WriteLine(GenerateCsvLine(HufflepuffWeights));
                    sw.WriteLine(GenerateCsvLine(SlytherinWeights));
                    sw.WriteLine(GenerateCsvLine(RavenclawWeights));
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Imports the weights stored in WEIGHTS_FILE_PATH file
        /// </summary>
        public static (float[], float[], float[], float[]) Import()
        {
            try
            {
                if (File.Exists(WEIGHTS_FILE_PATH))
                {
                    float[][] w = new float[4][];
                    w[0] = new float[13] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
                    w[1] = new float[13] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
                    w[2] = new float[13] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
                    w[3] = new float[13] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };

                    var lines = File.ReadAllLines(WEIGHTS_FILE_PATH);

                    //We check that we have the lines for each of our houses
                    if (lines.Length == 4)
                    {
                        //We split each line tom get all the weights
                        List<string[]> splittedLines = new List<string[]>();
                        foreach (var line in lines)
                        {
                            splittedLines.Add(line.Split(',').ToArray());
                        }
                        //We check that we have all of our weights
                        if (splittedLines.All(sl => sl.Length == 13))
                        {
                            //We tryparse our strings to floats and we check if they are well formated
                            for (int i = 0; i < splittedLines.Count; ++i)
                            {
                                for (int j = 0; j < splittedLines[i].Length; ++j)
                                {
                                    if (!float.TryParse(splittedLines[i][j], NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.GetCultureInfo("en-US"), out w[i][j]))
                                    {
                                        throw new Exception("A weight is not well formated. Please train your model again.");
                                    }
                                }
                            }
                            return (w[0], w[1], w[2], w[3]);
                        }
                    }
                    throw new Exception("Weights file is missing datas. Please train your model again.");
                }
                else
                {
                    throw new Exception("Weights file was not found. Did you train the model before trying to predict ?");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Generates a csv line with the weights
        /// </summary>
        /// <param name="w">Weights to put in the line</param>
        /// <returns></returns>
        private static string GenerateCsvLine(float[] w)
        {
            string csv = string.Empty;
            foreach (float f in w)
            {
                csv += f.ToString(CultureInfo.GetCultureInfo("en-US")) + ",";
            }
            //Remove the last comma
            csv = csv.Remove(csv.Length - 1);
            return csv;
        }
        #endregion
    }
}
