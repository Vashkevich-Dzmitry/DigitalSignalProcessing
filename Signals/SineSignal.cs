using System;
using System.Drawing;

namespace DSP.Signals
{
    class SineSignal : GeneratedSignal
    {
        public override SignalType Type { get; set; } = SignalType.Sine;

        public SineSignal(float phi0, float f, float a) : base(phi0, f, a) { }

        public override PointF[] Generate(float phi0, float A, float f, int N, float? d = null)
        {
            PointF[] points = new PointF[N];
            for (int n = 0; n < N; n++)
            {
                points[n].X = n;
                points[n].Y = (float)Math.Round(A * Math.Sin(2 * Math.PI * f * n / N + phi0), 3);
            }

            return points;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
