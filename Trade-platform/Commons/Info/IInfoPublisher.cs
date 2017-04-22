using TradePlatform.Commons.Info.Model.Message;

namespace TradePlatform.Commons.Info
{
    public interface IInfoPublisher
    {
        void PublishException(string message);
        void PublishInfo(InfoItem infoItem);
    }
}
