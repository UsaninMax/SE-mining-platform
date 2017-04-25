using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Trades;
using TradePlatform.StockDataDownload.DataServices.Serialization;
using TradePlatform.StockDataDownload.DataServices.Trades;

namespace TradePlatform.Commons.Holders
{
    public class DownloadedInstrumentsHolder : IDownloadedInstrumentsHolder
    {
        private readonly IInstrumentDownloadService _downloadService;
        private readonly HashSet<Instrument> _instrumnnets = new HashSet<Instrument>();
        public DownloadedInstrumentsHolder()
        {
            _downloadService = ContainerBuilder.Container.Resolve<IInstrumentDownloadService>();
        }

        public void Put(Instrument instrument)
        {
            _instrumnnets.Add(instrument);
        }

        public void Remove(Instrument instrument)
        {
            _instrumnnets.Remove(instrument);
        }

        public HashSet<Instrument> GetAll()
        {
            return _instrumnnets;
        }

        public void RestoreFromSettings()
        {
            try
            {
                var serializer = ContainerBuilder.Container.Resolve<IInstrumentsStorage>();
                serializer
                    .ReStore()
                    .Where(i => _downloadService.CheckFiles(i))
                    .Select(i => _instrumnnets.Add(i))
                    .ToArray();
            }
            catch (Exception e)
            {
                //TODO: log
            }
        }
    }
}
