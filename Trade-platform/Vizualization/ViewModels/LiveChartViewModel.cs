using LiveCharts;
using LiveCharts.Wpf;
using Prism.Mvvm;
using System.Windows.Input;
using Prism.Commands;
using System;
using TradePlatform.Sandbox.Models;
using LiveCharts.Configurations;
using System.Collections.Generic;
using System.Windows.Media;
using System.Threading.Tasks;
using TradePlatform.Vizualization.Populating;
using Microsoft.Practices.Unity;
using TradePlatform.Sandbox.Transactios.Models;
using System.Reflection;
using System.Linq;
using TradePlatform.Sandbox.Transactios.Enums;
using Castle.Core.Internal;

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

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                UpdateCharts();
            }
        }
        private int _index;

        private ZoomingOptions _zoomingMode = ZoomingOptions.None;
        public ICommand ChangeToogleZoomingModeCommand { get; private set; }

        public LiveChartViewModel(TimeSpan xAxis)
        {
            ChangeToogleZoomingModeCommand = new DelegateCommand(ChangeToogleZoomingMode);
            ToogleZoomingModeText = "Zooming mode " + ZoomingMode.ToString();

            if (xAxis != TimeSpan.Zero)
            {
                XFormatter = val => new DateTime((long)val * xAxis.Ticks).ToString("dd/MM/yy HH:mm:ss");

                Charting.For<Candle>(Mappers.Financial<Candle>()
                    .X(x => x.Date().Ticks / xAxis.Ticks)
                    .Open(x => x.Open)
                    .Close(x => x.Close)
                    .High(x => x.High)
                    .Low(x => x.Low), SeriesOrientation.Horizontal);

                Charting.For<Indicator>(Mappers.Xy<Indicator>()
                    .X(x => x.Date().Ticks / xAxis.Ticks)
                    .Y(x => x.Value), SeriesOrientation.Horizontal);

                Charting.For<Transaction>(Mappers.Xy<Transaction>()
                    .X(x => x.Date.Ticks / xAxis.Ticks)
                    .Y(x => x.ExecutedPrice), SeriesOrientation.Horizontal);
            }
        }

        private void UpdateCharts()
        {
            Task.Factory.StartNew(() => ContainerBuilder.Container.Resolve<IChartsPopulator>().Populate(this, _index));
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
            Series.Add(new LineSeries
            {
                StrokeThickness = 1,
                Fill = Brushes.Transparent,
                LineSmoothness = 1,
                PointGeometrySize = 2,
                PointForeground = Brushes.Black,
                Foreground = Brushes.Black,
                Values = new ChartValues<Indicator>(values)
            });
        }

        public void Push(IList<Candle> values)
        {
            Series.Add(new OhlcSeries
            {
                StrokeThickness = 1.3,
                Values = new ChartValues<Candle>(values)
            });
        }

        public void Push(IList<double> values)
        {
            Series.Add(new LineSeries
            {
                StrokeThickness = 0,
                Fill = Brushes.Transparent,
                LineSmoothness = 0,
                PointGeometrySize = 5,
                PointForeground = Brushes.Black,
                Values = new ChartValues<double>(values)
            });
        }

        public void Push(IList<Transaction> values)
        {
            IList<Transaction> buyTransactions = values.Where(x => x.Direction.Equals(Direction.Buy)).ToList();
            IList<Transaction> sellTransactions = values.Where(x => x.Direction.Equals(Direction.Sell)).ToList();

            if (!buyTransactions.IsNullOrEmpty())
            {
                Series.Add(new LineSeries
                {
                    StrokeThickness = 0,
                    Fill = Brushes.Transparent,
                    LineSmoothness = 0,
                    PointGeometrySize = 10,
                    PointForeground = Brushes.Green,
                    Values = new ChartValues<Transaction>(values.Where(x => x.Direction.Equals(Direction.Buy)).ToList())
                });
            }

            if (!sellTransactions.IsNullOrEmpty())
            {
                Series.Add(new LineSeries
                {
                    StrokeThickness = 0,
                    Fill = Brushes.Transparent,
                    LineSmoothness = 0,
                    PointGeometrySize = 10,
                    PointForeground = Brushes.Red,
                    Values = new ChartValues<Transaction>(values.Where(x => x.Direction.Equals(Direction.Sell)).ToList())
                });
            }
        }
    }
}
