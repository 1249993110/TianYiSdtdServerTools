using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IceCoffee.DbCore.Utils;
using System.Configuration;
using System.IO;
using IceCoffee.DbCore;
using IceCoffee.Common.LogManager;
using IceCoffee.DbCore.Domain;

namespace TianYiSdtdServerTools.Client.Services
{
    public static class Database
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            Task.Run(() =>
            {
                try
                {
                    string sql = File.ReadAllText(ConfigurationManager.AppSettings["SQL"]);

                    DBHelper.ExecuteSQlite(new DbConnectionInfo(DefaultDbConnectionString, DatabaseType.SQLite), sql);
                }
                catch (Exception e)
                {
                    Log.Error("初始化SQLite数据库错误", e);
                }
            });
        }

        /// <summary>
        /// 默认数据库连接串
        /// </summary>
        internal static readonly string DefaultDbConnectionString = ConfigurationManager.ConnectionStrings["DefaultSQLiteString"].ConnectionString;
    }
}
