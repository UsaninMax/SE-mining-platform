using System.Collections.Generic;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Results.Adaptors
{
    public interface IReportAdaptor
    {
        IEnumerable<Dictionary<string, string>> Adopt(IList<Transaction> rows);
        IEnumerable<Dictionary<string, string>> Adopt(IList<BalanceRow> rows);
    }
}
