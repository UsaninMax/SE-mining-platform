using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradePlatform.StockDataDownload.DataServices.SecuritiesInfo;
using Moq;
using Prism.Events;
using TradePlatform;
using TradePlatform.Commons.MessageSubscribers;
using TradePlatform.Commons.Securities;
using TradePlatform.Commons.Trades;
using TradePlatform.StockDataDownload.DataServices.Trades;
using TradePlatform.StockDataDownload.Presenters;
using TradePlatform.StockDataDownload.ViewModels;

namespace Trade_platform.tests.StockDataDownload.ViewModels
{
    [TestClass]
    public class FinamDownloadNewInstrumentViewModelTests
    {
        [TestMethod]
        public void WhenUpdateSecurityInfoWillFaildStatusWillChangedOnFail()
        {
            Mock<ISecuritiesInfoUpdater> infoUpdaterMock = new Mock<ISecuritiesInfoUpdater>();
            infoUpdaterMock.Setup(m => m.Update()).Throws(new Exception());
            ContainerBuilder.Container.RegisterInstance(infoUpdaterMock.Object);
            FinamDownloadNewInstrumentViewModel newInstrumentViewModel = new FinamDownloadNewInstrumentViewModel();
            newInstrumentViewModel.UpdateSecuritiesInfo();
            Thread.Sleep(500);
            Assert.IsTrue(SecuritiesInfoStatuses.FailToUpdateSecuritiesInfo.Equals(newInstrumentViewModel.StatusMessage));
            Assert.IsTrue(newInstrumentViewModel.HideWaitSpinnerBar);
            Assert.IsFalse(newInstrumentViewModel.IsEnabledPanel);
        }

        [TestMethod]
        public void WhenUpdateSecurityInfoWillNotFaildInformationWillUpdated()
        {
            Market market = new Market() {Id = "1234", Name = "Name"};
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

            Assert.IsTrue(newInstrumentViewModel.HideWaitSpinnerBar);
            Assert.IsTrue(newInstrumentViewModel.IsEnabledPanel);
            Assert.IsTrue(SecuritiesInfoStatuses.SecuritiesInfoUpdated.Equals(newInstrumentViewModel.StatusMessage));
            Assert.IsTrue(newInstrumentViewModel.Markets.Count == 1);
            Assert.IsTrue(market.Equals(newInstrumentViewModel.Markets[0]));
        }

        [TestMethod]
        public void WhenAddNewInstrumentWillPublishToAgregator()
        {
            ContainerBuilder.Container.RegisterType<IDounloadInstrumentPresenter, DounloadInstrumentPresenter>(new InjectionConstructor(typeof(Instrument)));
            ContainerBuilder.Container.RegisterInstance(new Mock<SecuritiesInfoHolder>().Object);
            ContainerBuilder.Container.RegisterInstance(new Mock<ISecuritiesInfoUpdater>().Object);
            ContainerBuilder.Container.RegisterInstance(new Mock<IInstrumentDownloadService>().Object);

            Mock<IEventAggregator> fakeEventAggregator = new Mock<IEventAggregator>();
            fakeEventAggregator.Setup(x => x.GetEvent<AddToList<IDounloadInstrumentPresenter>>()
                .Publish(It.IsAny<IDounloadInstrumentPresenter>()));

            ContainerBuilder.Container.RegisterInstance(fakeEventAggregator.Object);
            FinamDownloadNewInstrumentViewModel newInstrumentViewModel = new FinamDownloadNewInstrumentViewModel();
            newInstrumentViewModel.AddNewInstrument();

            fakeEventAggregator.Verify(x => x
                .GetEvent<AddToList<IDounloadInstrumentPresenter>>()
                .Publish(It.IsAny<IDounloadInstrumentPresenter>()), Times.Once);
        }
    }
}
