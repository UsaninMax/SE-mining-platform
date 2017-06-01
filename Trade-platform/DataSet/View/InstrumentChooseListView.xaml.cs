using System;
using TradePlatform.DataSet.ViewModel;
using Microsoft.Practices.Unity;
using System.Windows;
using TradePlatform.Commons.BaseModels;

namespace TradePlatform.DataSet.View

{
    public partial class InstrumentChooseListView : Window
    {
        public InstrumentChooseListView()
        {
            this.InitializeComponent();

            var modelWiew = ContainerBuilder.Container.Resolve<IInstrumentChooseListViewModel>();
            this.DataContext = modelWiew;


            IClosableWindow closableWindow = modelWiew as IClosableWindow;
    

            if (closableWindow != null)
            {
                closableWindow.CloseWindowNotification += new EventHandler(CloseWindowNotificationHandler);
            }
        }

        private void CloseWindowNotificationHandler(object source, EventArgs e)
        {
            Close();
        }
    }
}
