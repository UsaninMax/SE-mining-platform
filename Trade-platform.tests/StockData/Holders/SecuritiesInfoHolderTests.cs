using NUnit.Framework;
using System.Collections.Generic;
using TradePlatform.StockData.Holders;
using TradePlatform.StockData.Models;

namespace Trade_platform.tests.StockData.Holders
{
    [TestFixture]
    public class SecuritiesInfoHolderTests
    { 

        [Test]
        public void GetMarketsFromSecurities()
        {
            Market markert = new Market { Name = "321", Id = "ddd2" };
            IList<Security> securities = new List<Security> {
                new Security { Market = markert, Id = "dsfsdf" },
                new Security { Market = markert, Id = "wgretr" }
            };

            SecuritiesInfoHolder holder = new SecuritiesInfoHolder { Securities = securities };
            IList<Market> markets = new List<Market> (holder.Markets());

            Assert.That(markets.Count, Is.EqualTo(1));
            Assert.That(markets[0], Is.EqualTo(markert));
        }

        [Test]
        public void GeSecuritiesFromSecuritiesFilterByMarket()
        {
            Market markert = new Market { Name = "321", Id = "ddd2" };
            IList<Security> securities = new List<Security> {
                new Security { Market = markert, Id = "dsfsdf" },
                new Security { Market = markert, Id = "wgretr" }
            };

            SecuritiesInfoHolder holder = new SecuritiesInfoHolder { Securities = securities };
            IList<Security> securitiesBy = new List<Security>(holder.SecuritiesBy(markert));

            Assert.That(securitiesBy.Count, Is.EqualTo(2));
            Assert.That(securitiesBy, Is.EqualTo(securities));
        }

        [Test]
        public void GeEmptySecuritiesFromSecuritiesFilterByMarket()
        {
            Market markert = new Market { Name = "321", Id = "ddd2" };
            SecuritiesInfoHolder holder = new SecuritiesInfoHolder { Securities = new List<Security>() };
            IList<Security> securitiesBy = new List<Security>(holder.SecuritiesBy(markert));

            Assert.That(securitiesBy.Count, Is.EqualTo(0));
        }
    }
}
