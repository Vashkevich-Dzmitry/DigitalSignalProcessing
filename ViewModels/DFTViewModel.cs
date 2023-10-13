using DSP.ViewModels;
using ScottPlot;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace DSP
{
    class DFTViewModel : INotifyPropertyChanged
    {
        public FiltrationViewModel Filtration { get; set; }
        public WpfPlot AmplitudePlot { get; set; }
        public WpfPlot PhasePlot { get; set; }

        public ObservableCollection<Complex> ComplexValues { get; set; }

        private bool isComplexesVisible;
        public bool IsComplexesVisible
        {
            get => isComplexesVisible;
            set
            {
                if (isComplexesVisible != value)
                {
                    isComplexesVisible = value;
                    OnPropertyChanged(nameof(IsComplexesVisible));
                }
            }
        }

        public DFTViewModel(FiltrationViewModel filtration, WpfPlot phasePlot, WpfPlot amplitudePlot)
        {
            isComplexesVisible = false;

            Filtration = filtration;

            PhasePlot = phasePlot;
            AmplitudePlot = amplitudePlot;
            ComplexValues = new ObservableCollection<Complex>();
        }

        public double[] ExecuteDFT(double[] values, int k, int N)
        {
            double[] sinSpectrum = new double[k], cosSpectrum = new double[k], amplitudeSpectrum = new double[k], phaseSpectrum = new double[k];
            double[] restoredValues = new double[N];
            Complex[] complexes = new Complex[k];

                Parallel.For(0, k, (i, state) =>
                {
                    sinSpectrum[i] = 2 * values.AsParallel().Select((v, j) => (v, j)).Sum((p) => p.v * Math.Sin(2 * Math.PI * p.j * i / k)) / k;
                    cosSpectrum[i] = 2 * values.AsParallel().Select((v, j) => (v, j)).Sum((p) => p.v * Math.Cos(2 * Math.PI * p.j * i / k)) / k;

                    complexes[i] = new(cosSpectrum[i], sinSpectrum[i]);

                    amplitudeSpectrum[i] = Filtration.SelectedFiltration.Filter(i, Math.Sqrt(Math.Pow(sinSpectrum[i], 2) + Math.Pow(cosSpectrum[i], 2)));
                    phaseSpectrum[i] = Math.Atan2(cosSpectrum[i], sinSpectrum[i]);

                });

            ComplexValues.Clear();
            foreach (Complex complex in complexes) ComplexValues.Add(complex);

            PhasePlot.Plot.Clear();
            PhasePlot.Plot.AddBar(phaseSpectrum, System.Drawing.Color.LightGreen);
            PhasePlot.Refresh();

            AmplitudePlot.Plot.Clear();
            AmplitudePlot.Plot.AddBar(amplitudeSpectrum, System.Drawing.Color.LightGreen);
            AmplitudePlot.Refresh();


            Parallel.For(0, N, (i, state) =>
            {
                var amplitudesIndexed = amplitudeSpectrum.Take(k / 2).Select((a, j) => (a, j));
                var phasesIndexed = phaseSpectrum.Take(k / 2).Select((ph, j) => (ph, j));
                restoredValues[i] = amplitudeSpectrum[0] / 2 + amplitudesIndexed.Join(phasesIndexed, ap => ap.j, php => php.j, (ap, php) => (ap.a, php.ph, php.j)).Select(p => p.a * Math.Sin(2 * Math.PI * i * p.j / N + p.ph)).Sum();
            });

            return restoredValues;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
