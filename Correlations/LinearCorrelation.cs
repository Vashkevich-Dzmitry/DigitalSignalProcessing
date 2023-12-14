using System;

namespace DSP.Correlations
{
    public class LinearCorrelation : ICorrelation
    {
        public CorrelationType Type { get; set; } = CorrelationType.Linear;

        public double[] FindCorrelation(double[] firstSignal, double[] secondSignal)
        {
            throw new NotImplementedException();
        }
    }
}
