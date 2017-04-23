using System;
using TradePlatform.Commons.Info.Model.Message;

namespace TradePlatform.Commons.Info
{
    public interface IInfoPublisher
    {
        void PublishException(AggregateException exceptions);
        void PublishException(System.Exception exception);
        void PublishInfo(InfoItem infoItem);
    }
}
