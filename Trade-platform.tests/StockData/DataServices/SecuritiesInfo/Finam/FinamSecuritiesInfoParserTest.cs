using System;
using NUnit.Framework;
using TradePlatform.StockData.DataServices.SecuritiesInfo.Finam;

namespace Trade_platform.tests.StockData.DataServices.SecuritiesInfo.Finam
{
    [TestFixture]
    public class FinamSecuritiesInfoParserTest
    {
        [Test]
        public void WhenDownloaderWillReturnNullParserWillThrowException()
        {
            Assert.Throws<Exception>(() =>
            {
                FinamSecuritiesInfoParser parser = new FinamSecuritiesInfoParser();
                parser.Parse(null);
            });
        }

        [Test]
        public void WhenDownloaderWillReturnEmptyStringParserWillThrowException()
        {
            Assert.Throws<Exception>(() =>
            {
                FinamSecuritiesInfoParser parser = new FinamSecuritiesInfoParser();
                parser.Parse("");
            });
        }

        [Test]
        public void WhenDownloaderWillReturnNotAllowedStringParserWillThrowException()
        {
            Assert.Throws<Exception>(() =>
            {
                FinamSecuritiesInfoParser parser = new FinamSecuritiesInfoParser();
                parser.Parse("wffwefwefwe");
            });
        }
    }
}
