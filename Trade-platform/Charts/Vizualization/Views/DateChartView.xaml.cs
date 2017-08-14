using System.Windows;
using TradePlatform.Charts.Vizualization.ViewModels;

namespace TradePlatform.Charts.Vizualization.Views
{
    public partial class DateChartView
    {
        public DateChartView(IChartViewModel model)
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
