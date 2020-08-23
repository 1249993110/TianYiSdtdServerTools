using Autofac;
using AutoUpdaterDotNET;
using Icecoffee.Wpf.CustomControlLibrary.Utils;
using IceCoffee.Common.LogManager;
using IceCoffee.Common.Wpf;
using IceCoffee.Wpf.MvvmFrame.Messaging;
using Panuon.UI.Silver;
using System;
using System.Windows;
using System.Windows.Input;
using TianYiSdtdServerTools.Client.Models.MvvmMessages;
using TianYiSdtdServerTools.Client.ViewModels.Windows;
using TianYiSdtdServerTools.Shared;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;

namespace TianYiSdtdServerTools.Client.Views.Windows
{
    /// <summary>
    /// 默认实现IClearCaller清理ViewModel
    /// </summary>
    public class PanuonWindowXBase : WindowX, IClearCaller
    {
        protected override void OnClosed(EventArgs e)
        {
            InvokeClearMethod();
            base.OnClosed(e);
        }

        public virtual void InvokeClearMethod()
        {
            Util.ClearFrameworkElement(this);
        }
    }
}
