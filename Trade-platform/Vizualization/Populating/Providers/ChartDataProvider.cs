using System.Collections.Generic;
using TradePlatform.Sandbox.Holders;
using TradePlatform.Sandbox.Models;
using TradePlatform.Vizualization.Populating.Predicates;
using Microsoft.Practices.Unity;
using System.Linq;
using System;

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

        private IList<IData> GetData(ExistDataPredicate predicate)
        {
            ISandboxDataHolder dataHolder = ContainerBuilder.Container.Resolve<ISandboxDataHolder>();
            var skip = Math.Max(predicate.FromIndex != 0 ? predicate.FromIndex : dataHolder.Get().Count - predicate.GetCount, predicate.GetCount);
            return dataHolder.Get()
                .Skip(skip)
                .Take(predicate.GetCount)
                .SelectMany(x => x.Datas)
                .Where(x => x.Key.Equals(predicate.InstrumentId))
                .Select(x => x.Value)
                .ToList();
        }
    }
}
