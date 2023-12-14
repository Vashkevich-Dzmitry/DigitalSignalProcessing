using DSP.ViewModels;
using System.Windows;

namespace DSP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SignalsViewModel signalsViewModel = new(SignalsPlot, CorrelationPlot);
            DataContext = signalsViewModel;
        }
    }
}
