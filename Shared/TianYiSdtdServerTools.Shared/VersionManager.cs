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
        private static readonly Version _currentVersion;

        public static Version CurrentVersion => _currentVersion;

        static VersionManager()
        {
            _currentVersion = new Version(4, 0, 0, 0);
        }

        /// <summary>
        /// 检查客户端版本
        /// </summary>
        /// <param name="versionInfo"></param>
        /// <returns></returns>
        public static UpdateLevel CheckVersion(Version versionInfo)
        {
            if (_currentVersion == versionInfo)
            {
                return UpdateLevel.Unnecessary;
            }
            else if (versionInfo.Major < _currentVersion.Major)
            {
                return UpdateLevel.Necessary;
            }
            else if (versionInfo.Major == _currentVersion.Major && versionInfo.Minor < _currentVersion.Minor)
            {
                return UpdateLevel.Necessary;
            }
            else
            {
                return UpdateLevel.Optional;
            }
        }
    }
}
