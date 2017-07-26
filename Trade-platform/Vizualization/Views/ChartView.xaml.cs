using LiveCharts.Wpf;
using System.Windows;
using System.Windows.Controls;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Views
{
    public partial class ChartView : UserControl
    {

        public ChartView(IChartViewModel model)
        {
            InitializeComponent();
            DataContext = model;
            X.MaxValue = 100;
        }

        private void ResetZoomOnClick(object sender, RoutedEventArgs e)
        {
            //Use the axis MinValue/MaxValue properties to specify the values to display.
            //use double.Nan to clear it.

            X.MinValue = double.NaN;
            X.MaxValue = 100;
            Y.MinValue = double.NaN;
            Y.MaxValue = double.NaN;
        }
    }
}
