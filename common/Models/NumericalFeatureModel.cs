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

        public NumericalFeatureModel(string name, List<float> values)
        {
            FeatureName = name;
            Values = values;
        }

        public string FeatureName { get; set; }

        public List<float> Values { get; set; }

        public List<float> ValuesOrdered
        {
            get
            {
                return Values.OrderBy(x => x).ToList();
            }
        }

        public int Count
        {
            get
            {
                return MathUtils.Count(Values);
            }
        }

        public float Mean
        {
            get
            {
                return MathUtils.Mean(Values);
            }
        }

        public float StdDev
        {
            get
            {
                return MathUtils.StdDev(Values);
            }
        }

        public float Min
        {
            get
            {
                return MathUtils.Min(Values);
            }
        }

        public float Q1
        {
            get
            {
                return MathUtils.Q1(Values);
            }
        }

        public float Med
        {
            get
            {
                return MathUtils.Med(Values);
            }
        }

        public float Q3
        {
            get
            {
                return MathUtils.Q3(Values);
            }
        }

        public float Max
        {
            get
            {
                return MathUtils.Max(Values);
            }
        }
    }
}
