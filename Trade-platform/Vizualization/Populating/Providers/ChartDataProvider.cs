using System.Collections.Generic;
using TradePlatform.Sandbox.Holders;
using TradePlatform.Sandbox.Models;
using TradePlatform.Vizualization.Populating.Predicates;
using Microsoft.Practices.Unity;
using System.Linq;

namespace TradePlatform.Vizualization.Populating.Providers
{
    public class ChartDataProvider : IChartDataProvider
    {
        public IList<Indicator> Get(IndicatorDataPredicate predicate)
        {
           return GetData(predicate).OfType<Indicator>().ToList();
        }

        public IList<Candle> Get(CandlesDataPredicate predicate)
        {
            return GetData(predicate).OfType<Candle>().ToList();
        }

        private IList<IData> GetData(ChartPredicate predicate)
        {
            IList<Slice> slices = ContainerBuilder.Container.Resolve<ISandboxDataHolder>().Get();
            int index = predicate.Index == int.MaxValue ? slices.Count : predicate.Index;
            return slices
                .Skip(index - predicate.GetCount < 0 ? 0 : index - predicate.GetCount)
                .Take(index - predicate.GetCount < 0 ? index : predicate.GetCount)
                .SelectMany(x => x.Datas)
                .Where(x => x.Key.Equals(predicate.InstrumentId))
                .Select(x => x.Value)
                .ToList();
        }
    }
}
