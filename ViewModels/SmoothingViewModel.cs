using DSP.Filtering;
using DSP.Helpers;
using DSP.Smoothing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSP.ViewModels
{
    internal class SmoothingViewModel : INotifyPropertyChanged
    {

        private bool isSmoothingVisible;
        public bool IsSmoothingVisible
        {
            get => isSmoothingVisible;
            set
            {
                if (isSmoothingVisible != value)
                {
                    isSmoothingVisible = value;
                    OnPropertyChanged(nameof(IsSmoothingVisible));
                }
            }
        }

        private RelayCommand? changeSmoothingTypeCommand;
        public RelayCommand ChangeSmoothingTypeCommand
        {
            get
            {
                return changeSmoothingTypeCommand ??= new RelayCommand(obj =>
                {
                    SmoothingType? smoothingType = obj as SmoothingType?;
                    if (smoothingType != null)
                    {
                        SmoothingAlghoritm newSmoothing = smoothingType switch
                        {
                            SmoothingType.Parabolic => new SmoothingByParabolic(),
                            SmoothingType.MedianFilteringAlgorithm=> new MedianFilteringAlgorithm(5, 3),
                            SmoothingType.AlgorithmOfSlidingAveraging => new AlgorithmOfSlidingAveraging(5),
                            _ => new NoneSmoothingAlgorithm()
                        };

                        SelectedSmoothingAlgorithm = newSmoothing;
                    }
                });
            }
        }

        private SmoothingAlghoritm selectedSmoothingAlgorithm;
        public SmoothingAlghoritm SelectedSmoothingAlgorithm
        {
            get { return selectedSmoothingAlgorithm; }
            set
            {
                if (selectedSmoothingAlgorithm != value)
                {
                    selectedSmoothingAlgorithm.PropertyChanged -= (sender, args) =>
                    {
                        OnPropertyChanged(nameof(SelectedSmoothingAlgorithm));
                    };

                    selectedSmoothingAlgorithm = value;

                    selectedSmoothingAlgorithm.PropertyChanged += (sender, args) =>
                    {
                        OnPropertyChanged(nameof(SelectedSmoothingAlgorithm));
                    };

                    OnPropertyChanged(nameof(SelectedSmoothingAlgorithm));
                }
            }
        }

        public SmoothingViewModel()
        {
            selectedSmoothingAlgorithm = new NoneSmoothingAlgorithm();
            isSmoothingVisible = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}