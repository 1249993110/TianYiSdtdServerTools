using IceCoffee.Common.LogManager;
using IceCoffee.DbCore.CatchServiceException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TianYiSdtdServerTools.Client.Services.UI;

namespace TianYiSdtdServerTools.Client.ViewModels.Utils
{
    /// <summary>
    /// 异常处理助手
    /// </summary>
    public static class ExceptionHandleHelper
    {
        public static void ShowServiceException(IDialogService dialogService, ServiceException ex)
        {
            StringBuilder messageBuilder = new StringBuilder();
            Exception exception = ex.InnerException;
            while (exception != null)
            {
                messageBuilder.Append(exception.Message + Environment.NewLine);
                exception = exception.InnerException;
            }

            Log.Error(ex, ex.Message);

            if (SynchronizationContext.Current != null)// 如果为UI线程发起
            {
                dialogService.ShowInformation(messageBuilder.ToString(), ex.Message);
            }
        }
    }
}
