using IceCoffee.DbCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuoShuiTianYi.Sdtd.Services.Primitives
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    internal static class DatabaseConfig
    {
        /// <summary>
        /// 默认数据库连接信息
        /// </summary>
        internal static readonly DbConnectionInfo DefaultConnectionInfo;


        static DatabaseConfig()
        {
            DefaultConnectionInfo = new DbConnectionInfo("DefaultConnection");

        }
    }
}
