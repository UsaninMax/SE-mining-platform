using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Prism.Mvvm;

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

        public void Add(ChartValues<double> values)
        {
            Series.Add(new LineSeries {Values = values});
        }

        public void Add(ChartValues<OhlcPoint> serie)
        {
            Series.Add(new OhlcSeries {Values = serie});
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
