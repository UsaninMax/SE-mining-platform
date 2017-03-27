using System.Collections.Generic;
using TradePlatform.Commons.Securities;

namespace TradePlatform.Common.Securities
{
    class SecuritiesInfo
    {
        private IList<ISecurity> _securities;
        public IList<ISecurity> Securities
        {
            get { return _securities; }
            set { _securities = value; }

        }
    }
}
