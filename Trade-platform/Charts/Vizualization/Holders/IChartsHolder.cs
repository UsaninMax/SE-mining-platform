using System.Collections.Generic;
using TradePlatform.Charts.Vizualization.ViewModels;

namespace TradePlatform.Charts.Vizualization.Holders
{
    public interface IChartsHolder
    {
        void Set(IDictionary<string, IChartViewModel> map);
        IChartViewModel Get(string key);
        IEnumerable<string> Get(IChartViewModel model);
        bool Exist(string key);
    }
}
