using System.IO;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Trades.Finam
{
    class FinamInstrumentDownloadService : IInstrumentDownloadService
    {
        private readonly IInstrumentSplitter _instrumentSplitter;

        public FinamInstrumentDownloadService()
        {
            this._instrumentSplitter = ContainerBuilder.Container.Resolve<IInstrumentSplitter>();
        }
        // Finam can return data only synchronously
        public void Execute(Instrument instrument)
        {
            CreateLocalFolder(instrument.Path);
            foreach (var splitInstrument in _instrumentSplitter.Split(instrument))
            {
                ITradesDownloader downloader = ContainerBuilder.Container.Resolve<ITradesDownloader>();
                downloader.Download(splitInstrument);
            }
        }

        private void CreateLocalFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                foreach (string filePath in Directory.GetFiles(path))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
