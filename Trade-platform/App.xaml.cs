using System.Windows;

namespace TradePlatform
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ContainerBuilder.Initialize();
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
