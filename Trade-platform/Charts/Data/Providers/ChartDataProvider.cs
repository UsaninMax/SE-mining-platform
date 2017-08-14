using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using TradePlatform.Sandbox.Holders;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Charts.Data.Providers
{
    public class ChartDataProvider : IChartDataProvider
    {
        public IList<T> GetExistStorageData<T>(string instrumentId)
        {
            IList<Slice> slices = ContainerBuilder.Container.Resolve<ISandboxDataHolder>().Get();
            return slices
                .SelectMany(x => x.Datas)
                .Where(x => x.Key.Equals(instrumentId))
                .Select(x => x.Value)
                .OfType<T>()
                .ToList();
        }

        public IList<T> GetCustomStorageData<T>(string instrumentId)
        {
            IList<object> data = ContainerBuilder.Container.Resolve<ICustomDataHolder>().Get(instrumentId);
            return data
                .Cast<T>()
                .ToList();
        }
    }
}
