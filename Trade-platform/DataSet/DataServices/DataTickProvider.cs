using System.Collections.Generic;
using TradePlatform.DataSet.Models;
using Microsoft.Practices.Unity;
using System.Linq;
using TradePlatform.StockData.DataServices.Trades;
using System.Threading;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.DataServices
{
    public class DataTickProvider : IDataTickProvider
    {
        private readonly IDataTickParser _parser;

        public DataTickProvider()
        {
            _parser = ContainerBuilder.Container.Resolve<IDataTickParser>();
        }

        public IList<DataTick> Get(DataSetItem item, CancellationToken cancellationToken)
        {
            List<DataTick> fullDataSet = new List<DataTick>();
            foreach (SubInstrument subInstrument in item.SubInstruments)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new List<DataTick>(fullDataSet);
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

                fullDataSet = new List<DataTick>(fullDataSet.Concat(subSet));
            }
            fullDataSet.Sort((obj1, obj2) => obj1.Date.CompareTo(obj2.Date));
            return fullDataSet;
        }
    }
}
