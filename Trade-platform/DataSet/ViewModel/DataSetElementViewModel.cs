using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Castle.Core.Internal;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.Commons.BaseModels;
using TradePlatform.Commons.Info;
using TradePlatform.DataSet.Events;
using TradePlatform.DataSet.Holders;
using TradePlatform.DataSet.Models;
using TradePlatform.DataSet.View;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.ViewModel
{
    public class DataSetElementViewModel : BindableBase, IDataSetElementViewModel, IClosableWindow
    {
        public event EventHandler CloseWindowNotification;
        public ICommand ChooseSubInstrumentCommand { get; private set; }
        public ICommand RemoveSubInstrumentCommand { get; private set; }
        public ICommand CreateNewCommand { get; private set; }
        public string UniqueId { get; set; }
        private readonly IDataSetHolder _holder;
        private readonly IInfoPublisher _infoPublisher;
        private readonly IEventAggregator _eventAggregator;

        private ObservableCollection<SubInstrument> _instrumentsInfo = new ObservableCollection<SubInstrument>();
        public ObservableCollection<SubInstrument> InstrumentsInfo
        {
            get
            {
                return _instrumentsInfo;
            }
            set
            {
                _instrumentsInfo = value;
                RaisePropertyChanged();
            }
        }

        public DataSetElementViewModel()
        {
            _holder = ContainerBuilder.Container.Resolve<IDataSetHolder>();
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
            _eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<AddInstrumentToDatatSetEvent>().Subscribe(AddSelectedInstruments, false);
            _eventAggregator.GetEvent<CopyDataSetEvent>().Subscribe(CopyDataSet, false);
            ChooseSubInstrumentCommand = new DelegateCommand(ChooseSubInstrument);
            RemoveSubInstrumentCommand = new DelegateCommand<SubInstrument>(RemoveSubInstrument);
            CreateNewCommand = new DelegateCommand(CreateNew);
        }

        private void AddSelectedInstruments(IList<Instrument> instruments)
        {
            instruments?.ForEach(i =>
            {
                InstrumentsInfo.Add(new SubInstrument(i));

            });
        }

        private void ChooseSubInstrument()
        {
            var window = Application.Current.Windows.OfType<InstrumentChooseListView>().SingleOrDefault(x => x.IsInitialized);
            if (window != null)
            {
                window.Activate();
                return;
            }
            ContainerBuilder.Container.Resolve<InstrumentChooseListView>().Show();
        }

        private void CreateNew()
        {
            if (UniqueIdIsWrong())
            {
                _infoPublisher.PublishException( new Exception("Attempt to add new data set: Unique Id - " + UniqueId + " - is not unique."));
                return;
            }

            DataSetItem dataSet = new DataSetItem
                .Builder()
                .WithId(UniqueId)
                .WithSubInstruments(InstrumentsInfo)
                .Build();
            _holder.Put(dataSet);
            _eventAggregator.GetEvent<CreateDataSetItemEvent>().Publish(dataSet);

            CloseWindowNotify();
        }

        private bool UniqueIdIsWrong()
        {
            return UniqueId.IsNullOrEmpty() ||
                _holder.CheckIfExist(UniqueId);
        }

        private void RemoveSubInstrument(SubInstrument subInstrument)
        {
            InstrumentsInfo.Remove(subInstrument);
        }

        private void CloseWindowNotify()
        {
            CloseWindowNotification?.Invoke(this, EventArgs.Empty);
        }

        private void CopyDataSet(DataSetItem item)
        {
            item.SubInstruments.ForEach(InstrumentsInfo.Add);
        }
    }
}
