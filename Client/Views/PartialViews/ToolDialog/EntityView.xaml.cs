﻿using System;
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
using TianYiSdtdServerTools.Client.ViewModels.Primitives;
using TianYiSdtdServerTools.Client.ViewModels.ToolDialog;

namespace TianYiSdtdServerTools.Client.Views.PartialViews.ToolDialog
{
    /// <summary>
    /// EntityView.xaml 的交互逻辑
    /// </summary>
    public partial class EntityView : UserControl
    {
        public EntityViewModel ViewModel { get; set; }
        public EntityView()
        {
            InitializeComponent();
            ViewModel = IocContainer.Resolve<EntityViewModel>();
            this.DataContext = ViewModel;
        }

    }
}
