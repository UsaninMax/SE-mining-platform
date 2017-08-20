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
using SEMining.Commons.BaseModels;
using SEMining.Commons.Info;
using SEMining.DataSet.Events;
using SEMining.DataSet.Holders;
using SEMining.DataSet.Models;
using SEMining.DataSet.Views;
using SEMining.StockData.Models;

namespace SEMining.DataSet.ViewModels
{
    public class DataSetElementViewModel : BindableBase, IDataSetElementViewModel, IClosableWindow, IDisposable
    {
        public event EventHandler CloseWindowNotification;
        public ICommand ChooseSubInstrumentCommand { get; private set; }
        public ICommand RemoveSubInstrumentCommand { get; private set; }
        public ICommand CreateNewCommand { get; private set; }
        public string UniqueId {
            get
            {
                return _uniqueId;
            }
            set
            {
                _uniqueId = value;
                RaisePropertyChanged();
            }
        }

        private string _uniqueId;

        public double WarrantyCoverage
        {
            get
            {
                return _warrantyCoverage;
            }
            set
            {
                _warrantyCoverage = value;
                RaisePropertyChanged();
            }
        }

        private double _warrantyCoverage;

        public double StepSize
        {
            get
            {
                return _stepSize;
            }
            set
            {
                _stepSize = value;
                RaisePropertyChanged();
            }
        }

        private double _stepSize;

        private readonly IDataSetHolder _holder;
        private readonly IInfoPublisher _infoPublisher;
        protected readonly IEventAggregator _eventAggregator;

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

        private SubInstrument _selectedInstrument;
        public SubInstrument SelectedSubInstrument
        {
            get { return _selectedInstrument; }
            set
            {
                _selectedInstrument = value;
                RaisePropertyChanged();
                UpdateVisibilityOfContextMenu();
            }
        }

        public DataSetElementViewModel()
        {
            _holder = ContainerBuilder.Container.Resolve<IDataSetHolder>();
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
            _eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            _eventAggregator.GetEvent<AddInstrumentToDatatSetEvent>().Subscribe(AddSelectedInstruments);
            ChooseSubInstrumentCommand = new DelegateCommand(ChooseSubInstrument);
            RemoveSubInstrumentCommand = new DelegateCommand(RemoveSubInstrument, CanDoAction);
            CreateNewCommand = new DelegateCommand(CreateNew);
        }

        private void AddSelectedInstruments(IEnumerable<Instrument> instruments)
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
                _infoPublisher.PublishException(new Exception("Attempt to add new data set: Unique Id - " + UniqueId + " - is not unique."));
                return;
            }

            DataSetItem dataSet = new DataSetItem
                .Builder()
                .WithId(UniqueId)
                .WithStepSize(StepSize)
                .WithWarrantyCoverage(WarrantyCoverage)
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

        private void RemoveSubInstrument()
        {
            InstrumentsInfo.Remove(_selectedInstrument);
        }

        private void CloseWindowNotify()
        {
            CloseWindowNotification?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateVisibilityOfContextMenu()
        {
            (RemoveSubInstrumentCommand as DelegateCommand)?.RaiseCanExecuteChanged();
        }

        private bool CanDoAction()
        {
            return _selectedInstrument != null;
        }

        public virtual void Dispose()
        {
            _eventAggregator.GetEvent<AddInstrumentToDatatSetEvent>().Unsubscribe(AddSelectedInstruments);
        }
    }
}
