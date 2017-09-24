using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using SEMining.Sandbox.Holders;
using SE_mining_base.Sandbox.Models;

namespace SEMining.Charts.Data.Providers
{
    public class ChartDataProvider : IChartDataProvider
    {
        public IEnumerable<T> GetExistStorageData<T>(string instrumentId)
        {
            IEnumerable<Slice> slices = ContainerBuilder.Container.Resolve<ISandboxDataHolder>().Get();
            return slices
                .SelectMany(x => x.Datas)
                .Where(x => x.Key.Equals(instrumentId))
                .Select(x => x.Value)
                .OfType<T>()
                .ToList();
        }

        public IEnumerable<T> GetCustomStorageData<T>(string instrumentId)
        {
            IEnumerable<object> data = ContainerBuilder.Container.Resolve<ICustomDataHolder>().Get(instrumentId);
            return data
                .Cast<T>()
                .ToList();
        }
    }
}
