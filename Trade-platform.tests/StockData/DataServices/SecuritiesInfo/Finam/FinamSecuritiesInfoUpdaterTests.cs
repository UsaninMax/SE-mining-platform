using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;
using TradePlatform;
using TradePlatform.StockData.DataServices.SecuritiesInfo;
using TradePlatform.StockData.DataServices.SecuritiesInfo.Finam;
using TradePlatform.StockData.Holders;
using TradePlatform.StockData.Models;

namespace Trade_platform.tests.StockData.DataServices.SecuritiesInfo.Finam
{
    [TestFixture]
    public class FinamSecuritiesInfoUpdaterTests
    {
        [Test]
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

            Assert.IsTrue(infoHolder.Securities.Count() == 1);
            Assert.IsTrue(testSecurity.Equals(infoHolder.Securities.First()));
        }
    }
}
