using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.DataSet.Events;
using TradePlatform.DataSet.Models;
using TradePlatform.DataSet.View;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.ViewModel
{
    public class DataSetElementViewModel : BindableBase, IDataSetElementViewModel
    {
        public ICommand ChooseSubInstrumentCommand { get; private set; }
        public ICommand RemoveSubInstrumentCommand { get; private set; }

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
            ChooseSubInstrumentCommand = new DelegateCommand(ChooseSubInstrument);
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<AddInstrumentToDatatSet>().Subscribe(AddSelectedInstruments, false);
            RemoveSubInstrumentCommand = new DelegateCommand<SubInstrument>(RemoveSubInstrument);
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

        private void RemoveSubInstrument(SubInstrument subInstrument)
        {
            InstrumentsInfo.Remove(subInstrument);
        }
    }
}
