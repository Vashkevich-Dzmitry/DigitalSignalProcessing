using System.ComponentModel;

namespace DSP.Filtering
{
    internal class LowFrequenciesFiltration : Filtration
    {
        public override FiltrationType Type { get; set; } = FiltrationType.LowFrequencies;

        public LowFrequenciesFiltration(int maxHarmonic)
        {
            this.minHarmonic = null;
            this.maxHarmonic = maxHarmonic;
        }

        public override double Filter(int index, double value)
        {
            return index > MaxHarmonic ? 0 : value;
        }
    }
}
