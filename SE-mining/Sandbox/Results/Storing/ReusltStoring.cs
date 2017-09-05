using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace SEMining.Sandbox.Results.Storing
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

        public static void Store(IList<double> data)
        {
            ContainerBuilder.Container.Resolve<IResultStoring>().Store(data);
        }
    }
}
