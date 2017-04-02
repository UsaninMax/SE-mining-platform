using System.Threading;
using System.Threading.Tasks;
using TradePlatform.StockDataDownload.model;
using TradePlatform.StockDataDownload.Services;
using Microsoft.Practices.Unity;
using TradePlatform.StockDataDownload.Models;

using System.Linq;
using System.Collections.Generic;

namespace TradePlatform.StockDataDownload.DataServices.Finam
{
    class FinamInstrumentDownloadManager : IInstrumentDownloadManager
    {
        private IInstrumentSplitter _instrumentSplitter;

        public FinamInstrumentDownloadManager(IInstrumentSplitter instrumentSplitter)
        {
            this._instrumentSplitter = instrumentSplitter;
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
                    ITradesDownloader tradesDownloader = ContainerBuilder.Container.Resolve<ITradesDownloader>();
                    return tradesDownloader.Download(splitInstrument);
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
