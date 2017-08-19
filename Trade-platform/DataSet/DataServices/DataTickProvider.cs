using System;
using System.Collections.Generic;
using TradePlatform.DataSet.Models;
using Microsoft.Practices.Unity;
using System.Linq;
using TradePlatform.StockData.DataServices.Trades;
using System.Threading;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.DataServices
{
    public class DataTickProvider : IDataTickProvider
    {
        private readonly IDataTickParser _parser;
        private readonly IInfoPublisher _infoPublisher;

        public DataTickProvider()
        {
            _parser = ContainerBuilder.Container.Resolve<IDataTickParser>();
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
        }

        public IEnumerable<DataTick> Get(DataSetItem item, CancellationToken cancellationToken)
        {
            IEnumerable<DataTick> fullData = new List<DataTick>();
            foreach (SubInstrument subInstrument in item.SubInstruments)
            {
                _infoPublisher.PublishInfo(new DataSetInfo { Message = subInstrument + "- start reading ticks" });

                if (cancellationToken.IsCancellationRequested)
                {
                    return fullData;
                }
                IList<DataTick> subSet = new List<DataTick>();

                _parser.Parse(subInstrument)
                    .Where(m => m.Date >= subInstrument.SelectedFrom && m.Date <= subInstrument.SelectedTo)
                    .ToList().GroupBy(p => p.Date, p => p, (key, g) => new { Date = key, Data = g.ToList() })
                    .ForEach(x =>
                    {
                        subSet.Add(new DataTick
                        {
                            Date = x.Date,
                            Price = x.Data.Last().Price,
                            Volume = x.Data.Sum(y => y.Volume)
                        });
                    });
                fullData = fullData.Concat(subSet);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            List<DataTick> asList = fullData.ToList();
            asList.Sort((obj1, obj2) => obj1.Date.CompareTo(obj2.Date));
            return asList;
        }
    }
}
