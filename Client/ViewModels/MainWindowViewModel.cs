using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TianYiSdtdServerTools.Client.Models.ObservableObjects;
using TianYiSdtdServerTools.Client.ViewModels.ControlPanel;

namespace TianYiSdtdServerTools.Client.ViewModels
{
    public class MainWindowViewModel
    {       
        public ObservableCollection<ListViewItemModel> ControlPanelItems { get; set; }

        public MainWindowViewModel()
        {
            System.Reflection.Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.StartsWith("SdtdServerTools"));
            List<Type> types = assembly.GetTypes()
                .Where(x => x.Namespace.StartsWith("TianYiSdtdServerTools.Client.Views.PartialViews.ControlPanel") && x.IsSubclassOf(typeof(UserControl))).ToList();

            foreach (var item in types)
            {
                ControlPanelItems = new ObservableCollection<ListViewItemModel>()
                {
                    new ListViewItemModel("主页","HomePage"),
                    new ListViewItemModel("配置信息","ConfigInfo")
                    //new ListViewItemModel("开服助手","HomePage"),
                    //new ListViewItemModel("在线玩家","HomePage"),
                    //new ListViewItemModel("Telnet控制台","HomePage")
                };
            }
        }

    }
}
