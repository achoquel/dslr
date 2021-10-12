using common.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Models
{
    public class NumericalFeatureModel
    {

        public NumericalFeatureModel(string name, List<(float value, string house)> values)
        {
            FeatureName = name;
            RawValues = values.Select(v => v.value).ToList();
            Values = values;
            FeatureScaling();
        }

        public string FeatureName { get; set; }

        public List<float> RawValues { get; set; }

        public List<(float Value, string House)> Values { get; set; }

        public List<(float, string House)> ValuesStandardized { get; set; }

        public int Count
        {
            get
            {
                return MathUtils.Count(RawValues);
            }
        }

        public float Mean
        {
            get
            {
                return MathUtils.Mean(RawValues);
            }
        }

        public float StdDev
        {
            get
            {
                return MathUtils.StdDev(RawValues);
            }
        }

        public float Min
        {
            get
            {
                return MathUtils.Min(RawValues);
            }
        }

        public float Q1
        {
            get
            {
                return MathUtils.Q1(RawValues);
            }
        }

        public float Med
        {
            get
            {
                return MathUtils.Med(RawValues);
            }
        }

        public float Q3
        {
            get
            {
                return MathUtils.Q3(RawValues);
            }
        }

        public float Max
        {
            get
            {
                return MathUtils.Max(RawValues);
            }
        }

        #region Private Methods

        public static List<float> StandardizeValues(List<float> values)
        {
            return values.Select(x => (x - MathUtils.Mean(values)) / (MathUtils.StdDev(values))).ToList();
        }

        private void FeatureScaling()
        {
            //Perform Standardization (Z-score Normalization)
            // x' = x - average(x) / standard deviation
            ValuesStandardized = Values.Select(x => (((x.Value - Mean) / StdDev), x.House)).ToList();
        }
        #endregion
    }
}
