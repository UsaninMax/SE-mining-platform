using System.Collections.Generic;

namespace TradePlatform.Commons.Info
{
    public interface IInfoTab
    {
        string TabID();
        void Add(InfoItem item);
        int MessageCount();
    }
}
