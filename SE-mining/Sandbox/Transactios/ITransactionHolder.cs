using System.Collections.Generic;
using SE_mining_base.Sandbox.Models;
using SE_mining_base.Transactios.Enums;
using SE_mining_base.Transactios.Models;

namespace SEMining.Sandbox.Transactios
{
    public interface ITransactionHolder
    {
        void UpdateOpenTransactions(Transaction transaction);
        double GetCoverage(IDictionary<string, Tick> ticks, IEnumerable<OpenPositionRequest> activeRequests);
        double GetCoverage(IDictionary<string, Tick> ticks, IEnumerable<OpenPositionRequest> activeRequests, OpenPositionRequest newRequest);
        IEnumerable<Transaction> GetOpenTransactions();
        IEnumerable<Transaction> GetInvertedOpenTransactions(string instrumentId, Direction direction);
        IEnumerable<Transaction> GetOpenTransactions(string instrumentId, Direction direction);
        IEnumerable<Transaction> GetOpenTransactions(string instrumentId);
        int GetNumberOfOpenTransactions(string instrumentId);
        int GetSize();
        void Reset();
    }
}
