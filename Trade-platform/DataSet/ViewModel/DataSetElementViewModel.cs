using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using TradePlatform.DataSet.View;

namespace TradePlatform.DataSet.ViewModel
{
    public class DataSetElementViewModel : BindableBase, IDataSetElementViewModel
    {
        public ICommand ChooseInstrumentCommand { get; private set; }

        public DataSetElementViewModel()
        {
            ChooseInstrumentCommand = new DelegateCommand(ChooseInstrument);
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
