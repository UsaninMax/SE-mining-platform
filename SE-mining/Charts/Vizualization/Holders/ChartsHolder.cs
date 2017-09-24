using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using SEMining.Charts.Vizualization.ViewModels;
using SE_mining_base.Charts.Vizualization.Configurations;

namespace SEMining.Charts.Vizualization.Holders
{
    public class ChartsHolder : IChartsHolder
    {
        private IDictionary<string, IChartViewModel> _map = new Dictionary<string, IChartViewModel>();

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
            if (predicate.Ids.IsNullOrEmpty())
            {
                throw new Exception("internal problem, predicate was stored incorrectly");
            }
            return _map[predicate.Ids.First()];
        }

        public IEnumerable<IChartViewModel> GetAll()
        {
            return _map.Values;
        }

        public void Set(IDictionary<string, IChartViewModel> map)
        {
            _map = map;
        }
    }
}
