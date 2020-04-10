using IceCoffee.Common;
using IceCoffee.DbCore.Primitives;
using System;
using System.Collections.Generic;
using System.Reflection;
using TianYiSdtdServerTools.Client.Models.Dtos.Primitives;

namespace TianYiSdtdServerTools.Client.Models.Entitys.Primitives
{
    public abstract class MyEntityBase<TEntity, TDto> : EntityBase, IMyEntityBase 
        where TEntity : MyEntityBase<TEntity, TDto>, new() 
        where TDto : MyDtoBase<TDto, TEntity>, new()
    {
        public static string _default_Db_TableName;
        public static string Default_Db_TableName
        {
            get
            {
                if (_default_Db_TableName == null)
                {
                    _default_Db_TableName = typeof(TEntity).Name;
                }
                return _default_Db_TableName;
            }
        }

        public static string[] _default_Db_TableColumnNames;       
        public static string[] Default_Db_TableColumnNames
        {
            get
            {
                if (_default_Db_TableColumnNames == null)
                {
                    PropertyInfo[] properties = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    List<string> db_TableColumnNames = new List<string>();
                    foreach (PropertyInfo pi in properties)
                    {
                        if (pi.CanWrite)
                        {
                            db_TableColumnNames.Add(pi.Name);
                        }
                    }
                    _default_Db_TableColumnNames = db_TableColumnNames.ToArray();
                }
                return _default_Db_TableColumnNames;
            }
        }

        public override string Db_TableName { get { return Default_Db_TableName; } }

        public override string[] Db_TableColumnNames { get { return Default_Db_TableColumnNames; } }


        public static string KeyName { get { return nameof(UUID); } }

        /// <summary>
        /// GUID
        /// </summary>
        public string UUID { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateDateTime { get; set; }


        /// <summary>
        /// 转换为数据传输对象
        /// </summary>
        /// <returns></returns>
        public virtual TDto ToDto()
        {
            TDto dto = ObjectClone<TEntity, TDto>.ShallowCopy(this as TEntity);
            return dto;
        }

        /// <summary>
        /// 创建一个实体，包括UUID、CreateDateTime等信息
        /// </summary>
        public static TEntity Create()
        {
            TEntity entity = new TEntity
            {
                UUID = Guid.NewGuid().ToString(),
                CreateDateTime = DateTime.Now.ToString()
            };
            return entity;            
        }
    }
}
