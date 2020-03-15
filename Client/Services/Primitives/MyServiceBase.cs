﻿using IceCoffee.DbCore;
using IceCoffee.DbCore.Primitives;
using IceCoffee.DbCore.Repositorys;
using TianYiSdtdServerTools.Client.Services.CatchException;

namespace TianYiSdtdServerTools.Client.Services.Primitives
{
    public abstract class MyServiceBase<TEntity> : ServiceBase<TEntity>, IExceptionCaughtSignal where TEntity : class, IEntityBase
    {
        public MyServiceBase(bool useConnectionPool)
            : base(new SQLiteRepository<TEntity>(Database.DefaultDbConnectionString, useConnectionPool))
        {
        }

        /// <summary>
        /// 捕获DAL异常
        /// </summary>
        public static event ExceptionCaughtEventHandler ExceptionCaught;

        void IExceptionCaughtSignal.EmitExceptionCaughtSignal(object sender, ServiceException e)
        {
            ExceptionCaught?.Invoke(this, e);
        }
    }
}