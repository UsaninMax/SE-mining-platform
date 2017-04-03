using TradePlatform.StockDataDownload.model;
using TradePlatform.StockDataDownload.Services;
using Microsoft.Practices.Unity;
using System.IO;

namespace TradePlatform.StockDataDownload.DataServices.Finam
{
    class FinamInstrumentDownloadService : IInstrumentDownloadService
    {
        private IInstrumentSplitter _instrumentSplitter;

        public FinamInstrumentDownloadService()
        {
            this._instrumentSplitter = ContainerBuilder.Container.Resolve<IInstrumentSplitter>();
        }
        // Finam can return data only synchronously
        public bool Execute(Instrument instrument)
        {
            CreateLocalFolder(instrument.Path);

            foreach (var splitInstrument in _instrumentSplitter.Split(instrument))
            {
                ITradesDownloader downloader = ContainerBuilder.Container.Resolve<ITradesDownloader>();
                downloader.Download(splitInstrument);
            }

            return true;
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
