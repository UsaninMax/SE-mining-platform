using System;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using TradePlatform;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.Commons.Trades;
using TradePlatform.StockDataDownload.DataServices.Serialization;
using TradePlatform.StockDataDownload.DataServices.Trades;
using TradePlatform.StockDataDownload.Events;
using TradePlatform.StockDataDownload.Presenters;
using TradePlatform.StockDataDownload.ViewModels;

namespace Trade_platform.tests.StockDataDownload.ViewModels
{
    [TestFixture]
    public class DownloadedInstrumentsViewModelTests
    {
        [SetUp]
        public void SetUp()
        {
            ContainerBuilder
            .Container
            .RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
        }


        [Test]
        public void WhenReceivePresenterAddHeToHistoryTable()
        {
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            ContainerBuilder.Container.RegisterInstance(new Mock<IInstrumentDownloadService>().Object);

            var viewModel = new DownloadedInstrumentsViewModel();
            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            presenterMock.Setup(x => x.SoftDownloadData());

            eventAggregator
                .GetEvent<AddToList<IDounloadInstrumentPresenter>>()
                .Publish(presenterMock.Object);

            Assert.That(viewModel.InstrumentsInfo.Count, Is.EqualTo(1));
            Assert.That(viewModel.InstrumentsInfo[0], Is.EqualTo(presenterMock.Object));
        }

        [Test]
        public void WhenReceivePresenterAddHeToHistoryTableIfNotNull()
        {
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            ContainerBuilder.Container.RegisterInstance(new Mock<IInstrumentDownloadService>().Object);

            var viewModel = new DownloadedInstrumentsViewModel();
            eventAggregator
                .GetEvent<AddToList<IDounloadInstrumentPresenter>>()
                .Publish(null);

            Assert.That(viewModel.InstrumentsInfo.Count, Is.EqualTo(0));
        }

        [Test]
        public void WhenReceivePresenterStartDownloadingProcess()
        {
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            ContainerBuilder.Container.RegisterInstance(new Mock<IInstrumentDownloadService>().Object);

            var viewModel = new DownloadedInstrumentsViewModel();
            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            presenterMock.Setup(x => x.SoftDownloadData());

            eventAggregator
                .GetEvent<AddToList<IDounloadInstrumentPresenter>>()
                .Publish(presenterMock.Object);

            presenterMock.Verify(x => x.SoftDownloadData(), Times.Once);
        }

        [Test]
        public void WhenReceivePresenterDoNotStartDownloadingProcessWhenProcessIsExist()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            ContainerBuilder.Container.RegisterInstance(new Mock<IInstrumentDownloadService>().Object);

            var viewModel = new DownloadedInstrumentsViewModel();

            var storedPresenterMock = new Mock<IDounloadInstrumentPresenter>();
            storedPresenterMock.Setup(x => x.InDownloadingProgress()).Returns(true);
            viewModel.InstrumentsInfo.Add(storedPresenterMock.Object);

            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            presenterMock.Setup(x => x.SoftDownloadData());

