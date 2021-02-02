using IceCoffee.Common;
using IceCoffee.LogManager;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.NotifyPropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TianYiSdtdServerTools.Client.Models;
using TianYiSdtdServerTools.Client.Models.ObservableObjects;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.ViewModels.Managers;

namespace TianYiSdtdServerTools.Client.ViewModels.ToolDialog
{
    public class ItemViewModel : ObservableObject
    {
        private readonly PropertyObserver<ItemViewModel> currentPropertyObserver;

        private readonly IDialogService _dialogService;

        private readonly IDispatcherService _dispatcherService;

        private string _xmlPath = CommonHelper.GetAppSettings("ItemsXmlPath");

        public virtual string XmlPath
        {
            get { return _xmlPath; }
        }

        public ObservableCollection<ColoredImageData> ColoredImageDatas { get; set; }

        public ColoredImageData SelectedItem { get; [NPCA_Method]set; }

        public string SearchText { get; [NPCA_Method]set; }

        public RelayCommand Search { get; private set; }
        
        public ItemViewModel(IDispatcherService dispatcherService, IDialogService dialogService)
        {
            _dispatcherService = dispatcherService;
            _dialogService = dialogService;

            ColoredImageDatas = new ObservableCollection<ColoredImageData>();

            Search = new RelayCommand(PrivateSearch);

            currentPropertyObserver = new PropertyObserver<ItemViewModel>(this);
            currentPropertyObserver.RegisterHandler(p => p.SearchText, (vm) => { vm.PrivateSearch(); });

            Task.Run(() =>
            {
                try
                {
                    LoadXml(ColoredImageDatas, XmlPath);
                }
                catch (Exception ex)
                {
                    string title = string.Format("加载 {0} 失败", XmlPath);

                    Log.Error(ex, title);

                    _dispatcherService.InvokeAsync(() =>
                    {                        
                        _dialogService.ShowInformation(ex.Message, title);
                    });
                }
            });

        }

        private void PrivateSearch()
        {
            Task.Run(() =>
            {
                if (string.IsNullOrEmpty(SearchText))
                {
                    foreach (var item in ColoredImageDatas)
                    {
                        item.Visible = true;
                    }
                }
                else
                {
                    foreach (var item in ColoredImageDatas)
                    {
                        if (item.Chinese.Contains(SearchText))
                        {
                            item.MatchText = SearchText;
                            item.Visible = true;
                        }
                        else if (item.English.Contains(SearchText))
                        {
                            item.MatchText = null;
                            item.Visible = true;
                        }
                        else
                        {
                            item.MatchText = null;
                            item.Visible = false;
                        }
                    }
                }
            });
        }

        private void LoadXml(ObservableCollection<ColoredImageData> imageDatas, string path)
        {
            string itemIconsDirectory = CommonHelper.GetAppSettings("ItemIconsDirectory").Replace("/", "\\");

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode baseNode = doc.SelectSingleNode(doc.DocumentElement.Name);

            foreach (XmlNode node in baseNode.ChildNodes)
            {
                if (node is XmlElement)
                {
                    string itemName = node.Attributes["name"]?.Value;

                    string customIcon = (node.SelectSingleNode("property[@name='CustomIcon']") as XmlElement)?.GetAttribute("value");

                    string uriStr = itemIconsDirectory + "\\" + (customIcon ?? itemName) + ".png";

                    if (SdtdLocalizationManager.Instance.LocalizationDict.TryGetValue(itemName, out string chinese)
                        && System.IO.File.Exists(uriStr))
                    {
                        string customIconTint = (node.SelectSingleNode("property[@name='CustomIconTint']") as XmlElement)?.GetAttribute("value");

                        ColoredImageData coloredImageData = new ColoredImageData()
                        {
                            Source = new Uri(uriStr, UriKind.Relative),
                            HexColor = customIconTint ?? null,
                            ToolTip = itemName + Environment.NewLine + chinese,
                            English = itemName,
                            Chinese = chinese
                        };

                        _dispatcherService.InvokeAsync(() =>
                        {
                            imageDatas.Add(coloredImageData);
                        }, DispatcherPriority.ApplicationIdle);
                    }
                }
            }
        }
    }
}
