namespace DSP.Filtering
{
    internal class NoneFiltration : Filtration
    {
        public override FiltrationType Type { get; set; } = FiltrationType.None;

        public NoneFiltration() 
        {
            this.minHarmonic = null;
            this.maxHarmonic = null;
        }

        public override double Filter(int index, double value)
        {
            return value;
        }
    }
}
