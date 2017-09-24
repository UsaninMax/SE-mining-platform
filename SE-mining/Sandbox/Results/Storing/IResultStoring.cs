using System.Collections.Generic;

namespace SEMining.Sandbox.Results.Storing
{
    public interface IResultStoring
    {
        void Store(IEnumerable<Dictionary<string, string>> data, string separator);
        void Store(IList<double> data);
        void Store(IList<string> data);
    }
}
