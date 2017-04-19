using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Prism.Events;
using TradePlatform.Commons.MessageEvents;

namespace TradePlatform.Commons.Info.Exception
{
    public class ExceptionHandler
    {
        private readonly Dispatcher _dispatcher;

        public ExceptionHandler()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<PuplishInfo<InfoItem>>().Subscribe(PublishException, false);
        }

        private void PublishException(object param)
        {
          
        }
    }
}
