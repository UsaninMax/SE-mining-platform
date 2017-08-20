using System;
using SEMining.Commons.Info.Model.Message;

namespace SEMining.Commons.Info
{
    public interface IInfoPublisher
    {
        void PublishException(AggregateException exceptions);
        void PublishException(System.Exception exception);
        void PublishInfo(InfoItem infoItem);
    }
}
