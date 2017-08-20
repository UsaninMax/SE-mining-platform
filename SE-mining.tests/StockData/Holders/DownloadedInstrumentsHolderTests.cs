using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using SEMining.StockData.DataServices.Serialization;
using SEMining.StockData.Holders;
using SEMining.StockData.Models;

namespace SEMining.tests.StockData.Holders
{
    [TestFixture]
    public class DownloadedInstrumentsHolderTests
    {

        [Test]
        public void InstrumentsHolderWillReturnInstrumentAfterRestoring()
        {
            var instrumentsStorage = new Mock<IInstrumentsStorage>();
            ContainerBuilder.Container.RegisterInstance(instrumentsStorage.Object);

            Instrument instrument = new Instrument.Builder().WithId("34534").WithName("rwe").Build();
            instrumentsStorage.Setup(x => x.ReStore()).Returns(new List<Instrument> { instrument, instrument });

            DownloadedInstrumentsHolder holder = new DownloadedInstrumentsHolder();
            holder.Restore();

            instrumentsStorage.Verify(x => x.ReStore(), Times.Once);

            Assert.That(holder.GetAll().Count, Is.EqualTo(1));
            Assert.That(holder.GetAll().Contains(instrument), Is.True);
        }

        [Test]
        public void InstrumentsHolderWillCallStorageOfInstruments()
        {
            var instrumentsStorage = new Mock<IInstrumentsStorage>();
            ContainerBuilder.Container.RegisterInstance(instrumentsStorage.Object);
            DownloadedInstrumentsHolder holder = new DownloadedInstrumentsHolder();
            holder.Store();
            instrumentsStorage.Verify(x => x.Store(It.IsAny<IEnumerable<Instrument>>()), Times.Once);
        }
    }
}
