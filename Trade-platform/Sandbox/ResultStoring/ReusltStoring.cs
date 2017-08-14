using System.Collections.Generic;
using Microsoft.Practices.Unity;
using TradePlatform.Sandbox.ResultProcessing;

namespace TradePlatform.Sandbox.ResultStoring
{
    public static class ReusltStoring
    {
        public static void Store(IEnumerable<Dictionary<string, string>> data)
        {
            ContainerBuilder.Container.Resolve<IResultStoring>().Store(data, "/");
        }

        public static void Store(IEnumerable<Dictionary<string, string>> data, string separator)
        {
            ContainerBuilder.Container.Resolve<IResultStoring>().Store(data, separator);
        }
    }
}
