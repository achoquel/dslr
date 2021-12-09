using common.Controllers;
using System;
using System.Linq;

namespace logreg_predict.Controllers
{
    public class PredictionController
    {
        /// <summary>
        /// Runs a prediction using Logistic Regression (one-vs-all) on the entry to determinate to which house belongs the student
        /// </summary>
        /// <param name="features">The features values of the student</param>
        /// <param name="gWeights">The weights for Gryffindor</param>
        /// <param name="hWeights">The weights for Hufflepuff</param>
        /// <param name="sWeights">The weights for Slytherin</param>
        /// <param name="rWeights">The weights for Ravenclaw</param>
        /// <returns></returns>
        public static string RunPredictionOnEntry(float[] features, float[] gWeights, float[] hWeights, float[] sWeights, float[] rWeights)
        {
            //We predict the chance that the student belongs to each houses
            float gryffindorPred = MathUtils.Sigmoid(MathUtils.Dot(features, gWeights));
            float hufflepuffPred = MathUtils.Sigmoid(MathUtils.Dot(features, hWeights));
            float slytherinPred = MathUtils.Sigmoid(MathUtils.Dot(features, sWeights));
            float ravenclawPred = MathUtils.Sigmoid(MathUtils.Dot(features, rWeights));

            //We assign the student to the house that has the highest prediction
            return new[]
            {
                Tuple.Create(gryffindorPred, "Gryffindor"),
                Tuple.Create(hufflepuffPred, "Hufflepuff"),
                Tuple.Create(slytherinPred, "Slytherin"),
                Tuple.Create(ravenclawPred, "Ravenclaw")
            }.Max()?.Item2;
        }
    }
}
