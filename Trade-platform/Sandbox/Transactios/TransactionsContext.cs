using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using MoreLinq;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Enums;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public class TransactionsContext : ITransactionsContext
    {
        private List<OpenPositionRequest> _requests = new List<OpenPositionRequest>();
        private List<OpenPositionRequest> _requestsHistory = new List<OpenPositionRequest>();
        private IDictionary<string, BrokerCost> _brokerCosts;
        private IDictionary<string, WorkingPeriod> _workingPeriods;
        private IList<BalanceRow> _balanceHistory = new List<BalanceRow>();
        private BalanceRow _currentBalance;
        private List<Transaction> _openPositions = new List<Transaction>();
        private List<string> _openPositionsIds = new List<string>();
        private IDictionary<string, Tick> _lastTick = new Dictionary<string, Tick>();

        public bool IsPrepared()
        {
            if (_brokerCosts.IsNullOrEmpty())
            {
                return false;
            }

            if (_currentBalance == null)
            {
                return false;
            }

            if (_workingPeriods.IsNullOrEmpty())
            {
                return false;
            }

            return true;
        }

        public void SetUpCosts(IDictionary<string, BrokerCost> value)
        {
            _brokerCosts = value;
        }

        public void SetUpBalance(double value)
        {
            _currentBalance = new BalanceRow.Builder().Total(value).Build();
            _balanceHistory.Add(_currentBalance);
        }

        public void SetUpWorkingPeriod(IDictionary<string, WorkingPeriod> value)
        {
            _workingPeriods = value;
        }

        public double GetBalance()
        {
            return _currentBalance.Total;
        }

        public void Reset()
        {
            _requests = new List<OpenPositionRequest>();
            _requestsHistory = new List<OpenPositionRequest>();
            _currentBalance = _balanceHistory.First();
            _balanceHistory = new List<BalanceRow>();
            _openPositions = new List<Transaction>();
            _lastTick = new Dictionary<string, Tick>();

        }

        public int AvailableNumber(string instrumentId)
        {
            var costs = _brokerCosts[instrumentId];
            return (int)Math.Floor(_currentBalance.Total / (_lastTick[instrumentId].Price * costs.Coverage));
        }

        public IList<Transaction> GetTransactionHistory()
        {
            return _requestsHistory.SelectMany(x => x.GetTransactions()).ToList();
        }

        public IList<BalanceRow> GetBalanceHistory()
        {
            return _balanceHistory;
        }

        private void UpdateBalance(OpenPositionRequest request)
        {
            var costs = _brokerCosts[request.InstrumentId];
            var coverage = _lastTick[request.InstrumentId].Price * costs.Coverage * request.Number;
            var transactionCost = costs.TransactionCost;

            _currentBalance = new BalanceRow.Builder()
                .Coverage(coverage)
                .TransactionCost(transactionCost)
                .Total(_currentBalance.Total - (coverage + transactionCost))
                .Build();
            _balanceHistory.Add(_currentBalance);
        }

        private Direction Invert(Direction direction)
        {
            return direction == Direction.Buy ? Direction.Sell : Direction.Buy;
        }

        private void UpdateBalance(Transaction transaction)
        {
            var openPosition = _openPositions.First(x => x.InstrumentId.Equals(transaction.InstrumentId) &&
                                       x.Direction == Invert(transaction.Direction));

            if (openPosition == null)
            {
                return;
            }

            var forCalculation = Math.Min(openPosition.Number, transaction.Number);
            var openSum = openPosition.ExecutedPrice * forCalculation;
            var closeSum = transaction.ExecutedPrice * forCalculation;
            var profit = openPosition.Direction == Direction.Buy ? closeSum - openSum : openSum - closeSum;
            var coverage = openPosition.ExecutedPrice * forCalculation * _brokerCosts[transaction.InstrumentId].Coverage;
            _currentBalance = new BalanceRow.Builder()
                .Coverage(-coverage)
                .TransactionMargin(profit)
                .Total(_currentBalance.Total + profit + coverage)
                .Build();

            _balanceHistory.Add(_currentBalance);

        }

        public bool OpenPosition(ImmediatePositionRequest request)
        {
            if (!IsWorkingTime(request))
            {
                return false;
            }

            UpdateBalance(request);
            _requests.Add(request);
            _requestsHistory.Add(request);
            return true;
        }

        private bool IsWorkingTime(OpenPositionRequest request)
        {
            WorkingPeriod working = _workingPeriods[request.InstrumentId];
            DateTime open = request.Date.Add(working.Open);
            DateTime close = request.Date.Add(working.Close);

            if (request.Date <= open || request.Date >= close)
            {
                return false;
            }

            return true;
        }

        private bool IsWorkingTime(string instrumentId)
        {
            Tick tick = _lastTick[instrumentId];
            WorkingPeriod working = _workingPeriods[instrumentId];
            DateTime open = tick.Date().Add(working.Open);
            DateTime close = tick.Date().Add(working.Close);

            if (tick.Date() <= open || tick.Date() >= close)
            {
                return false;
            }

            return true;
        }

        public bool OpenPosition(PostponedPositionRequest request)
        {
            if (!IsWorkingTime(request))
            {
                return false;
            }

            UpdateBalance(request);
            _requests.Add(request);
            _requestsHistory.Add(request);
            return true;
        }

        public bool ClosePosition(Guid guid)
        {
            var forRemove = _requests
                .OfType<PostponedPositionRequest>()
                .FirstOrDefault(x => x.Id.Equals(guid));

            return _requests.Remove(forRemove);
        }

        private void UpdateOpenPositions(Transaction transaction)
        {
            Transaction openPosition = _openPositions.First(x => x.InstrumentId.Equals(transaction.InstrumentId) &&
                                                         x.Direction == Invert(transaction.Direction));

            if (openPosition == null)
            {
                _openPositions.Add(transaction);
                return;
            }

            if (openPosition.Number > transaction.Number)
            {
                openPosition.Number = openPosition.Number - transaction.Number;
                return;
            }

            if (openPosition.Number < transaction.Number)
            {
                transaction.Number = transaction.Number - openPosition.Number;
                _openPositions.Add(transaction);
            }
            openPosition.Number = 0;
            _openPositions.RemoveAll(x => x.Number == 0);
            _openPositionsIds = _openPositions.Select(x => x.InstrumentId).Distinct().ToList();
        }

        private void UpdateLastTicks(IDictionary<string, Tick> ticks)
        {
            _lastTick = ticks.Values.ToDictionary(x => x.Id(),
                x => new Tick.Builder()
                .WithDate(x.Date())
                .WithId(x.Id())
                .WithPrice(x.Price)
                .WithVolume(x.Volume / 2)
                .Build());
        }


        private void ForceToClosePositions(IEnumerable<string> ids)
        {
            _requests.RemoveAll(x => ids.Contains(x.InstrumentId));
            _openPositions.Where(x => ids.Contains(x.InstrumentId)).ForEach(x =>
            {
                Tick tick = _lastTick[x.InstrumentId];
                if (tick.Volume == 0)
                {
                    return;
                }

                int willExecute = Math.Min(tick.Volume, x.Number);
                tick.Volume = tick.Volume - willExecute;
                Transaction transaction = new Transaction.Builder()
                    .InstrumentId(x.InstrumentId)
                    .Direction(Invert(x.Direction))
                    .ExecutedPrice(tick.Price)
                    .Number(willExecute)
                    .Build();
                UpdateBalance(transaction);
                UpdateOpenPositions(transaction);
            });
        }

        public void ProcessTick(IDictionary<string, Tick> ticks)
        {
            UpdateLastTicks(ticks);

            if (_requests.IsNullOrEmpty()
                && _openPositions.IsNullOrEmpty())
            {
                return;
            }

            ForceToClosePositions(_openPositionsIds.Where(x => !IsWorkingTime(x)));

            if (_requests.IsNullOrEmpty())
            {
                return;
            }

            _requests.ForEach(request =>
            {
                Tick tick = _lastTick[request.InstrumentId];

                if (tick.Volume == 0)
                {
                    return;
                }

                int willExecute = Math.Min(tick.Volume , request.RemainingNumber);
                tick.Volume = tick.Volume - willExecute;
                request.RemainingNumber = request.RemainingNumber - willExecute;

                Transaction transaction = new Transaction.Builder()
                .InstrumentId(request.InstrumentId)
                .Direction(request.Direction)
                .ExecutedPrice(tick.Price)
                .Number(willExecute)
                .Build();
                request.AddTransaction(transaction);
                UpdateBalance(transaction);
                UpdateOpenPositions(transaction);
            });

            _requests.RemoveAll(x => x.RemainingNumber == 0);
        }
    }
}
