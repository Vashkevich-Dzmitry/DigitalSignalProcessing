using System.ComponentModel;

namespace DSP.Filtering
{
    public abstract class Filtration : INotifyPropertyChanged
    {
        public abstract FiltrationType Type { get; set; }

        protected int? maxHarmonic;
        public int? MaxHarmonic
        {
            get => maxHarmonic;
            set
            {
                if (value > 0 && (minHarmonic == null || value >= minHarmonic))
                {
                    maxHarmonic = value;

                    OnPropertyChanged(nameof(MaxHarmonic));
                }
            }
        }

        protected int? minHarmonic;
        public int? MinHarmonic
        {
            get => minHarmonic;
            set
            {
                if (value > 0 && (maxHarmonic == null || value <= maxHarmonic))
                {
                    minHarmonic = value;

                    OnPropertyChanged(nameof(MinHarmonic));
                }
            }
        }

        public Filtration() { }

        public abstract double Filter(int index, double value);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
