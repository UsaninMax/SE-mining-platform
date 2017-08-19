﻿using System;
using System.Collections.Generic;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Enums;
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
        IEnumerable<Transaction> GetTransactionHistory();
        IEnumerable<BalanceRow> GetBalanceHistory();
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
