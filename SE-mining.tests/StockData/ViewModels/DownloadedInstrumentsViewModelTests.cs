using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Prism.Events;
using SEMining.Commons.Info;
using SEMining.StockData.DataServices.Trades;
using SEMining.StockData.Events;
using SEMining.StockData.Holders;
using SEMining.StockData.Models;
using SEMining.StockData.Presenters;
using SEMining.StockData.ViewModels;
using SEMining.Commons.Info.Model.Message;

namespace SEMining.tests.StockData.ViewModels
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
        public void WhenReceivePresenterAddItToHistoryTable()
        {
            ContainerBuilder.Container.RegisterType<IDounloadInstrumentPresenter, DounloadInstrumentPresenter>(new InjectionConstructor(typeof(Instrument)));
            ContainerBuilder.Container.RegisterInstance(new Mock<IDownloadedInstrumentsHolder>().Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            ContainerBuilder.Container.RegisterInstance(new Mock<IInstrumentService>().Object);

            var viewModel = new DownloadedInstrumentsViewModel();

            Instrument instrument = new Instrument.Builder().WithId("1122").Build();
            eventAggregator
                .GetEvent<AddInstrumentToListEvent>()
                .Publish(instrument);

            Assert.That(viewModel.InstrumentsInfo.Count, Is.EqualTo(1));
            Assert.That(viewModel.InstrumentsInfo[0].Instrument(), Is.EqualTo(instrument));
        }


        [Test]
        public void WhenReceivePresenterStartDownloadingProcess()
        {
            ContainerBuilder.Container.RegisterType<IDounloadInstrumentPresenter, DounloadInstrumentPresenter>(new InjectionConstructor(typeof(Instrument)));
            ContainerBuilder.Container.RegisterInstance(new Mock<IDownloadedInstrumentsHolder>().Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            ContainerBuilder.Container.RegisterInstance(new Mock<IInstrumentService>().Object);

            var holderMock = new Mock<IDownloadedInstrumentsHolder>();
            ContainerBuilder.Container.RegisterInstance(holderMock.Object);
            var viewModel = new DownloadedInstrumentsViewModel();

            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            presenterMock.Setup(x => x.SoftDownloadData());
            ContainerBuilder.Container.RegisterInstance(presenterMock.Object);
            eventAggregator
                .GetEvent<AddInstrumentToListEvent>()
                .Publish(new Instrument.Builder().Build());

            presenterMock.Verify(x => x.SoftDownloadData(), Times.Once);
            holderMock.Verify(x => x.Put(It.IsAny<Instrument>()), Times.Once);
        }

        [Test]
        public void WhenReceivePresenterDoNotStartDownloadingProcessWhenProcessIsExist()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            IEventAggregator eventAggregator = new EventAggregator();
            ContainerBuilder.Container.RegisterInstance(eventAggregator);
            ContainerBuilder.Container.RegisterInstance(new Mock<IInstrumentService>().Object);

            var viewModel = new DownloadedInstrumentsViewModel();

            var storedPresenterMock = new Mock<IDounloadInstrumentPresenter>();
            storedPresenterMock.Setup(x => x.InDownloadingProgress()).Returns(true);
            viewModel.InstrumentsInfo.Add(storedPresenterMock.Object);

            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            presenterMock.Setup(x => x.SoftDownloadData());

            eventAggregator
                .GetEvent<AddInstrumentToListEvent>()
                .Publish(new Instrument.Builder().Build());
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DownloadInfo>()), Times.Once);
            presenterMock.Verify(x => x.SoftDownloadData(), Times.Never);

        }

        [Test]
        public void CheckOpenDataFolderCommand()
        {
            ContainerBuilder.Container.RegisterInstance(new Mock<IDownloadedInstrumentsHolder>().Object);
            var viewModel = new DownloadedInstrumentsViewModel();
            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            viewModel.SelectedPresenter = presenterMock.Object;
            viewModel.OpenFolderCommand.Execute(presenterMock.Object);
            presenterMock.Verify(x => x.ShowDataInFolder(), Times.Once);
        }

        [Test]
        public void CheckRemoveDataCommand()
        {
            ContainerBuilder.Container.RegisterInstance(new Mock<IDownloadedInstrumentsHolder>().Object);
            var viewModel = new DownloadedInstrumentsViewModel();
            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            viewModel.SelectedPresenter = presenterMock.Object;
            viewModel.RemoveCommand.Execute(presenterMock.Object);
            presenterMock.Verify(x => x.DeleteData(), Times.Once);
        }

        [Test]
        public void CheckHardReloadCommand()
        {
            ContainerBuilder.Container.RegisterInstance(new Mock<IDownloadedInstrumentsHolder>().Object);
            var viewModel = new DownloadedInstrumentsViewModel();
            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            viewModel.SelectedPresenter = presenterMock.Object;
            viewModel.HardReloadCommand.Execute(null);
            presenterMock.Verify(x => x.HardReloadData(), Times.Once);
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
            ContainerBuilder.Container.RegisterInstance(new Mock<IDownloadedInstrumentsHolder>().Object);
            var viewModel = new DownloadedInstrumentsViewModel();
            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            viewModel.SelectedPresenter = presenterMock.Object;
            viewModel.SoftReloadCommand.Execute(presenterMock.Object);
            presenterMock.Verify(x => x.SoftReloadData(), Times.Once);
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
            var holder = new Mock<IDownloadedInstrumentsHolder>();
            Instrument instr = new Instrument.Builder().WithId("12").Build();
            HashSet<Instrument> instruments = new HashSet<Instrument> { instr, instr };
            holder.Setup(x => x.GetAll()).Returns(instruments);

            var presenterMock = new Mock<IDounloadInstrumentPresenter>();
            ContainerBuilder.Container.RegisterInstance(presenterMock.Object);
            ContainerBuilder.Container.RegisterInstance(holder.Object);

            var viewModel = new DownloadedInstrumentsViewModel();
            viewModel.LoadedWindowCommand.Execute(null);
            Thread.Sleep(500);

            presenterMock.Verify(x => x.CheckData(), Times.Exactly(1));
            Assert.That(viewModel.InstrumentsInfo.Count, Is.EqualTo(1));
        }
    }
}
