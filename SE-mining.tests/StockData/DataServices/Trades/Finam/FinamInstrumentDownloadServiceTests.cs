using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using SEMining.Commons.Info;
using SEMining.Commons.Sistem;
using SEMining.StockData.DataServices.Trades;
using SEMining.StockData.DataServices.Trades.Finam;
using SEMining.StockData.Holders;
using SEMining.StockData.Models;
using SE_mining_base.Info.Message;

namespace SEMining.tests.StockData.DataServices.Trades.Finam
{
    [TestFixture]
    public class FinamInstrumentDownloadServiceTests
    {
        private Instrument _instrument;

        [SetUp]
        public void SetUp()
        {
            _instrument = new Instrument.Builder()
               .WithDataProvider("FINAM_TEST")
               .WithName("Name_1")
               .WithFrom(DateTime.Now)
               .WithTo(DateTime.Now)
               .Build();
            var infoPublisher = new Mock<IInfoPublisher>();
            var holder = new Mock<IDownloadedInstrumentsHolder>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            ContainerBuilder.Container.RegisterInstance(holder.Object);
        }

        [Test]
        public void CheckDownload()
        {
            var splitter = new Mock<IInstrumentSplitter>();
            var fileManager = new Mock<IFileManager>();
            var downloader = new Mock<ITradesDownloader>();
            ContainerBuilder.Container.RegisterInstance(splitter.Object);
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            ContainerBuilder.Container.RegisterInstance(downloader.Object);
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);

            var downloadService = new FinamInstrumentService();
            var cancellationTokenSource = new CancellationTokenSource();

            String expectedDeletePath = null;
            String expectedCreatePath = null;
            Instrument expectedInstrumentForSplit = null;
            Instrument expectedInstrumentForDownload = null;

            fileManager.Setup(x => x.DeleteFolder(It.IsAny<string>())).Callback<string>(x => expectedDeletePath = x);
            fileManager.Setup(x => x.CreateFolder(It.IsAny<string>())).Callback<string>(x => expectedCreatePath = x);
            splitter.Setup(x => x.Split(It.IsAny<Instrument>()))
                .Callback<Instrument>(x => expectedInstrumentForSplit = x)
                .Returns(new List<Instrument> { _instrument });
            downloader.Setup(x => x.Download(It.IsAny<Instrument>()))
                .Callback<Instrument>(x => expectedInstrumentForDownload = x);

            downloadService.Download(_instrument, cancellationTokenSource.Token);

