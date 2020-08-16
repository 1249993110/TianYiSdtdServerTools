using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TianYiSdtdServerTools.Client.Services.UI
{
    public interface IDialogService
    {
        /// <summary>
        /// 显示一个只有确定按钮无返回值的消息框
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        void ShowInformation(string content, string title = "提示");

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        bool ShowYesNo(string content, string title = "提示");

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        bool ShowOKCancel(string content, string title = "提示");

        /// <summary>
        /// 显示输入对话框
        /// </summary>
        /// <param name="question"></param>
        /// <param name="defaultAnswer"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        string ShowInputDialog(string question, string defaultAnswer = "", string title = "输入");

        /// <summary>
        /// 显示玩家背包
        /// </summary>
        /// <param name="steamID"></param>
        /// <returns></returns>
        void ShowPlayerInventory(string steamID);

        /// <summary>
        /// 显示自动更新器
        /// </summary>
        void ShowAutoUpdater(string xmlUrl);
    }
}
