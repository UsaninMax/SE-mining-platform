using System;
using TradePlatform.StockDataDownload.DataServices.SecuritiesInfo.Finam;
using NUnit.Framework;

namespace Trade_platform.tests.StockDataDownload.DataServices.SecuritiesInfo.Finam
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
