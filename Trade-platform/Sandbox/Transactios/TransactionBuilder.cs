using System;
using System.Collections.Generic;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public class TransactionBuilder : ITransactionBuilder
    {
        private IDictionary<string, Tick> _ticks = new Dictionary<string, Tick>();
        public Transaction Build(OpenPositionRequest request, Tick input, DateTime date)
        {
            Tick tick = UpdateTick(input);

            if (tick.Volume == 0)
            {
                return null;
            }

            int willExecute = Math.Min(tick.Volume / 2, request.RemainingNumber);
            tick.Volume = tick.Volume - willExecute;
            Transaction transaction = new Transaction.Builder()
                .WithDate(date)
                .InstrumentId(request.InstrumentId)
                .Direction(request.Direction)
                .ExecutedPrice(tick.Price)
                .Number(willExecute)
                .Build();
            return transaction;
        }

        public void Reset()
        {
            _ticks.Clear();
        }

        private Tick UpdateTick(Tick tick)
        {
            if (!_ticks.ContainsKey(tick.Id()))
            {
                _ticks.Add(tick.Id(), tick);
                return tick;
            }

            Tick current = _ticks[tick.Id()];
            if (DateTime.Compare(current.Date(), tick.Date()) < 0)
            {
                _ticks.Add(tick.Id(), tick);
                return tick;
            }

            return current;
        }
    }
}
