using IceCoffee.Common;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using IceCoffee.Wpf.MvvmFrame.Utils;
using System;
using System.Timers;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;
using TianYiSdtdServerTools.Client.TelnetClient;
using TianYiSdtdServerTools.Client.ViewModels.Primitives;

namespace TianYiSdtdServerTools.Client.ViewModels.FunctionPanel
{
    public class GameNoticeViewModel : FunctionViewModelBase
    {
        private PropertyObserver<GameNoticeViewModel> _currentViewModelObserver;

        /// <summary>
        /// 欢迎公告
        /// </summary>
        [ConfigNode(System.Xml.XmlNodeType.Attribute)]
        public string WelcomeNotice { get; [NPCA_Method]set; }

        /// <summary>
        /// 轮播公告
        /// </summary>
        [ConfigNode(System.Xml.XmlNodeType.Attribute)]
        public string AlternateNotice { get; [NPCA_Method]set; }

        /// <summary>
        /// 轮播周期
        /// </summary>
        [ConfigNode(System.Xml.XmlNodeType.Attribute)]
        public int AlternateInterval { get; [NPCA_Method]set; } = 300;

        public RelayCommand SendWelcomeNotice { get; private set; }

        public RelayCommand SendAlternateNotice { get; private set; }

        private Timer Timer { get; set; }

        public GameNoticeViewModel(IDispatcherService dispatcherService, string functionTag) : base(dispatcherService, functionTag)
        {
            SendWelcomeNotice = new RelayCommand(()=>
            {
                FormatAlternate(WelcomeNotice);
            });

            SendAlternateNotice = new RelayCommand(() =>
            {
                FormatAlternate(AlternateNotice);
            });
        }

        private static void FormatAlternate(string msg)
        {            
            foreach (var item in msg.Split(Environment.NewLine))
            {
                SdtdConsole.Instance.SendGlobalMessage(item);
            }
        }

        private void OnPlayerEnterGame(Models.Players.PlayerInfo playerInfo)
        {
            foreach (var item in WelcomeNotice.Split(Environment.NewLine))
            {
                SdtdConsole.Instance.SendMessageToPlayer(playerInfo.SteamID, item);
            }            
        }

        private void OnTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SendAlternateNotice.Execute(AlternateNotice);
        }

        protected override void DisableFunction()
        {
            Timer.Stop();
            SdtdConsole.Instance.PlayerEnterGame -= OnPlayerEnterGame;
        }

        protected override void EnableFunction()
        {
            if(Timer == null)
            {
                Timer = new Timer() { AutoReset = true, Interval = AlternateInterval * 1000 };
                Timer.Elapsed += OnTimer_Elapsed;

                _currentViewModelObserver = new PropertyObserver<GameNoticeViewModel>(this);
                _currentViewModelObserver.RegisterHandler(currentViewModel => currentViewModel.AlternateInterval,
                    (propertySource) =>
                    {
                        if (propertySource.AlternateInterval > 0)
                        {
                            Timer.Interval = propertySource.AlternateInterval * 1000;
                        }
                    });
            }

            Timer.Start();
            SdtdConsole.Instance.PlayerEnterGame -= OnPlayerEnterGame;
            SdtdConsole.Instance.PlayerEnterGame += OnPlayerEnterGame;
        }
    }
}
