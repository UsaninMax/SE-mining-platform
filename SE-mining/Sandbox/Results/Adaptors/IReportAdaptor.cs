using System.Collections.Generic;
using SEMining.Sandbox.Transactios.Models;

namespace SEMining.Sandbox.Results.Adaptors
{
    public interface IReportAdaptor
    {
        IEnumerable<Dictionary<string, string>> Adopt(IEnumerable<Transaction> rows);
        IEnumerable<Dictionary<string, string>> Adopt(IEnumerable<BalanceRow> rows);
        IEnumerable<Dictionary<string, string>> Adopt(IEnumerable<OpenPositionRequest> rows);
    }
}
