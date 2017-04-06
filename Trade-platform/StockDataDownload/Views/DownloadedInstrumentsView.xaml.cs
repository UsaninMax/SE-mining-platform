using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using TradePlatform.StockDataDownload.ViewModels;

namespace TradePlatform.StockDataDownload.Views
{
    public partial class DownloadedInstrumentsView : UserControl
    {

        public static readonly DependencyProperty IsClosingProperty = DependencyProperty.Register("IsClosing", typeof(bool), typeof(DownloadedInstrumentsView), new PropertyMetadata(false));

        public bool IsClosing
        {
            get
            {
                return (bool)GetValue(IsClosingProperty);
            }
            set
            {
                SetValue(IsClosingProperty, value);
            }
        }

        public DownloadedInstrumentsView()
        {
            this.InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = ContainerBuilder.Container.Resolve<IDownloadedInstrumentsViewModel>();
            }

            this.Loaded += UserControlLoaded;

        }
        void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Closing += WindowClosing;
        }

        void WindowClosing(object sender, global::System.ComponentModel.CancelEventArgs e)
        {
            IsClosing = true;
        }
    }
}
