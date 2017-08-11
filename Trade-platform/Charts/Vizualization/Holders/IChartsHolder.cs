using System.Collections.Generic;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.ViewModels;

namespace TradePlatform.Charts.Vizualization.Holders
{
    public interface IChartsHolder
    {
        void Set(IDictionary<string, IChartViewModel> map);
        IChartViewModel Get(string key);
        IChartViewModel Get(ChartViewPredicate predicate);
        IEnumerable<string> Get(IChartViewModel model);
        IEnumerable<IChartViewModel> GetAll();
    }
}
