using DSP.Helpers;
using DSP.Noises;
using DSP.Signals;
using ScottPlot;
using ScottPlot.Renderable;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DSP.ViewModels
{
    class SignalsViewModel : INotifyPropertyChanged
    {
        private int n;
        public int N
        {
            get => n;
            set
            {
                if (n != value)
                {
                    n = value;

                    (ResultingX1, ResultingY1) = ComputeResultingSignal(Signals1, Noises1, N);

                    OnPropertyChanged(nameof(N));
                    
                    MinX = 0;
                    MaxX = value;
                    
                }
            }
        }

        private int minX;
        public int MinX
        {
            get => minX;
            set
            {
                if (value <= maxX)
                {
                    minX = value;

                    (ResultingX2, ResultingY2) = ComputeResultingSignal(Signals2, Noises2, N, MinX, MaxX);

                    OnPropertyChanged(nameof(MinX));
                }

            }
        }

        private int maxX;
        public int MaxX
        {
            get => maxX;
            set
            {
                if (value >= minX)
                {
                    maxX = value;

                    (ResultingX2, ResultingY2) = ComputeResultingSignal(Signals2, Noises2, N, MinX, MaxX);

                    OnPropertyChanged(nameof(MaxX));
                }
            }
        }


        private GeneratedSignal? selectedSignal1;
        private GeneratedSignal? selectedSignal2;
        public GeneratedSignal? SelectedSignal1
        {
            get { return selectedSignal1; }
            set
            {
                if (selectedSignal1 != value)
                {
                    if (selectedSignal1 != null)
                    {
                        selectedSignal1.PropertyChanged -= (sender, args) =>
                        {
                            (ResultingX1, ResultingY1) = ComputeResultingSignal(Signals1, Noises1, N);
                        };
                    }

                    selectedSignal1 = value;
                    IsSignal1Selected = selectedSignal1 != null;

                    if (selectedSignal1 != null)
                    {
                        selectedSignal1.PropertyChanged += (sender, args) =>
                        {
                            (ResultingX1, ResultingY1) = ComputeResultingSignal(Signals1, Noises1, N);
                        };
                    }

                    OnPropertyChanged(nameof(SelectedSignal1));
                }
            }
        }

        public GeneratedSignal? SelectedSignal2
        {
            get { return selectedSignal2; }
            set
            {
                if (selectedSignal1 != value)
                {
                    if (selectedSignal2 != null)
                    {
                        selectedSignal2.PropertyChanged -= (sender, args) =>
                        {
                            (ResultingX2, ResultingY2) = ComputeResultingSignal(Signals2, Noises2, N, MinX, MaxX);
                        };
                    }

                    selectedSignal2 = value;
                    IsSignal2Selected = selectedSignal2 != null;

                    if (selectedSignal2 != null)
                    {
                        selectedSignal2.PropertyChanged += (sender, args) =>
                        {
                            (ResultingX2, ResultingY2) = ComputeResultingSignal(Signals2, Noises2, N, MinX, MaxX);
                        };
                    }

                    OnPropertyChanged(nameof(SelectedSignal2));
                }
            }
        }


        private bool isSignal1Selected;
        private bool isSignal2Selected;
        public bool IsSignal1Selected
        {
            get => isSignal1Selected;
            set
            {
                if (isSignal1Selected != value)
                {
                    isSignal1Selected = value;
                    OnPropertyChanged(nameof(IsSignal1Selected));
                }
            }
        }

        public bool IsSignal2Selected
        {
            get => isSignal2Selected;
            set
            {
                if (isSignal2Selected != value)
                {
                    isSignal2Selected = value;
                    OnPropertyChanged(nameof(IsSignal2Selected));
                }
            }
        }


        private ObservableCollection<double> resultingY1;
        public ObservableCollection<double> ResultingY1
        {
            get => resultingY1;
            set
            {
                resultingY1 = value;
                OnPropertyChanged(nameof(ResultingY1));

                DrawCharts();
            }
        }


        private ObservableCollection<double> resultingX1;
        public ObservableCollection<double> ResultingX1
        {
            get => resultingX1;
            set
            {
                resultingX1 = value;
                OnPropertyChanged(nameof(ResultingX1));
            }
        }





        private ObservableCollection<double> resultingY2;
        public ObservableCollection<double> ResultingY2
        {
            get => resultingY2;
            set
            {
                resultingY2 = value;
                OnPropertyChanged(nameof(ResultingY2));

                DrawCharts();
            }
        }


        private ObservableCollection<double> resultingX2;
        public ObservableCollection<double> ResultingX2
        {
            get => resultingX2;
            set
            {
                resultingX2 = value;
                OnPropertyChanged(nameof(ResultingX2));
            }
        }

        public (ObservableCollection<double> x, ObservableCollection<double> y) ComputeResultingSignal(ObservableCollection<GeneratedSignal> signals, NoisesViewModel noises, int pointsAmount, int? minX = null, int? maxX = null)
        {
            ConcurrentBag<(double x, double y)> concurrentResult = new();

            minX = minX.HasValue ? minX.Value : 0;
            maxX = maxX.HasValue ? maxX.Value : pointsAmount - 1;

            Parallel.ForEach(signals.SelectMany(signal => noises.MakeNoise(signal.Generate(signal.Phi0, signal.A, signal.F, pointsAmount, signal.D), signal.A)).Where(point => point.X >= minX && point.X <= maxX).GroupBy(point => point.X),
                group =>
                {
                    concurrentResult.Add((group.Key, group.Sum(point => point.Y)));
                });

            return (new ObservableCollection<double>(concurrentResult.OrderBy(item => item.x).Select(item => item.x - minX.Value)),
                new ObservableCollection<double>(concurrentResult.OrderBy(item => item.x).Select(item => item.y)));
        }




        private RelayCommand? addSignal1Command;
        private RelayCommand? addSignal2Command;
        private RelayCommand? deleteSignal1Command;
        private RelayCommand? deleteSignal2Command;
        private RelayCommand? changeSignal1TypeCommand;
        private RelayCommand? changeSignal2TypeCommand;

        public RelayCommand AddSignal1Command
        {
            get
            {
                return addSignal1Command ??= new RelayCommand(obj =>
                {
                    GeneratedSignal signal = new SineSignal(0, 1, 1);
                    Signals1.Insert(0, signal);
                    SelectedSignal1 = signal;

                    (_, ResultingY1) = ComputeResultingSignal(Signals1, Noises1, N);
                }, (obj) => Signals1.Count < 5);
            }
        }
        public RelayCommand AddSignal2Command
        {
            get
            {
                return addSignal2Command ??= new RelayCommand(obj =>
                {
                    GeneratedSignal signal = new SineSignal(0, 1, 1);
                    Signals2.Insert(0, signal);
                    SelectedSignal2 = signal;

                    (_, ResultingY2) = ComputeResultingSignal(Signals2, Noises2, N, MinX, MaxX);
                }, (obj) => Signals2.Count < 5);
            }
        }
        public RelayCommand DeleteSignal1Command
        {
            get
            {
                return deleteSignal1Command ??= new RelayCommand(obj =>
                {
                    if (obj is GeneratedSignal signal)
                    {
                        Signals1.Remove(signal);
                        SelectedSignal1 = null;

                        (_, ResultingY1) = ComputeResultingSignal(Signals1, Noises1, N);
                    }
                }, (obj) => SelectedSignal1 != null && Signals1.Count > 1);
            }
        }
        public RelayCommand DeleteSignal2Command
        {
            get
            {
                return deleteSignal2Command ??= new RelayCommand(obj =>
                {
                    if (obj is GeneratedSignal signal)
                    {
                        Signals2.Remove(signal);
                        SelectedSignal2 = null;

                        (_, ResultingY2) = ComputeResultingSignal(Signals2, Noises2, N, MinX, MaxX);
                    }
                }, (obj) => SelectedSignal2 != null && Signals2.Count > 1);
            }
        }
        public RelayCommand ChangeSignal1TypeCommand
        {
            get
            {
                return changeSignal1TypeCommand ??= new RelayCommand(obj =>
                {
                    SignalType? signalType = obj as SignalType?;
                    if (signalType != null && signalType == SelectedSignal1!.Type)
                    {
                        GeneratedSignal newSignal = signalType switch
                        {
                            SignalType.Triangle => new TriangleSignal(SelectedSignal1!.Phi0, SelectedSignal1!.F, SelectedSignal1!.A),
                            SignalType.Sawtooth => new SawtoothSignal(SelectedSignal1!.Phi0, SelectedSignal1!.F, SelectedSignal1!.A),
                            SignalType.Pulse => new PulseSignal(SelectedSignal1!.Phi0, SelectedSignal1!.F, SelectedSignal1!.A, 0.5f),
                            SignalType.Sine => new SineSignal(SelectedSignal1!.Phi0, SelectedSignal1!.F, SelectedSignal1!.A),
                            _ => new CosineSignal(SelectedSignal1!.Phi0, SelectedSignal1!.F, SelectedSignal1!.A),
                        };

                        int signalIndex = Signals1.IndexOf(SelectedSignal1);
                        Signals1.Insert(signalIndex, newSignal);
                        SelectedSignal1 = Signals1[signalIndex];
                        Signals1.RemoveAt(signalIndex + 1);

                        (_, ResultingY1) = ComputeResultingSignal(Signals1, Noises1, N);
                    }
                }, (obj) => SelectedSignal1 != null && obj != null);
            }
        }
        public RelayCommand ChangeSignal2TypeCommand
        {
            get
            {
                return changeSignal2TypeCommand ??= new RelayCommand(obj =>
                {
                    SignalType? signalType = obj as SignalType?;
                    if (signalType != null && signalType == SelectedSignal2!.Type)
                    {
                        GeneratedSignal newSignal = signalType switch
                        {
                            SignalType.Triangle => new TriangleSignal(SelectedSignal2!.Phi0, SelectedSignal2!.F, SelectedSignal2!.A),
                            SignalType.Sawtooth => new SawtoothSignal(SelectedSignal2!.Phi0, SelectedSignal2!.F, SelectedSignal2!.A),
                            SignalType.Pulse => new PulseSignal(SelectedSignal2!.Phi0, SelectedSignal2!.F, SelectedSignal2!.A, 0.5f),
                            SignalType.Sine => new SineSignal(SelectedSignal2!.Phi0, SelectedSignal2!.F, SelectedSignal2!.A),
                            _ => new CosineSignal(SelectedSignal2!.Phi0, SelectedSignal2!.F, SelectedSignal2!.A),
                        };

                        int signalIndex = Signals2.IndexOf(SelectedSignal2);
                        Signals2.Insert(signalIndex, newSignal);
                        SelectedSignal2 = Signals2[signalIndex];
                        Signals2.RemoveAt(signalIndex + 1);

                        (_, ResultingY2) = ComputeResultingSignal(Signals2, Noises2, N, MinX, MaxX);
                    }
                }, (obj) => SelectedSignal2 != null && obj != null);
            }
        }

        public NoisesViewModel Noises1 { get; set; }
        public NoisesViewModel Noises2 { get; set; }

        public ObservableCollection<GeneratedSignal> Signals1 { get; set; }
        public ObservableCollection<GeneratedSignal> Signals2 { get; set; }

        public WpfPlot SignalsPlot { get; set; }

        public SignalsViewModel(WpfPlot signalsPlot, WpfPlot phasePlot, WpfPlot amplitudePlot)
        {
            n = 256;
            minX = 0;
            maxX = 256;

            Noises1 = new();
            Noises1.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Noises1.P))
                {
                    (_, ResultingY1) = ComputeResultingSignal(Signals1, Noises1, N);
                }
            };

            Signals1 = new ObservableCollection<GeneratedSignal>
            {
                new SineSignal(0, 1, 1)
            };

            Noises2 = new();
            Noises2.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Noises2.P))
                {
                    (_, ResultingY2) = ComputeResultingSignal(Signals2, Noises2, N, MinX, MaxX);
                }
            };

            Signals2 = new ObservableCollection<GeneratedSignal>
            {
                new SineSignal(0, 1, 1)
            };

            SignalsPlot = signalsPlot;

            (resultingX1, resultingY1) = ComputeResultingSignal(Signals1, Noises1, N);
            (resultingX2, resultingY2) = ComputeResultingSignal(Signals2, Noises2, N, MinX, MaxX);

            DrawCharts();
        }

        private void DrawCharts()
        {
            SignalsPlot.Plot.Clear();
            SignalsPlot.Plot.SetAxisLimits(xMin: 0, xMax: N);
            SignalsPlot.Plot.AddScatter(ResultingX1.ToArray(), ResultingY1.ToArray(), System.Drawing.Color.LightGreen, 3);
            SignalsPlot.Plot.AddScatter(ResultingX2.ToArray(), ResultingY2.ToArray(), System.Drawing.Color.Blue, 3);
            SignalsPlot.Refresh();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
