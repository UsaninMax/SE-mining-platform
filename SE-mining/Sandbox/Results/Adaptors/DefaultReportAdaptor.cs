using Microsoft.Practices.ObjectBuilder2;
using System.Collections.Generic;
using System.Globalization;
using SE_mining_base.Transactios.Models;

namespace SEMining.Sandbox.Results.Adaptors
{
    public class DefaultReportAdaptor : IReportAdaptor
    {
        public IEnumerable<Dictionary<string, string>> Adopt(IEnumerable<Transaction> rows)
        {
            IList<Dictionary<string, string>> report = new List<Dictionary<string, string>>();
            rows.ForEach(row =>
            {
                report.Add(new Dictionary<string, string>
                {
                    {"Date" , row.Date.ToString(CultureInfo.InvariantCulture)},
                    {"InstrumentId" , row.InstrumentId},
                    {"Direction" , row.Direction.ToString()},
                    {"Number" , row.Number.ToString()},
                    {"ExecutedPrice" , row.ExecutedPrice.ToString(CultureInfo.InvariantCulture)}
                });
            });
            return report;
        }

        public IEnumerable<Dictionary<string, string>> Adopt(IEnumerable<BalanceRow> rows)
        {
            IList<Dictionary<string, string>> report = new List<Dictionary<string, string>>();
            rows.ForEach(row =>
            {
                report.Add(new Dictionary<string, string>
                {
                    {"Date" , row.Date.ToString(CultureInfo.InvariantCulture)},
                    {"TransactionCost" , row.TransactionCost.ToString(CultureInfo.InvariantCulture)},
                    {"TransactionMargin" , row.TransactionMargin.ToString(CultureInfo.InvariantCulture)},
                    {"Total" , row.Total.ToString(CultureInfo.InvariantCulture)},
                    {"RequestId" , row.RequestId.ToString()}
                });
            });
            return report;
        }

        public IEnumerable<Dictionary<string, string>> Adopt(IEnumerable<OpenPositionRequest> rows)
        {
            IList<Dictionary<string, string>> report = new List<Dictionary<string, string>>();
            rows.ForEach(request =>
                {
                    request.GetTransactions().ForEach(transaction =>
                    {
                        Dictionary<string, string> reportRequest = new Dictionary<string, string>
                        {
                            {"Date" , request.Date.ToString(CultureInfo.InvariantCulture)},
                            {"InstrumentId" , request.InstrumentId.ToString()},
                            {"Number" , request.Number.ToString()},
                            {"RemainingNumber" , request.RemainingNumber.ToString()},
                            {"TransactionDate" , transaction.Date.ToString(CultureInfo.InvariantCulture)},
                            {"TransactionInstrumentId" , transaction.InstrumentId.ToString()},
                            {"TransactionDirection" , transaction.Direction.ToString()},
                            {"TransactionNumber" , transaction.Number.ToString()},
                            {"TransactionExecutedPrice" , transaction.ExecutedPrice.ToString(CultureInfo.InvariantCulture)},
                            {"Id" , request.Id.ToString()}
                        };
                        report.Add(reportRequest);
                    });
                });
            return report;
        }
    }
}
