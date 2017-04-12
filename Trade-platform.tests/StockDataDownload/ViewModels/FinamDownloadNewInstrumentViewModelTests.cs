using System;
using System.Threading;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradePlatform.StockDataDownload.DataServices.SecuritiesInfo;
using Moq;
using TradePlatform;
using TradePlatform.StockDataDownload.ViewModels;

namespace Trade_platform.tests.StockDataDownload.ViewModels
{
    [TestClass]
    public class FinamDownloadNewInstrumentViewModelTests
    {
        [TestMethod]
        public void WhenUpdateSecurityInfoIsFaildStatusWillChanged()
        {
            Mock<ISecuritiesInfoUpdater> infoUpdaterMock = new Mock<ISecuritiesInfoUpdater>();
            infoUpdaterMock.Setup(m => m.Update()).Throws(new Exception());
            ContainerBuilder.Container.RegisterInstance<ISecuritiesInfoUpdater>(infoUpdaterMock.Object);
            FinamDownloadNewInstrumentViewModel newInstrumentViewModel = new FinamDownloadNewInstrumentViewModel();
            newInstrumentViewModel.UpdateSecuritiesInfo();
            Thread.Sleep(500);
            Assert.IsTrue("Fail to update securities info".Equals(newInstrumentViewModel.StatusMessage));
        }
    }
}
