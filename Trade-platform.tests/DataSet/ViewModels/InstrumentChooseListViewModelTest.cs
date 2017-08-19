using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Prism.Events;
using TradePlatform;
using TradePlatform.Commons.Info;
using TradePlatform.DataSet.Events;
using TradePlatform.DataSet.Holders;
using TradePlatform.DataSet.Presenters;
using TradePlatform.DataSet.ViewModels;
using TradePlatform.StockData.DataServices.Trades;
using TradePlatform.StockData.Holders;
using TradePlatform.StockData.Models;

namespace Trade_platform.tests.DataSet.ViewModels
{
    [TestFixture]
    public class InstrumentChooseListViewModelTest
    {
        [SetUp]
        public void SetUp()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        [Test]
        public void TestAddSelectedItems()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            Mock<IEventAggregator> fakeEventAggregator = new Mock<IEventAggregator>();
            ContainerBuilder.Container.RegisterInstance(fakeEventAggregator.Object);
            bool isHit = false;
            fakeEventAggregator.Setup(x => x.GetEvent<AddInstrumentToDatatSetEvent>()
                .Publish(It.IsAny<IEnumerable<Instrument>>()));
            Instrument instrument = new Instrument.Builder().WithCode("test_id").Build();
            IEnumerable<Instrument> instruments = new List<Instrument> { instrument };
            InstrumentChooseListViewModel model = new InstrumentChooseListViewModel();
            model.CloseWindowNotification += (s, e) =>
            {
                isHit = true;
            };
            model.AddSelectedItemsCommand.Execute(instruments);

            fakeEventAggregator.Verify(x => x.GetEvent<AddInstrumentToDatatSetEvent>()
                .Publish(instruments), Times.Once);
            Assert.IsTrue(isHit);
        }

        [Test]
        public void CheckLoadWindowHistory()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            var holder = new Mock<IDownloadedInstrumentsHolder>();
            ContainerBuilder.Container.RegisterInstance(holder.Object);
            var instrumentService = new Mock<IInstrumentService>();
            ContainerBuilder.Container.RegisterInstance(instrumentService.Object);

            Instrument instrument_1 = new Instrument.Builder().WithCode("test_id_1").Build();
            Instrument instrument_2 = new Instrument.Builder().WithCode("test_id_2").Build();

            ISet<Instrument> instruments = new HashSet<Instrument> {
                instrument_1, instrument_2
            };

            holder.Setup(x => x.GetAll()).Returns(instruments);

            instrumentService.Setup(x => x.CheckFiles(instrument_1)).Returns(true);
            instrumentService.Setup(x => x.CheckFiles(instrument_2)).Returns(false);

            var model = new InstrumentChooseListViewModel();
            model.LoadedWindowCommand.Execute(null);

            
            Thread.Sleep(500);


            Assert.That(model.InstrumentsInfo.Count, Is.EqualTo(1));
        }

        [Test]
        public void CheckLoadWindowHistoryWhenExceptionOcure()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            var holder = new Mock<IDownloadedInstrumentsHolder>();
            ContainerBuilder.Container.RegisterInstance(holder.Object);
            var instrumentService = new Mock<IInstrumentService>();
            ContainerBuilder.Container.RegisterInstance(instrumentService.Object);
            Instrument instrument_1 = new Instrument.Builder().WithCode("test_id_1").Build();
            Instrument instrument_2 = new Instrument.Builder().WithCode("test_id_2").Build();

            ISet<Instrument> instruments = new HashSet<Instrument> {
                instrument_1, instrument_2
            };
            holder.Setup(x => x.GetAll()).Returns(instruments);
            instrumentService.Setup(x => x.CheckFiles(instrument_1)).Throws<Exception>();

            var model = new InstrumentChooseListViewModel();
            model.LoadedWindowCommand.Execute(null);
            Thread.Sleep(500);
            infoPublisher.Verify(x => x.PublishException(It.IsAny<AggregateException>()), Times.Once);
        }

        [Test]
        public void CheckLoadWindowHistoryWhenFail()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            var holder = new Mock<IDataSetHolder>();
            holder.Setup(x => x.GetAll()).Throws<Exception>();

            var presenterMock = new Mock<IDataSetPresenter>();
            ContainerBuilder.Container.RegisterInstance(presenterMock.Object);
            ContainerBuilder.Container.RegisterInstance(holder.Object);

            var viewModel = new DataSetListViewModel();
            viewModel.LoadedWindowCommand.Execute(null);
            Thread.Sleep(500);

            infoPublisher.Verify(x => x.PublishException(It.IsAny<AggregateException>()), Times.Once);
            Assert.That(viewModel.DataSetPresenterInfo.Count, Is.EqualTo(0));
        }
    }
}
