using Microsoft.Practices.ObjectBuilder2;
using System.Collections.Generic;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Results.Adaptors
{
    public class DefaultReportAdaptor : IReportAdaptor
    {
        public IEnumerable<Dictionary<string, string>> Adopt(IList<Transaction> rows)
        {
            IList<Dictionary<string, string>> report = new List<Dictionary<string, string>>();
            rows.ForEach(row =>
            {
                report.Add(new Dictionary<string, string>
                {
                    {"Date" , row.Date.ToString()},
                    {"InstrumentId" , row.InstrumentId},
                    {"Direction" , row.Direction.ToString()},
                    {"Number" , row.Number.ToString()},
                    {"ExecutedPrice" , row.ExecutedPrice.ToString()},
                    {"RemainingNumber" , row.RemainingNumber.ToString()}
                });
            });
            return report;
        }

        public IEnumerable<Dictionary<string, string>> Adopt(IList<BalanceRow> rows)
        {
            IList<Dictionary<string, string>> report = new List<Dictionary<string, string>>();
            rows.ForEach(row =>
            {
                report.Add(new Dictionary<string, string>
                {
                    {"Date" , row.Date.ToString()},
                    {"TransactionCost" , row.TransactionCost.ToString()},
                    {"TransactionMargin" , row.TransactionMargin.ToString()},
                    {"Total" , row.Total.ToString()}
                });
            });
            return report;
        }
    }
}
