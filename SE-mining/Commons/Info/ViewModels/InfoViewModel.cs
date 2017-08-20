using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using SEMining.Commons.Info.Events;
using SEMining.Commons.Info.Model;
using SEMining.Commons.Info.Model.Message;

namespace SEMining.Commons.Info.ViewModels
{
    public class InfoViewModel : BindableBase, IInfoViewModel
    {
        private readonly Dispatcher _dispatcher;
        private ObservableCollection<IInfoTab> _tabs = new ObservableCollection<IInfoTab>();

        public ObservableCollection<IInfoTab> Tabs
        {
            get { return _tabs; }
            set
            {
                _tabs = value;
                RaisePropertyChanged();
            }
        }

        public InfoViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<PuplishInfo>().Subscribe(PublishInfo, false);
            eventAggregator.GetEvent<CloseTabEvent>().Subscribe(DeleteInfoTab, false);
        }

        private void DeleteInfoTab(IInfoTab infoTab)
        {
            Tabs.Remove(infoTab);
        }

        private void PublishInfo(object param)
        {
            var item = param as InfoItem;
            if (item == null)
            {
                return;
            }

            _dispatcher.BeginInvoke((Action)(() =>
            {
                var tab = Tabs.FirstOrDefault(x => x.TabId().Equals(item.TabId));
                if (tab == null)
                {
                    tab = new InfoTab(item.TabId);
                    Tabs.Add(tab);
                }

                tab.Add(item);
            }));
        }
    }
}
