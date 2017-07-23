using System;
using System.Collections.Generic;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Prism.Mvvm;

namespace TradePlatform.Vizualization.ViewModels
{
    public class OhclChartViewModel : BindableBase, IOhclChartViewModel
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                RaisePropertyChanged();
            }
        }
        private string[] _labels;


        public OhclChartViewModel()
        {
            SeriesCollection = new SeriesCollection
            {
                new OhlcSeries()
                {
                    Values = new ChartValues<OhlcPoint>
                    {
                        new OhlcPoint(32, 35, 30, 32),
                        new OhlcPoint(33, 38, 31, 37),
                        new OhlcPoint(35, 42, 30, 40),
                        new OhlcPoint(37, 40, 35, 38),
                        new OhlcPoint(35, 38, 32, 33)
                    }

                },
                new LineSeries
                {
                    Values = new ChartValues<double> {30, 32, 35, 30, 28},
                    Fill = Brushes.Transparent
                }
            };

            _labels = new[]
            {
                DateTime.Now.ToString("dd MMM"),
                DateTime.Now.AddDays(1).ToString("dd MMM"),
                DateTime.Now.AddDays(2).ToString("dd MMM"),
                DateTime.Now.AddDays(3).ToString("dd MMM"),
                DateTime.Now.AddDays(4).ToString("dd MMM"),
            };
        }

        public void AddLine(string id, ChartValues<OhlcPoint> values)
        {
            throw new NotImplementedException();
        }

        public void AddSeries(string id, ChartValues<OhlcPoint> serie)
        {
   
            throw new NotImplementedException();
        }

        public void Clean(string id)
        {
            throw new NotImplementedException();
        }

        public void CleanAll(string id)
        {
            throw new NotImplementedException();
        }

        private IDictionary<string, int> _indexMapper = new Dictionary<string, int>();
        private int GetChartIndexById(string id)
        {
            if (_indexMapper.ContainsKey(id))
            {
                return _indexMapper[id];
            }

            _indexMapper.Add(id, SeriesCollection.Count);
            return SeriesCollection.Count;
        }
    }
}
