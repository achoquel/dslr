using common.Models;
using System.Collections.Generic;

namespace logreg_train.Models
{
    public class LogRegDatasModel
    {
        /// <summary>
        /// Creates a LogRegDatasModel based on the features from the dataset
        /// </summary>
        /// <param name="features"></param>
        public LogRegDatasModel(List<NumericalFeatureModel> features)
        {
            Features = features;
            TotalCount = Features[0].Count;
        }

        /// <summary>
        /// The total ammount of entries
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// The features from the dataset
        /// </summary>
        public List<NumericalFeatureModel> Features { get; set; }

        /// <summary>
        /// Features standardized, reorganized for practical purposes during LogReg
        /// </summary>
        public List<float[]> X
        {
            get
            {
                List<float[]> xlist = new List<float[]>();
                for (int i = 0; i < TotalCount; ++i)
                {
                    var e = new float[Features.Count];
                    for (int j = 0; j < Features.Count; ++j)
                    {
                        e[j] = Features[j].ValuesStandardized[i].Value;
                    }
                    xlist.Add(e);
                }
                return xlist;
            }
        }

        /// <summary>
        /// An array of int, defining if students belongs to Gryffindor or not
        /// </summary>
        public int[] YForGryffindor
        {
            get
            {
                int[] res = new int[Features[0].Count];
                for (int i = 0; i < Features[0].Count; ++i)
                {
                    if (Features[0].Values[i].House == "Gryffindor")
                    {
                        res[i] = 1;
                    }
                    else
                    {
                        res[i] = 0;
                    }

                    ++i;
                }
                return res;
            }
        }

        /// <summary>
        /// An array of int, defining if students belongs to Hufflepuff or not
        /// </summary>
        public int[] YForHufflepuff
        {
            get
            {
                int[] res = new int[Features[0].Count];
                for (int i = 0; i < Features[0].Count; ++i)
                {
                    if (Features[0].Values[i].House == "Hufflepuff")
                    {
                        res[i] = 1;
                    }
                    else
                    {
                        res[i] = 0;
                    }

                    ++i;
                }
                return res;
            }
        }

        /// <summary>
        /// An array of int, defining if students belongs to Slytherin or not
        /// </summary>
        public int[] YForSlytherin
        {
            get
            {
                int[] res = new int[Features[0].Count];
                for (int i = 0; i < Features[0].Count; ++i)
                {
                    if (Features[0].Values[i].House == "Slytherin")
                    {
                        res[i] = 1;
                    }
                    else
                    {
                        res[i] = 0;
                    }

                    ++i;
                }
                return res;
            }
        }

        /// <summary>
        /// An array of int, defining if students belongs to Ravenclaw or not
        /// </summary>
        public int[] YForRavenclaw
        {
            get
            {
                int[] res = new int[Features[0].Count];
                for (int i = 0; i < Features[0].Count; ++i)
                {
                    if (Features[0].Values[i].House == "Ravenclaw")
                    {
                        res[i] = 1;
                    }
                    else
                    {
                        res[i] = 0;
                    }

                    ++i;
                }
                return res;
            }
        }
    }
}
