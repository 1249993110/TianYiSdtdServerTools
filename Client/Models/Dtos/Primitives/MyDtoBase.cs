using IceCoffee.Common;
using System;
using TianYiSdtdServerTools.Client.Models.Entitys.Primitives;
using IceCoffee.Wpf.MvvmFrame;
using System.Reflection;
using System.Collections.Generic;

namespace TianYiSdtdServerTools.Client.Models.Dtos.Primitives
{
    public abstract class MyDtoBase<TDto, TEntity>
        where TDto : MyDtoBase<TDto, TEntity>, new() 
        where TEntity : MyEntityBase<TEntity, TDto>, new()
    {
        /// <summary>
        /// 转换为实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual TEntity ToEntity()
        {
            TEntity entity = ObjectClone<TDto, TEntity>.ShallowCopy(this as TDto);
            return entity;
        }

        /// <summary>
        /// 创建一个实体，包括UUID、CreateDateTime等信息
        /// </summary>
        /// <returns></returns>
        public virtual TEntity CreateEntity()
        {
            TEntity entity = ObjectClone<TDto, TEntity>.ShallowCopy(this as TDto);
            entity.UUID = Guid.NewGuid().ToString();
            entity.CreateDateTime = DateTime.Now.ToString();
            return entity;
        }

        private static string[] _columnNames;

        /// <summary>
        /// 当前Dto的所有列
        /// </summary>
        public virtual string[] ColumnNames
        {
            get
            {
                if (_columnNames == null)
                {
                    PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    List<string> db_TableColumnNames = new List<string>();
                    foreach (PropertyInfo pi in properties)
                    {
                        if (pi.CanWrite)
                        {
                            db_TableColumnNames.Add(pi.Name);
                        }
                    }
                    _columnNames = db_TableColumnNames.ToArray();
                }
                return _columnNames;
            }
        }
    }
}
