using IceCoffee.DbCore.Primitives.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuoShuiTianYi.Sdtd.Services.Dtos
{
    public class OnlineUserDto : DtoBaseGuid
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Fk_UserID { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 使用期限
        /// </summary>
        public DateTime? ExpiryTime { get; set; }

        /// <summary>
        /// 用户角色ID
        /// </summary>
        public int Fk_RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
}
