using DSP.Filtering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DSP.Smoothing
{
    public abstract class SmoothingAlghoritm
    {
        public abstract SmoothingType Type { get; set; }

        protected int? windowSize;
        public int? WindowSize
        {
            get => windowSize;
            set
            {
                if (windowSize != value && (k == null || value > k) && value % 2 == 1)
                {
                    windowSize = value;
                    OnPropertyChanged(nameof(WindowSize));
                }
            }
        }

        protected int? k;
        public int? K
        {
            get => k;
            set
            {
                if (k != value && value % 2 == 1 && windowSize > value) //нечетное
                {
                    k = value;
                    OnPropertyChanged(nameof(K));
                }
            }
        }

        public SmoothingAlghoritm() { }

        public abstract double[] Execute(double[] values);

        public double GetZeroOrElementWithIndex(double[] values, int index)
        {
            return (index >= 0 && index < values.Length) ? values[index] : 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
