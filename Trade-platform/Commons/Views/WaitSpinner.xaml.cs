using System.Windows.Media.Animation;

namespace TradePlatform.Commons.Views
{
    public partial class WaitSpinner
    {
        public WaitSpinner()
        {
            InitializeComponent();
            ((Storyboard)FindResource("WaitStoryboard")).Begin();
        }
    }
}
