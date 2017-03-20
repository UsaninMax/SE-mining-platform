﻿using System.Windows.Controls;
using TradePlatform.StockDataUploud.viewModel;
using Microsoft.Practices.Unity;
using System.ComponentModel;

namespace TradePlatform.StockDataUploud.view
{
    /// <summary>
    /// Interaction logic for DownloadedData.xaml
    /// </summary>
    public partial class DownloadedDataView : UserControl
    {
        public DownloadedDataView()
        {
            this.InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = ContainerBuilder.Container.Resolve<IDownloadedDataViewModel>();
            }
        }
    }
}
