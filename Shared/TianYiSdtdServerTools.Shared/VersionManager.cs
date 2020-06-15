using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Shared.Models;

namespace TianYiSdtdServerTools.Shared
{
    public enum UpdateLevel
    {
        /// <summary>
        /// 无需更新
        /// </summary>
        Unnecessary,

        /// <summary>
        /// 可以选择更新
        /// </summary>
        Optional,

        /// <summary>
        /// 必须更新
        /// </summary>
        Necessary
    }

    public static class VersionManager
    {
        public static VersionInfo CurrentVersion { get; set; }

        static VersionManager()
        {
            CurrentVersion = new VersionInfo(4, 0, 0, 0);
        }

        /// <summary>
        /// 检查客户端版本
        /// </summary>
        /// <param name="versionInfo"></param>
        /// <returns></returns>
        public static UpdateLevel CheckVersion(VersionInfo versionInfo)
        {
            if (CurrentVersion == versionInfo)
            {
                return UpdateLevel.Unnecessary;
            }
            else if (versionInfo.Major < CurrentVersion.Major)
            {
                return UpdateLevel.Necessary;
            }
            else if (versionInfo.Major == CurrentVersion.Major && versionInfo.Minor < CurrentVersion.Minor)
            {
                return UpdateLevel.Necessary;
            }
            return UpdateLevel.Optional;
        }
    }
}
