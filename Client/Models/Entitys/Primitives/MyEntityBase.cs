using IceCoffee.Common;
using IceCoffee.DbCore.Primitives;
using System;
using TianYiSdtdServerTools.Client.Models.Dtos.Primitives;

namespace TianYiSdtdServerTools.Client.Models.Entitys.Primitives
{
    public abstract class MyEntityBase<TEntity, TDto> : EntityBase, IMyEntityBase 
        where TEntity : MyEntityBase<TEntity, TDto>, new() 
        where TDto : MyDtoBase<TDto, TEntity>, new()
    {
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
            return ObjectClone<TEntity, TDto>.ShallowCopy(this as TEntity);
        }

    }
}
