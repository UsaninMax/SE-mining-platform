﻿using System.Windows.Controls;
using TradePlatform.StockDataUploud.viewModel;
using Microsoft.Practices.Unity;
using System.ComponentModel;

namespace TradePlatform.StockDataUploud.view
{
    /// <summary>
    /// Interaction logic for DownloadNewData.xaml
    /// </summary>
    public partial class DownloadNewDataView : UserControl
    {
        public DownloadNewDataView()
        {
            this.InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = ContainerBuilder.Container.Resolve<IDownloadNewDataViewModel>();
            }
        }
    }
}