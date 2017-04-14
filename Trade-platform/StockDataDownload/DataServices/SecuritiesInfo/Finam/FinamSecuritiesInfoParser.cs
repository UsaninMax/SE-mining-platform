using System;
using System.Collections.Generic;
using Castle.Core.Internal;
using TradePlatform.Commons.Securities;

namespace TradePlatform.StockDataDownload.DataServices.SecuritiesInfo.Finam
{
    public class FinamSecuritiesInfoParser : ISecuritiesInfoParser
    {
        private string[] _ids;
        private string[] _names;
        private string[] _codes;
        private string[] _markets;

        public IList<Security> Parse(string message)
        {
            if (message.IsNullOrEmpty())
            {
                throw new Exception("Securities info parsing has mistakes!");
            }

            try
            {
                string[] sets = message.Split('=');
                _ids = CustomSplit(sets[1], new[] {"','", ","});
                _names = CustomSplit(sets[2], new[] {"','"});
                _codes = CustomSplit(sets[3], new[] {"','"});
                _markets = CustomSplit(sets[4], new[] {","});
            }
            catch (Exception)
            {
                throw new Exception("Securities info parsing has mistakes!");
            }

            if (!ParseChecker())
            {
                throw new Exception("Securities info parsing has mistakes!");
            }

            return BuildSecutities();
        }

        private string[] CustomSplit(string input, string[] splitters)
        {
            string trimmed = input.Trim();
            string replaced = trimmed
                .Replace("{", "")
                .Replace("[", "")
                .Replace("['", "")
                .Replace("'];\r\nvar aEmitentNames", "")
                .Replace("];\r\nvar aEmitentDecp", "")
                .Replace("};\r\nvar aDataFormatStrs", "")
                .Replace("];\r\nvar aEmitentUrls", "")
                .Replace("'];\r\nvar aEmitentMarkets", "")
                .Replace("};", "");

            return replaced.Split(splitters, StringSplitOptions.None); ;
        }

        private IList<Security> BuildSecutities()
        {
            IList<Security> securities = new List<Security>(_ids.Length);
            for (int i = 0; i < _ids.Length; i++)
            {
                securities.Add(new Security()
                {
                    Id = _ids[i],
                    Name = _names[i],
                    Code = _codes[i],
                    Market = new Market() { Name = FinamMarketHelper.Markets[_markets[i]], Id = _markets[i] }
                });
            }
            return securities;
        }
        private bool ParseChecker()
        {
            int baseLength = _ids.Length;
            return
                _names.Length == baseLength &&
                _codes.Length == baseLength &&
                _markets.Length == baseLength;
        }
    }
}
