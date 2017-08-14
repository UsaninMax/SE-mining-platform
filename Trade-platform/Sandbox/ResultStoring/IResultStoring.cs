using System.Collections.Generic;

namespace TradePlatform.Sandbox.ResultProcessing
{
    public interface IResultStoring
    {
        void Store(IEnumerable<Dictionary<string, string>> data, string separator);
    }
}
