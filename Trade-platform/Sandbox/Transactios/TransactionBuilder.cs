using System;
using System.Collections.Generic;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public class TransactionBuilder : ITransactionBuilder
    {
        private readonly IDictionary<string, Tick> _ticks = new Dictionary<string, Tick>();
        public Transaction Build(OpenPositionRequest request, Tick inputTick)
        {
            Tick tick = UpdateTick(inputTick);
            int willExecute = Math.Min(tick.Volume / 2, request.RemainingNumber);

            if (tick.Volume == 0 || willExecute == 0)
            {
                return null;
            }

            tick.Volume = tick.Volume - willExecute;
            Transaction transaction = new Transaction.Builder()
                .WithDate(tick.Date())
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

        private Tick UpdateTick(Tick inputTick)
        {
            if (!_ticks.ContainsKey(inputTick.Id()))
            {
                Tick tick = new Tick(inputTick);
                _ticks.Add(tick.Id(), tick);
                return tick;
            }

            Tick current = _ticks[inputTick.Id()];
            if (DateTime.Compare(current.Date(), inputTick.Date()) < 0)
            {
                current = new Tick(inputTick);
            }

            return current;
        }
    }
}
