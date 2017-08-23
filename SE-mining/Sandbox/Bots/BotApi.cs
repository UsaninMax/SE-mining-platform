﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using SEMining.Charts.Data.Holders;
using SEMining.Charts.Data.Populating;
using SEMining.Charts.Data.Predicates.Basis;
using SEMining.Commons.Info;
using SEMining.Commons.Info.Model.Message;
using SEMining.Sandbox.Models;
using SEMining.Sandbox.Transactios;
using SEMining.Sandbox.Transactios.Models;
using SEMining.Sandbox.Holders;
using SEMining.Sandbox.Transactios.Enums;

namespace SEMining.Sandbox.Bots
{
    public abstract class BotApi : IBot
    {
        private string _id;
        private string _sandboxId;
        private readonly IInfoPublisher _infoPublisher;
        private BotPredicate _predicate;
        private readonly ITransactionsContext _context;

        protected BotApi(IDictionary<string, BrokerCost> brokerCosts)
        {
            _context = ContainerBuilder.Container.Resolve<ITransactionsContext>(new DependencyOverride<IDictionary<string, BrokerCost>>(brokerCosts));
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
        }

        public string GetId()
        {
            return _id;
        }

        public void SetUpId(string id)
        {
            _id = id;
        }

        public void SetUpPredicate(BotPredicate predicate)
        {
            _predicate = predicate;
        }

        public void SetUpWorkingPeriod(IDictionary<string, WorkingPeriod> value)
        {
            _context.SetUpWorkingPeriod(value);
        }

        public void SetUpBalance(double value)
        {
            _context.SetUpBalance(value);
        }

        public void OpenPosition(OpenPositionRequest request)
        {
            _context.OpenPosition(request);
        }

        public bool IsPrepared()
        {
            return _context.IsPrepared();
        }

        public void ResetTransactionContext()
        {
            _context.Reset();
        }

        public IEnumerable<Transaction> GetTansactionsHistory()
        {
            return _context.GetTransactionHistory();
        }

        public IEnumerable<BalanceRow> GetBalanceHistory()
        {
            return _context.GetBalanceHistory();
        }

        public void Execute()
        {
            _infoPublisher.PublishInfo(new InfoItem("Bot " + _id) { Message = "Start execute - " + _predicate });
            ContainerBuilder.Container.Resolve<ISandboxDataHolder>()
                .Get()
                .Where(m =>
                (_predicate.From == DateTime.MinValue || m.DateTime >= _predicate.From) &&
                (_predicate.To == DateTime.MinValue || m.DateTime <= _predicate.To))
                .ForEach(x =>
                {
                    if (x.Ticks.Any())
                    {
                        _context.ProcessTick(x.Ticks, x.DateTime);
                    }
                    if (x.Datas.Any())
                    {
                        Execution(x.Datas);
                    }
                });
            _infoPublisher.PublishInfo(new InfoItem("Bot " + _id) { Message = "Executed - " + _predicate });
        }

        public abstract void Execution(IDictionary<string, IData> data);
        public abstract int Score();

        public void SetUpSandboxId(string id)
        {
            _sandboxId = id;
        }

        public void PopulateCharts(IEnumerable<ChartPredicate> predicates)
        {
            ContainerBuilder.Container.Resolve<IChartPredicatesHolder>().Add(predicates);
            ContainerBuilder.Container.Resolve<IChartsPopulator>().Populate();
        }

        public void StoreCustomData(string key, IEnumerable<object> data)
        {
            ContainerBuilder.Container.Resolve<ICustomDataHolder>().Add(key, data);
        }

        public void CleanCustomeStorage()
        {
            ContainerBuilder.Container.Resolve<ICustomDataHolder>().CleanAll();
        }

        public int GetAvailableNumberToOpen(string instrumentId)
        {
            return _context.GetAvailableNumberToOpen(instrumentId);
        }

        public int GetNumberOfOpenTransactions(string instrumentId)
        {
            return _context.GetNumberOfOpenTransactions(instrumentId);
        }

        public IEnumerable<Transaction> GetOpenTransactions()
        {
            return _context.GetOpenTransactions();
        }

        public IEnumerable<Transaction> GetOpenTransactions(string instrumentId, Direction direction)
        {
           return _context.GetOpenTransactions(instrumentId, direction);
        }

        public IEnumerable<OpenPositionRequest> GetRequestsHistory()
        {
            return _context.GetRequestsHistory();
        }

        public IEnumerable<OpenPositionRequest> GetActiveRequests(string instrumentId, Direction direction)
        {
            return _context.GetActiveRequests(instrumentId, direction);
        }
    }
}