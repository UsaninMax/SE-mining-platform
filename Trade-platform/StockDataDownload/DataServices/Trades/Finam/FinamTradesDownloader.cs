using System.Net;
using System.Text;
using System.Threading.Tasks;
using TradePlatform.StockDataDownload.model;

namespace TradePlatform.StockDataDownload.Services
{
    class FinamTradesDownloader : ITradesDownloader
    {
        private static string DATE_FRORMAT = "ddMMyy";
        private string _fileName;

        public void Download(Instrument instrument)
        {
            _fileName = new StringBuilder()
                .Append(instrument.Name)
                .Append("_")
                .Append(instrument.From.ToString(DATE_FRORMAT))
                .Append("_")
                .Append(instrument.To.ToString(DATE_FRORMAT))
                .ToString();

            new WebClient().DownloadFile(Url(instrument), instrument.Path +"\\"+ _fileName + ".txt");
        }

        private string Url(Instrument instrument)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder
                .Append("http://")
                .Append("78.41.196.47")
                .Append("/")
                .Append(_fileName)
                .Append(".txt?")
                .Append("market=").Append(instrument.MarketId).Append("&")
                .Append("em=").Append(instrument.Id).Append("&")
                .Append("code=").Append(instrument.Code).Append("&")
                .Append("df=").Append(instrument.From.Day).Append("&")
                .Append("mf=").Append(instrument.From.Month - 1).Append("&")
                .Append("yf=").Append(instrument.From.Year).Append("&")
                .Append("from=").Append(instrument.From.ToShortDateString()).Append("&")
                .Append("dt=").Append(instrument.To.Day).Append("&")
                .Append("mt=").Append(instrument.To.Month - 1).Append("&")
                .Append("yt=").Append(instrument.To.Year).Append("&")
                .Append("to=").Append(instrument.To.ToShortDateString()).Append("&")
                .Append("p=").Append(1).Append("&")
                .Append("f=")
                .Append(_fileName)
                .Append("&")
                .Append("e=").Append(".txt").Append("&")
                .Append("cn=").Append(instrument.Name).Append("&")
                .Append("dtf=").Append(1).Append("&")
                .Append("tmf=").Append(1).Append("&")
                .Append("MSOR=").Append(1).Append("&")
                .Append("mstime=").Append("on").Append("&")
                .Append("mstimever=").Append(1).Append("&")
                .Append("sep=").Append(1).Append("&")
                .Append("sep2=").Append(1).Append("&")
                .Append("datf=").Append(9).Append("&")
                .Append("at=").Append(0);

            return urlBuilder.ToString();
        }
    }
}
