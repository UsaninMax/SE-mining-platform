using System;
using System.ComponentModel;
using TradePlatform.DataSet.ViewModels;
using Microsoft.Practices.Unity;
using System.Windows;

namespace TradePlatform.DataSet.Views
{
    public partial class ShowDataSetElementView
    {
        public ShowDataSetElementView()
        {
            InitializeComponent();
            DataContext = ContainerBuilder.Container.Resolve<IDataSetElementViewModel>("ShowDataSet");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var disposable = DataContext as IDisposable;
            disposable?.Dispose();
            base.OnClosing(e);
        }
    }
}

