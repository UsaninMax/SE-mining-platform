using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Prism.Events;
using TradePlatform.Commons.Info.Model.Message;
using System.Linq;
using System.Windows;
using TradePlatform.Commons.Info.Views;
using System;
using TradePlatform.Commons.Info.Events;
using TradePlatform.Commons.Info.MessageEvents;

namespace TradePlatform.Commons.Info.Exception
{
    public class ExceptionActualizer
    {
        private readonly Dispatcher _dispatcher;
        private readonly IEventAggregator _eventAggregator;

        public ExceptionActualizer()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<PuplishExceptionInfo<ExceptionInfo>>().Subscribe(PublishException, false);
        }

        private void PublishException(object param)
        {
            var item = param as ExceptionInfo;
            if (item == null)
            {
                return;
            }

            _dispatcher.BeginInvoke((Action)(() =>
            {
                var window = Application.Current.Windows.OfType<InfoView>().SingleOrDefault(x => x.IsInitialized);
                if (window == null)
                {
                    ContainerBuilder.Container.Resolve<InfoView>().Show();
                }
                _eventAggregator.GetEvent<PuplishInfo<InfoItem>>().Publish(item);
            })); 
        }
    }
}
