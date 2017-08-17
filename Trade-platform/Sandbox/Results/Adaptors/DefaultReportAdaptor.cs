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
                    {"ExecutedPrice" , row.ExecutedPrice.ToString()}
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

        public IEnumerable<Dictionary<string, string>> Adopt(IList<OpenPositionRequest> rows)
        {
            IList<Dictionary<string, string>> report = new List<Dictionary<string, string>>();
            rows.ForEach(request =>
                {
                    request.GetTransactions().ForEach(transaction =>
                    {
                        Dictionary<string, string> reportRequest = new Dictionary<string, string>
                        {
                            {"Date" , request.Date.ToString()},
                            {"InstrumentId" , request.InstrumentId.ToString()},
                            {"Number" , request.Number.ToString()},
                            {"RemainingNumber" , request.RemainingNumber.ToString()},
                            {"TransactionDate" , transaction.Date.ToString()},
                            {"TransactionInstrumentId" , transaction.InstrumentId.ToString()},
                            {"TransactionDirection" , transaction.Direction.ToString()},
                            {"TransactionNumber" , transaction.Number.ToString()},
                            {"TransactionExecutedPrice" , transaction.ExecutedPrice.ToString()}
                        };
                        report.Add(reportRequest);
                    });
                });
            return report;
        }
    }
}
