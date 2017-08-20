using System;
using NUnit.Framework;
using SEMining.StockData.DataServices.SecuritiesInfo.Finam;

namespace SEMining.tests.StockData.DataServices.SecuritiesInfo.Finam
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
