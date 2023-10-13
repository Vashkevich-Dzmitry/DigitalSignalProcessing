using System;
using System.ComponentModel;
using System.Drawing;

namespace DSP.Signals
{
    public abstract class GeneratedSignal : INotifyPropertyChanged
    {
        protected float phi0;
        protected float f;
        protected float a;
        protected float? d;

        public float Phi0
        {
            get => phi0;
            set
            {
                if (phi0 != value)
                {
                    phi0 = value;
                    OnPropertyChanged(nameof(Phi0));
                }
            }
        }
        public float F
        {
            get => f;
            set
            {
                if (f != value)
                {
                    f = value;
                    OnPropertyChanged(nameof(F));
                }
            }
        }
        public float A
        {
            get => a;
            set
            {
                if (a != value)
                {
                    a = value;
                    OnPropertyChanged(nameof(A));
                }
            }
        }

        public float? D
        {
            get => d;
            set
            {
                if (d != value)
                {
                    d = value;
                    OnPropertyChanged(nameof(D));
                }
            }
        }

        public abstract SignalType Type { get; set; }

        public GeneratedSignal(float phi0, float f, float a)
        {
            this.phi0 = phi0;
            this.f = f;
            this.a = a;
        }

        public abstract PointF[] Generate(float Phi0, float A, float f, int N, float? d = null);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
