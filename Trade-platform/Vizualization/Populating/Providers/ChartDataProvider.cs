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
            return dataHolder
                .Get()
                .Where(y => (predicate.From == DateTime.MinValue || y.DateTime >= predicate.From) &&
                            (predicate.To == DateTime.MinValue || y.DateTime <= predicate.To))
                .SelectMany(x => x.Datas)
                .Where(x => x.Key.Equals(predicate.InstrumentId))
                .Select(x => x.Value)
                .ToList();
        }
    }
}
