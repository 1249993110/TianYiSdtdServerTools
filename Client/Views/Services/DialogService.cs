﻿using AutoUpdaterDotNET;
using IceCoffee.Wpf.CustomControlLibrary.Controls;
using Panuon.UI.Silver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.Views.Windows;

namespace TianYiSdtdServerTools.Client.Views.Services
{
    public class DialogService : IDialogService
    {
        static DialogService()
        {
            MessageBoxX.MessageBoxXConfigurations.Add("CommonTheme", new Panuon.UI.Silver.Core.MessageBoxXConfigurations()
            {
                // 反转消息框的按钮顺序
                ReverseButtonSequence = true
            });
        }

        public void ShowInformation(string message, string title = "提示")
        {
            MessageBoxX.Show(message, title, configKey: "CommonTheme");
        }

        public bool ShowOKCancel(string message, string title = "提示")
        {
            return MessageBoxX.Show(message, title, Application.Current.MainWindow ?? null, MessageBoxButton.OKCancel, configKey: "CommonTheme")
                == MessageBoxResult.OK;
        }

        public bool ShowYesNo(string message, string title = "提示")
        {
            return MessageBoxX.Show(message, title, Application.Current.MainWindow ?? null, MessageBoxButton.YesNo, configKey: "CommonTheme")
                == MessageBoxResult.Yes;
        }

        public string ShowInputDialog(string question, string defaultAnswer = "", string title = "输入")
        {
            InputDialog inputDialog = new InputDialog(question, defaultAnswer, title);
            if(inputDialog.ShowDialog() == true)
            {
                return inputDialog.Answer;
            }
            return null;
        }

        public void ShowPlayerInventory(string steamID)
        {
            new PlayerInventoryDialog(steamID).Show();
        }

        public void ShowAutoUpdater(string xmlUrl)
        {
            AutoUpdater.ReportErrors = true;
            AutoUpdater.Mandatory = true;
            AutoUpdater.UpdateMode = Mode.Forced;
            AutoUpdater.Start(xmlUrl);
        }
    }
}
