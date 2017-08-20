using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using SEMining.StockData.DataServices.Serialization;
using SEMining.StockData.Models;
using SEMining.Commons.Loggers;

namespace SEMining.StockData.Holders
{
    public class DownloadedInstrumentsHolder : IDownloadedInstrumentsHolder
    {
        private readonly HashSet<Instrument> _instrumnnets = new HashSet<Instrument>();
        private readonly IInstrumentsStorage _instrumentsStorage;

        public DownloadedInstrumentsHolder()
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
                    .ForEach(i => _instrumnnets.Add(i));
            }
            catch (Exception e)
            {
                SystemLogger.Log.Error(e);
            }
        }

        public void Store()
        {
            _instrumentsStorage.Store(_instrumnnets);
        }
    }
}
