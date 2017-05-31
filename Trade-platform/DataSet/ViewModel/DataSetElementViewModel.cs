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
using TradePlatform.DataSet.View;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.ViewModel
{
    public class DataSetElementViewModel : BindableBase, IDataSetElementViewModel
    {
        public ICommand ChooseInstrumentCommand { get; private set; }

        private ObservableCollection<Instrument> _instrumentsInfo = new ObservableCollection<Instrument>();
        public ObservableCollection<Instrument> InstrumentsInfo
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
            ChooseInstrumentCommand = new DelegateCommand(ChooseInstrument);
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<AddInstrumentToDatatSet>().Subscribe(AddSelectedInstruments, false);
        }

        private void AddSelectedInstruments(IList<Instrument> instruments)
        {
            instruments?.ForEach(InstrumentsInfo.Add); 
        }

        private void ChooseInstrument()
        {
            var window = Application.Current.Windows.OfType<InstrumentChooseListView>().SingleOrDefault(x => x.IsInitialized);
            if (window != null)
            {
                window.Activate();
                return;
            }
            ContainerBuilder.Container.Resolve<InstrumentChooseListView>().Show();
        }



    }
}
