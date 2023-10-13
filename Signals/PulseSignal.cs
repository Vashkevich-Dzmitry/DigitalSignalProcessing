using System;
using System.Drawing;

namespace DSP.Signals
{
    class PulseSignal : GeneratedSignal
    {
        public override SignalType Type { get; set; } = SignalType.Pulse;

        public PulseSignal(float phi0, float f, float a, float d): base(phi0, f, a)
        {
            this.d = d;
        }

        public override PointF[] Generate(float phi0, float A, float f, int N, float? d = 0.5f)
        {
            PointF[] points = new PointF[N];
            for (int n = 0; n < N; n++)
            {
                points[n].X = n / (float)N;
                points[n].Y = (2 * Math.PI * f * n / N + phi0) % (2 * Math.PI) / (2 * Math.PI) <= d ? A : -A;
            }

            return points;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
