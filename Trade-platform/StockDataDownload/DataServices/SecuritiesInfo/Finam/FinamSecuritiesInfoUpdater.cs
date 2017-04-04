using Microsoft.Practices.Unity;
using TradePlatform.Commons.Securities;

namespace TradePlatform.StockDataDownload.DataServices.SecuritiesInfo.Finam
{
    class FinamSecuritiesInfoUpdater : ISecuritiesInfoUpdater
    {
        private readonly ISecuritiesInfoDownloader _securitiesInfoDownloader;
        private readonly ISecuritiesInfoParser _securitiesInfoParser;

        public FinamSecuritiesInfoUpdater()
        {
            _securitiesInfoDownloader = ContainerBuilder.Container.Resolve<ISecuritiesInfoDownloader>();
            _securitiesInfoParser = ContainerBuilder.Container.Resolve<ISecuritiesInfoParser>();
        }

        public void Update()
        {
            SecuritiesInfoHolder securitiesInfo = ContainerBuilder.Container.Resolve<SecuritiesInfoHolder>();
            securitiesInfo.Securities = _securitiesInfoParser.Parse(_securitiesInfoDownloader.Download());
        }
    }
}
