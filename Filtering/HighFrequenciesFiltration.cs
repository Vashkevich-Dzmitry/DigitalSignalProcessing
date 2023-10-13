namespace DSP.Filtering
{
    internal class HighFrequenciesFiltration : Filtration
    {
        public override FiltrationType Type { get; set; } = FiltrationType.HighFrequencies;

        public HighFrequenciesFiltration(int minHarmonic)
        {
            this.minHarmonic = minHarmonic;
            this.maxHarmonic = null;
        }

        public override double Filter(int index, double value)
        {
            return index < MinHarmonic ? 0 : value;
        }
    }
}
