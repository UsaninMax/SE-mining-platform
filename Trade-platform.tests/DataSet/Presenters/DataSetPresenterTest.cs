using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Prism.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using TradePlatform;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.DataSet.DataServices;
using TradePlatform.DataSet.Events;
using TradePlatform.DataSet.Models;
using TradePlatform.DataSet.Presenters;

namespace Trade_platform.tests.DataSet.Presenters
{
    [TestFixture]
    public class DataSetPresenterTest
    {
        [SetUp]
        public void SetUp()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        [Test]
        public void TestPrepareData()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var dataSetService = new Mock<IDataSetService>();
            ContainerBuilder.Container.RegisterInstance(dataSetService.Object);
            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();

            dataSetService.Setup(x => x.BuildSet(item, It.IsAny<CancellationToken>()));
            DataSetPresenter presenter = new DataSetPresenter(item);
            presenter.PrepareData();
            Thread.Sleep(500);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DataSetInfo>()), Times.Once);
            Assert.That(presenter.StatusMessage, Is.EqualTo(Status.IsReady));
        }

        [Test]
        public void TestPrepareDataWhenThrowException()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var dataSetService = new Mock<IDataSetService>();
            ContainerBuilder.Container.RegisterInstance(dataSetService.Object);
            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();

            dataSetService.Setup(x => x.BuildSet(item, It.IsAny<CancellationToken>())).Throws<Exception>();
            DataSetPresenter presenter = new DataSetPresenter(item);
            presenter.PrepareData();
            Thread.Sleep(500);
            infoPublisher.Verify(x => x.PublishException(It.IsAny<AggregateException>()), Times.Once);
            Assert.That(presenter.StatusMessage, Is.EqualTo(Status.FailToDownloud));
        }

        [Test]
        public void TestDeleteData()
        {
            var eventAggregator = new Mock<IEventAggregator>();
            eventAggregator.Setup(x => x.GetEvent<RemovePresenterFromListEvent>()
                .Publish(It.IsAny<DataSetPresenter>()));
            ContainerBuilder.Container.RegisterInstance(eventAggregator.Object);

            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var dataSetService = new Mock<IDataSetService>();
            ContainerBuilder.Container.RegisterInstance(dataSetService.Object);
            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();

            dataSetService.Setup(x => x.Delete(item, It.IsAny<Task>(), It.IsAny<CancellationTokenSource>()));
            DataSetPresenter presenter = new DataSetPresenter(item);
            presenter.DeleteData();
            Thread.Sleep(500);
            eventAggregator.Verify(x => x.GetEvent<RemovePresenterFromListEvent>()
            .Publish(It.IsAny<DataSetPresenter>()), Times.Once);


            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DataSetInfo>()), Times.Once);
        }

        [Test]
        public void TestDeleteDataWhenFaild()
        {

            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var dataSetService = new Mock<IDataSetService>();
            ContainerBuilder.Container.RegisterInstance(dataSetService.Object);
            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();

            dataSetService.Setup(x => x.Delete(item, It.IsAny<Task>(), It.IsAny<CancellationTokenSource>())).Throws<Exception>();
            DataSetPresenter presenter = new DataSetPresenter(item);
            presenter.DeleteData();
            Thread.Sleep(500);
            infoPublisher.Verify(x => x.PublishException(It.IsAny<AggregateException>()), Times.Once);
            Assert.That(presenter.StatusMessage, Is.EqualTo(Status.FailToDelete));
        }

        [Test]
        public void TestCheckData()
        {

            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var dataSetService = new Mock<IDataSetService>();
            ContainerBuilder.Container.RegisterInstance(dataSetService.Object);
            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();

            dataSetService.Setup(x => x.CheckFiles(item)).Returns(true);
            DataSetPresenter presenter = new DataSetPresenter(item);
            presenter.CheckData();
            Thread.Sleep(500);

            Assert.That(presenter.StatusMessage, Is.EqualTo(Status.IsReady));
        }

        [Test]
        public void TestCheckDataWhenIsCorrupted()
        {

            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var dataSetService = new Mock<IDataSetService>();
            ContainerBuilder.Container.RegisterInstance(dataSetService.Object);
            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();

            dataSetService.Setup(x => x.CheckFiles(item)).Returns(false);
            DataSetPresenter presenter = new DataSetPresenter(item);
            presenter.CheckData();
            Thread.Sleep(500);

            Assert.That(presenter.StatusMessage, Is.EqualTo(Status.DataIsCorrupted));
        }

        [Test]
        public void TestCheckDataWhenIsFail()
        {

            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            var dataSetService = new Mock<IDataSetService>();
            ContainerBuilder.Container.RegisterInstance(dataSetService.Object);
            DataSetItem item = new DataSetItem.Builder().WithId("test_id").Build();

            dataSetService.Setup(x => x.CheckFiles(item)).Throws<Exception>();
            DataSetPresenter presenter = new DataSetPresenter(item);
            presenter.CheckData();
            Thread.Sleep(500);

            Assert.That(presenter.StatusMessage, Is.EqualTo(Status.FailToCheck));
            infoPublisher.Verify(x => x.PublishException(It.IsAny<AggregateException>()), Times.Once);
        }
    }
}
