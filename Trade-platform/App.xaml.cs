using System.Windows;
namespace TradePlatform
{
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
