namespace DSP.Correlations
{
    public interface ICorrelation
    {
        public CorrelationType Type { get; set; }
        public double[] FindCorrelation(double[] firstSignal, double[] secondSignal);
    }
}
