using System.Collections.Generic;
using LiveCharts.Wpf;

namespace TradePlatform.Vizualization.ViewModels
{
    public interface IChartViewModel
    {
        void Push(LineSeries series);
        void Push(OhlcSeries series);
        void AddLabels(IEnumerable<string> labels);
        void ClearAll();
    }
}
