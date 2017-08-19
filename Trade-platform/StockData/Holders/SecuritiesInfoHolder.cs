using System.Collections.Generic;
using System.Linq;
using TradePlatform.StockData.Models;

namespace TradePlatform.StockData.Holders
{
    public class SecuritiesInfoHolder
    {
        public IEnumerable<Security> Securities { get; set; }

        public IEnumerable<Market> Markets()
        {
            return new HashSet<Market>(Securities.Select(s => s.Market).OrderByDescending(o => o.Name).ToList());
        }

        public IEnumerable<Security> SecuritiesBy(Market chosenMarket)
        {
            return Securities.Where(w => w.Market.Equals(chosenMarket)).ToList();
        }
    }
}
