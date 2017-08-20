using System.Windows;
using SEMining.Charts.Vizualization.ViewModels;

namespace SEMining.Charts.Vizualization.Views
{
    public partial class IndexChartView
    {
        public IndexChartView(IChartViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void ResetZoomOnClick(object sender, RoutedEventArgs e)
        {
            X.MinValue = double.NaN;
            X.MaxValue = double.NaN;
            Y.MinValue = double.NaN;
            Y.MaxValue = double.NaN;
        }
    }
}
