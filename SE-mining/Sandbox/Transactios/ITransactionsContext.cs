using System;
using System.Collections.Generic;
using SE_mining_base.Sandbox.Models;
using SE_mining_base.Transactios.Enums;
using SE_mining_base.Transactios.Models;

namespace SEMining.Sandbox.Transactios
{
    public interface ITransactionsContext
    {
        bool IsPrepared();
        void SetUpBalance(double value);
        void SetUpWorkingPeriod(IDictionary<string, WorkingPeriod> value);
        double CurrentBalance();
        void Reset();
        int GetAvailableNumberToOpen(string instrumentId);
        int GetNumberOfOpenTransactions(string instrumentId);
        IList<Transaction> GetTransactionHistory();
        IList<BalanceRow> GetBalanceHistory();
        bool OpenPosition(OpenPositionRequest request);
        void CancelPosition(Guid guid);
        void ProcessTick(IDictionary<string, Tick> ticks, DateTime dateTime);
        IEnumerable<OpenPositionRequest> GetActiveRequests();
        List<OpenPositionRequest> GetActiveRequests(string instrumentId, Direction direction);
        IEnumerable<OpenPositionRequest> GetRequestsHistory();
        IEnumerable<Transaction> GetOpenTransactions();
        IEnumerable<Transaction> GetOpenTransactions(string instrumentId, Direction direction);
        double GetCoverage();
        double GetCoverage(OpenPositionRequest request);
    }
}
