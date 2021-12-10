using common.Controllers;
using common.Enumerations;
using common.Models;
using logreg_train.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace logreg_train.Controllers
{
    public static class LogregController
    {
        /// <summary>
        /// The number of epochs
        /// </summary>
        private static int NB_EPOCHS = 1500;

        /// <summary>
        /// The learning rate
        /// </summary>
        private static float LR = 0.1f;

        /// <summary>
        /// Handles the training of a dataset
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="progressBar"></param>
        /// <param name="epochs"></param>
        /// <param name="lr"></param>
        /// <returns></returns>
        public static LogRegTrainingResultsModel Train(DatasetModel dataset, bool progressBar = true, int? epochs = null, float? lr = null)
        {
            LogRegDatasModel datas = new LogRegDatasModel(dataset.FilledFeatures);

            //add backend verif for min maxs
            if (epochs.HasValue && epochs.Value >= 10 && epochs.Value <= 100000)
            {
                NB_EPOCHS = epochs.Value;
            }

            if (lr.HasValue && lr.Value > 0f && lr <= 10)
            {
                LR = lr.Value;
            }

            float[] gryffindorWeights = new float[13], hufflepuffWeights = new float[13], slytherinWeights = new float[13], ravenclawWeights = new float[13];
            var gryffindorLossHistory = new float[NB_EPOCHS]; var hufflepuffLossHistory = new float[NB_EPOCHS]; var slytherinLossHistory = new float[NB_EPOCHS]; var ravenclawLossHistory = new float[NB_EPOCHS];

            Parallel.Invoke(() =>
            {
                gryffindorWeights = LogisticRegression(datas.X, datas.YForGryffindor, ref gryffindorLossHistory, progressBar, dataset.Mode == ExecutionModeEnum.VISUALIZE);
            },
            () =>
            {
                hufflepuffWeights = LogisticRegression(datas.X, datas.YForHufflepuff, ref hufflepuffLossHistory, lossHistoryCalculation: dataset.Mode == ExecutionModeEnum.VISUALIZE);
            },
            () =>
            {
                slytherinWeights = LogisticRegression(datas.X, datas.YForSlytherin, ref slytherinLossHistory, lossHistoryCalculation: dataset.Mode == ExecutionModeEnum.VISUALIZE);
            },
            () =>
            {
                ravenclawWeights = LogisticRegression(datas.X, datas.YForRavenclaw, ref ravenclawLossHistory, lossHistoryCalculation: dataset.Mode == ExecutionModeEnum.VISUALIZE);
            }
            );

            return new LogRegTrainingResultsModel(gryffindorWeights, hufflepuffWeights, slytherinWeights, ravenclawWeights)
            {
                GryffindorLossHistory = gryffindorLossHistory,
                HufflepuffLossHistory = hufflepuffLossHistory,
                SlytherinLossHistory = slytherinLossHistory,
                RavenclawLossHistory = ravenclawLossHistory
            };
        }

        /// <summary>
        /// Handles the logistic regression over all features
        /// </summary>
        /// <param name="x">The list of features values</param>
        /// <param name="y">The belonging to a specific houses of the students</param>
        /// <param name="lossHistory"></param>
        /// <param name="displayProgressBar">An option to display the progress bar in the console on runtime. Only one call should have it set to true.</param>
        /// <param name="lossHistoryCalculation"></param>
        /// <returns></returns>
        private static float[] LogisticRegression(List<float[]> x, int[] y, ref float[] lossHistory, bool displayProgressBar = false, bool lossHistoryCalculation = false)
        {
            //We initialize the weights to 0
            float[] w = new float[13] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };

            if (displayProgressBar)
            {
                Console.Write("Performing model training... ");
                using (var progress = new ProgressBar())
                {
                    for (int i = 0; i < NB_EPOCHS; ++i)
                    {
                        if (lossHistoryCalculation)
                        {
                            lossHistory[i] = Loss(x, y, w);
                        }

                        w = GradientDescent(x, y, w);
                        progress.Report((double)i / NB_EPOCHS);
                    }
                }
            }
            else
            {
                for (int i = 0; i < NB_EPOCHS; ++i)
                {
                    if (lossHistoryCalculation)
                    {
                        lossHistory[i] = Loss(x, y, w);
                    }

                    w = GradientDescent(x, y, w);
                }
            }

            return w;
        }

        /// <summary>
        /// Runs the gradient descent to find the good weights
        /// </summary>
        /// <param name="x">The list of features values</param>
        /// <param name="y">The belonging to a specific houses of the students</param>
        /// <param name="weights">The actual values for the weights</param>
        /// <returns></returns>
        private static float[] GradientDescent(List<float[]> x, int[] y, float[] weights)
        {
            float[] wSum = new float[13] { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
            int m = x.Count;
            for (int i = 0; i < m; ++i)
            {
                float h = MathUtils.Sigmoid(MathUtils.Dot(x[i], weights));
                float diff = h - y[i];
                var i1 = i;
                Parallel.For(0, wSum.Length, (iter, state) =>
                {
                    wSum[iter] += x[i1][iter] * diff;
                });
            }
            Parallel.For(0, weights.Length, (iter, state) =>
            {
                weights[iter] -= LR * (wSum[iter] / m);
            });

            return weights;
        }

        private static float Loss(List<float[]> x, int[] y, float[] weights)
        {
            float sum = 0f;
            for (int i = 0; i < x.Count; ++i)
            {
                float h = MathUtils.Sigmoid(MathUtils.Dot(x[i], weights));
                sum += y[i] * MathF.Log(h) + (1f - y[i]) * MathF.Log(1f - h);
            }
            return sum / (-x.Count);
        }
    }
}
