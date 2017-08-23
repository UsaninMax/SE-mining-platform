using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Prism.Events;
using System.Linq;
using System.Windows;
using SEMining.Commons.Info.Views;
using System;
using SEMining.Commons.Info.Events;
using SE_mining_base.Info.Message;

namespace SEMining.Commons.Info.Exception
{
    public class ExceptionActualizer
    {
        private readonly Dispatcher _dispatcher;
        private readonly IEventAggregator _eventAggregator;

        public ExceptionActualizer()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<PuplishExceptionInfo>().Subscribe(PublishException, false);
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
                _eventAggregator.GetEvent<PuplishInfo>().Publish(item);
            })); 
        }
    }
}
