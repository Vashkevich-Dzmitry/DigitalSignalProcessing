namespace DSP.Filtering
{
    internal class BandPassFiltration : Filtration
    {
        public override FiltrationType Type { get; set; } = FiltrationType.BandPass;

        public BandPassFiltration(int minHarmonic, int maxHarmonic)
        {
            this.minHarmonic = minHarmonic;
            this.maxHarmonic = maxHarmonic;
        }

        public override double Filter(int index, double value)
        {
            return (index > MaxHarmonic || index < MinHarmonic) ? 0 : value;
        }
    }
}
