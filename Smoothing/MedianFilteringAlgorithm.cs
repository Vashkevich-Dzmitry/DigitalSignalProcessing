using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DSP.Smoothing
{
    internal class MedianFilteringAlgorithm : SmoothingAlghoritm
    {
        public override SmoothingType Type { get ; set; } = SmoothingType.MedianFilteringAlgorithm;

        public MedianFilteringAlgorithm(int windowSize, int N)
        { 
            this.windowSize = windowSize;
            this.k = N;
        }
        //public override double[] Execute(double[] values)
        //{
        //    double[] smoothedValues = new double[values.Length];
        //    int windowSize = this.WindowSize!.Value;
        //    int K = this.K!.Value;
        //    int offset = (windowSize - 1) / 2;

        //    Parallel.ForEach(values, (value, state, index) =>
        //    {
        //        double sum = 0;
        //        for (int j = (int)index - offset + K; j <= (int)index + offset - K; j++)
        //        {
        //            sum += GetZeroOrElementWithIndex(values, j);
        //        }

        //        double koef = 1.0 / (windowSize - 2.0 * K);
        //        smoothedValues[index] = sum * koef;
        //    });

        //    return smoothedValues;
        //}

        public override double[] Execute(double[] values)
        {
            double[] smoothedValues = new double[values.Length];
            int windowSize = this.WindowSize!.Value;
            int K = this.K!.Value;
            int offset = (windowSize - 1) / 2;

            Parallel.ForEach(values, (value, state, index) =>
            {
                List<double> windowValues = new List<double>();

                for (int j = (int)index - offset; j <= (int)index + offset; j++)
                {
                    windowValues.Add(GetZeroOrElementWithIndex(values, j));
                }

                windowValues.Sort();

                int elementsToKeep = windowValues.Count - 2 * K;

                if (elementsToKeep > 0)
                {
                    double median = windowValues.Skip(K).Take(elementsToKeep).Average();
                    smoothedValues[index] = median;
                }
                else
                {
                    smoothedValues[index] = 0;
                }
            });

            return smoothedValues;
        }
    }
}
