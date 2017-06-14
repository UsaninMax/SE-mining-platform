using System;
using System.ComponentModel;
using TradePlatform.DataSet.ViewModels;
using Microsoft.Practices.Unity;
using System.Windows;
using TradePlatform.Commons.BaseModels;

namespace TradePlatform.DataSet.Views
{
    public partial class CopyDataSetElementView : Window
    {
        public CopyDataSetElementView()
        {
            this.InitializeComponent();
            var modelWiew = ContainerBuilder.Container.Resolve<IDataSetElementViewModel>("CopyDataSet");
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

        protected override void OnClosing(CancelEventArgs e)
        {
            var disposable = this.DataContext as IDisposable;
            disposable?.Dispose();
            base.OnClosing(e);
        }
    }
}

