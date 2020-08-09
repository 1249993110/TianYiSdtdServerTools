using IceCoffee.Common.LogManager;
using LuoShuiTianYi.Sdtd.Domain.Aggregates;
using LuoShuiTianYi.Sdtd.Services.Contracts;
using LuoShuiTianYi.Sdtd.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TianYiSdtdServerTools.Server.Sockets.Primitives;
using TianYiSdtdServerTools.Shared;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Server.Sockets.BusinessHandlers
{
    class LoginHandler : BusinessHandlerBase
    {
        private readonly IUserService _userService;

        private readonly IOnlineUserService _onlineUserService;

        public LoginHandler(TcpSession tcpSession,
            IUserService userService, IOnlineUserService onlineUserService) : base(tcpSession)
        {
            _userService = userService;
            _onlineUserService = onlineUserService;
        }

        public void RequestLogin(REQ_ClientInfo clientInfo)
        {
            string userID = clientInfo.UserID;

            string clientIPEndPoint = tcpSession.clientIPEndPoint;

            Log.Info("客户请求登录，来自IP：{0}，用户ID：{1}", clientIPEndPoint, userID);

            if (VersionManager.CheckVersion(clientInfo.ClientVersion) == UpdateLevel.Necessary)
            {
                SendAutoUpdaterConfig(SocketConfig.UpdateXmlUrl_complete);
                return;
            }
            else if (VersionManager.CheckVersion(clientInfo.ClientVersion) == UpdateLevel.Optional)
            {
                SendAutoUpdaterConfig(SocketConfig.UpdateXmlUrl_patch);
            }

            var v_user = _userService.GetByUserId(userID);

            if (clientInfo.IsAuthorized)
            {
                var onlineUser = _onlineUserService.GetById(userID, "UserID").FirstOrDefault();
                if(onlineUser == null)
                {
                    PopDialogueBox("登录超时，请重新登录");
                    CloseClient();
                    return;
                }
                else
                {
                    _onlineUserService.Update(User2OnlineUser(v_user));
                }
            }
            else
            {
                _onlineUserService.Insert(User2OnlineUser(v_user));
            }

            SendUserInfo(v_user);

            _userService.UpdateAny("LastLoginTime=@LastLoginTime,LastLoginIP=@LastLoginIP",
                "UserID=@UserID", new 
                { 
                    LastLoginTime = DateTime.Now, 
                    LastLoginIP = clientIPEndPoint 
                });
        }

        private OnlineUserDto User2OnlineUser(V_User user)
        {
            return new OnlineUserDto()
            {
                DisplayName = user.DisplayName,
                ExpiryTime = user.ExpiryTime,
                Fk_RoleID = user.RoleID,
                Fk_UserID = user.UserID,
                IPAddress = tcpSession.clientIPEndPoint,
                LoginTime = DateTime.Now,
                RoleName = user.RoleName
            };
        }

        /// <summary>
        /// 发送自动更新配置
        /// </summary>
        /// <param name="xmlUrl"></param>
        private void SendAutoUpdaterConfig(string xmlUrl)
        {
            tcpSession.Send(new RSP_AutoUpdaterConfig() { XmlUrl = xmlUrl });

        }

        private void SendUserInfo(V_User user)
        {
            var result = new RSP_ClientInfo()
            {
                UserID = user.UserID,
                DisplayName = user.DisplayName,
                LastLoginIP = user.LastLoginIP,
                LastLoginTime = user.LastLoginTime.GetValueOrDefault(),
                RoleID = user.RoleID,
                RoleName = user.RoleName,
                ExpiryTime = user.ExpiryTime.GetValueOrDefault()
            };
            tcpSession.Send(result);
        }
    }
}
