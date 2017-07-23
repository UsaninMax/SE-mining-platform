using LiveCharts;
using LiveCharts.Defaults;

namespace TradePlatform.Vizualization.ViewModels
{
    public interface IOhclChartViewModel
    {
        void AddLine(string id, ChartValues<OhlcPoint> values);
        void AddSeries(string id, ChartValues<OhlcPoint> serie);
        void Clean(string id);
        void CleanAll(string id);
    }
}
