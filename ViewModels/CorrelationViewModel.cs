using DSP.Correlations;
using DSP.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace DSP.ViewModels
{
    class CorrelationViewModel : INotifyPropertyChanged
    {

        private bool isCorrelationPanelVisible;
        public bool IsCorrelationPanelVisible
        {
            get => isCorrelationPanelVisible;
            set
            {
                if (isCorrelationPanelVisible != value)
                {
                    isCorrelationPanelVisible = value;
                    OnPropertyChanged(nameof(IsCorrelationPanelVisible));
                }
            }
        }

        private RelayCommand? changeCorrelationTypeCommand;
        public RelayCommand ChangeCorrelationTypeCommand
        {
            get
            {
                return changeCorrelationTypeCommand ??= new RelayCommand(obj =>
                {
                    CorrelationType? correlationType = obj as CorrelationType?;
                    if (correlationType != null)
                    {
                        ICorrelation newCorrelation = correlationType switch
                        {
                            CorrelationType.Linear => new LinearCorrelation(),
                            _ => new FastCorrelation()
                        };

                        SelectedAlgorithm = newCorrelation;
                    }
                });
            }
        }

        private ICorrelation selectedAlgorithm;
        public ICorrelation SelectedAlgorithm
        {
            get { return selectedAlgorithm; }
            set
            {
                if (selectedAlgorithm != value)
                {
                    //selectedCorrelationAlgorithm.PropertyChanged -= (sender, args) =>
                    //{
                    //    OnPropertyChanged(nameof(SelectedCorrelationAlgorithm));
                    //};

                    selectedAlgorithm = value;

                    //selectedCorrelationAlgorithm.PropertyChanged += (sender, args) =>
                    //{
                    //    OnPropertyChanged(nameof(SelectedCorrelationAlgorithm));
                    //};

                    OnPropertyChanged(nameof(SelectedAlgorithm));
                }
            }
        }

        public CorrelationViewModel()
        {
            selectedAlgorithm = new LinearCorrelation();
            isCorrelationPanelVisible = false;
        }

        public (ObservableCollection<double> x, ObservableCollection<double> y) Find(ObservableCollection<double> firstSignal, ObservableCollection<double> secondSignal)
        {
            ObservableCollection<double> x = new ();
            ObservableCollection<double> y = new ();

            double[] correlationResult = SelectedAlgorithm.FindCorrelation(firstSignal.ToArray(), secondSignal.ToArray());

            for (int i = 0; i < correlationResult.Length; i++)
            {
                x.Add(i);
                y.Add(correlationResult[i]);
            }

            return (x, y);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}