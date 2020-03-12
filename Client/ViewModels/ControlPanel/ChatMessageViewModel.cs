using IceCoffee.Common.LogManager;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Wpf.MvvmFrame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TianYiSdtdServerTools.Client.Models.Chat;
using TianYiSdtdServerTools.Client.Models.Players;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.ControlPanel
{
    public class ChatMessageViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        private readonly IRichTextBoxService _chatMessageViewService;

        public string Message { get; set; }

        [ConfigNode(XmlNodeType.Attribute)]
        public bool FilteSystemMessage { get; [NPCA_Method]set; }

        public RelayCommand SendMessage { get; private set; }

        public RelayCommand ViewRecord { get; private set; }


        public ChatMessageViewModel(IDispatcherService dispatcherService, IDialogService dialogService, IRichTextBoxService chatMessageViewService)
            : base(dispatcherService)
        {
            _dialogService = dialogService;
            _chatMessageViewService = chatMessageViewService;

            SendMessage = new RelayCommand(() => { SdtdConsole.Instance.SendGlobalMessage(Message); });

            ViewRecord = new RelayCommand(() =>
            {
                Utils.FileHelper.OpenTextFileByNotepad(_dialogService, System.Configuration.ConfigurationManager.AppSettings["ChatMessageRecordPath"]);
            });

            SdtdConsole.Instance.ChatHook += ChatHook;

            SdtdConsole.Instance.PlayerEnterGame += OnPlayerEnterGame;
            SdtdConsole.Instance.PlayerLeftGame += OnPlayerLeftGame;
        }

        private void OnPlayerLeftGame(PlayerInfo playerInfo)
        {
            OnPlayerEnterLeftGame(playerInfo, false);
        }

        private void OnPlayerEnterGame(PlayerInfo playerInfo)
        {
            OnPlayerEnterLeftGame(playerInfo, true);
        }

        private void OnPlayerEnterLeftGame(PlayerInfo playerInfo, bool isEnter)
        {
            string msg = string.Format("玩家: {0} {1}了游戏, SteamID: {2}", playerInfo.PlayerName, isEnter ? "进入" : "离开", playerInfo.SteamID);

            Log.Info(msg);

            _chatMessageViewService.AppendBlock(new RichText(DateTime.Now.ToString() + " "),
                new RichText(msg, "#00FA9A"));
        }

        private void ChatHook(PlayerInfo playerInfo, string message, ChatType chatType, SenderType senderType)
        {
            if(senderType == SenderType.Server && FilteSystemMessage)
            {
                return;
            }

            RichText richText1 = new RichText(DateTime.Now.ToString());
            RichText richText2 = new RichText();
            RichText richText3 = new RichText();
            switch (senderType)
            {
                case SenderType.Server:
                    richText2.content = " 'Server': ";
                    richText2.color = "#FF0000";
                    break;
                case SenderType.Player:
                    richText2.content = string.Format(" '{0}': ", playerInfo.PlayerName);
                    richText2.color = "#FFD700";
                    break;
            }
            richText3.content = message;

            _chatMessageViewService.AppendBlock(richText1, richText2, richText3);

            SaveChatMessage(richText2.content, message, playerInfo?.SteamID);
        }

        /// <summary>
        /// 保存聊天信息
        /// </summary>
        private static void SaveChatMessage(string name, string message, string steamID)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(DateTime.Now.ToString());
            stringBuilder.Append("   ");
            stringBuilder.Append(steamID);
            stringBuilder.Append(name);
            stringBuilder.Append(message);
            stringBuilder.Append(Environment.NewLine);

            try
            {
                System.IO.File.AppendAllText(System.Configuration.ConfigurationManager.AppSettings["ChatMessageRecordPath"],
                    stringBuilder.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Log.Error("保存聊天信息异常", ex);
            }            
        }
    }
}
