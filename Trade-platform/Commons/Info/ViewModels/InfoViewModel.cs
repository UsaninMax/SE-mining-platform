using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace TradePlatform.Commons.Info.ViewModels
{
    public class InfoViewModel : IInfoViewModel
    {
        public ObservableCollection<TabItem> Tabs { get; set; }
        public InfoViewModel()
        {
            Tabs = new ObservableCollection<TabItem>();
            Tabs.Add(new TabItem { Header = "One", Content = "One's content" });
            Tabs.Add(new TabItem { Header = "Two", Content = "Two's content" });
        }

    }
}
