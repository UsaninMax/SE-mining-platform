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
        private const int COUNT = 100;

        public IList<T> GetExistStorageData<T>(ChartPredicate predicate)
        {
            IList<Slice> slices = ContainerBuilder.Container.Resolve<ISandboxDataHolder>().Get();
            int index = predicate.Index == int.MaxValue ? slices.Count : predicate.Index;
            return slices
                .Skip(index - COUNT < 0 ? 0 : index - COUNT)
                .Take(index - COUNT < 0 ? index : COUNT)
                .SelectMany(x => x.Datas)
                .Where(x => x.Key.Equals(predicate.InstrumentId))
                .Select(x => x.Value)
                .OfType<T>()
                .ToList();
        }

        public IList<T> GetCustomStorageData<T>(ChartPredicate predicate)
        {
            IList<object> data = ContainerBuilder.Container.Resolve<ICustomDataHolder>().Get(predicate.InstrumentId);
            int index = predicate.Index == int.MaxValue ? data.Count : predicate.Index;
            return data
                .Skip(index - COUNT < 0 ? 0 : index - COUNT)
                .Take(index - COUNT < 0 ? index : COUNT)
                .Cast<T>()
                .ToList();
        }
    }
}
