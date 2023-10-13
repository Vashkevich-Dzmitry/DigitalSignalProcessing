using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP.Smoothing
{
    internal class SmoothingByParabolic : SmoothingAlghoritm
    {
        public override SmoothingType Type { get; set; } = SmoothingType.Parabolic;

        public SmoothingByParabolic() { }
        public override double[] Execute(double[] values)
        {
            double[] smoothedValues = new double[values.Length];
            Parallel.ForEach(values, (value, state, index) =>
            {
                smoothedValues[index] = (-3.0 * GetZeroOrElementWithIndex(values, (int)index - 7)
                                                 - 6.0 * GetZeroOrElementWithIndex(values, (int)index - 6)
                                                 - 5.0 * GetZeroOrElementWithIndex(values, (int)index - 5)
                                                 + 3.0 * GetZeroOrElementWithIndex(values, (int)index - 4)
                                                 + 21.0 * GetZeroOrElementWithIndex(values, (int)index - 3)
                                                 + 46.0 * GetZeroOrElementWithIndex(values, (int)index - 2)
                                                 + 67.0 * GetZeroOrElementWithIndex(values, (int)index - 1)
                                                 + 74.0 * value
                                                 - 3.0 * GetZeroOrElementWithIndex(values, (int)index + 7)
                                                 - 6.0 * GetZeroOrElementWithIndex(values, (int)index + 6)
                                                 - 5.0 * GetZeroOrElementWithIndex(values, (int)index + 5)
                                                 + 3.0 * GetZeroOrElementWithIndex(values, (int)index + 4)
                                                 + 21.0 * GetZeroOrElementWithIndex(values, (int)index + 3)
                                                 + 46.0 * GetZeroOrElementWithIndex(values, (int)index + 2)
                                                 + 67.0 * GetZeroOrElementWithIndex(values, (int)index + 1)) / 320.0;
            });
            return smoothedValues;
        }
    }
}
