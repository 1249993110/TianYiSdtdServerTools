using IceCoffee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Shared.Models
{
    [Serializable]
    public class VersionInfo
    {
        /// <summary>
        /// 主版本号
        /// </summary>
        public int Major { get; set; }

        /// <summary>
        /// 次版本号
        /// </summary>
        public int Minor { get; set; }

        /// <summary>
        /// 内部版本号
        /// </summary>
        public int Build { get; set; }

        /// <summary>
        /// 修订版本号
        /// </summary>
        public int Revision { get; set; }

        public VersionInfo(int major, int minor, int build, int revision)
        {
            Major = major;
            Minor = minor;
            Build = build;
            Revision = revision;
        }

        public static bool operator ==(VersionInfo v1, VersionInfo v2)
        {
            if (v1.Major == v2.Major && v1.Minor == v2.Minor && v1.Build == v2.Build && v1.Revision == v2.Revision)
                return true;
            return false;
        }

        public static bool operator !=(VersionInfo v1, VersionInfo v2)
        {
            return !(v1 == v2);
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", Major, Minor, Build, Revision);
        }

        public static VersionInfo Parse(string parm)
        {
            string[] vs = parm.Split('.');
            return new VersionInfo(vs[0].ToInt(), vs[1].ToInt(), vs[2].ToInt(), vs[3].ToInt());
        }
    }
}
