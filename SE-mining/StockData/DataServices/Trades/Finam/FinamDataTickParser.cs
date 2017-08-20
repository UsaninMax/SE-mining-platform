using System;
using System.Collections.Generic;
using SEMining.StockData.Models;
using Microsoft.Practices.Unity;
using System.IO;
using System.Globalization;

namespace SEMining.StockData.DataServices.Trades.Finam
{
    public class FinamDataTickParser : IDataTickParser
    {
        private readonly IInstrumentSplitter _splitter;

        public FinamDataTickParser ()
        {
            _splitter = ContainerBuilder.Container.Resolve<IInstrumentSplitter>();
        }

        public IEnumerable<DataTick> Parse(Instrument instrument)
        {

            List<DataTick> allDataTick = new List<DataTick>();

            foreach (Instrument splited in _splitter.Split(instrument))
            {
                string line;
                StreamReader file = new StreamReader(splited.DataProvider + "\\" + splited.Path + "\\" + splited.FileName + ".txt");
                while ((line = file.ReadLine()) != null)
                {
                    string[] splitedRow = line.Split(',');
                    allDataTick.Add(new DataTick
                    {
                        Date = DateTime.ParseExact(splitedRow[0] + " " + splitedRow[1], "yyyyMMdd HHmmss", CultureInfo.InvariantCulture),
                        Price = double.Parse(splitedRow[2], CultureInfo.InvariantCulture),
                        Volume = Int32.Parse(splitedRow[3])
                    });
                }

                file.Close();
            }
            return allDataTick;
        }
    }
}
