using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP.Smoothing
{
    internal class NoneSmoothingAlgorithm : SmoothingAlghoritm
    {
        public override SmoothingType Type { get; set; } = SmoothingType.None;

        public NoneSmoothingAlgorithm() { }
        public override double[] Execute(double[] values)
        {
            return values;
        }
    }
}