using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TianYiSdtdServerTools.Client.ViewModels.Windows;
using TianYiSdtdServerTools.Client.Models.ObservableObjects;
using Panuon.UI.Silver;
using Autofac;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.Views.Services;
using IceCoffee.Wpf.MvvmFrame.Messaging;
using TianYiSdtdServerTools.Client.Models.MvvmMessages;
using System.Windows.Media.Imaging;
using IceCoffee.Common.LogManager;
using System.Reflection;

namespace TianYiSdtdServerTools.Client.Views.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : IceCoffee.Wpf.CustomControlLibrary.Windows.FramelessWindow
    {
        private static IDictionary<string, Type> _partialViewDic;

        public MainWindowViewModel ViewModel { get; set; }

        static MainWindow()
        {
            _partialViewDic = new Dictionary<string, Type>();
            //AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.StartsWith("SdtdServerTools"));
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var item in assembly.GetTypes())
            {
                if (string.IsNullOrEmpty(item.Namespace))
                {
                    continue;
                }

                if (item.Namespace.StartsWith("TianYiSdtdServerTools.Client.Views.PartialViews") && item.IsSubclassOf(typeof(UserControl)))
                {
                    _partialViewDic.Add(item.Name.Remove(item.Name.Length - 4), item);
                }
            }
            //assembly.GetTypes().Where(x => x.Namespace.StartsWith("TianYiSdtdServerTools.Client.Views.PartialViews")
            //                            && x.IsSubclassOf(typeof(UserControl)))
            //                            .ToList().ForEach(x => _partialViewDic.Add(x.Name.Remove(x.Name.Length - 4), x));
        }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = Autofac.Resolve<MainWindowViewModel>(new TypedParameter(typeof(IRichTextBoxService), new RichTextBoxService(richTextBox_runLog)));
            base.DataContext = ViewModel;

            Messenger.Default.Register<CommonEnumMessage>(this, InitChatMessageAndTelnetConsoleView);
            Messenger.Default.Register<FunctionSwitchStateChangedMessage>(this, OnFunctionSwitchStateChanged);
        }

        private void OnFunctionSwitchStateChanged(FunctionSwitchStateChangedMessage message)
        {
            System.Diagnostics.Debug.Assert(_partialViewDic.ContainsKey(message.FunctionTag));

            foreach (TabItem tab in this.leftTabControl2.Items)
            {
                if (tab.Tag.ToString() == message.FunctionTag)
                {
                    return;
                }
            }

            // 创建视图
            FrameworkElement view = (FrameworkElement)Activator.CreateInstance(_partialViewDic[message.FunctionTag],
                new object[] { message.FunctionTag });

            this.leftTabControl2.Items.Add(new TabItem()
            {
                Header = ViewModel.FunctionPanelItems.First(p => p.Tag == message.FunctionTag).Header,
                Tag = message.FunctionTag,
                Content = view
            });

            // 再次发送一条通知消息，因为在此之前ViewModel没有收到开关状态改变消息
            Messenger.Default.Send(message, view.DataContext.GetType());
        }

        /// <summary>
        /// 提前初始化聊天信息和Telnet控制台View
        /// </summary>
        private void InitChatMessageAndTelnetConsoleView(CommonEnumMessage enumMessage)
        {
            if (enumMessage == CommonEnumMessage.InitControlPanelView)
            {
                CheckControlPanelView("ChatMessage");
                CheckControlPanelView("TelnetConsole");
                Messenger.Default.UnregisterAllRecipientByType<CommonEnumMessage>();
            }
        }

        private void CheckControlPanelView(string tag)
        {
            foreach (TabItem tab in this.leftTabControl1.Items)
            {
                if (tab.Tag.ToString() == tag)
                {
                    return;
                }
            }

            this.leftTabControl1.Items.Add(new TabItem()
            {
                Header = ViewModel.ControlPanelItems.First(p => p.Tag == tag).Header,
                Tag = tag,
                // 创建视图
                Content = Activator.CreateInstance(_partialViewDic[tag])
            });
        }


        private void OnClickCloseButton(object sender, EventArgs e)
        {
            if (MessageBoxX.Show("确定退出程序吗？", "提示", this, MessageBoxButton.OKCancel, configKey: "CommonTheme") == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        private void OnLeftListBox1SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeSelectItem(this.leftListBox1, this.leftTabControl1);
        }

        private void OnLeftListBox2SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeSelectItem(this.leftListBox2, this.leftTabControl2);
        }

        private void ChangeSelectItem(ListBox listBox, TabControl tabControl)
        {
            ListViewItemModel selectedItem = listBox.SelectedItem as ListViewItemModel;

            foreach (TabItem tab in tabControl.Items)
            {
                if (tab.Tag.ToString() == selectedItem.Tag)
                {
                    tabControl.SelectedItem = tab;
                    return;
                }
            }

            System.Diagnostics.Debug.Assert(_partialViewDic.ContainsKey(selectedItem.Tag));

            TabItem tabItem = new TabItem()
            {
                Header = selectedItem.Header,
                Tag = selectedItem.Tag,
                // 创建视图
                Content = listBox == this.leftListBox2 ? Activator.CreateInstance(_partialViewDic[selectedItem.Tag], new object[] { selectedItem.Tag })
                                                       : Activator.CreateInstance(_partialViewDic[selectedItem.Tag])
            };

            tabControl.Items.Add(tabItem);
            tabControl.SelectedItem = tabItem;
        }

        private void OnButtonMenu_more_Initialized(object sender, EventArgs e)
        {
            // 设置右键菜单为null
            this.buttonMenu_more.ContextMenu = null;

            this.buttonMenu_more.Click += OnButtonMenu_Click;
        }

        private void OnButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            // 目标
            this.contextMenu_more.PlacementTarget = this.buttonMenu_more;
            // 位置
            //this.contextMenu_more.Placement = PlacementMode.Bottom;
            // 显示菜单
            this.contextMenu_more.IsOpen = true;
        }

        private void OnMenuItem_Tools_Click(object sender, RoutedEventArgs e)
        {
            ToolDialog toolDialog = new ToolDialog();
            toolDialog.Show();
        }
    }

}
