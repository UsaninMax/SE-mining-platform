using System;
using System.Collections.Generic;
using TradePlatform.StockData.Models;
using Microsoft.Practices.Unity;
using System.IO;
using System.Globalization;

namespace TradePlatform.StockData.DataServices.Trades.Finam
{
    public class FinamDataTickParser : IDataTickParser
    {
        private readonly IInstrumentSplitter _splitter;

        public FinamDataTickParser ()
        {
            _splitter = ContainerBuilder.Container.Resolve<IInstrumentSplitter>();
        }

        public IList<DataTick> Parse(Instrument instrument)
        {

            List<DataTick> allDataTick = new List<DataTick>();

            foreach (Instrument splited in _splitter.Split(instrument))
            {
                string line;
                StreamReader file = new StreamReader(splited.DataProvider + "\\" + splited.Path + "\\" + splited.FileName + ".txt");
                while ((line = file.ReadLine()) != null)
                {
                    string[] splitedRow = line.Split(',');
                    allDataTick.Add(new DataTick()
                    {
                        Date = DateTime.ParseExact(splitedRow[0] + " " + splitedRow[1], "yyyyMMdd HHmmss", CultureInfo.InvariantCulture),
                        Price = double.Parse(splitedRow[2], CultureInfo.InvariantCulture),
                        Volume = Int32.Parse(splitedRow[3])
                    });
                }

                file.Close();
            }
            allDataTick.Sort((obj1, obj2) => obj1.Date.CompareTo(obj2.Date));
            return allDataTick;
        }
    }
}
