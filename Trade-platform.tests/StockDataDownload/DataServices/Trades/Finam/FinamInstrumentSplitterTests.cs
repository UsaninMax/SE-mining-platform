using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TradePlatform.StockData.DataServices.Trades.Finam;
using TradePlatform.StockData.Models;

namespace Trade_platform.tests.StockDataDownload.DataServices.Trades.Finam
{
    [TestFixture]
    public class FinamInstrumentSplitterTests
    {
        [Test]
        public void CheckSplitProcess()
        {
            DateTime today = DateTime.Today;
            IEnumerable<Instrument> splitted =
                new FinamInstrumentSplitter().Split(new Instrument.Builder()
                .WithFrom(today)
                .WithTo(today.AddDays(2))
                .Build());

            Assert.That(splitted.Count(), Is.EqualTo(3));
        }
    }
}
