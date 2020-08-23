using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.ViewModels.ToolDialog;

namespace TianYiSdtdServerTools.Client.Views.PartialViews.ToolDialog
{
    public class BlockView : ItemView
    {
        public new BlockViewModel ViewModel { get; set; }
        
        public BlockView() : base(false)
        {
            InitializeComponent();
            ViewModel = IocContainer.Resolve<BlockViewModel>();
            base.DataContext = ViewModel;
        }
    }
}
