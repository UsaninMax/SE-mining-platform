using System;
using System.Collections.Generic;
using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Trades.Finam
{
    class FinamInstrumentSplitter : IInstrumentSplitter
    {
        public IEnumerable<Instrument> Split(Instrument instrument)
        {
            IList<Instrument> instruments = new List<Instrument>();
            for (DateTime date = instrument.From; date <= instrument.To; date = date.AddDays(1))
            {
                instruments.Add(new Instrument.Builder()
                    .WithFrom(date)
                    .WithTo(date)
                    .WithParent(instrument)
                    .Build());
            }
            return instruments;
        }
    }
}
