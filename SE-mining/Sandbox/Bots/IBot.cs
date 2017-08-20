using System.Collections.Generic;
using SEMining.Charts.Data.Predicates.Basis;
using SEMining.Sandbox.Models;
using SEMining.Sandbox.Transactios.Enums;
using SEMining.Sandbox.Transactios.Models;

namespace SEMining.Sandbox.Bots
{
    public interface IBot
    {
        string GetId();
        void SetUpId(string id);
        void SetUpSandboxId(string id);
        void SetUpPredicate(BotPredicate predicate);
        void Execute();
        void Execution(IDictionary<string, IData> data);
        int Score();
        void SetUpWorkingPeriod(IDictionary<string, WorkingPeriod> value);
        void SetUpBalance(double value);
        void OpenPosition(OpenPositionRequest request);
        bool IsPrepared();
        void ResetTransactionContext();
        void PopulateCharts(IEnumerable<ChartPredicate> predicates);
        void StoreCustomData(string key, IEnumerable<object> data);
        void CleanCustomeStorage();
        IEnumerable<Transaction> GetOpenTransactions();
        IEnumerable<Transaction> GetOpenTransactions(string instrumentId, Direction direction);
        IEnumerable<BalanceRow> GetBalanceHistory();
        IEnumerable<Transaction> GetTansactionsHistory();
        IEnumerable<OpenPositionRequest> GetRequestsHistory();
        IEnumerable<OpenPositionRequest> GetActiveRequests(string instrumentId, Direction direction);
    }
}
