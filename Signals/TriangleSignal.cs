using System;
using System.Drawing;

namespace DSP.Signals
{
    class TriangleSignal : GeneratedSignal
    {
        public override SignalType Type { get; set; } = SignalType.Triangle;

        public TriangleSignal(float phi0, float f, float a) : base(phi0, f, a) { }

        public override PointF[] Generate(float phi0, float A, float f, int N, float? d = null)
        {
            PointF[] points = new PointF[N];
            for (int n = 0; n < N; n++)
            {
                points[n].X = n;
                points[n].Y = (float)Math.Round((2 * A / Math.PI) * Math.Abs(Math.Abs((((2 * Math.PI * f * n) / N + phi0 - (Math.PI / 2)) % (2 * Math.PI))) - Math.PI) - A, 3);
            }

            return points;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
