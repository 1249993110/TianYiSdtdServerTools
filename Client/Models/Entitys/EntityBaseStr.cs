using IceCoffee.DbCore.OptionalAttributes;
using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.Entitys
{
    /// <summary>
    /// EntityBase泛型参数为string的默认实现
    /// </summary>
    public abstract class EntityBaseStr : EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey, Column("GUID"), IgnoreUpdate]
        public virtual string Key { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [IgnoreUpdate, IgnoreInsert]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 初始化，默认生成主键，不生成创建日期，应由数据库生成
        /// </summary>
        public override object Init()
        {
            Key = Guid.NewGuid().ToString();
            // CreatedDate = DateTime.Now;
            return this;
        }
    }
}
