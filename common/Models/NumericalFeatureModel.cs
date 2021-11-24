using common.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace common.Models
{
    public class NumericalFeatureModel
    {
        /// <summary>
        /// Creates a NumericalFeatureModel
        /// </summary>
        /// <param name="name">The name of the feature</param>
        /// <param name="values">A Tuple<float, string> containing the values binded with their houses</param>
        public NumericalFeatureModel(string name, List<(float value, string house)> values)
        {
            FeatureName = name;
            RawValues = values.Select(v => v.value).ToList();
            Values = values;
            ValuesStandardized = Values.Select(x => ((x.Value - Mean) / StdDev, x.House)).ToList();
        }

        /// <summary>
        /// The name of the feature
        /// </summary>
        public string FeatureName { get; set; }

        /// <summary>
        /// The raw values, not binded with their houses
        /// </summary>
        public List<float> RawValues { get; set; }

        /// <summary>
        /// The raw values binded with their houses
        /// </summary>
        public List<(float Value, string House)> Values { get; set; }

        /// <summary>
        /// The standardized values, binded with their houses
        /// </summary>
        public List<(float Value, string House)> ValuesStandardized { get; set; }

        /// <summary>
        /// The ammount of values for that feature
        /// </summary>
        public int Count
        {
            get
            {
                return MathUtils.Count(RawValues);
            }
        }

        /// <summary>
        /// The mean of that feature
        /// </summary>
        public float Mean
        {
            get
            {
                return MathUtils.Mean(RawValues);
            }
        }

        /// <summary>
        /// The standard deviation of that feature
        /// </summary>
        public float StdDev
        {
            get
            {
                return MathUtils.StdDev(RawValues);
            }
        }

        /// <summary>
        /// The minimum value of the feature
        /// </summary>
        public float Min
        {
            get
            {
                return MathUtils.Min(RawValues);
            }
        }

        /// <summary>
        /// The first quartile of the feature
        /// </summary>
        public float Q1
        {
            get
            {
                return MathUtils.Q1(RawValues);
            }
        }

        /// <summary>
        /// The median value of the feature
        /// </summary>
        public float Med
        {
            get
            {
                return MathUtils.Med(RawValues);
            }
        }

        /// <summary>
        /// The third quartile of the feature
        /// </summary>
        public float Q3
        {
            get
            {
                return MathUtils.Q3(RawValues);
            }
        }

        /// <summary>
        /// The maximum value of the feature
        /// </summary>
        public float Max
        {
            get
            {
                return MathUtils.Max(RawValues);
            }
        }

        #region Private Methods

        /// <summary>
        /// Standardizes a serie of value using Z-score Normalization
        /// </summary>
        /// <param name="values">The values to standardize</param>
        /// <returns></returns>
        public static List<float> StandardizeValues(List<float> values)
        {
            return values.Select(x => (x - MathUtils.Mean(values)) / (MathUtils.StdDev(values))).ToList();
        }
        #endregion
    }
}
