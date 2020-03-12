using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.Entitys.Primitives
{
    public interface IMyEntityBase
    {
        /// <summary>
        /// GUID
        /// </summary>
        string UUID { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        string CreateDateTime { get; set; }
    }
}
