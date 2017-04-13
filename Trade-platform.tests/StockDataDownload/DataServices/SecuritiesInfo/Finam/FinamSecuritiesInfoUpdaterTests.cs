using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TradePlatform;
using TradePlatform.Commons.Securities;
using TradePlatform.StockDataDownload.DataServices.SecuritiesInfo;
using TradePlatform.StockDataDownload.DataServices.SecuritiesInfo.Finam;

namespace Trade_platform.tests.StockDataDownload.DataServices.SecuritiesInfo.Finam
{
    [TestClass]
    public class FinamSecuritiesInfoUpdaterTests
    {
        [TestMethod]
        public void ParserWillStoreDataToSecuritiesInfoHolder()
        {
            SecuritiesInfoHolder infoHolder = new SecuritiesInfoHolder();
            ContainerBuilder.Container.RegisterInstance(infoHolder);
            Mock<ISecuritiesInfoParser> parser = new Mock<ISecuritiesInfoParser>();
            ContainerBuilder.Container.RegisterInstance(parser.Object);
            Mock<ISecuritiesInfoDownloader> downloader = new Mock<ISecuritiesInfoDownloader>();
            ContainerBuilder.Container.RegisterInstance(downloader.Object);

            Security testSecurity = new Security();
            downloader.Setup(x => x.Download()).Returns("");
            parser.Setup(s => s.Parse(It.IsAny<string>())).Returns(new List<Security>() {testSecurity});

            FinamSecuritiesInfoUpdater finamSecuritiesInfoUpdater = new FinamSecuritiesInfoUpdater();
            finamSecuritiesInfoUpdater.Update();

            Assert.IsTrue(infoHolder.Securities.Count == 1);
            Assert.IsTrue(testSecurity.Equals(infoHolder.Securities[0]));
        }
    }
}
