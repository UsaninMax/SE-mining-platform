using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Prism.Events;
using SEMining.Commons.Info;
using SEMining.DataSet.Events;
using SEMining.DataSet.Holders;
using SEMining.DataSet.Models;
using SEMining.DataSet.Presenters;
using SEMining.DataSet.ViewModels;

namespace SEMining.tests.DataSet.ViewModels
{
    [TestFixture]
    public class DataSetListViewModelTest
    {

        [SetUp]
        public void SetUp()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        [Test]
        public void TestProcessCreation()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            var dataSetPresenter = new Mock<IDataSetPresenter>();
            ContainerBuilder.Container.RegisterInstance(dataSetPresenter.Object);

            DataSetListViewModel model = new DataSetListViewModel();

            eventAggregator.GetEvent<CreateDataSetItemEvent>().Publish(new DataSetItem.Builder().Build());

            Assert.That(model.DataSetPresenterInfo.Count, Is.EqualTo(1));
            Assert.That(model.DataSetPresenterInfo[0], Is.EqualTo(dataSetPresenter.Object));
            dataSetPresenter.Verify(x => x.PrepareData(), Times.Exactly(1));
        }

        [Test]
        public void TestRemovePresenmterFromList()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            var dataSetPresenter = new Mock<IDataSetPresenter>();
            ContainerBuilder.Container.RegisterInstance(dataSetPresenter.Object);
            DataSetListViewModel model = new DataSetListViewModel();
            model.DataSetPresenterInfo.Add(dataSetPresenter.Object);
            eventAggregator.GetEvent<RemovePresenterFromListEvent>().Publish(dataSetPresenter.Object);

            Assert.That(model.DataSetPresenterInfo.Count, Is.EqualTo(0));

        }

        [Test]
        public void TestRemoveDataSet()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            var dataSetPresenter = new Mock<IDataSetPresenter>();
            ContainerBuilder.Container.RegisterInstance(dataSetPresenter.Object);
            DataSetListViewModel model = new DataSetListViewModel { SelectedSetPresenter = dataSetPresenter.Object };
            model.RemoveDataSetCommand.Execute(null);


            dataSetPresenter.Verify(x => x.DeleteData(), Times.Exactly(1));

        }

        [Test]
        public void TestOpenFolder()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            var dataSetPresenter = new Mock<IDataSetPresenter>();
            ContainerBuilder.Container.RegisterInstance(dataSetPresenter.Object);
            DataSetListViewModel model = new DataSetListViewModel { SelectedSetPresenter = dataSetPresenter.Object };
            model.OpenFolderCommand.Execute(null);


            dataSetPresenter.Verify(x => x.ShowDataInFolder(), Times.Exactly(1));

        }

        [Test]
        public void CheckLoadWindowHistory()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            var holder = new Mock<IDataSetHolder>();
            DataSetItem item = new DataSetItem.Builder().WithId("12").Build();
            IEnumerable<DataSetItem> instruments = new List<DataSetItem> { item, item };
            holder.Setup(x => x.GetAll()).Returns(instruments);

            var presenterMock = new Mock<IDataSetPresenter>();
            ContainerBuilder.Container.RegisterInstance(presenterMock.Object);
            ContainerBuilder.Container.RegisterInstance(holder.Object);

            var viewModel = new DataSetListViewModel();
            viewModel.LoadedWindowCommand.Execute(null);
            Thread.Sleep(500);

            presenterMock.Verify(x => x.CheckData(), Times.Exactly(2));
            Assert.That(viewModel.DataSetPresenterInfo.Count, Is.EqualTo(2));
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
