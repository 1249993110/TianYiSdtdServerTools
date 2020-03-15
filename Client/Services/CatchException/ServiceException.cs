using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.Common;

namespace TianYiSdtdServerTools.Client.Services.CatchException
{   
    public class ServiceException : CustomExceptionBase
    {
        public ServiceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }

    public delegate void ExceptionCaughtEventHandler(object sender, ServiceException e);
}
