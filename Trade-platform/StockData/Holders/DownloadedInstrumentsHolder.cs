using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using TradePlatform.StockData.DataServices.Serialization;
using TradePlatform.StockData.Models;

namespace TradePlatform.StockData.Holders
{
    public class DownloadedInstrumentsHolder : IDownloadedInstrumentsHolder
    {
        private readonly HashSet<Instrument> _instrumnnets = new HashSet<Instrument>();
        private readonly IInstrumentsStorage _instrumentsStorage;

        public DownloadedInstrumentsHolder ()
        {
            _instrumentsStorage = ContainerBuilder.Container.Resolve<IInstrumentsStorage>();
        }

        public void Put(Instrument instrument)
        {
            _instrumnnets.Add(instrument);
        }

        public void Remove(Instrument instrument)
        {
            _instrumnnets.Remove(instrument);
        }

        public ISet<Instrument> GetAll()
        {
            return _instrumnnets;
        }

        public void Restore()
        {
            try
            {
                    _instrumentsStorage
                    .ReStore()
                    .Select(i => _instrumnnets.Add(i))
                    .ToArray();
            }
            catch (Exception e)
            {
                //TODO: log
            }
        }

        public void Store()
        {
            try
            {
                _instrumentsStorage.Store(_instrumnnets);
               
            }
            catch (Exception e)
            {
                //TODO: log
            }
        }
    }
}
