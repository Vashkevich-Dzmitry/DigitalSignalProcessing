using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DSP.Correlations
{
    public class LinearCorrelation : ICorrelation
    {
        public CorrelationType Type { get; set; } = CorrelationType.Linear;

        public double[] FindCorrelation(double[] firstSignal, double[] secondSignal)
        {
            int count = firstSignal.Length + secondSignal.Length - 1;

            var firstArray = new double[count];
            firstSignal.CopyTo(firstArray, secondSignal.Length - 1);

            var secondArray = new double[count];
            secondSignal.CopyTo(secondArray, 0);

            var result = new double[count];
            

            Parallel.For(0, count, j =>
            {
                result[j] = FindValue(j, firstArray, secondArray) / count;
            });

            return result;
        }

        private static double FindValue(int j, double[] firstZeroed, double[] secondZeroed)
        {
            var result = 0.0;

            var count = firstZeroed.Length;
            for (int i = 0; i < count; ++i)
            {
                result += firstZeroed[i] * secondZeroed[(i + j) % count];
            }

            return result;
        }
    }
}
