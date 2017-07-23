using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Defaults;

namespace TradePlatform.Vizualization.ViewModels
{
    public interface IChartViewModel
    {
        void Add(ChartValues<double> values);
        void Add(ChartValues<OhlcPoint> serie);
        void AddLabels(IEnumerable<string> labels);
        void ClearAll();
    }
}
