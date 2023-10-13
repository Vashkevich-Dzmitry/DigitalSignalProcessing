using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DSP.Smoothing
{
    internal class AlgorithmOfSlidingAveraging : SmoothingAlghoritm
    {
        public override SmoothingType Type { get; set; } = SmoothingType.AlgorithmOfSlidingAveraging;

        public AlgorithmOfSlidingAveraging(int windowSize)
        {
            this.windowSize = windowSize;
        }
        public override double[] Execute(double[] values)
        {
            double[] smoothedValues = new double[values.Length];
            int windowSize = WindowSize!.Value;
            int offset = (windowSize - 1) / 2;

            Parallel.ForEach(values, (value, state, index) =>
            {
                double sum = 0;

                for (int j = (int)index - offset; j <= (int)index + offset; j++)
                {
                    sum += GetZeroOrElementWithIndex(values, j);
                }

                smoothedValues[index] = sum / windowSize;
            });

            return smoothedValues;
        }
    }
}
