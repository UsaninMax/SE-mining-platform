using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Trades.Finam
{
    class FinamInstrumentDownloadService : IInstrumentDownloadService
    {
        private readonly IInstrumentSplitter _instrumentSplitter;

        public FinamInstrumentDownloadService()
        {
            _instrumentSplitter = ContainerBuilder.Container.Resolve<IInstrumentSplitter>();
        }
        // Finam can return data only synchronously
        public void Download(Instrument instrument, CancellationToken cancellationToken)
        {
            DeleteFolder(instrument);
            CreateFolder(instrument);

            _instrumentSplitter.Split(instrument).ForEach(i =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                var downloader = ContainerBuilder.Container.Resolve<ITradesDownloader>();
                downloader.Download(i);
            });
        }

        public void Delete(Instrument instrument, Task download, CancellationTokenSource cancellationTokenSource)
        {
            if (download != null && !download.IsCompleted)
            {
                cancellationTokenSource.Cancel();
                download.Wait();
            }
            DeleteFolder(instrument);
        }

        public bool CheckFiles(Instrument instrument)
        {
            return Directory.Exists(instrument.Path) &&
                _instrumentSplitter.Split(instrument).All(splitedInstrument => File.Exists(instrument.DataProvider + "\\" +instrument.DataProvider + "\\" + splitedInstrument.Path + "\\" + splitedInstrument.FileName + ".txt"));
        }

        private static void DeleteFolder(Instrument instrument)
        {
            string path = instrument.DataProvider + "\\" + instrument.Path;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        private static void CreateFolder(Instrument instrument)
        {
            string path = instrument.DataProvider + "\\" + instrument.Path;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
