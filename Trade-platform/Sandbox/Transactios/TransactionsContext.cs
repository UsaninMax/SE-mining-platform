using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Microsoft.Practices.Unity;
using MoreLinq;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Enums;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public class TransactionsContext : ITransactionsContext
    {
        private List<OpenPositionRequest> _activeRequests = new List<OpenPositionRequest>();
        private List<OpenPositionRequest> _requestsHistory = new List<OpenPositionRequest>();

        private IDictionary<string, BrokerCost> _brokerCosts;
        private IDictionary<string, Tick> _lastTicks = new Dictionary<string, Tick>();
        private DateTime _lastDate;
        private ITransactionHolder _transactionHolder;
        private IBalance _balance;
        private IWorkingPeriodHolder _workingPeriodHolder;
        private ITransactionBuilder _transactionBuilder;

        public TransactionsContext(IDictionary<string, BrokerCost> brokerCosts)
        {
            _brokerCosts = brokerCosts;
            _transactionHolder = ContainerBuilder.Container.Resolve<ITransactionHolder>(new DependencyOverride<IDictionary<string, BrokerCost>>(brokerCosts));
            _balance = ContainerBuilder.Container.Resolve<IBalance>();
            _transactionBuilder = ContainerBuilder.Container.Resolve<ITransactionBuilder>();
            _workingPeriodHolder = ContainerBuilder.Container.Resolve<IWorkingPeriodHolder>();
        }

        public bool IsPrepared()
        {
            if (_brokerCosts.IsNullOrEmpty())
            {
                return false;
            }

            if (_balance.GetHistory().IsNullOrEmpty())
            {
                return false;
            }

            return true;
        }

        public void SetUpBalance(double value)
        {
            _balance.AddMoney(value);
        }

        public void SetUpWorkingPeriod(IDictionary<string, WorkingPeriod> value)
        {
            _workingPeriodHolder.SetUp(value);
        }

        public double GetBalance()
        {
            return _balance.GetTotal();
        }

        public void Reset()
        {
            _activeRequests = new List<OpenPositionRequest>();
            _requestsHistory = new List<OpenPositionRequest>();
            _balance.Reset();
            _transactionHolder.Reset();
            _lastTicks = new Dictionary<string, Tick>();
        }

        public int AvailableNumber(string instrumentId)
        {
            if (_lastTicks.IsNullOrEmpty())
            {
                return 0;
            }
            if (!_lastTicks.ContainsKey(instrumentId))
            {
                throw new Exception("Context incorrectly installed");
            }

            var costs = _brokerCosts[instrumentId];
            return (int)Math.Floor((_balance.GetTotal() - GetCoverage()) / (_lastTicks[instrumentId].Price * costs.Coverage));
        }

        public IList<Transaction> GetTransactionHistory()
        {
            return _requestsHistory.SelectMany(x => x.GetTransactions()).ToList();
        }

        public IList<BalanceRow> GetBalanceHistory()
        {
            return _balance.GetHistory();
        }

        public bool OpenPosition(OpenPositionRequest request)
        {
            if (_lastTicks.IsNullOrEmpty())
            {
                return false;
            }

            if (!CoverageIsOk(request))
            {
                return false;
            }

            if (!IsWorkingTime(request.InstrumentId))
            {
                return false;
            }

            _activeRequests.Add(request);
            _requestsHistory.Add(request);
            _balance.AddTransactionCost(_brokerCosts[request.InstrumentId].TransactionCost);
            return true;
        }

        private bool CoverageIsOk(OpenPositionRequest request)
        {
            return _balance.GetTotal()
                - GetCoverage()
                - request.RemainingNumber
                * _lastTicks[request.InstrumentId].Price
                * _brokerCosts[request.InstrumentId].Coverage > 0;
        }

        public void CancelPosition(Guid guid)
        {
            _activeRequests.RemoveAll(x => x.Id.Equals(guid));
        }

        private void ProcessTransaction(Transaction transaction)
        {
            _balance.AddTransactionMargin(transaction,
                _transactionHolder.GetOpenTransactions(transaction.InstrumentId, transaction.Direction));
            _transactionHolder.UpdateOpenTransactions(transaction);
        }

        private void ForceToClosePositions(string instrumentId)
        {
            _activeRequests.RemoveAll(x => x.InstrumentId.Equals(instrumentId));
            _transactionHolder.GetOpenTransactions(instrumentId).GroupBy(y => y.Direction).ForEach(y =>
             {
                 OpenPositionRequest openPosition = new OpenPositionRequest.Builder()
                     .InstrumentId(instrumentId)
                     .Direction(y.Key == Direction.Buy ? Direction.Sell : Direction.Buy)
                     .Number(y.Sum(z => z.RemainingNumber))
                     .Build();
                 _activeRequests.Add(openPosition);
                 _requestsHistory.Add(openPosition);
                 _balance.AddTransactionCost(_brokerCosts[openPosition.InstrumentId].TransactionCost);
             });
        }

        private bool ForceClosePositionChecker(string instrumentId)
        {
            if (_workingPeriodHolder.IsStoredPoint(instrumentId, _lastDate.Date))
            {
                return false;
            }
            _workingPeriodHolder.StorePoint(instrumentId, _lastDate.Date);
            return !IsWorkingTime(instrumentId);
        }

        public void ProcessTick(IDictionary<string, Tick> ticks, DateTime dateTime)
        {
            _lastTicks = ticks;
            _lastDate = dateTime;
            if (_activeRequests.IsNullOrEmpty()
                && _transactionHolder.GetSize() == 0)
            {
                return;
            }
            ticks.Keys.Where(ForceClosePositionChecker).ForEach(ForceToClosePositions);
            _activeRequests
                .ForEach(x =>
                {
                    Transaction transaction = _transactionBuilder.Build(x, _lastTicks[x.InstrumentId]);
                    if (transaction == null)
                    {
                        return;
                    }
                    x.AddTransaction(transaction);
                    x.RemainingNumber -= transaction.Number;
                    ProcessTransaction(transaction);
                });

            _activeRequests.RemoveAll(x => x.RemainingNumber == 0);
        }

        public IList<OpenPositionRequest> GetActiveRequests()
        {
            return _activeRequests;
        }

        public IList<OpenPositionRequest> GetHistoryRequests()
        {
            return _requestsHistory;
        }

        public IList<Transaction> GetActiveTransactions()
        {
            return _transactionHolder.GetOpenTransactions();
        }

        public double GetCoverage()
        {
            double fromOpenRequests = _activeRequests.GroupBy(x => x.InstrumentId)
                .Select(x => _lastTicks[x.Key].Price * x.Sum(y => y.RemainingNumber) * _brokerCosts[x.Key].Coverage).Sum();

            return _transactionHolder.GetCoverage(_lastTicks) + fromOpenRequests;
        }

        private bool IsWorkingTime(string instrumentId)
        {
            WorkingPeriod working = _workingPeriodHolder.Get(instrumentId);

            if (working == null)
            {
                return true;
            }

            Tick tick = _lastTicks[instrumentId];
            DateTime open = tick.Date().Date.Add(working.Open);
            DateTime close = tick.Date().Date.Add(working.Close);

            if (tick.Date() <= open || tick.Date() >= close)
            {
                return false;
            }

            return true;
        }
    }
}
