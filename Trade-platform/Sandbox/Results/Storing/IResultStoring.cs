using System.Collections.Generic;

namespace TradePlatform.Sandbox.Results.Storing
{
    public interface IResultStoring
    {
        void Store(IEnumerable<Dictionary<string, string>> data, string separator);
    }
}
