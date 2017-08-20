using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Microsoft.Practices.Unity;
using MoreLinq;
using SEMining.Sandbox.Models;
using SEMining.Sandbox.Transactios.Enums;
using SEMining.Sandbox.Transactios.Models;

namespace SEMining.Sandbox.Transactios
{
    public class TransactionsContext : ITransactionsContext
    {
        private readonly List<OpenPositionRequest> _activeRequests = new List<OpenPositionRequest>();
        private readonly List<OpenPositionRequest> _requestsHistory = new List<OpenPositionRequest>();

        private readonly IDictionary<string, BrokerCost> _brokerCosts;
        private IDictionary<string, Tick> _lastTicks = new Dictionary<string, Tick>();
        private DateTime _lastDate;
        private readonly ITransactionHolder _transactionHolder;
        private readonly IBalance _balance;
        private readonly IWorkingPeriodHolder _workingPeriodHolder;
        private readonly ITransactionBuilder _transactionBuilder;

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
            _transactionBuilder.Reset();
            _activeRequests.Clear();
            _requestsHistory.Clear();
            _balance.Reset();
            _transactionHolder.Reset();
            _workingPeriodHolder.Reset();
            _lastTicks.Clear();
            _lastDate = DateTime.MinValue;
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

        public IEnumerable<Transaction> GetTransactionHistory()
        {
            return _requestsHistory.SelectMany(x => x.GetTransactions()).ToList();
        }

        public IEnumerable<BalanceRow> GetBalanceHistory()
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
            request.Date = _lastDate;
            _activeRequests.Add(request);
            _requestsHistory.Add(request);
            _balance.AddTransactionCost(_brokerCosts[request.InstrumentId].TransactionCost, _lastDate);
            return true;
        }

        private bool CoverageIsOk(OpenPositionRequest request)
        {
            return _balance.GetTotal() - GetCoverage(request) > 0;
        }

        public void CancelPosition(Guid guid)
        {
            _activeRequests.RemoveAll(x => x.Id.Equals(guid));
        }

        private void ProcessTransaction(Transaction transaction)
        {
            _balance.AddTransactionMargin(transaction,
                _transactionHolder.GetInvertedOpenTransactions(transaction.InstrumentId, transaction.Direction), _lastDate);
            _transactionHolder.UpdateOpenTransactions(transaction);
        }

        private void ForceToClosePositions(string instrumentId)
        {
            _activeRequests.RemoveAll(x => x.InstrumentId.Equals(instrumentId));
            _transactionHolder
                .GetOpenTransactions(instrumentId)
                .GroupBy(y => y.Direction)
                .ForEach(y =>
                    {
                        OpenPositionRequest openPosition = new OpenPositionRequest.Builder()
                            .InstrumentId(instrumentId)
                            .Direction(y.Key == Direction.Buy ? Direction.Sell : Direction.Buy)
                            .Number(y.Sum(z => z.RemainingNumber))
                            .Build();
                        openPosition.Date = _lastDate;
                        _activeRequests.Add(openPosition);
                        _requestsHistory.Add(openPosition);
                        _balance.AddTransactionCost(_brokerCosts[openPosition.InstrumentId].TransactionCost, _lastDate);
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

        public IEnumerable<OpenPositionRequest> GetActiveRequests()
        {
            return _activeRequests;
        }

        public List<OpenPositionRequest> GetActiveRequests(string instrumentId, Direction direction)
        {
            return _activeRequests
                .Where(request => request.Direction == direction && request.InstrumentId.Equals(instrumentId))
                .ToList();
        }

        public IEnumerable<OpenPositionRequest> GetRequestsHistory()
        {
            return _requestsHistory;
        }

        public IEnumerable<Transaction> GetOpenTransactions()
        {
            return _transactionHolder.GetOpenTransactions();
        }

        public double GetCoverage()
        {
            return _transactionHolder.GetCoverage(_lastTicks, _activeRequests);
        }

        public double GetCoverage(OpenPositionRequest request)
        {
            return _transactionHolder.GetCoverage(_lastTicks, _activeRequests, request);
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

        public IEnumerable<Transaction> GetOpenTransactions(string instrumentId, Direction direction)
        {
            return _transactionHolder.GetOpenTransactions(instrumentId, direction);
        }
    }
}
