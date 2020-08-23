using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TianYiSdtdServerTools.Client.Services.UI;

namespace TianYiSdtdServerTools.Client.Views.Services
{
    public class PlainTextBoxService : IPlainTextBoxService
    {
        private readonly TextBox _textBox;

        public PlainTextBoxService(TextBox textBox)
        {
            _textBox = textBox;
        }

        public void AppendPlainText(string plainText)
        {
            Application.Current?.Dispatcher.InvokeAsync(() =>
            {
                if (_textBox.Text.Length > 40960)
                {
                    _textBox.Text.Remove(0, 4096);
                }

                _textBox.AppendText(plainText);

                // 如果控件可见，则将编辑控件的视图滚动到内容的末尾。SelectedText
                if (_textBox.IsVisible && _textBox.IsSelectionActive == false)
                {
                    _textBox.ScrollToEnd();
                }
            });
        }
    }
}
