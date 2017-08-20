using System.Collections.Generic;
using SEMining.Charts.Vizualization.Configurations;
using SEMining.Charts.Vizualization.ViewModels;

namespace SEMining.Charts.Vizualization.Holders
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
