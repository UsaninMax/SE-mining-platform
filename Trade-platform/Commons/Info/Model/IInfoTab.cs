using TradePlatform.Commons.Info.Model.Message;

namespace TradePlatform.Commons.Info.Model
{
    public interface IInfoTab
    {
        string TabId();
        void Add(InfoItem item);
    }
}
