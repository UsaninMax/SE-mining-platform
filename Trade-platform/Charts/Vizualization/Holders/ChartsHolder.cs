using System;
using System.Collections.Generic;
using System.Linq;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.ViewModels;

namespace TradePlatform.Charts.Vizualization.Holders
{
    public class ChartsHolder : IChartsHolder
    {
        private IDictionary<string, IChartViewModel> _map = new Dictionary<string, IChartViewModel>();

        public bool Exist(string key)
        {
            return _map.ContainsKey(key);
        }

        public IChartViewModel Get(string key)
        {
            return _map[key];
        }

        public IEnumerable<string> Get(IChartViewModel model)
        {
            return _map.Where(x => x.Value.Equals(model)).Select(x => x.Key).Distinct();
        }

        public IChartViewModel Get(ChartViewPredicate predicate)
        {
            return _map[predicate.Ids.First()];
        }

        public void Set(IDictionary<string, IChartViewModel> map)
        {
            _map = map;
        }
    }
}
