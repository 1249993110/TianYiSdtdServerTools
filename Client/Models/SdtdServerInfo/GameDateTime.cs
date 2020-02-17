using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.SdtdServerInfo
{
    /// <summary>
    /// 游戏日期时间
    /// </summary>
    public class GameDateTime
    {
        /// <summary>
        /// 分钟
        /// </summary>
        public int Minute { get; set; }

        /// <summary>
        /// 小时
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// 游戏进行天数
        /// </summary>
        public int Day { get; set; }


        public override string ToString()
        {
            return string.Format("Day {0}, {1:D2}:{2:D2}", Day.ToString(), Hour.ToString(), Minute.ToString());
        }
    }
}
