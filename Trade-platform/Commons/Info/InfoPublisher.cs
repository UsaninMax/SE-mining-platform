using Microsoft.Practices.Unity;
using Prism.Events;
using TradePlatform.Commons.Info.Events;
using TradePlatform.Commons.Info.MessageEvents;
using TradePlatform.Commons.Info.Model.Message;

namespace TradePlatform.Commons.Info
{
    public class InfoPublisher : IInfoPublisher
    {

        private readonly IEventAggregator _eventAggregator;

        public InfoPublisher()
        {
            _eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
        }

        public void PublishException(string message)
        {
            _eventAggregator.GetEvent<PuplishExceptionInfo<ExceptionInfo>>()
                .Publish(new ExceptionInfo { Message = message });
        }

        public void PublishInfo(InfoItem infoItem)
        {
            _eventAggregator.GetEvent<PuplishInfo<InfoItem>>().Publish(infoItem);
        }
    }
}