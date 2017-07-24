using System.Collections.Generic;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Holders
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

        public void Set(IDictionary<string, IChartViewModel> map)
        {
            _map = map;
        }
    }
}
