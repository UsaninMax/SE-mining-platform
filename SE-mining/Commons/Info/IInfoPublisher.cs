using System;
using SE_mining_base.Info.Message;

namespace SEMining.Commons.Info
{
    public interface IInfoPublisher
    {
        void PublishException(AggregateException exceptions);
        void PublishException(System.Exception exception);
        void PublishInfo(InfoItem infoItem);
    }
}
