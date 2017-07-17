using System.Collections.Generic;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Enums;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public interface ITransactionHolder
    {
        void UpdateOpenTransactions(Transaction transaction);
        double GetCoverage(IDictionary<string, Tick> ticks, IEnumerable<OpenPositionRequest> activeRequests);
        double GetCoverage(IDictionary<string, Tick> ticks, IEnumerable<OpenPositionRequest> activeRequests, OpenPositionRequest newRequest);
        IList<Transaction> GetOpenTransactions();
        IList<Transaction> GetOpenTransactions(string instrumentId, Direction direction);
        IList<Transaction> GetOpenTransactions(string instrumentId);
        int GetSize();
        void Reset();
    }
}
