using LiveCharts;
using LiveCharts.Wpf;
using Prism.Mvvm;
using System.Windows.Input;
using Prism.Commands;
using System;

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
        public LiveChartViewModel()
        {
            ChangeToogleZoomingModeCommand = new DelegateCommand(ChangeToogleZoomingMode);
            ToogleZoomingModeText = "Zooming mode " + ZoomingMode.ToString();
            XFormatter = (val =>
            {
                return new DateTime((long)val * TimeSpan.FromSeconds(1).Ticks).ToString("dd/MM/yy HH:mm:ss");
            });
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

        public void Push(LineSeries series)
        {
            Series.Add(series);
        }

        public void Push(OhlcSeries series)
        {
            Series.Add(series);
        }
    }
}
