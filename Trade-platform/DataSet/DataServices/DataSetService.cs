using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.Commons.Sistem;
using TradePlatform.DataSet.DataServices.Serialization;
using TradePlatform.DataSet.Holders;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.DataServices
{
    public class DataSetService : IDataSetService
    {
        private readonly IInfoPublisher _infoPublisher;
        private readonly IFileManager _fileManager;
        private readonly IDataTickStorage _tickStorage;

        public DataSetService()
        {
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
            _fileManager = ContainerBuilder.Container.Resolve<IFileManager>();
            _tickStorage = ContainerBuilder.Container.Resolve<IDataTickStorage>();
        }


        public void BuildSet(DataSetItem item, CancellationToken cancellationToken)
        {
            DeleteFolder(item);
            CreateFolder(item);

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            var tickProvider = ContainerBuilder.Container.Resolve<IDataTickProvider>();
            _tickStorage.Store(tickProvider.Get(item), DataSetItem.RootPath + "\\" + item.Path, item.Path);
            _infoPublisher.PublishInfo(new DownloadInfo { Message = item + "- was created" });
        }

        public void Delete(DataSetItem item, Task build, CancellationTokenSource cancellationTokenSource)
        {

            if (build != null && !build.IsCompleted)
            {
                _infoPublisher.PublishInfo(new DownloadInfo { Message = item + "- cancellation will wait till build data set will finished" });
                cancellationTokenSource.Cancel();
                build.Wait();
            }
            DeleteFolder(item);
            var instrumentsHolder = ContainerBuilder.Container.Resolve<IDataSetHolder>();
            instrumentsHolder.Remove(item);
            _infoPublisher.PublishInfo(new DownloadInfo { Message = item + "- is deleted" });
        }

        private void DeleteFolder(DataSetItem item)
        {
            _fileManager.DeleteFolder(DataSetItem.RootPath + "\\" + item.Path);
        }

        private void CreateFolder(DataSetItem item)
        {
            _fileManager.CreateFolder(DataSetItem.RootPath + "\\" + item.Path);
        }

        public bool CheckFiles(DataSetItem item)
        {
            return _fileManager.IsDirectoryExist(DataSetItem.RootPath + "\\" + item.Path) &&
                   FileExist(item);
        }

        private bool FileExist(DataSetItem item)
        {
            return _fileManager.IsFileExist(DataSetItem.RootPath + "\\" + item.Path + "\\" + item.Path + ".xml");
        }
    }
}
