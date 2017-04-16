﻿using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Prism.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using TradePlatform;
using TradePlatform.Commons.MessageSubscribers;
using TradePlatform.Commons.Sistem;
using TradePlatform.Commons.Trades;
using TradePlatform.StockDataDownload.DataServices.Trades;
using TradePlatform.StockDataDownload.Presenters;

namespace Trade_platform.tests.StockDataDownload.Presenters
{
    [TestFixture]
    class DounloadInstrumentPresenterTests
    {
        [SetUp]
        public void SetUp()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        [Test]
        public void CheckHardDownloadData()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);

            var presenter = new DounloadInstrumentPresenter(new Instrument.Builder().Build());
            presenter.HardDownloadData();
            Thread.Sleep(500);
            downloadService.Verify(x => x.Download(It.IsAny<Instrument>(),
                It.IsAny<CancellationToken>()),
                Times.Exactly(1));
            Assert.That(presenter.StatusMessage, Is.EqualTo(TradesStatuses.IsReady));
        }

        [Test]
        public void CheckHardDownloadDataWithFail()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);
            var presenter = new DounloadInstrumentPresenter(new Instrument.Builder().Build());
            downloadService.Setup(x => x.Download(
                It.IsAny<Instrument>(),
                It.IsAny<CancellationToken>())).Throws<Exception>();
            presenter.HardDownloadData();
            Thread.Sleep(500);
            downloadService.Verify(x => x.Download(It.IsAny<Instrument>(),
                It.IsAny<CancellationToken>()),
                Times.Exactly(1));
            Assert.That(presenter.StatusMessage, Is.EqualTo(TradesStatuses.FailToDownloud));
        }


        [Test]
        public void IfIsActiveDowloadTaskPresentHardDownloadDataWillNotStart()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);
            var presenter = new DounloadInstrumentPresenter(new Instrument.Builder().Build());
            downloadService.Setup(x => x.Download(
                It.IsAny<Instrument>(),
                It.IsAny<CancellationToken>()))
                .Callback(() => Thread.Sleep(500));
            presenter.HardDownloadData();
            presenter.HardDownloadData();
            Thread.Sleep(1000);
            downloadService.Verify(x => x.Download(It.IsAny<Instrument>(),
                It.IsAny<CancellationToken>()),
                Times.Exactly(1));
        }

        [Test]
        public void CheckSoftDownloadData()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);
            var presenter = new DounloadInstrumentPresenter(new Instrument.Builder().Build());
            presenter.SoftDownloadData();
            Thread.Sleep(500);
            downloadService.Verify(x => x.SoftDownload(It.IsAny<Instrument>(),
                It.IsAny<CancellationToken>()),
                Times.Exactly(1));
            Assert.That(presenter.StatusMessage, Is.EqualTo(TradesStatuses.IsReady));

        }

        [Test]
        public void CheckSoftDownloadDataWithFail()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);
            var presenter = new DounloadInstrumentPresenter(new Instrument.Builder().Build());
            downloadService.Setup(x => x.SoftDownload(
                It.IsAny<Instrument>(),
                It.IsAny<CancellationToken>())).Throws<Exception>();
            presenter.SoftDownloadData();
            Thread.Sleep(500);
            downloadService.Verify(x => x.SoftDownload(It.IsAny<Instrument>(),
               It.IsAny<CancellationToken>()),
               Times.Exactly(1));
            Assert.That(presenter.StatusMessage, Is.EqualTo(TradesStatuses.FailToDownloud));
        }


        [Test]
        public void IfIsActiveDowloadTaskPresentSoftDownloadDataWillNotStart()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);
            var presenter = new DounloadInstrumentPresenter(new Instrument.Builder().Build());
            downloadService.Setup(x => x.SoftDownload(
                It.IsAny<Instrument>(),
                It.IsAny<CancellationToken>()))
                .Callback(() => Thread.Sleep(500));
            presenter.SoftDownloadData();
            presenter.SoftDownloadData();
            Thread.Sleep(1000);
            downloadService.Verify(x => x.SoftDownload(It.IsAny<Instrument>(),
                It.IsAny<CancellationToken>()),
                Times.Exactly(1));
        }

        [Test]
        public void CheckDeleteData()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);
            var presenter = new DounloadInstrumentPresenter(new Instrument.Builder().Build());
            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(x => x.GetEvent<RemoveFromList<IDounloadInstrumentPresenter>>()
                .Publish(It.IsAny<IDounloadInstrumentPresenter>()));

            ContainerBuilder.Container.RegisterInstance(eventAggregator.Object);
            presenter.DeleteData();
            Thread.Sleep(500);
            downloadService.Verify(x => x.Delete(
                It.IsAny<Instrument>(),
                It.IsAny<Task>(),
                It.IsAny<CancellationTokenSource>()),
                Times.Exactly(1));
            eventAggregator.Verify(x => x.GetEvent<RemoveFromList<IDounloadInstrumentPresenter>>()
            .Publish(It.IsAny<IDounloadInstrumentPresenter>()), Times.Once);
        }

        [Test]
        public void DeleteDataWithFail()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);
            var presenter = new DounloadInstrumentPresenter(new Instrument.Builder().Build());
            downloadService.Setup(x => x.Delete(
                It.IsAny<Instrument>(),
                It.IsAny<Task>(),
                It.IsAny<CancellationTokenSource>()
                )).Throws<Exception>();
            presenter.DeleteData();
            Thread.Sleep(500);
            downloadService.Verify(x => x.Delete(
                It.IsAny<Instrument>(),
                It.IsAny<Task>(),
                It.IsAny<CancellationTokenSource>()),
                Times.Exactly(1));
            Assert.That(presenter.StatusMessage, Is.EqualTo(TradesStatuses.FailToDelete));
        }

        [Test]
        public void CheckDataWithFail()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);
            var presenter = new DounloadInstrumentPresenter(new Instrument.Builder().Build());
            downloadService.Setup(x => x.CheckFiles(It.IsAny<Instrument>())).Throws<Exception>();
            presenter.CheckData();
            Thread.Sleep(500);
            downloadService.Verify(x => x.CheckFiles(It.IsAny<Instrument>()), Times.Once);
            Assert.That(presenter.StatusMessage, Is.EqualTo(TradesStatuses.FailToCheck));

        }

        [Test]
        public void CheckDataWithSuccessResult()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);
            var presenter = new DounloadInstrumentPresenter(new Instrument.Builder().Build());
            downloadService.Setup(x => x.CheckFiles(It.IsAny<Instrument>())).Returns(true);
            presenter.CheckData();
            Thread.Sleep(500);
            downloadService.Verify(x => x.CheckFiles(It.IsAny<Instrument>()), Times.Once);
            Assert.That(presenter.StatusMessage, Is.EqualTo(TradesStatuses.IsReady));

        }

        [Test]
        public void CheckDataWithCorruptedDataResult()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);
            var presenter = new DounloadInstrumentPresenter(new Instrument.Builder().Build());
            downloadService.Setup(x => x.CheckFiles(It.IsAny<Instrument>())).Returns(false);
            presenter.CheckData();
            Thread.Sleep(500);
            downloadService.Verify(x => x.CheckFiles(It.IsAny<Instrument>()), Times.Once);
            Assert.That(presenter.StatusMessage, Is.EqualTo(TradesStatuses.DataIsCorrapted));

        }

        [Test]
        public void CheckInDownloadingProgressStatus()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);
            var presenter = new DounloadInstrumentPresenter(new Instrument.Builder().Build());
            Assert.That(presenter.InDownloadingProgress, Is.False);
            presenter.HardDownloadData();
            Thread.Sleep(500);
            Assert.That(presenter.InDownloadingProgress, Is.False);
            downloadService.Setup(x => x.Download(
                It.IsAny<Instrument>(),
                It.IsAny<CancellationToken>()))
                .Callback(() => Thread.Sleep(1000));
            presenter.HardDownloadData();
            Assert.That(presenter.InDownloadingProgress, Is.True);
        }

        [Test]
        public void ShowDataInFolderWillCallFileManager()
        {
            var downloadService = new Mock<IInstrumentDownloadService>();
            ContainerBuilder.Container.RegisterInstance(downloadService.Object);
            Instrument instrument = new Instrument.Builder()
                .WithDataProvider("FINAM_TEST")
                .WithName("Name_1")
                .WithFrom(DateTime.Now)
                .WithTo(DateTime.Now)
                .Build();
            var presenter = new DounloadInstrumentPresenter(instrument);
            var fileManager = new Mock<IFileManager>();
            string expectedResult = null;

            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            fileManager.Setup(x => x.OpenFolder(It.IsAny<string>())).Callback<string>(x => expectedResult = x);

            presenter.ShowDataInFolder();

            fileManager.Verify(x => x.OpenFolder(It.IsAny<string>()), Times.Once);
            Assert.That(expectedResult, Is.EqualTo(instrument.DataProvider + "\\" + instrument.Path));
        }

    }
}