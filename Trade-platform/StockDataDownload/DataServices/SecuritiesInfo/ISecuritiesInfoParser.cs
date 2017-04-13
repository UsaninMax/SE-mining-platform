using System.Collections.Generic;
using TradePlatform.Commons.Securities;

namespace TradePlatform.StockDataDownload.DataServices.SecuritiesInfo
{
    public interface ISecuritiesInfoParser
    {
        IList<Security> Parse(string message);
    }
}
