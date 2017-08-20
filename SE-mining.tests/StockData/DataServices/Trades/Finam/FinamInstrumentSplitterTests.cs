using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SEMining.StockData.DataServices.Trades.Finam;
using SEMining.StockData.Models;

namespace SEMining.tests.StockData.DataServices.Trades.Finam
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
