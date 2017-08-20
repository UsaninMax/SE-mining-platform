using System.Windows;
namespace SEMining
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
