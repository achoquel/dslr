using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Controllers
{
    public class MathUtils
    {
        public static int Count<T>(List<T> entries)
        {
            int count = 0;
            foreach (var e in entries)
                ++count;
            return count;
        }

        public static float Mean(List<float> values)
        {
            int count = Count(values);
            float sum = 0f;
            foreach (var v in values)
                sum += v;
            return sum / count;
        }

        public static float Mean(List<int> values)
        {
            int count = Count(values);
            float sum = 0f;
            foreach (var v in values)
                sum += v;
            return sum / count;
        }

        public static float StdDev(List<int> values)
        {
            float mean = Mean(values);
            float count = Count(values);

            return MathF.Sqrt(values.Sum(x => MathF.Pow(x - mean, 2)) / count);
        }

        public static float StdDev(List<float> values)
        {
            float mean = Mean(values);
            float count = Count(values);

            return MathF.Sqrt(values.Sum(x => MathF.Pow(x - mean, 2)) / count);
        }

        public static int Min(List<int> values)
        {
            return values.OrderBy(x => x).First();
        }

        public static float Min(List<float> values)
        {
            return values.OrderBy(x => x).First();
        }

        public static int Max(List<int> values)
        {
            return values.OrderBy(x => x).Last();
        }

        public static float Max(List<float> values)
        {
            return values.OrderBy(x => x).Last();
        }

        public static int Q1(List<int> values)
        {
            int rank = (int)MathF.Ceiling(Count(values) / 4f) - 1;

            return values.OrderBy(x => x).ToArray()[rank];
        }

        public static float Q1(List<float> values)
        {
            int rank = (int)MathF.Ceiling(Count(values) / 4f) - 1;

            return values.OrderBy(x => x).ToArray()[rank];
        }

        public static float Med(List<int> values)
        {
            int count = Count(values);
            int rank = (int)MathF.Ceiling(Count(values) / 2f) - 1;
            int[] orderedValues = values.OrderBy(x => x).ToArray();
            if (IsOdd(count))
                return orderedValues[rank];
            else
                return (orderedValues[rank] + orderedValues[rank + 1]) / 2f;
        }

        public static float Med(List<float> values)
        {
            int count = Count(values);
            int rank = (int)MathF.Ceiling(Count(values) / 2f) - 1;
            float[] orderedValues = values.OrderBy(x => x).ToArray();
            if (IsOdd(count))
                return orderedValues[rank];
            else
                return (orderedValues[rank] + orderedValues[rank + 1]) / 2f;
        }

        public static int Q3(List<int> values)
        {
            int rank = (int)MathF.Ceiling(Count(values) * 3f / 4f) - 1;

            return values.OrderBy(x => x).ToArray()[rank];
        }

        public static float Q3(List<float> values)
        {
            int rank = (int)MathF.Ceiling(Count(values) * 3f/ 4f) - 1;

            return values.OrderBy(x => x).ToArray()[rank];
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
    }
}
