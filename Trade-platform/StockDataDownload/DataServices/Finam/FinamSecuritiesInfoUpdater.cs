using System;
using TradePlatform.StockDataDownload.Services;
using Microsoft.Practices.Unity;
using TradePlatform.Common.Securities;
using TradePlatform.StockDataDownload.DataParsers;

namespace TradePlatform.StockDataDownload.DataServices
{
    class FinamSecuritiesInfoUpdater : ISecuritiesInfoUpdater
    {
        private ISecuritiesInfoDownloader _securitiesInfoDownloader;
        private ISecuritiesInfoParser _securitiesInfoParser;

        public FinamSecuritiesInfoUpdater()
        {
            _securitiesInfoDownloader = ContainerBuilder.Container.Resolve<ISecuritiesInfoDownloader>();
            _securitiesInfoParser = ContainerBuilder.Container.Resolve<ISecuritiesInfoParser>();
        }

        public bool Update()
        {
            try
            {
                SecuritiesInfo securitiesInfo = ContainerBuilder.Container.Resolve<SecuritiesInfo>();
                securitiesInfo.Securities = _securitiesInfoParser.Parse(_securitiesInfoDownloader.Download());
                return true;
            }
            catch (Exception e)
            {
                //TODO:- need to add to log 
                throw new Exception(e.Message);
            }
            //return false;
        }
    }
}
