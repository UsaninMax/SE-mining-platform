using System.Windows;
using System.Windows.Controls;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Views
{
    public partial class DateTimeChartView : UserControl
    {
        public DateTimeChartView(IChartViewModel model)
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
