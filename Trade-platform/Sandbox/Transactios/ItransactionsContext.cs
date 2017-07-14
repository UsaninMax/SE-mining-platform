using System;
using System.Collections.Generic;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public interface ITransactionsContext
    {
        bool IsPrepared();
        void SetUpBalance(double value);
        void SetUpWorkingPeriod(IDictionary<string, WorkingPeriod> value);
        double GetBalance();
        void Reset();
        int AvailableNumber(string instrumentId);
        IList<Transaction> GetTransactionHistory();
        IList<BalanceRow> GetBalanceHistory();
        bool OpenPosition(OpenPositionRequest request);
        void CancelPosition(Guid guid);
        void ProcessTick(IDictionary<string, Tick> ticks, DateTime dateTime);
        IList<OpenPositionRequest> GetActiveRequests();
        IList<OpenPositionRequest> GetHistoryRequests();
        IList<Transaction> GetActiveTransactions();
        double GetCoverage();
    }
}
