using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradePlatform.StockDataDownload.DataServices.SecuritiesInfo.Finam;

namespace Trade_platform.tests.StockDataDownload.DataServices.SecuritiesInfo.Finam
{
    [TestClass]
    public class FinamSecuritiesInfoParserTest
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void WhenDownloaderWillReturnNullParserWillThrowException()
        {
            FinamSecuritiesInfoParser parser = new FinamSecuritiesInfoParser();
            parser.Parse(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void WhenDownloaderWillReturnEmptyStringParserWillThrowException()
        {
            FinamSecuritiesInfoParser parser = new FinamSecuritiesInfoParser();
            parser.Parse("");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void WhenDownloaderWillReturnNotAllowedStringParserWillThrowException()
        {
            FinamSecuritiesInfoParser parser = new FinamSecuritiesInfoParser();
            parser.Parse("wffwefwefwe");
        }
    }
}
