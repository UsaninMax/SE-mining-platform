using System;
using Microsoft.Practices.Unity;
using Prism.Events;
using TradePlatform.Commons.Info.Events;
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

        public void PublishException(AggregateException exceptions)
        {
            foreach (var ex in exceptions.Flatten().InnerExceptions)
            {
                _eventAggregator.GetEvent<PuplishExceptionInfo>()
                    .Publish(new ExceptionInfo { Message = ex.GetType().Name + ", - " + ex.Message });
            }
        }

        public void PublishException(System.Exception exception)
        {
            _eventAggregator.GetEvent<PuplishExceptionInfo>()
                .Publish(new ExceptionInfo { Message = exception.GetType().Name + ", - " + exception.Message });

        }

        public void PublishInfo(InfoItem infoItem)
        {
            _eventAggregator.GetEvent<PuplishInfo>().Publish(infoItem);
        }
    }
}