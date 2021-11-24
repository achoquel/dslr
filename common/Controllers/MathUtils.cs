using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace common.Controllers
{
    /// <summary>
    /// Because here at 42 we love to recode everything :)
    /// </summary>
    public class MathUtils
    {
        /// <summary>
        /// Count the values of an enumerable
        /// </summary>
        /// <param name="entries">The enumerable to count from</param>
        /// <returns></returns>
        public static int Count(IEnumerable entries)
        {
            int count = 0;
            foreach (var e in entries)
            {
                ++count;
            }

            return count;
        }

        #region Mean
        /// <summary>
        /// Returns the mean of a List of floats
        /// </summary>
        /// <param name="values">The list of values</param>
        /// <returns></returns>
        public static float Mean(List<float> values)
        {
            int count = Count(values);
            float sum = 0f;
            foreach (var v in values)
            {
                sum += v;
            }

            return sum / count;
        }

        /// <summary>
        /// Returns the mean of a list of int
        /// </summary>
        /// <param name="values">The list of int</param>
        /// <returns></returns>
        public static float Mean(List<int> values)
        {
            int count = Count(values);
            float sum = 0f;
            foreach (var v in values)
            {
                sum += v;
            }

            return sum / count;
        }

        /// <summary>
        /// Returns the average date of a list of dates
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public static DateTime Mean(List<DateTime> dates)
        {
            var count = dates.Count;
            double temp = 0D;
            foreach (var date in dates)
            {
                temp += date.Ticks / (double)count;
            }
            return new DateTime((long)temp);
        }
        #endregion

        #region StdDev
        /// <summary>
        /// Returns the standard deviation of a list of int
        /// </summary>
        /// <param name="values">The list of int</param>
        /// <returns></returns>
        public static float StdDev(List<int> values)
        {
            float mean = Mean(values);
            float count = Count(values);

            return MathF.Sqrt(values.Sum(x => MathF.Pow(x - mean, 2)) / count);
        }

        /// <summary>
        /// Returns the standard deviation of a list of float
        /// </summary>
        /// <param name="values">The list of float</param>
        /// <returns></returns>
        public static float StdDev(List<float> values)
        {
            float mean = Mean(values);
            float count = Count(values);

            return MathF.Sqrt(values.Sum(x => MathF.Pow(x - mean, 2)) / count);
        }

        /// <summary>
        /// Returns the standard deviation of a list of DateTime
        /// </summary>
        /// <param name="dates">The list of dates</param>
        /// <returns></returns>
        public static DateTime StdDev(List<DateTime> dates)
        {
            double mean = Mean(dates).Ticks;
            double count = Count(dates);

            return new DateTime((long)Math.Sqrt(dates.Sum(x => Math.Pow(x.Ticks - mean, 2)) / count));
        }
        #endregion

        #region Min
        /// <summary>
        /// Returns the min value of a list of int
        /// </summary>
        /// <param name="values">The list of int</param>
        /// <returns></returns>
        public static int Min(List<int> values)
        {
            return values.OrderBy(x => x).First();
        }

        /// <summary>
        /// Returns the min value of a list of float
        /// </summary>
        /// <param name="values">The list of float</param>
        /// <returns></returns>
        public static float Min(List<float> values)
        {
            return values.OrderBy(x => x).First();
        }

        /// <summary>
        /// Returns the min date of a list of dates
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        public static DateTime Min(List<DateTime> dates)
        {
            return dates.OrderBy(x => x).First();
        }
        #endregion

        #region Max
        /// <summary>
        /// Returns the max value of a list of int
        /// </summary>
        /// <param name="values">The list of int</param>
        /// <returns></returns>
        public static int Max(List<int> values)
        {
            return values.OrderBy(x => x).Last();
        }

        /// <summary>
        /// Returns the maximum value of a list of float
        /// </summary>
        /// <param name="values">The list of float</param>
        /// <returns></returns>
        public static float Max(List<float> values)
        {
            return values.OrderBy(x => x).Last();
        }

        /// <summary>
        /// Returns the maximum date of a list of dates
        /// </summary>
        /// <param name="dates">The list of dates</param>
        /// <returns></returns>
        public static DateTime Max(List<DateTime> dates)
        {
            return dates.OrderBy(x => x).Last();
        }
        #endregion

        #region Q1
        /// <summary>
        /// Returns the first quartile of a list of int
        /// </summary>
        /// <param name="values">The list of int</param>
        /// <returns></returns>
        public static int Q1(List<int> values)
        {
            int rank = (int)MathF.Ceiling(Count(values) / 4f) - 1;

            return values.OrderBy(x => x).ToArray()[rank];
        }

        /// <summary>
        /// Returns the first quartile of a list of float
        /// </summary>
        /// <param name="values">The list of float</param>
        /// <returns></returns>
        public static float Q1(List<float> values)
        {
            int rank = (int)MathF.Ceiling(Count(values) / 4f) - 1;

            return values.OrderBy(x => x).ToArray()[rank];
        }

        /// <summary>
        /// Returns the first quartile of a list of dates
        /// </summary>
        /// <param name="dates">The list of dates</param>
        /// <returns></returns>
        public static DateTime Q1(List<DateTime> dates)
        {
            int rank = (int)MathF.Ceiling(Count(dates) / 4f) - 1;

            return dates.OrderBy(x => x).ToArray()[rank];
        }
        #endregion

        #region Med
        /// <summary>
        /// Returns the median of a list of int
        /// </summary>
        /// <param name="values">The list of int</param>
        /// <returns></returns>
        public static float Med(List<int> values)
        {
            int count = Count(values);
            int rank = (int)MathF.Ceiling(Count(values) / 2f) - 1;
            int[] orderedValues = values.OrderBy(x => x).ToArray();
            if (IsOdd(count))
            {
                return orderedValues[rank];
            }
            else
            {
                return (orderedValues[rank] + orderedValues[rank + 1]) / 2f;
            }
        }

        /// <summary>
        /// Returns the median of a list of float
        /// </summary>
        /// <param name="values">The list of float</param>
        /// <returns></returns>
        public static float Med(List<float> values)
        {
            int count = Count(values);
            int rank = (int)MathF.Ceiling(Count(values) / 2f) - 1;
            float[] orderedValues = values.OrderBy(x => x).ToArray();
            if (IsOdd(count))
            {
                return orderedValues[rank];
            }
            else
            {
                return (orderedValues[rank] + orderedValues[rank + 1]) / 2f;
            }
        }

        /// <summary>
        /// Returns the median of a list of dates
        /// </summary>
        /// <param name="dates">The list of dates</param>
        /// <returns></returns>
        public static DateTime Med(List<DateTime> dates)
        {
            int count = Count(dates);
            int rank = (int)MathF.Ceiling(Count(dates) / 2f) - 1;
            DateTime[] orderedValues = dates.OrderBy(x => x).ToArray();
            if (IsOdd(count))
            {
                return orderedValues[rank];
            }
            else
            {
                return new DateTime((long)((orderedValues[rank].Ticks + orderedValues[rank + 1].Ticks) / 2f));
            }
        }
        #endregion

        #region Q3
        /// <summary>
        /// Returns the third quartile of a list of int
        /// </summary>
        /// <param name="values">The list of int</param>
        /// <returns></returns>
        public static int Q3(List<int> values)
        {
            int rank = (int)MathF.Ceiling(Count(values) * 3f / 4f) - 1;

            return values.OrderBy(x => x).ToArray()[rank];
        }

        /// <summary>
        /// Returns the third quartile of a list of float
        /// </summary>
        /// <param name="values">The list of float</param>
        /// <returns></returns>
        public static float Q3(List<float> values)
        {
            int rank = (int)MathF.Ceiling(Count(values) * 3f / 4f) - 1;

            return values.OrderBy(x => x).ToArray()[rank];
        }

        /// <summary>
        /// Returns the third quartile of a list of dates
        /// </summary>
        /// <param name="dates">The list of dates</param>
        /// <returns></returns>
        public static DateTime Q3(List<DateTime> dates)
        {
            int rank = (int)MathF.Ceiling(Count(dates) * 3f / 4f) - 1;

            return dates.OrderBy(x => x).ToArray()[rank];
        }
        #endregion

        #region Misc
        /// <summary>
        /// Tells if a number is odd
        /// </summary>
        /// <param name="value">The number to test</param>
        /// <returns></returns>
        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        /// <summary>
        /// Returns the sigmoid of z
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public static float Sigmoid(float z)
        {
            return 1f / (1f + MathF.Exp(-z));
        }

        /// <summary>
        /// Returns the dot product of two vectors
        /// </summary>
        /// <param name="x">First vector</param>
        /// <param name="y">Second vector</param>
        /// <returns></returns>
        public static float Dot(float[] x, float[] y)
        {
            float dot = 0f;
            if (Count(x.ToList()) == Count(y.ToList()))
            {
                for (int i = 0; i < Count(x.ToList()); ++i)
                {
                    dot += x[i] * y[i];
                }
            }
            return dot;
        }
        #endregion
    }
}
