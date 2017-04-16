using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Trades;
using TradePlatform.Commons.Sistem;

namespace TradePlatform.StockDataDownload.DataServices.Trades.Finam
{
    public class FinamInstrumentDownloadService : IInstrumentDownloadService
    {
        private readonly IInstrumentSplitter _instrumentSplitter;
        private readonly IFileManager _fileManager;

        public FinamInstrumentDownloadService()
        {
            _instrumentSplitter = ContainerBuilder.Container.Resolve<IInstrumentSplitter>();
            _fileManager = ContainerBuilder.Container.Resolve<IFileManager>();
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

        public void SoftDownload(Instrument instrument, CancellationToken cancellationToken)
        {
            CreateFolder(instrument);

            _instrumentSplitter.Split(instrument).ForEach(i =>
            {
                if (cancellationToken.IsCancellationRequested || FileExist(i))
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
            return _fileManager.IsDirectoryExist(instrument.DataProvider + "\\" + instrument.Path) &&
                _instrumentSplitter.Split(instrument).All(FileExist);
        }

        private bool FileExist(Instrument instrument)
        {
            return _fileManager.IsFileExist(instrument.DataProvider + "\\" + instrument.Path + "\\" + instrument.FileName + ".txt");
        }

        private void DeleteFolder(Instrument instrument)
        {
            _fileManager.DeleteFolder(instrument.DataProvider + "\\" + instrument.Path);
        }

        private void CreateFolder(Instrument instrument)
        {
            _fileManager.CreateFolder(instrument.DataProvider + "\\" + instrument.Path);
        }
    }
}
