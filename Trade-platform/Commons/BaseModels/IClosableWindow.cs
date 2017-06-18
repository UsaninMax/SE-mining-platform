using System;

namespace TradePlatform.Commons.BaseModels
{
    interface IClosableWindow
    {
        event EventHandler CloseWindowNotification;
    }
}
