using IceCoffee.DbCore.Primitives.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuoShuiTianYi.Sdtd.Services.Dtos
{
    public class StandardAccountDto : DtoBaseGuid
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
