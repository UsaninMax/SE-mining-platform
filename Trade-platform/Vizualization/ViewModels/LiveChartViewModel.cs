using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Prism.Mvvm;
using System.Windows.Input;
using Prism.Commands;
using System;
using System.Windows.Media;

namespace TradePlatform.Vizualization.ViewModels
{
    public class LiveChartViewModel : BindableBase, IChartViewModel
    {
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

        public ObservableCollection<string> Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<string> _labels = new ObservableCollection<string>();

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
        public LiveChartViewModel()
        {
            ChangeToogleZoomingModeCommand = new DelegateCommand(ChangeToogleZoomingMode);
            ToogleZoomingModeText = "Zooming mode " + ZoomingMode.ToString();
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

        public void Add(ChartValues<double> values)
        {
            Series.Add(new LineSeries
            {
                Values = values,
                Foreground = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
                LineSmoothness = 1,
                StrokeThickness = 1.5
            });
        }

        public void Add(ChartValues<OhlcPoint> values)
        {
            Series.Add(new OhlcSeries {
                Values = values,
                Foreground = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
                StrokeThickness = 1.5
            });
        }

        public void Add(ChartValues<DateTimePoint> values)
        {
            Series.Add(new LineSeries {
                Values = values,
                Foreground = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
                LineSmoothness = 1,
                StrokeThickness = 1.5
            });
        }

        public void AddLabels(IEnumerable<string> labels)
        {
            Labels = new ObservableCollection<string>(labels);
        }

        public void ClearAll()
        {
            Series.Clear();
            Labels.Clear();
        }
    }
}
