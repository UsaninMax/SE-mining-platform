using LiveCharts;
using LiveCharts.Wpf;
using Prism.Mvvm;
using System.Windows.Input;
using Prism.Commands;
using System;
using TradePlatform.Sandbox.Models;
using LiveCharts.Configurations;
using System.Collections.Generic;

namespace TradePlatform.Vizualization.ViewModels
{
    public class LiveChartViewModel : BindableBase, IChartViewModel
    {
        public Func<double, string> XFormatter
        {
            get;
            set;
        }

        public SeriesCollection Series
        {
            get { return _series; }
            set
            {
                _series = value;
                RaisePropertyChanged();
            }
        }
        private SeriesCollection _series = new SeriesCollection();

        public ZoomingOptions ZoomingMode
        {
            get { return _zoomingMode; }
            set
            {
                _zoomingMode = value;
                RaisePropertyChanged();
            }
        }

        private ZoomingOptions _zoomingMode = ZoomingOptions.None;
        public ICommand ChangeToogleZoomingModeCommand { get; private set; }

        public LiveChartViewModel(long xAxisInterval)
        {
            ChangeToogleZoomingModeCommand = new DelegateCommand(ChangeToogleZoomingMode);
            ToogleZoomingModeText = "Zooming mode " + ZoomingMode.ToString();
            XFormatter = val => new DateTime((long)val * xAxisInterval).ToString("dd/MM/yy HH:mm:ss");

            Charting.For<Candle>(Mappers.Financial<Candle>()
                .X(x => x.Date().Ticks / xAxisInterval)
                .Open(x => x.Open)
                .Close(x => x.Close)
                .High(x => x.High)
                .Low(x => x.Low), SeriesOrientation.Horizontal);

            Charting.For<Indicator>(Mappers.Xy<Indicator>()
                .X(x => x.Date().Ticks / xAxisInterval)
                .Y(x => x.Value), SeriesOrientation.Horizontal);
        }

        private void ChangeToogleZoomingMode()
        {
            switch (ZoomingMode)
            {
                case ZoomingOptions.None:
                    ZoomingMode = ZoomingOptions.X;
                    ToogleZoomingModeText = "Zooming mode " + ZoomingMode.ToString();
                    break;
                case ZoomingOptions.X:
                    ZoomingMode = ZoomingOptions.Y;
                    ToogleZoomingModeText = "Zooming mode " + ZoomingMode.ToString();
                    break;
                case ZoomingOptions.Y:
                    ZoomingMode = ZoomingOptions.Xy;
                    ToogleZoomingModeText = "Zooming mode " + ZoomingMode.ToString();
                    break;
                case ZoomingOptions.Xy:
                    ZoomingMode = ZoomingOptions.None;
                    ToogleZoomingModeText = "Zooming mode " + ZoomingMode.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string ToogleZoomingModeText
        {
            get
            {
                return _toogleZoomingModeText;
            }
            set
            {
                _toogleZoomingModeText = value;
                RaisePropertyChanged();
            }
        }

        private string _toogleZoomingModeText;

        public void ClearAll()
        {
            Series.Clear();
        }

        public void Push(IList<Indicator> values)
        {
            Series.Add(new LineSeries()
            {
                Values = new ChartValues<Indicator>(values)
            });
        }

        public void Push(IList<Candle> values)
        {
            Series.Add(new OhlcSeries()
            {
                Values = new ChartValues<Candle>(values)
            });
        }
    }
}
