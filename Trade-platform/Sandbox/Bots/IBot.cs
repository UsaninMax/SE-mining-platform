using System.Collections.Generic;
using TradePlatform.Charts.Data.Predicates.Basis;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Bots
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
        void PopulateCharts(ICollection<ChartPredicate> predicates);
        void StoreCustomData(string key, IList<object> data);
        void CleanCustomeStorage();
        IList<Transaction> GetTansactionsHistory();
        IList<BalanceRow> GetBalanceHistory();
    }
}
