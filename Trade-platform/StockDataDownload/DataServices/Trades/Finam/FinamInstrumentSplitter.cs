using System;
using System.Collections.Generic;
using TradePlatform.StockDataDownload.model;

namespace TradePlatform.StockDataDownload.DataServices.Finam
{
    class FinamInstrumentSplitter : IInstrumentSplitter
    {
        public IList<Instrument> Split(Instrument instrument)
        {
            IList<Instrument> instruments = new List<Instrument>();
            for (DateTime date = instrument.From; date <= instrument.To; date = date.AddDays(1))
            {
                instruments.Add(new Instrument() {
                    Name = instrument.Name,
                    From = date,
                    To = date,
                    MarketId = instrument.MarketId,
                    Code = instrument.Code,
                    Id = instrument.Id
                });
            }
            return instruments;
        }
    }
}