            fileManager.Verify(x => x.DeleteFolder(It.IsAny<string>()), Times.Once);
            fileManager.Verify(x => x.CreateFolder(It.IsAny<string>()), Times.Once);
            splitter.Verify(x => x.Split(It.IsAny<Instrument>()), Times.Once);
            downloader.Verify(x => x.Download(It.IsAny<Instrument>()), Times.Once);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DownloadInfo>()), Times.Once);

            Assert.That(expectedDeletePath, Is.EqualTo(_instrument.DataProvider + "\\" + _instrument.Path));
            Assert.That(expectedCreatePath, Is.EqualTo(_instrument.DataProvider + "\\" + _instrument.Path));
            Assert.That(expectedInstrumentForSplit, Is.EqualTo(_instrument));
            Assert.That(expectedInstrumentForDownload, Is.EqualTo(_instrument));
        }
        [Test]
        public void CheckDownloadIfCancelationTockenIsActivated()
        {
            var splitter = new Mock<IInstrumentSplitter>();
            var fileManager = new Mock<IFileManager>();
            var downloader = new Mock<ITradesDownloader>();
            ContainerBuilder.Container.RegisterInstance(splitter.Object);
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            ContainerBuilder.Container.RegisterInstance(downloader.Object);
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);

            var downloadService = new FinamInstrumentService();
            var cancellationTokenSource = new CancellationTokenSource();

            String expectedDeletePath = null;
            String expectedCreatePath = null;
            Instrument expectedInstrumentForSplit = null;

            fileManager.Setup(x => x.DeleteFolder(It.IsAny<string>())).Callback<string>(x => expectedDeletePath = x);
            fileManager.Setup(x => x.CreateFolder(It.IsAny<string>())).Callback<string>(x => expectedCreatePath = x);
            splitter.Setup(x => x.Split(It.IsAny<Instrument>()))
                .Callback<Instrument>(x => expectedInstrumentForSplit = x)
                .Returns(new List<Instrument> { _instrument });
            cancellationTokenSource.Cancel();
            downloadService.Download(_instrument, cancellationTokenSource.Token);

            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DownloadInfo>()), Times.Never);
            fileManager.Verify(x => x.DeleteFolder(It.IsAny<string>()), Times.Once);
            fileManager.Verify(x => x.CreateFolder(It.IsAny<string>()), Times.Once);
            splitter.Verify(x => x.Split(It.IsAny<Instrument>()), Times.Once);
            downloader.Verify(x => x.Download(It.IsAny<Instrument>()), Times.Never);

            Assert.That(expectedDeletePath, Is.EqualTo(_instrument.DataProvider + "\\" + _instrument.Path));
            Assert.That(expectedCreatePath, Is.EqualTo(_instrument.DataProvider + "\\" + _instrument.Path));
            Assert.That(expectedInstrumentForSplit, Is.EqualTo(_instrument));
        }

        [Test]
        public void CheckSoftDownload()
        {
            var splitter = new Mock<IInstrumentSplitter>();
            var fileManager = new Mock<IFileManager>();
            var downloader = new Mock<ITradesDownloader>();
            ContainerBuilder.Container.RegisterInstance(splitter.Object);
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            ContainerBuilder.Container.RegisterInstance(downloader.Object);
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);

            var downloadService = new FinamInstrumentService();
            var cancellationTokenSource = new CancellationTokenSource();

            String expectedIsExistPath = null;
            String expectedCreatePath = null;
            Instrument expectedInstrumentForSplit = null;
            Instrument expectedInstrumentForDownload = null;

            fileManager.Setup(x => x.IsFileExist(It.IsAny<string>())).Callback<string>(x => expectedIsExistPath = x);
            fileManager.Setup(x => x.CreateFolder(It.IsAny<string>())).Callback<string>(x => expectedCreatePath = x);
            splitter.Setup(x => x.Split(It.IsAny<Instrument>()))
                .Callback<Instrument>(x => expectedInstrumentForSplit = x)
                .Returns(new List<Instrument> { _instrument });
            downloader.Setup(x => x.Download(It.IsAny<Instrument>()))
                .Callback<Instrument>(x => expectedInstrumentForDownload = x);

            downloadService.SoftDownload(_instrument, cancellationTokenSource.Token);

            fileManager.Verify(x => x.IsFileExist(It.IsAny<string>()), Times.Once);
            fileManager.Verify(x => x.CreateFolder(It.IsAny<string>()), Times.Once);
            splitter.Verify(x => x.Split(It.IsAny<Instrument>()), Times.Once);
            downloader.Verify(x => x.Download(It.IsAny<Instrument>()), Times.Once);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DownloadInfo>()), Times.Once);

            Assert.That(expectedIsExistPath, Is.EqualTo(_instrument.DataProvider + "\\" + _instrument.Path + "\\" + _instrument.FileName + ".txt"));
            Assert.That(expectedCreatePath, Is.EqualTo(_instrument.DataProvider + "\\" + _instrument.Path));
            Assert.That(expectedInstrumentForSplit, Is.EqualTo(_instrument));
            Assert.That(expectedInstrumentForDownload, Is.EqualTo(_instrument));
        }

        [Test]
        public void SoftDownloadIfCancelationTockenIsActivated()
        {
            var splitter = new Mock<IInstrumentSplitter>();
            var fileManager = new Mock<IFileManager>();
            var downloader = new Mock<ITradesDownloader>();
            ContainerBuilder.Container.RegisterInstance(splitter.Object);
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            ContainerBuilder.Container.RegisterInstance(downloader.Object);
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);

            var downloadService = new FinamInstrumentService();
            var cancellationTokenSource = new CancellationTokenSource();

            String expectedCreatePath = null;
            Instrument expectedInstrumentForSplit = null;

            fileManager.Setup(x => x.CreateFolder(It.IsAny<string>())).Callback<string>(x => expectedCreatePath = x);
            splitter.Setup(x => x.Split(It.IsAny<Instrument>()))
                .Callback<Instrument>(x => expectedInstrumentForSplit = x)
                .Returns(new List<Instrument> { _instrument });

            cancellationTokenSource.Cancel();
            downloadService.SoftDownload(_instrument, cancellationTokenSource.Token);

            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DownloadInfo>()), Times.Never);
            fileManager.Verify(x => x.IsFileExist(It.IsAny<string>()), Times.Never);
            fileManager.Verify(x => x.CreateFolder(It.IsAny<string>()), Times.Once);
            splitter.Verify(x => x.Split(It.IsAny<Instrument>()), Times.Once);
            downloader.Verify(x => x.Download(It.IsAny<Instrument>()), Times.Never);

            Assert.That(expectedCreatePath, Is.EqualTo(_instrument.DataProvider + "\\" + _instrument.Path));
            Assert.That(expectedInstrumentForSplit, Is.EqualTo(_instrument));
        }

        [Test]
        public void CheckSoftDownloadIfFileExist()
        {
            var splitter = new Mock<IInstrumentSplitter>();
            var fileManager = new Mock<IFileManager>();
            var downloader = new Mock<ITradesDownloader>();
            ContainerBuilder.Container.RegisterInstance(splitter.Object);
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            ContainerBuilder.Container.RegisterInstance(downloader.Object);

            var downloadService = new FinamInstrumentService();
            var cancellationTokenSource = new CancellationTokenSource();

            String expectedIsExistPath = null;
            String expectedCreatePath = null;
            Instrument expectedInstrumentForSplit = null;

            fileManager.Setup(x => x.IsFileExist(It.IsAny<string>()))
                .Callback<string>(x => expectedIsExistPath = x)
                .Returns(true);
            fileManager.Setup(x => x.CreateFolder(It.IsAny<string>())).Callback<string>(x => expectedCreatePath = x);
            splitter.Setup(x => x.Split(It.IsAny<Instrument>()))
                .Callback<Instrument>(x => expectedInstrumentForSplit = x)
                .Returns(new List<Instrument> { _instrument });

            downloadService.SoftDownload(_instrument, cancellationTokenSource.Token);

            fileManager.Verify(x => x.IsFileExist(It.IsAny<string>()), Times.Once);
            fileManager.Verify(x => x.CreateFolder(It.IsAny<string>()), Times.Once);
            splitter.Verify(x => x.Split(It.IsAny<Instrument>()), Times.Once);
            downloader.Verify(x => x.Download(It.IsAny<Instrument>()), Times.Never);

            Assert.That(expectedIsExistPath, Is.EqualTo(_instrument.DataProvider + "\\" + _instrument.Path + "\\" + _instrument.FileName + ".txt"));
            Assert.That(expectedCreatePath, Is.EqualTo(_instrument.DataProvider + "\\" + _instrument.Path));
            Assert.That(expectedInstrumentForSplit, Is.EqualTo(_instrument));
        }

        [Test]
        public void CheckDelete()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var splitter = new Mock<IInstrumentSplitter>();
            ContainerBuilder.Container.RegisterInstance(splitter.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var holderMock = new Mock<IDownloadedInstrumentsHolder>();
            ContainerBuilder.Container.RegisterInstance(holderMock.Object);
            var downloadService = new FinamInstrumentService();
            var cancellationTokenSource = new CancellationTokenSource();
            downloadService.Delete(_instrument,null, cancellationTokenSource);
            fileManager.Verify(x => x.DeleteFolder(It.IsAny<string>()), Times.Once);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DownloadInfo>()), Times.Once);
            holderMock.Verify(x => x.Remove(It.IsAny<Instrument>()), Times.Once);

        }

        [Test]
        public void DeleteIfDownloadTaskRunning()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var splitter = new Mock<IInstrumentSplitter>();
            ContainerBuilder.Container.RegisterInstance(splitter.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var downloadService = new FinamInstrumentService();
            var cancellationTokenSource = new CancellationTokenSource();
            fileManager.Setup(x => x.DeleteFolder(It.IsAny<string>()));
            var downloadTask = new Task(() => Thread.Sleep(1000));
            downloadTask.Start();
            downloadService.Delete(_instrument, downloadTask, cancellationTokenSource);
            fileManager.Verify(x => x.DeleteFolder(It.IsAny<string>()), Times.Once);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DownloadInfo>()), Times.Exactly(2));
            Assert.That(cancellationTokenSource.Token.IsCancellationRequested, Is.True);
        }

        [Test]
        public void CheckFiles()
        {
            var splitter = new Mock<IInstrumentSplitter>();
            ContainerBuilder.Container.RegisterInstance(splitter.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var downloadService = new FinamInstrumentService();

            String expectedIsExistPath = null;
            String expectedIsExistDirectoryPath = null;
            Instrument expectedInstrumentForSplit = null;

            fileManager.Setup(x => x.IsFileExist(It.IsAny<string>()))
                .Callback<string>(x => expectedIsExistPath = x)
                .Returns(true);
            fileManager.Setup(x => x.IsDirectoryExist(It.IsAny<string>()))
                .Callback<string>(x => expectedIsExistDirectoryPath = x)
                .Returns(true);
            splitter.Setup(x => x.Split(It.IsAny<Instrument>()))
                .Callback<Instrument>(x => expectedInstrumentForSplit = x)
                .Returns(new List<Instrument> { _instrument });

            bool result = downloadService.CheckFiles(_instrument);

            fileManager.Verify(x => x.IsDirectoryExist(It.IsAny<string>()), Times.Once);
            fileManager.Verify(x => x.IsFileExist(It.IsAny<string>()), Times.Once);
            Assert.That(expectedIsExistDirectoryPath, Is.EqualTo(_instrument.DataProvider + "\\" + _instrument.Path));
            Assert.That(expectedIsExistPath, Is.EqualTo(_instrument.DataProvider + "\\" + _instrument.Path + "\\" + _instrument.FileName + ".txt"));
            Assert.That(expectedInstrumentForSplit, Is.EqualTo(_instrument));
            Assert.That(result, Is.True);

        }

        [Test]
        public void CheckFilesIfFileNotExist()
        {
            var splitter = new Mock<IInstrumentSplitter>();
            ContainerBuilder.Container.RegisterInstance(splitter.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var downloadService = new FinamInstrumentService();

            fileManager.Setup(x => x.IsFileExist(It.IsAny<string>()))
                .Returns(false);
            fileManager.Setup(x => x.IsDirectoryExist(It.IsAny<string>()))
                .Returns(true);
            splitter.Setup(x => x.Split(It.IsAny<Instrument>()))
                .Returns(new List<Instrument> { _instrument });

            bool result = downloadService.CheckFiles(_instrument);
            Assert.That(result, Is.False);
        }

        [Test]
        public void CheckFilesIfDirectoryNotExist()
        {
            var splitter = new Mock<IInstrumentSplitter>();
            ContainerBuilder.Container.RegisterInstance(splitter.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var downloadService = new FinamInstrumentService();

            fileManager.Setup(x => x.IsFileExist(It.IsAny<string>()))
                .Returns(true);
            fileManager.Setup(x => x.IsDirectoryExist(It.IsAny<string>()))
                .Returns(false);
            splitter.Setup(x => x.Split(It.IsAny<Instrument>()))
                .Returns(new List<Instrument> { _instrument });

            bool result = downloadService.CheckFiles(_instrument);
            Assert.That(result, Is.False);
        }

        [Test]
        public void CheckFilesIfDirectoryAndFileNotExist()
        {
            var splitter = new Mock<IInstrumentSplitter>();
            ContainerBuilder.Container.RegisterInstance(splitter.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var downloadService = new FinamInstrumentService();

            fileManager.Setup(x => x.IsFileExist(It.IsAny<string>()))
                .Returns(false);
            fileManager.Setup(x => x.IsDirectoryExist(It.IsAny<string>()))
                .Returns(false);
            splitter.Setup(x => x.Split(It.IsAny<Instrument>()))
                .Returns(new List<Instrument> { _instrument });

            bool result = downloadService.CheckFiles(_instrument);
            Assert.That(result, Is.False);
        }
    }
}
