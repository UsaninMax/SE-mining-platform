using System;
using Microsoft.Practices.Unity;
using Prism.Events;
using SEMining.Commons.Info.Events;
using SE_mining_base.Info.Message;

namespace SEMining.Commons.Info
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