using System;
using System.ComponentModel;
using SEMining.DataSet.ViewModels;
using Microsoft.Practices.Unity;

namespace SEMining.DataSet.Views
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

