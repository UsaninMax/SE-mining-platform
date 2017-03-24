using System;
using System.Threading;
using TradePlatform.StockDataDownload.model;

namespace TradePlatform.StockDataDownload.Services
{
    class DownloadFinamInstrument : IDownloadInstrument
    {
        public bool download(Instrument instrument, CancellationToken ct)
        {

            while (true)
            {
                // do some heavy work here
                Thread.Sleep(100);
                if (ct.IsCancellationRequested)
                {
                    // another thread decided to cancel
                    Console.WriteLine("task canceled");
                    break;
                }
            }
            return true;
        }
    }
}
