using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TradePlatform;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.Commons.Sistem;
using TradePlatform.DataSet.DataServices;
using TradePlatform.DataSet.DataServices.Serialization;
using TradePlatform.DataSet.Holders;
using TradePlatform.DataSet.Models;
using TradePlatform.StockData.Models;

namespace Trade_platform.tests.DataSet.DataServices
{
    [TestFixture]
    public class DataSetServiceTest
    {
        private Mock<IDataSetHolder> _instrumentsHolder;
        private Mock<IInfoPublisher> _infoPublisher;
        private Mock<IFileManager> _fileManager;
        private Mock<IDataTickStorage> _dataTickStorage;
        private Mock<IDataTickProvider> _dataTickProvider;

        [SetUp]
        public void SetUp()
        {
            _instrumentsHolder = new Mock<IDataSetHolder>();
            ContainerBuilder.Container.RegisterInstance(_instrumentsHolder.Object);
            _infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(_infoPublisher.Object);
            _fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(_fileManager.Object);
            _dataTickStorage = new Mock<IDataTickStorage>();
            ContainerBuilder.Container.RegisterInstance(_dataTickStorage.Object);
            _dataTickProvider = new Mock<IDataTickProvider>();
            ContainerBuilder.Container.RegisterInstance(_dataTickProvider.Object);
        }



        [Test]
        public void TestBuildSet()
        {
            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();
            IEnumerable<DataTick> ticks = new List<DataTick>()
            {
                new DataTick() { Date = new DateTime(2017, 3, 17)},
                new DataTick() { Date = new DateTime(2017, 3, 15)},
                new DataTick() { Date = new DateTime(2017, 3, 16)}
            };

           _dataTickProvider.Setup(x => x.Get(item, It.IsAny<CancellationToken>())).Returns(ticks);

            DataSetService datasevice = new DataSetService();
            datasevice.Store(item, new CancellationToken());

            _infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DataSetInfo>()), Times.Exactly(3));
            _dataTickStorage.Verify(x => x.Store(ticks, DataSetItem.RootPath + "\\" + item.Path, item.Path), Times.Once);
            _fileManager.Verify(x => x.DeleteFolder(DataSetItem.RootPath + "\\" + item.Path), Times.Once);
            _fileManager.Verify(x => x.CreateFolder(DataSetItem.RootPath + "\\" + item.Path), Times.Once);

        }

        [Test]
        public void TestBuildSetIfCancellationRequired()
        {
            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();
            IEnumerable<DataTick> ticks = new List<DataTick>()
            {
                new DataTick() { Date = new DateTime(2017, 3, 17)},
                new DataTick() { Date = new DateTime(2017, 3, 15)},
                new DataTick() { Date = new DateTime(2017, 3, 16)}
            };

            _dataTickProvider.Setup(x => x.Get(item, It.IsAny<CancellationToken>())).Returns(ticks);

            CancellationToken cancellationToken = new CancellationToken(true);
            DataSetService datasevice = new DataSetService();
            datasevice.Store(item, cancellationToken);

            _infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DataSetInfo>()), Times.Exactly(2));
            _dataTickStorage.Verify(x => x.Store(ticks, DataSetItem.RootPath + "\\" + item.Path, item.Path), Times.Never);
            _fileManager.Verify(x => x.DeleteFolder(DataSetItem.RootPath + "\\" + item.Path), Times.Never);
            _fileManager.Verify(x => x.CreateFolder(DataSetItem.RootPath + "\\" + item.Path), Times.Never);

        }

        [Test]
        public void DeleteDataSetWhenBuildTaskDoesNotFinished()
        {
            var keepAliveTask = new Task(() => Thread.Sleep(100));
            keepAliveTask.Start();

            DataSetService datasevice = new DataSetService();

            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            datasevice.Delete(item, keepAliveTask, cancellationTokenSource);
            Assert.That(cancellationTokenSource.IsCancellationRequested, Is.True);
            _infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DataSetInfo>()), Times.Exactly(2));
            _fileManager.Verify(x => x.DeleteFolder(DataSetItem.RootPath + "\\" + item.Path), Times.Once);
            _instrumentsHolder.Verify(x => x.Remove(item), Times.Once);
        }

        [Test]
        public void DeleteDataSetWhenBuildTaskWasFinished()
        {
            var keepAliveTask = new Task(() => { });
            keepAliveTask.Start();
            Thread.Sleep(500);
            DataSetService datasevice = new DataSetService();

            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            datasevice.Delete(item, keepAliveTask, cancellationTokenSource);
            Assert.That(cancellationTokenSource.IsCancellationRequested, Is.False);
            _infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DataSetInfo>()), Times.Exactly(1));
            _fileManager.Verify(x => x.DeleteFolder(DataSetItem.RootPath + "\\" + item.Path), Times.Once);
            _instrumentsHolder.Verify(x => x.Remove(item), Times.Once);
        }


        [Test]
        public void ChechFilesIfExist()
        {
            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();
            _fileManager.Setup(x => x.IsDirectoryExist(DataSetItem.RootPath + "\\" + item.Path)).Returns(true);

            DataSetService datasevice = new DataSetService();
            datasevice.CheckIfExist(item);

            _fileManager.Verify(x => x.IsDirectoryExist(DataSetItem.RootPath + "\\" + item.Path), Times.Once);
            _fileManager.Verify(x => x.IsFileExist(DataSetItem.RootPath + "\\" + item.Path + "\\" + item.Path + ".xml"), Times.Once);

        }
    }
}
