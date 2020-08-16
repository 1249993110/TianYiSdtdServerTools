using IceCoffee.DbCore.Primitives.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuoShuiTianYi.Sdtd.Domain.Entitys
{
    public class T_StandardAccount : EntityBaseGuid
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Fk_UserID { get; set; }

        /// <summary>
        /// 密码哈希值
        /// </summary>
        public string PasswordHash { get; set; }
    }
}
