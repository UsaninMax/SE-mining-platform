using System.Threading;
using System.Threading.Tasks;
using TradePlatform.StockDataDownload.model;
using TradePlatform.StockDataDownload.Services;
using Microsoft.Practices.Unity;
using TradePlatform.StockDataDownload.Models;

using System.Linq;
using System.Collections.Generic;
using TradePlatform.StockDataDownload.DataServices.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Finam
{
    class FinamInstrumentDownloadService : IInstrumentDownloadService
    {
        private IInstrumentSplitter _instrumentSplitter;

        public FinamInstrumentDownloadService()
        {
            this._instrumentSplitter = ContainerBuilder.Container.Resolve<IInstrumentSplitter>();
        }

        public bool Execute(Instrument instrument)
        {
            IList<Instrument> splitted = _instrumentSplitter.Split(instrument);
            IList<Task> tasks = new List<Task>();
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            foreach (var splitInstrument in splitted)
            {
                tasks.Add(Task.Factory.StartNew(fn =>
                {
                    ITradesDownloader downloader = ContainerBuilder.Container.Resolve<ITradesDownloader>();
                    ITradesParser parser = ContainerBuilder.Container.Resolve<ITradesParser>();
                    return parser.Parse(downloader.Download(splitInstrument));
                }, token));
            }

            Task.Factory.ContinueWhenAll(tasks.ToArray(), contTasks =>
            {
                IList<Trade> allTrades = AggregateResult(contTasks);
            }, token).Wait();

            return true;
        }

        private IList<Trade> AggregateResult(Task[] tasks)
        {
            IEnumerable<Trade> allTrades = new List<Trade>();
            foreach (Task<IList<Trade>> task in tasks)
            {
                allTrades = allTrades.Concat(task.Result);
            }
            return allTrades.OrderBy(trade => trade.Date).ToList();
        }
    }
}
