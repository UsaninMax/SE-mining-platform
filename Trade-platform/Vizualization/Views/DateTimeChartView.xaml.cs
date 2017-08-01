using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Windows;
using System.Windows.Controls;
using TradePlatform.Sandbox.Models;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Views
{
    public partial class DateTimeChartView : UserControl
    {
        public DateTimeChartView(IChartViewModel model)
        {
            InitializeComponent();


            Charting.For<Candle>(Mappers.Financial<Candle>()
                .X(x => x.Date().Ticks / TimeSpan.FromSeconds(1).Ticks)
                .Open(x => x.Open)
                .Close(x => x.Close)
                .High(x => x.High)
                .Low(x => x.Low), SeriesOrientation.Horizontal);



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
