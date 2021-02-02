using IceCoffee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using TianYiSdtdServerTools.Client.Models.Chat;
using TianYiSdtdServerTools.Client.Services.UI;
using TianYiSdtdServerTools.Client.Views.PartialViews.ControlPanel;

namespace TianYiSdtdServerTools.Client.Views.Services
{
    public class RichTextBoxService : IRichTextBoxService
    {
        private const int maximumBlockCount = 500;

        private readonly RichTextBox _richTextBox;

        public RichTextBoxService(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
            _richTextBox.Document.Blocks.Clear();
        }

        public void AppendBlock(params RichText[] richTexts)
        {
            AppendBlock((IEnumerable<RichText>)richTexts);
        }

        public void AppendBlock(IEnumerable<RichText> richTexts)
        {            
            Application.Current?.Dispatcher.InvokeAsync(() =>
            {
                BrushConverter brushConverter = new BrushConverter();
                FontFamilyConverter fontFamilyConverter = new FontFamilyConverter();

                Paragraph para = new Paragraph();

                foreach (var item in richTexts)
                {
                    Brush foreground = null;
                    try
                    {
                        foreground = (Brush)brushConverter.ConvertFromString(item.color);
                    }
                    catch
                    {
                    }

                    Run run = new Run(item.content);

                    if (foreground != null)
                    {
                        run.Foreground = foreground;
                    }

                    para.Inlines.Add(run);
                }

                _richTextBox.BeginChange();
                while (_richTextBox.Document.Blocks.Count > maximumBlockCount)
                {
                    _richTextBox.Document.Blocks.Remove(_richTextBox.Document.Blocks.FirstBlock);
                }

                _richTextBox.Document.Blocks.Add(para);
                _richTextBox.EndChange();

                // 如果控件可见并且没有选中文本，则将编辑控件的视图滚动到内容的末尾。
                if (_richTextBox.IsVisible && _richTextBox.IsSelectionActive == false)
                {
                    _richTextBox.ScrollToEnd();
                }
            });
        }

        public void AppendPlainText(string plainText)
        {
            Application.Current?.Dispatcher.InvokeAsync(() =>
            {
                Paragraph paragraph = new Paragraph(new Run(plainText));

                _richTextBox.BeginChange();
                while (_richTextBox.Document.Blocks.Count > maximumBlockCount)
                {
                    _richTextBox.Document.Blocks.Remove(_richTextBox.Document.Blocks.FirstBlock);
                }

                _richTextBox.Document.Blocks.Add(paragraph);
                _richTextBox.EndChange();

                // 如果控件可见，则将编辑控件的视图滚动到内容的末尾。
                if (_richTextBox.IsVisible && _richTextBox.IsSelectionActive == false)
                {
                    _richTextBox.ScrollToEnd();
                }
            });
        }
    }
}
