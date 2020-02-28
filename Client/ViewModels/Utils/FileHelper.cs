using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;

namespace TianYiSdtdServerTools.Client.ViewModels.Utils
{
    public static class FileHelper
    {
        /// <summary>
        /// 通过记事本打开文本文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static void OpenTextFileByNotepad(IDialogService dialogService, string fileName)
        {
            if(System.IO.File.Exists(fileName) == false)
            {
                dialogService.ShowInformation("暂无记录");
            }
            else
            {
                try
                {
                    System.Diagnostics.Process.Start("notepad.exe", fileName);
                }
                catch (Exception ex)
                {
                    dialogService.ShowInformation(ex.Message);
                }
            }
        }
    }
}
