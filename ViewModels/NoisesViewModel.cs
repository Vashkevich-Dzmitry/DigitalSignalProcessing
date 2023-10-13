using DSP.Filtering;
using DSP.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP.ViewModels
{
    internal class NoisesViewModel : INotifyPropertyChanged
    {

        private bool isNoisesVisible;
        public bool IsNoisesVisible
        {
            get => isNoisesVisible;
            set
            {
                if (isNoisesVisible != value)
                {
                    isNoisesVisible = value;
                    OnPropertyChanged(nameof(IsNoisesVisible));
                }
            }
        }

        private float p;
        public float P
        {
            get => p;
            set
            {
                if (p != value)
                {
                    p = value;

                    OnPropertyChanged(nameof(P));
                }
            }
        }

        private Random random;
        public NoisesViewModel()
        {
            p = 0.1f;
            isNoisesVisible = false;
            random = new Random();
        }

        public PointF[] MakeNoise(PointF[] values, float A)
        {
            Parallel.ForEach(values, (value, state, index) =>
            {
                float noise = 2 * (random.NextSingle() - 0.5f) * P * A;
                values[index] = new PointF(value.X, value.Y + noise);
            });

            return values;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
