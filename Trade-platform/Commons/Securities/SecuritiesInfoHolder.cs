using System.Collections.Generic;
using System.Linq;

namespace TradePlatform.Commons.Securities
{
    public class SecuritiesInfoHolder
    {
        private IList<Security> _securities;
        public IList<Security> Securities
        {
            get { return _securities; }
            set { _securities = value; }

        }

        public IEnumerable<Market> Markets()
        {
            return new HashSet<Market>(_securities.Select(s => s.Market).OrderByDescending(o => o.Name).ToList());
        }

        public IEnumerable<Security> SecuritiesBy(Market chosenMarket)
        {
            return _securities.Where(w => w.Market.Equals(chosenMarket)).ToList();
        }
    }
}
