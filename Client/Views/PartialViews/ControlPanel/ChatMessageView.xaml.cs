using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.ViewModels.ControlPanel;
using TianYiSdtdServerTools.Client.Views.Services;

namespace TianYiSdtdServerTools.Client.Views.PartialViews.ControlPanel
{
    /// <summary>
    /// ChatMessageView.xaml 的交互逻辑
    /// </summary>
    public partial class ChatMessageView : UserControl
    {
        public ChatMessageViewModel ChatMessageViewModel { get; set; }

        public ChatMessageView()
        {
            InitializeComponent();

            ChatMessageViewModel = Autofac.Resolve<ChatMessageViewModel>(new TypedParameter(typeof(IRichTextBoxService),
                new RichTextBoxService(richTextBox_chatMessage)));

            this.DataContext = ChatMessageViewModel;
        }
    }
}
