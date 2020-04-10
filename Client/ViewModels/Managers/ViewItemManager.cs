using IceCoffee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.ObservableClasses;

namespace TianYiSdtdServerTools.Client.ViewModels.Managers
{
    public sealed class ViewItemManager : Singleton1<ViewItemManager>
    {
        public List<ListViewItemModel> ControlPanelItems { get; set; }

        public List<FunctionPanelViewItemModel> FunctionPanelItems { get; set; }

        public ViewItemManager()
        {
            ControlPanelItems = new List<ListViewItemModel>()
            {
                new ListViewItemModel("配置信息", "ConfigInfo"),
                new ListViewItemModel("在线玩家", "OnlinePlayer"),
                new ListViewItemModel("聊天信息", "ChatMessage"),
                new ListViewItemModel("Telnet控制台", "TelnetConsole"),
                new ListViewItemModel("历史玩家", "HistoryPlayer"),
                new ListViewItemModel("权限管理", "PermissionManagement"),
            };

            FunctionPanelItems = new List<FunctionPanelViewItemModel>()
            {
                new FunctionPanelViewItemModel("游戏公告", "GameNotice"),
                new FunctionPanelViewItemModel("积分系统", "ScoreSystem"),
                new FunctionPanelViewItemModel("公共回城", "TeleportCity"),
                new FunctionPanelViewItemModel("私人回家", "TeleportHome"),
                new FunctionPanelViewItemModel("游戏商店", "GameStore"),
                new FunctionPanelViewItemModel("抽奖系统", "LotterySystem"),
            };
        }
    }
}
