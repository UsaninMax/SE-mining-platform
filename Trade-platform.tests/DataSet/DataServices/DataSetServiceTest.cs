using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TradePlatform;
using TradePlatform.Commons.BaseModels;
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
        [Test]
        public void TestBuildSet()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var dataTickStorage = new Mock<IDataTickStorage>();
            ContainerBuilder.Container.RegisterInstance(dataTickStorage.Object);
            var dataTickProvider = new Mock<IDataTickProvider>();
            ContainerBuilder.Container.RegisterInstance(dataTickProvider.Object);

            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();
            IList<DataTick> ticks = new List<DataTick>()
            {
                new DataTick() { Date = new DateTime(2017, 3, 17)},
                new DataTick() { Date = new DateTime(2017, 3, 15)},
                new DataTick() { Date = new DateTime(2017, 3, 16)}
            };

            dataTickProvider.Setup(x => x.Get(item, It.IsAny<CancellationToken>())).Returns(ticks);

            DataSetService datasevice = new DataSetService();
            datasevice.Store(item, new CancellationToken());

            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DataSetInfo>()), Times.Once);
            dataTickStorage.Verify(x => x.Store(ticks, DataSetItem.RootPath + "\\" + item.Path, item.Path), Times.Once);
            fileManager.Verify(x => x.DeleteFolder(DataSetItem.RootPath + "\\" + item.Path), Times.Once);
            fileManager.Verify(x => x.CreateFolder(DataSetItem.RootPath + "\\" + item.Path), Times.Once);

        }

        [Test]
        public void TestBuildSetIfCancellationRequired()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var dataTickStorage = new Mock<IDataTickStorage>();
            ContainerBuilder.Container.RegisterInstance(dataTickStorage.Object);
            var dataTickProvider = new Mock<IDataTickProvider>();
            ContainerBuilder.Container.RegisterInstance(dataTickProvider.Object);

            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();
            IList<DataTick> ticks = new List<DataTick>()
            {
                new DataTick() { Date = new DateTime(2017, 3, 17)},
                new DataTick() { Date = new DateTime(2017, 3, 15)},
                new DataTick() { Date = new DateTime(2017, 3, 16)}
            };

            dataTickProvider.Setup(x => x.Get(item, It.IsAny<CancellationToken>())).Returns(ticks);

            CancellationToken cancellationToken = new CancellationToken(true);
            DataSetService datasevice = new DataSetService();
            datasevice.Store(item, cancellationToken);

            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DataSetInfo>()), Times.Never);
            dataTickStorage.Verify(x => x.Store(ticks, DataSetItem.RootPath + "\\" + item.Path, item.Path), Times.Never);
            fileManager.Verify(x => x.DeleteFolder(DataSetItem.RootPath + "\\" + item.Path), Times.Never);
            fileManager.Verify(x => x.CreateFolder(DataSetItem.RootPath + "\\" + item.Path), Times.Never);

        }

        [Test]
        public void DeleteDataSetWhenBuildTaskDoesNotFinished()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var dataTickStorage = new Mock<IDataTickStorage>();
            ContainerBuilder.Container.RegisterInstance(dataTickStorage.Object);
            var instrumentsHolder = new Mock<IDataSetHolder>();
            ContainerBuilder.Container.RegisterInstance(instrumentsHolder.Object);

            var keepAliveTask = new Task(() => Thread.Sleep(100));
            keepAliveTask.Start();

            DataSetService datasevice = new DataSetService();

            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            datasevice.Delete(item, keepAliveTask, cancellationTokenSource);
            Assert.That(cancellationTokenSource.IsCancellationRequested, Is.True);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DataSetInfo>()), Times.Exactly(2));
            fileManager.Verify(x => x.DeleteFolder(DataSetItem.RootPath + "\\" + item.Path), Times.Once);
            instrumentsHolder.Verify(x => x.Remove(item), Times.Once);
        }

        [Test]
        public void DeleteDataSetWhenBuildTaskWasFinished()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var dataTickStorage = new Mock<IDataTickStorage>();
            ContainerBuilder.Container.RegisterInstance(dataTickStorage.Object);
            var instrumentsHolder = new Mock<IDataSetHolder>();
            ContainerBuilder.Container.RegisterInstance(instrumentsHolder.Object);

            var keepAliveTask = new Task(() => { });
            keepAliveTask.Start();
            Thread.Sleep(500);
            DataSetService datasevice = new DataSetService();

            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            datasevice.Delete(item, keepAliveTask, cancellationTokenSource);
            Assert.That(cancellationTokenSource.IsCancellationRequested, Is.False);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DataSetInfo>()), Times.Exactly(1));
            fileManager.Verify(x => x.DeleteFolder(DataSetItem.RootPath + "\\" + item.Path), Times.Once);
            instrumentsHolder.Verify(x => x.Remove(item), Times.Once);
        }


        [Test]
        public void ChechFilesIfExist()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var dataSetHolder = new Mock<IDataSetHolder>();
            ContainerBuilder.Container.RegisterInstance(dataSetHolder.Object);
            var fileManager = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(fileManager.Object);
            var dataTickStorage = new Mock<IDataTickStorage>();
            ContainerBuilder.Container.RegisterInstance(dataTickStorage.Object);
            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();
            fileManager.Setup(x => x.IsDirectoryExist(DataSetItem.RootPath + "\\" + item.Path)).Returns(true);

            DataSetService datasevice = new DataSetService();
            datasevice.CheckIfExist(item);

            fileManager.Verify(x => x.IsDirectoryExist(DataSetItem.RootPath + "\\" + item.Path), Times.Once);
            fileManager.Verify(x => x.IsFileExist(DataSetItem.RootPath + "\\" + item.Path + "\\" + item.Path + ".xml"), Times.Once);

        }
    }
}
