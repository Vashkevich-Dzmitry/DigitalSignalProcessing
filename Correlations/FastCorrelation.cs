using System;

namespace DSP.Correlations
{
    class FastCorrelation : ICorrelation
    {
        public CorrelationType Type { get; set; } = CorrelationType.Fast;
        public double[] FindCorrelation(double[] firstSignal, double[] secondSignal)
        {
            throw new NotImplementedException();
        }
    }
}
