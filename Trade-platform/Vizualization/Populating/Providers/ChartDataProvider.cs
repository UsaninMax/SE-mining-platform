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

        private IList<IData> GetData(ChartPredicate predicate)
        {
            ISandboxDataHolder dataHolder = ContainerBuilder.Container.Resolve<ISandboxDataHolder>();
            var index = dataHolder.Get()
                .Where(x => predicate.DateTo == DateTime.MinValue || x.DateTime <= predicate.DateTo)
                .Select((value, i) => i).LastOrDefault();

            return dataHolder.Get()
                .Skip(index - predicate.GetCount < 0 ? 0 : index)
                .Take(index)
                .SelectMany(x => x.Datas)
                .Where(x => x.Key.Equals(predicate.InstrumentId))
                .Select(x => x.Value)
                .ToList();
        }
    }
}
