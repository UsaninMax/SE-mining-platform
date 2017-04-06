using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TradePlatform.Commons.Views
{
    public partial class WaitSpinner : UserControl
    {
        public WaitSpinner()
        {
            this.InitializeComponent();
            ((Storyboard)FindResource("WaitStoryboard")).Begin();
        }
    }
}
