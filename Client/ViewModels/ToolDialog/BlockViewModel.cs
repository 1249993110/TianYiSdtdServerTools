using IceCoffee.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Services.UI;

namespace TianYiSdtdServerTools.Client.ViewModels.ToolDialog
{
    public class BlockViewModel : ItemViewModel
    {
        public override string XmlPath
        {
            get { return CommonHelper.GetAppSettings("BlocksXmlPath"); }
        }

        public BlockViewModel(IDispatcherService dispatcherService, IDialogService dialogService) : base(dispatcherService, dialogService)
        {
        }
    }
}
