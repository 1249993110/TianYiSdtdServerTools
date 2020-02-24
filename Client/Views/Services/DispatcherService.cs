using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;

namespace TianYiSdtdServerTools.Client.Views.Services
{
    public class DispatcherService : IDispatcherService
    {
        public event EventHandler ShutdownStarted
        {
            add { Application.Current.MainWindow.Dispatcher.ShutdownStarted += value; }
            remove { Application.Current.MainWindow.Dispatcher.ShutdownStarted -= value; }
        }

        public void Invoke(Action action)
        {
            Application.Current.MainWindow.Dispatcher.Invoke(action);
        }

        public void InvokeAsync(Action action)
        {
            Application.Current.MainWindow.Dispatcher.InvokeAsync(action);
        }
    }
}
