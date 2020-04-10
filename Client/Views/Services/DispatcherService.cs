using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TianYiSdtdServerTools.Client.Services.UI;

namespace TianYiSdtdServerTools.Client.Views.Services
{
    public class DispatcherService : IDispatcherService
    {
        public event EventHandler ShutdownStarted
        {
            add { Application.Current.Dispatcher.ShutdownStarted += value; }
            remove { Application.Current.Dispatcher.ShutdownStarted -= value; }
        }

        public void Invoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        public void Invoke(Action action, DispatcherPriority dispatcherPriority)
        {
            Application.Current.Dispatcher.Invoke(action, (System.Windows.Threading.DispatcherPriority)dispatcherPriority);
        }

        public void InvokeAsync(Action action)
        {
            Application.Current.Dispatcher.InvokeAsync(action);
        }

        public void InvokeAsync(Action action, DispatcherPriority dispatcherPriority)
        {
            Application.Current.Dispatcher.InvokeAsync(action, (System.Windows.Threading.DispatcherPriority)dispatcherPriority);
        }
    }
}
