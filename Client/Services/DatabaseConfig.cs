using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.DbCore.Utils;
using System.Configuration;
using System.IO;
using IceCoffee.DbCore;
using IceCoffee.LogManager;
using IceCoffee.DbCore.Domain;
using IceCoffee.Common;

namespace TianYiSdtdServerTools.Client.Services
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        public static void InitializeDatabase()
        {
            Task.Run(() =>
            {
                try
                {
                    string sql = File.ReadAllText(CommonHelper.GetAppSettings("SQL"));

                    DBHelper.ExecuteSQlite(DefaultSQLiteConnectionInfo, sql);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "初始化SQLite数据库错误");
                }
            });
        }

        /// <summary>
        /// 默认SQLite数据库连接信息
        /// </summary>
        internal static readonly DbConnectionInfo DefaultSQLiteConnectionInfo = new DbConnectionInfo("DefaultSQLiteConnection");
    }
}
