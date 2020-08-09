using IceCoffee.DbCore.Primitives;
using IceCoffee.DbCore.Primitives.Dto;
using LuoShuiTianYi.Sdtd.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuoShuiTianYi.Sdtd.Services.Dtos
{
    public class UserDto : DtoBaseGuid
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 上次登录IP
        /// </summary>
        public string LastLoginIP { get; set; }       
    }
}
