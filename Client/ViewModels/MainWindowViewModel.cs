using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.ObservableClasses;
using TianYiSdtdServerTools.Client.ViewModels.ControlPanel;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {       
        public ObservableCollection<ListViewItemModel> ControlPanelItems { get; set; }

        public MainWindowViewModel()
        {
            // 反射获取已加载到此应用程序域的执行上下文中的程序集。
            var assemblys = AppDomain.CurrentDomain.GetAssemblies();
            // 反射获取UserControl类型
            Type userControlType = assemblys.FirstOrDefault(x => x.FullName.StartsWith("PresentationFramework")).GetType("System.Windows.Controls.UserControl");

            // 反射获取ControlPanel下的所有View
            List<Type> types = assemblys.FirstOrDefault(x => x.FullName.StartsWith("SdtdServerTools")).GetTypes()
                .Where(x => x.Namespace.StartsWith("TianYiSdtdServerTools.Client.Views.PartialViews.ControlPanel") && x.IsSubclassOf(userControlType)).ToList();

            foreach (var item in types)
            {
                ControlPanelItems = new ObservableCollection<ListViewItemModel>()
                {
                    //new ListViewItemModel("主页","HomePage"),
                    new ListViewItemModel("配置信息","ConfigInfo"),
                    //new ListViewItemModel("开服助手","HomePage"),
                    new ListViewItemModel("在线玩家","OnlinePlayer"),
                    new ListViewItemModel("聊天信息","ChatMessage"),
                    //new ListViewItemModel("Telnet控制台","HomePage")
                };
            }
        }
    }
}
