using System;
using SEMining.DataSet.ViewModels;
using Microsoft.Practices.Unity;
using SEMining.Commons.BaseModels;

namespace SEMining.DataSet.Views

{
    public partial class InstrumentChooseListView
    {
        public InstrumentChooseListView()
        {
            InitializeComponent();

            var modelWiew = ContainerBuilder.Container.Resolve<IInstrumentChooseListViewModel>();
            DataContext = modelWiew;


            IClosableWindow closableWindow = modelWiew as IClosableWindow;
    

            if (closableWindow != null)
            {
                closableWindow.CloseWindowNotification += CloseWindowNotificationHandler;
            }
        }

        private void CloseWindowNotificationHandler(object source, EventArgs e)
        {
            Close();
        }
    }
}
