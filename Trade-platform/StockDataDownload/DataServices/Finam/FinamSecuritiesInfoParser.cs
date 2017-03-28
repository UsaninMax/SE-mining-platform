using System.Collections.Generic;
using TradePlatform.Commons.Securities;
using System;

namespace TradePlatform.StockDataDownload.DataParsers
{
    class FinamSecuritiesInfoParser : ISecuritiesInfoParser
    {
        private string[] _ids;
        private string[] _names;
        private string[] _codes;
        private string[] _markets;
        private string[] _decp;
        private string[] _emitentChild;
        private string[] _emitentUrls;

        public IList<ISecurity> Parse(string message)
        {
            string[] sets = message.Split('=');
            _ids = CustomSplit(sets[1], new string[] {"','",","});
            _names = CustomSplit(sets[2], new string[] {"','"});
            _codes = CustomSplit(sets[3], new string[] {"','"});
            _markets = CustomSplit(sets[4], new string[] {","});
            _decp = CustomSplit(sets[5], new string[] { "," });
            _emitentChild = CustomSplit(sets[7], new string[] { "," });
            _emitentUrls = CustomSplit(sets[8], new string[] { "," });

            if(!ParseChecker())
            {
                throw new Exception("Securities info parsing has mistakes!");
            }

            return new List<ISecurity>();
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

        private bool ParseChecker()
        {
            int baseLength = _ids.Length;
            return
                _names.Length == baseLength &&
                _codes.Length == baseLength &&
                _markets.Length == baseLength &&
                _decp.Length == baseLength &&
                _emitentChild.Length == baseLength &&
                _emitentUrls.Length == baseLength; 
        }
    }
}
