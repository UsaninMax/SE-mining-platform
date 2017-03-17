using System.Windows;
using TradePlatform.Common.ViewModel;

namespace TradePlatform.view
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView(IViewModel viewModel)
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
