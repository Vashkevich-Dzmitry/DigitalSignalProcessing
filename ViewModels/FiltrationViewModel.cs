using DSP.Filtering;
using DSP.Helpers;
using DSP.Signals;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP.ViewModels
{
    internal class FiltrationViewModel : INotifyPropertyChanged
    {

        private bool isFiltrationVisible;
        public bool IsFiltrationVisible
        {
            get => isFiltrationVisible;
            set
            {
                if (isFiltrationVisible != value)
                {
                    isFiltrationVisible = value;
                    OnPropertyChanged(nameof(IsFiltrationVisible));
                }
            }
        }

        private RelayCommand? changeFiltrationTypeCommand;
        public RelayCommand ChangeFiltrationTypeCommand
        {
            get
            {
                return changeFiltrationTypeCommand ??= new RelayCommand(obj =>
                {
                    FiltrationType? filtrationType = obj as FiltrationType?;
                    if (filtrationType != null)
                    {
                        Filtration newFiltration = filtrationType switch
                        {
                            FiltrationType.LowFrequencies => new LowFrequenciesFiltration(SelectedFiltration.MaxHarmonic ?? 1),
                            FiltrationType.HighFrequencies => new HighFrequenciesFiltration(SelectedFiltration.MinHarmonic ?? 1),
                            FiltrationType.BandPass => new BandPassFiltration(SelectedFiltration.MinHarmonic ?? 1, SelectedFiltration.MaxHarmonic ?? (SelectedFiltration.MinHarmonic ?? 1)),
                            _ => new NoneFiltration()
                        };

                        SelectedFiltration = newFiltration;
                    }
                });
            }
        }

        private Filtration selectedFiltration;
        public Filtration SelectedFiltration
        {
            get { return selectedFiltration; }
            set
            {
                if (selectedFiltration != value)
                {
                    selectedFiltration.PropertyChanged -= (sender, args) =>
                    {
                        OnPropertyChanged(nameof(SelectedFiltration));
                    };

                    selectedFiltration = value;

                    selectedFiltration.PropertyChanged += (sender, args) =>
                    {
                        OnPropertyChanged(nameof(SelectedFiltration));
                    };

                    OnPropertyChanged(nameof(SelectedFiltration));
                }
            }
        }

        public FiltrationViewModel()
        {
            selectedFiltration = new NoneFiltration();
            isFiltrationVisible = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
