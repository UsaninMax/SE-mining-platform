using System.Windows.Media.Animation;

namespace SEMining.Commons.Views
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
