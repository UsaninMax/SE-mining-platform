using System.Collections.ObjectModel;
using Prism.Mvvm;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.Commons.Utils;

namespace TradePlatform.Commons.Info.Model
{
    public class InfoTab : BindableBase, IInfoTab
    {
        public string Id { get; private set; }
        private ObservableCollection<InfoItem> _messages = new FixedSizeObservableCollection<InfoItem>(1000);

        public ObservableCollection<InfoItem> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                RaisePropertyChanged();
            }
        }

        public InfoTab(string id)
        {
            this.Id = id;
        }


        public string TabId()
        {
            return Id;
        }

        public void Add(InfoItem item)
        {
            Messages.Add(item);
        }

        public int MessageCount()
        {
            return Messages.Count;
        }
    }
}
