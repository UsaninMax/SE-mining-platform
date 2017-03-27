using System.Collections.Generic;
using System.Text.RegularExpressions;
using TradePlatform.Commons.Securities;
using System.Linq;
using System;

namespace TradePlatform.StockDataDownload.DataParsers
{
    class FinamSecuritiesInfoParser : ISecuritiesInfoParser
    {
        public IList<ISecurity> Parse(string message)
        {

            string[] sets = message.Split('=');
            string[] ids = sets[1].Split('[')[1].Split(']')[0].Split(',');
            //string[] names = SplitRegexp(SplitRegexp(SplitRegexp(sets[2], @"\[\'")[2], @"\'\]")[1], @"\'\,\'");
            string[] codes = sets[3].Split('[')[1].Split(']')[0].Split(',');
            string[] markets = sets[4].Split('[')[1].Split(']')[0].Split(',');
            string[] decp = sets[5].Split('{')[1].Split('}')[0].Split(',');
            string[] emitentChild = sets[7].Split('[')[1].Split(']')[0].Split(',');
            string[] emitentUrls = sets[8].Split('{')[1].Split('}')[0].Split(',');

            return new List<ISecurity>();
        }

        private string[] SplitRegexp (string input, string pattern)
        {
            return Regex.Matches(input, pattern).Cast<Match>().Select(m => m.Value).ToArray();
        } 
    }
}
