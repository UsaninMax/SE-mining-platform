﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using SEMining.Commons.Info;
using SEMining.Commons.Sistem;
using SEMining.DataSet.DataServices.Serialization;
using SEMining.DataSet.Holders;
using SEMining.DataSet.Models;
using System.Collections.Generic;
using SEMining.StockData.Models;
using SE_mining_base.Info.Message;

namespace SEMining.DataSet.DataServices
{
    public class DataSetService : IDataSetService
    {
        private readonly IInfoPublisher _infoPublisher;
        private readonly IFileManager _fileManager;
        private readonly IDataTickStorage _tickStorage;
        private readonly IDataSetHolder _dataSetHolder;

        public DataSetService()
        {
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
            _fileManager = ContainerBuilder.Container.Resolve<IFileManager>();
            _tickStorage = ContainerBuilder.Container.Resolve<IDataTickStorage>();
            _dataSetHolder = ContainerBuilder.Container.Resolve<IDataSetHolder>();
        }


        public void Store(DataSetItem item, CancellationToken cancellationToken)
        {
            var tickProvider = ContainerBuilder.Container.Resolve<IDataTickProvider>();
            _infoPublisher.PublishInfo(new DataSetInfo { Message = item + "- start read ticks from files" });
            IEnumerable<DataTick> ticks = tickProvider.Get(item, cancellationToken);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            _infoPublisher.PublishInfo(new DataSetInfo { Message = item + "- ticks is ready, start saving data set" });

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            DeleteFolder(item);
            CreateFolder(item);
            _tickStorage.Store(ticks, DataSetItem.RootPath + "\\" + item.Path, item.Path);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            _infoPublisher.PublishInfo(new DataSetInfo { Message = item + "- data set was saved " });
        }

        public IEnumerable<DataTick> Get(string id)
        {
            if (!_dataSetHolder.CheckIfExist(id))
            {
                throw new Exception("Data set with id = " + id + " was not exist");
            }
            DataSetItem item = _dataSetHolder.Get(id);
            return _tickStorage.ReStore(DataSetItem.RootPath + "\\" + item.Path + "\\" + item.Path + ".xml");
        }

        public void Delete(DataSetItem item, Task build, CancellationTokenSource cancellationTokenSource)
        {

            if (build != null && !build.IsCompleted)
            {
                _infoPublisher.PublishInfo(new DataSetInfo { Message = item + "- cancellation will wait till build data set will finish" });
                cancellationTokenSource.Cancel();
                build.Wait();
            }
            DeleteFolder(item);
            var instrumentsHolder = ContainerBuilder.Container.Resolve<IDataSetHolder>();
            instrumentsHolder.Remove(item);
            _infoPublisher.PublishInfo(new DataSetInfo { Message = item + "- is deleted" });
        }

        private void DeleteFolder(DataSetItem item)
        {
            _fileManager.DeleteFolder(DataSetItem.RootPath + "\\" + item.Path);
        }

        private void CreateFolder(DataSetItem item)
        {
            _fileManager.CreateFolder(DataSetItem.RootPath + "\\" + item.Path);
        }

        public bool CheckIfExist(DataSetItem item)
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
