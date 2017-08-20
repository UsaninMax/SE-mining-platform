using System;

namespace SEMining.Commons.BaseModels
{
    interface IClosableWindow
    {
        event EventHandler CloseWindowNotification;
    }
}
