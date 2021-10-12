using common.Models;
using logreg_train.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logreg_train.Controllers
{
    /// <summary>
    /// https://www.youtube.com/watch?v=z_xiwjEdAC4 C1W2L09
    /// https://www.youtube.com/watch?v=KKfZLXcF-aE C1W2L10
    /// </summary>
    public class LogregController
    {
        private static readonly int NB_EPOCHS = 1000;
        private static readonly float LR = 0.1f;

        public static void Train()
        {
            //recuperer les donnees et creer le LogRegDatasModel
            LogRegDatasModel data = new LogRegDatasModel();
            //potentiellement multithreader ici
            (var gryffindorWeights, var gryffindorB) = LogisticRegression(data.X, data.YForGryffindor);
            (var hufflepuffWeights, var hufflepuffB) = LogisticRegression(data.X, data.YForHufflepuff);
            (var slytherinWeights, var slytherinB) = LogisticRegression(data.X, data.YForSlytherin);
            (var ravenclawWeights, var ravenclawB) = LogisticRegression(data.X, data.YForRavenclaw);

        }

        private static (float[], float) LogisticRegression(LogRegEntryModel[] x, int[] y)
        {
            float[] w = new float[5] { 0f, 0f, 0f, 0f, 0f };
            float b = 0f;

            for (int i = 0; i < NB_EPOCHS; ++i)
                (w, b) = GradientDescent(x, y, w, b);
            return (w, b);
        }

        private static (float[], float) GradientDescent(LogRegEntryModel[] x, int[] y, float[] w, float b)
        {
            float[] wsum = new float[5] { 0f, 0f, 0f, 0f, 0f};
            float bsum = 0f;
            int m = 0;
            for (int i = 0; i < m; ++i)
            {
                float z = DotProduct(x[i].X, w, b);
                float a = SigmaFunction(z);
                float dz = a - y[i];
                Parallel.For(0, wsum.Length, (iter, state) => 
                { 
                    wsum[iter] += x[i].X[iter] * dz;
                });
                bsum += dz;
            }
            Parallel.For(0, wsum.Length, (iter, state) =>
            {
                wsum[iter] -= LR * (wsum[iter] / m);
            });
            bsum = b - LR * (bsum / m);

            return (wsum, bsum);
        }

        private static float DotProduct(float[] x, float[] w, float b)
        {
            //check si 'sum = b' au debut != 'sum += b' a la fin
            float sum = b;
            for(int i = 0; i < x.Length && i < w.Length; ++i)
            {
                sum += x[i] * w[i];
            }
            return sum;
        }

        private static float SigmaFunction(float z)
        {
            return (1 / (1 - MathF.Exp(-z)));
        }
    }
}