            eventAggregator
                .GetEvent<AddToList<IDounloadInstrumentPresenter>>()
                .Publish(presenterMock.Object);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DownloadInfo>()), Times.Once);
            presenterMock.Verify(x => x.SoftDownloadData(), Times.Never);

        }

        [Test]
        public void CheckOpenDataFolderCommand()
        {
            var viewModel = new DownloadedInstrumentsViewModel();
            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            viewModel.OpenFolderCommand.Execute(presenterMock.Object);
            presenterMock.Verify(x => x.ShowDataInFolder(), Times.Once);
        }

        [Test]
        public void CheckOpenDataFolderCommandIfNull()
        {
            var viewModel = new DownloadedInstrumentsViewModel();

            Assert.DoesNotThrow(() =>
            {
                viewModel.OpenFolderCommand.Execute(null);
            });
        }

        [Test]
        public void CheckRemoveDataCommand()
        {
            var viewModel = new DownloadedInstrumentsViewModel();
            var presenterMock = new Mock<IDounloadInstrumentPresenter>();

            viewModel.RemoveCommand.Execute(presenterMock.Object);
            presenterMock.Verify(x => x.DeleteData(), Times.Once);
        }


        [Test]
        public void CheckRemoveDataCommandIfNull()
        {
            var viewModel = new DownloadedInstrumentsViewModel();

            Assert.DoesNotThrow(() =>
            {
                viewModel.RemoveCommand.Execute(null);
            });
        }

        [Test]
        public void CheckHardReloadCommand()
        {
            var viewModel = new DownloadedInstrumentsViewModel();
            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            viewModel.HardReloadCommand.Execute(presenterMock.Object);
            presenterMock.Verify(x => x.HardReloadData(), Times.Once);
        }

        [Test]
        public void CheckHardReloadCommandIfNull()
        {
            var viewModel = new DownloadedInstrumentsViewModel();

            Assert.DoesNotThrow(() =>
            {
                viewModel.HardReloadCommand.Execute(null);
            });
        }

        [Test]
        public void HardReloadCommandWillNotStartWhenExistDownloadingProcess()
        {
            var viewModel = new DownloadedInstrumentsViewModel();
            var presenterMock = new Mock<IDounloadInstrumentPresenter>();

            var storedPresenterMock = new Mock<IDounloadInstrumentPresenter>();
            storedPresenterMock.Setup(x => x.InDownloadingProgress()).Returns(true);
            viewModel.InstrumentsInfo.Add(storedPresenterMock.Object);

            viewModel.HardReloadCommand.Execute(presenterMock.Object);
            presenterMock.Verify(x => x.HardReloadData(), Times.Never);

        }

        [Test]
        public void CheckSoftReloadCommand()
        {
            var viewModel = new DownloadedInstrumentsViewModel();
            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            viewModel.SoftReloadCommand.Execute(presenterMock.Object);
            presenterMock.Verify(x => x.SoftReloadData(), Times.Once);
        }

        [Test]
        public void CheckSoftReloadCommandIfNull()
        {
            var viewModel = new DownloadedInstrumentsViewModel();

            Assert.DoesNotThrow(() =>
            {
                viewModel.SoftReloadCommand.Execute(null);
            });
        }

        [Test]
        public void SoftReloadCommandWillNotStartWhenExistDownloadingProcess()
        {
            var viewModel = new DownloadedInstrumentsViewModel();
            var presenterMock = new Mock<IDounloadInstrumentPresenter>();

            var storedPresenterMock = new Mock<IDounloadInstrumentPresenter>();
            storedPresenterMock.Setup(x => x.InDownloadingProgress()).Returns(true);
            viewModel.InstrumentsInfo.Add(storedPresenterMock.Object);

            viewModel.SoftReloadCommand.Execute(presenterMock.Object);
            presenterMock.Verify(x => x.SoftReloadData(), Times.Never);

        }

        [Test]
        public void CheckLoadWindowHistory()
        {
            var serializer = new Mock<IInstrumentsStorage>();
            Instrument instr = new Instrument.Builder().WithId("12").Build();
            IList<Instrument> instruments = new List<Instrument> { instr, instr };
            serializer.Setup(x => x.ReStore()).Returns(instruments);

            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            ContainerBuilder.Container.RegisterInstance(presenterMock.Object);
            ContainerBuilder.Container.RegisterInstance(serializer.Object);

            var viewModel = new DownloadedInstrumentsViewModel();
            viewModel.LoadedWindowCommand.Execute(null);
            Thread.Sleep(500);

            presenterMock.Verify(x => x.CheckData(), Times.Exactly(2));
            Assert.That(viewModel.InstrumentsInfo.Count, Is.EqualTo(2));
        }

        [Test]
        public void CheckLoadWindowHistoryThrowException()
        {
            var serializer = new Mock<IInstrumentsStorage>();
            serializer.Setup(x => x.ReStore()).Throws(new Exception());
            ContainerBuilder.Container.RegisterInstance(serializer.Object);
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);

            var viewModel = new DownloadedInstrumentsViewModel();
            viewModel.LoadedWindowCommand.Execute(null);
            Thread.Sleep(500);

            infoPublisher.Verify(x => x.PublishException(It.IsAny<AggregateException>()), Times.Once);
            Assert.That(viewModel.InstrumentsInfo.Count, Is.EqualTo(0));
        }

        [Test]
        public void CheckSaveWindowHistory()
        {
            var xmlStorage = new Mock<IInstrumentsStorage>();
            xmlStorage.Setup(x => x.Store(It.IsAny<IEnumerable<Instrument>>()));

            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            ContainerBuilder.Container.RegisterInstance(xmlStorage.Object);

            var viewModel = new DownloadedInstrumentsViewModel();
            viewModel.InstrumentsInfo = new ObservableCollection<IDounloadInstrumentPresenter>(
                new List<IDounloadInstrumentPresenter> {
                    presenterMock.Object,
                    presenterMock.Object
                });
            viewModel.ClosingWindowCommand.Execute(null);
            Thread.Sleep(500);

            presenterMock.Verify(x => x.StopDownload(), Times.Exactly(2));
        }

        [Test]
        public void CheckSaveWindowHistoryThrowException()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var xmlStorage = new Mock<IInstrumentsStorage>();
            xmlStorage.Setup(x => x.Store(It.IsAny<IEnumerable<Instrument>>())).Throws(new Exception());
            ContainerBuilder.Container.RegisterInstance(xmlStorage.Object);
            var viewModel = new DownloadedInstrumentsViewModel();
            viewModel.ClosingWindowCommand.Execute(null);
            Thread.Sleep(500);

            infoPublisher.Verify(x => x.PublishException(It.IsAny<AggregateException>()), Times.Once);
        }
    }
}
