using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.Commons.Info.Events;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.Commons.Utils;

namespace TradePlatform.Commons.Info.Model
{
    public class InfoTab : BindableBase, IInfoTab
    {
        public string Id { get; private set; }
        private ObservableCollection<InfoItem> _messages = new FixedSizeObservableCollection<InfoItem>(1000);
        public ICommand CloseTabCommand { get; private set; }

        public ObservableCollection<InfoItem> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                RaisePropertyChanged();
            }
        }

        public InfoTab(string id)
        {
            this.Id = id;
            CloseTabCommand = new DelegateCommand(CloseTab);
        }

        private void CloseTab()
        {
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<CloseTabEvent<InfoTab>>().Publish(this);
        }

        public string TabId()
        {
            return Id;
        }

        public void Add(InfoItem item)
        {
            Messages.Add(item);
        }
    }
}
