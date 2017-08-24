using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Castle.Core.Internal;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using SEMining.Charts.Data.Populating;
using SE_mining_base.Charts.Data.Predicates.Basis;
using SE_mining_base.Sandbox.Models;
using SE_mining_base.Transactios.Enums;
using SE_mining_base.Transactios.Models;

namespace SEMining.Charts.Vizualization.ViewModels
{
    public class DateChartViewModel : BindableBase, IChartViewModel
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

        public DateTime From
        {
            get { return _from; }
            set
            {
                _from = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _from;

        public DateTime To
        {
            get { return _to; }
            set
            {
                _to = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _to;

        private ZoomingOptions _zoomingMode = ZoomingOptions.None;
        public ICommand ChangeToogleZoomingModeCommand { get; private set; }
        public ICommand ShowDataCommand { get; private set; }

        private readonly TimeSpan _xAxis;

        public DateChartViewModel(TimeSpan xAxis)
        {
            _xAxis = xAxis;
            ChangeToogleZoomingModeCommand = new DelegateCommand(ChangeToogleZoomingMode);
            ShowDataCommand = new DelegateCommand(UpdateCharts);
            ToogleZoomingModeText = "Zooming mode " + ZoomingMode;
            XFormatter = val => new DateTime((long)val * _xAxis.Ticks).ToString("dd/MM/yy HH:mm:ss");

        }

        private void UpdateCharts()
        {
            Task.Factory.StartNew(() => ContainerBuilder.Container.Resolve<IChartsPopulator>().Populate(this, From, To));
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

        public void Push(ICollection<Indicator> values, ChartPredicate predicate)
        {
            UpdateRange(predicate);
            if (values.IsNullOrEmpty())
            {
                return;
            }

            var mapper = Mappers.Xy<Indicator>()
                .X(x => x.Date().Ticks / _xAxis.Ticks)
                .Y(x => x.Value);

            Series.Add(new LineSeries(mapper)
            {
                Title = predicate.InstrumentId,
                StrokeThickness = 1,
                Fill = Brushes.Transparent,
                LineSmoothness = 1,
                PointGeometrySize = 2,
                Stroke = predicate.Color,
                PointForeground = predicate.Color,
                Foreground = predicate.Color,
                Values = new ChartValues<Indicator>(values)
            });
        }

        public void Push(ICollection<Candle> values, ChartPredicate predicate)
        {
            UpdateRange(predicate);
            if (values.IsNullOrEmpty())
            {
                return;
            }

            var mapper = Mappers.Financial<Candle>()
                .X(x => x.Date().Ticks / _xAxis.Ticks)
                .Open(x => x.Open)
                .Close(x => x.Close)
                .High(x => x.High)
                .Low(x => x.Low);

            Series.Add(new OhlcSeries(mapper)
            {
                Title = predicate.InstrumentId,
                StrokeThickness = 1.3,
                Values = new ChartValues<Candle>(values)
            });
        }

        public void Push(ICollection<double> values, ChartPredicate predicate)
        {
            throw new NotSupportedException("Cannot use double list in datetime chart");
        }

        public void Push(ICollection<Transaction> values)
        {
            if (values.IsNullOrEmpty())
            {
                return;
            }

            IEnumerable<Transaction> buyTransactions = values.Where(x => x.Direction.Equals(Direction.Buy)).ToList();
            IEnumerable<Transaction> sellTransactions = values.Where(x => x.Direction.Equals(Direction.Sell)).ToList();

            var mapper = Mappers.Xy<Transaction>()
                .X(x => x.Date.Ticks / _xAxis.Ticks)
                .Y(x => x.ExecutedPrice);

            if (buyTransactions.Any())
            {
                Series.Add(new LineSeries(mapper)
                {
                    Title = "Buy",
                    StrokeThickness = 0,
                    Fill = Brushes.Transparent,
                    LineSmoothness = 0,
                    PointGeometrySize = 10,
                    PointForeground = Brushes.Green,
                    Values = new ChartValues<Transaction>(values.Where(x => x.Direction.Equals(Direction.Buy)).ToList())
                });
            }

            if (sellTransactions.Any())
            {
                Series.Add(new LineSeries(mapper)
                {
                    Title = "Sell",
                    StrokeThickness = 0,
                    Fill = Brushes.Transparent,
                    LineSmoothness = 0,
                    PointGeometrySize = 10,
                    PointForeground = Brushes.Red,
                    Values = new ChartValues<Transaction>(values.Where(x => x.Direction.Equals(Direction.Sell)).ToList())
                });
            }
        }

        private void UpdateRange(ChartPredicate predicate)
        {
            if (predicate is DateChartPredicate)
            {
                From = (predicate as DateChartPredicate).From;
                To = (predicate as DateChartPredicate).To;
            }
        }
    }
}
