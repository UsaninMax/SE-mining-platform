using System.Collections.Generic;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Vizualization.Populating.Predicates
{
    public class PreparedDataPredicate : ChartPredicate
    {
        public IList<IData> data { get; set; }
    }
}
