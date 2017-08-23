using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using Prism.Events;
using SEMining.Commons.Info;
using SEMining.StockData.DataServices.SecuritiesInfo;
using SEMining.StockData.DataServices.Trades;
using SEMining.StockData.Events;
using SEMining.StockData.Holders;
using SEMining.StockData.Models;
using SEMining.StockData.Presenters;
using SEMining.StockData.ViewModels;
using SE_mining_base.Info.Message;

namespace SEMining.tests.StockData.ViewModels
{
    [TestFixture]
    public class FinamDownloadNewInstrumentViewModelTests
    {
        [SetUp]
        public void SetUp()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
        }
        [Test]
        public void WhenUpdateSecurityInfoWillFaildStatusWillChangedOnFail()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            Mock<ISecuritiesInfoUpdater> infoUpdaterMock = new Mock<ISecuritiesInfoUpdater>();
            infoUpdaterMock.Setup(m => m.Update()).Throws(new Exception());
            ContainerBuilder.Container.RegisterInstance(infoUpdaterMock.Object);
            FinamDownloadNewInstrumentViewModel newInstrumentViewModel = new FinamDownloadNewInstrumentViewModel();
            newInstrumentViewModel.UpdateSecuritiesInfo();
            Thread.Sleep(500);

            infoPublisher.Verify(x => x.PublishException(It.IsAny<AggregateException>()), Times.Once);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DownloadInfo>()), Times.Never);
            Assert.That(SecuritiesInfoStatuses.FailToUpdateSecuritiesInfo, Is.EqualTo(newInstrumentViewModel.StatusMessage));
            Assert.That(newInstrumentViewModel.HideWaitSpinnerBar, Is.True);
            Assert.That(newInstrumentViewModel.IsEnabledPanel, Is.False);
        }

        [Test]
        public void WhenUpdateSecurityInfoWillNotFaildInformationWillUpdated()
        {
            var infoPublisher = new Mock<IInfoPublisher>();
            ContainerBuilder.Container.RegisterInstance(infoPublisher.Object);
            Market market = new Market() { Id = "1234", Name = "Name" };
            Mock<ISecuritiesInfoUpdater> infoUpdaterMock = new Mock<ISecuritiesInfoUpdater>();
            SecuritiesInfoHolder infoHolder = new SecuritiesInfoHolder();
            infoHolder.Securities = new List<Security>
            {
                new Security()
                {
                    Id = "174",
                    Name = "Name",
                    Code = "123",
                    Market = market
                }
            };
            ContainerBuilder.Container.RegisterInstance(infoHolder);
            ContainerBuilder.Container.RegisterInstance(infoUpdaterMock.Object);
            FinamDownloadNewInstrumentViewModel newInstrumentViewModel = new FinamDownloadNewInstrumentViewModel();
            newInstrumentViewModel.UpdateSecuritiesInfo();
            Thread.Sleep(500);

            infoPublisher.Verify(x => x.PublishException(It.IsAny<AggregateException>()), Times.Never);
            infoPublisher.Verify(x => x.PublishInfo(It.IsAny<DownloadInfo>()), Times.Once);

            Assert.That(market, Is.EqualTo(newInstrumentViewModel.Markets[0]));
            Assert.That(newInstrumentViewModel.Markets.Count, Is.EqualTo(1));
            Assert.That(SecuritiesInfoStatuses.SecuritiesInfoUpdated, Is.EqualTo(newInstrumentViewModel.StatusMessage));
            Assert.That(newInstrumentViewModel.IsEnabledPanel, Is.True);
            Assert.That(newInstrumentViewModel.HideWaitSpinnerBar, Is.True);
        }

        [Test]
        public void WhenAddNewInstrumentWillPublishInAgregator()
        {

            ContainerBuilder.Container.RegisterInstance(new Mock<SecuritiesInfoHolder>().Object);
            ContainerBuilder.Container.RegisterInstance(new Mock<ISecuritiesInfoUpdater>().Object);
            ContainerBuilder.Container.RegisterInstance(new Mock<IInstrumentService>().Object);

            Mock<IEventAggregator> fakeEventAggregator = new Mock<IEventAggregator>();
            fakeEventAggregator.Setup(x => x.GetEvent<AddInstrumentToListEvent>()
                .Publish(It.IsAny<Instrument>()));

            ContainerBuilder.Container.RegisterInstance(fakeEventAggregator.Object);
            FinamDownloadNewInstrumentViewModel newInstrumentViewModel = new FinamDownloadNewInstrumentViewModel();
            newInstrumentViewModel.AddNewInstrument();

            fakeEventAggregator.Verify(x => x
                .GetEvent<AddInstrumentToListEvent>()
                .Publish(It.IsAny<Instrument>()), Times.Once);
        }

        [Test]
        public void WhenAddNewInstrumentFilledPresenterWillPublishInAgregator()
        {
            ContainerBuilder.Container.RegisterType<IDounloadInstrumentPresenter, DounloadInstrumentPresenter>(new InjectionConstructor(typeof(Instrument)));
            ContainerBuilder.Container.RegisterInstance(new Mock<SecuritiesInfoHolder>().Object);
            ContainerBuilder.Container.RegisterInstance(new Mock<ISecuritiesInfoUpdater>().Object);
            ContainerBuilder.Container.RegisterInstance(new Mock<IInstrumentService>().Object);
            Mock<IEventAggregator> fakeEventAggregator = new Mock<IEventAggregator>();
            ContainerBuilder.Container.RegisterInstance(fakeEventAggregator.Object);
            FinamDownloadNewInstrumentViewModel newInstrumentViewModel = new FinamDownloadNewInstrumentViewModel();

            DateTime currentDate = DateTime.Now;
            Security security = new Security
            {
                Id = "id",
                Code = "2344",
                Name = "name",
                Market = new Market
                {
                    Id = "33",
                    Name = "ff"
                }
            };

            newInstrumentViewModel.DateFrom = currentDate;
            newInstrumentViewModel.DateTo = currentDate;
            newInstrumentViewModel.SelectedSecurity = security;
            fakeEventAggregator.Setup(x => x.GetEvent<AddInstrumentToListEvent>()
            .Publish(It.IsAny<Instrument>()));

            newInstrumentViewModel.AddNewInstrument();
            fakeEventAggregator.Verify(x => x.GetEvent<AddInstrumentToListEvent>()
            .Publish(It.Is<Instrument>(
                m =>
                "FINAM".Equals(m.DataProvider) &&
                currentDate.Equals(m.From) &&
                currentDate.Equals(m.To) &&
                security.Market.Id.Equals(m.MarketId) &&
                security.Id.Equals(m.Id) &&
                security.Code.Equals(m.Code) &&
                security.Name.Equals(m.Name))), Times.Once);
        }

        [Test]
        public void WhenAddNewInstrumentWillFilledPresenterPublishInAgregatorWithoutSelectedSecurity()
        {
            ContainerBuilder.Container.RegisterType<IDounloadInstrumentPresenter, DounloadInstrumentPresenter>(new InjectionConstructor(typeof(Instrument)));
            ContainerBuilder.Container.RegisterInstance(new Mock<SecuritiesInfoHolder>().Object);
            ContainerBuilder.Container.RegisterInstance(new Mock<ISecuritiesInfoUpdater>().Object);
            ContainerBuilder.Container.RegisterInstance(new Mock<IInstrumentService>().Object);
            Mock<IEventAggregator> fakeEventAggregator = new Mock<IEventAggregator>();
            ContainerBuilder.Container.RegisterInstance(fakeEventAggregator.Object);
            FinamDownloadNewInstrumentViewModel newInstrumentViewModel = new FinamDownloadNewInstrumentViewModel();

            DateTime currentDate = DateTime.Now;
            newInstrumentViewModel.DateFrom = currentDate;
            newInstrumentViewModel.DateTo = currentDate;
            fakeEventAggregator.Setup(x => x.GetEvent<AddInstrumentToListEvent>()
            .Publish(It.IsAny<Instrument>()));

            newInstrumentViewModel.AddNewInstrument();
            fakeEventAggregator.Verify(x => x.GetEvent<AddInstrumentToListEvent>()
            .Publish(It.Is<Instrument>(
                m =>
                "FINAM".Equals(m.DataProvider) &&
                currentDate.Equals(m.From) &&
                currentDate.Equals(m.To) &&
                m.MarketId == null &&
                m.Id == null &&
                m.Code == null &&
                m.Name == null
                )), Times.Once);
        }
    }
}
