using IceCoffee.LogManager;
using IceCoffee.Network.CatchException;
using LuoShuiTianYi.Sdtd.Domain.Aggregates;
using LuoShuiTianYi.Sdtd.Services.Contracts;
using LuoShuiTianYi.Sdtd.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TianYiSdtdServerTools.Server.Sockets.Primitives;
using TianYiSdtdServerTools.Shared;
using TianYiSdtdServerTools.Shared.Models;
using TianYiSdtdServerTools.Shared.Models.NetDataObjects;
using TianYiSdtdServerTools.Shared.Primitives;

namespace TianYiSdtdServerTools.Server.Sockets.BusinessHandlers
{
    class LoginHandler : BusinessHandlerBase
    {
        private readonly IUserService _userService;

        private readonly IOnlineUserService _onlineUserService;

        private readonly IStandardAccountService _standardAccountService;

        private string _currentUserID;

        private bool _isAuthorized;

        private string _onlineUserToken;

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID => _currentUserID;

        /// <summary>
        /// 是否已经授权
        /// </summary>
        public bool IsAuthorized => _isAuthorized;

        /// <summary>
        /// UserOnlineToken对应OnlineUser的GUID
        /// </summary>
        public string OnlineUserToken => _onlineUserToken;

        public LoginHandler(TcpSession tcpSession,
            IUserService userService, IOnlineUserService onlineUserService,
            IStandardAccountService standardAccountService) : base(tcpSession)
        {
            _userService = userService;
            _onlineUserService = onlineUserService;
            _standardAccountService = standardAccountService;

            tcpSession.SessionClosed += OnTcpSession_SessionClosed;
        }

        private void OnTcpSession_SessionClosed(System.Net.Sockets.SocketError closedReason)
        {
            if (_isAuthorized && string.IsNullOrEmpty(_onlineUserToken) == false)
            {
                _onlineUserService.RemoveById(_onlineUserToken, "GUID");
            }

            _currentUserID = null;
            _isAuthorized = false;
            _onlineUserToken = null;
        }

        #region 登录
        public void RequestLogin(REQ_Login loginInfo)
        {
            _currentUserID = loginInfo.UserID;

            Log.Info("客户请求登录，来自IP：{0}，用户ID：{1}", tcpSession.RemoteIPEndPoint, _currentUserID);

            UpdateLevel updateLevel = VersionManager.CheckVersion(loginInfo.ClientVersion);

            if (updateLevel == UpdateLevel.Necessary)
            {
                SendAutoUpdaterConfig(SocketConfig.UpdateXmlUrl_complete);
                return;
            }
            else if (updateLevel == UpdateLevel.Optional)
            {
                SendAutoUpdaterConfig(SocketConfig.UpdateXmlUrl_patch);
                return;// 临时的
            }

            if(loginInfo.LoginType == LoginType.First)
            {
                FirstLogin(loginInfo);
            }
            else if(loginInfo.LoginType == LoginType.Reconnect)
            {
                Reconnect(loginInfo);
            }
        }

        private OnlineUserDto V_User2OnlineUser(V_User v_user)
        {
            if (v_user == null)
            {
                return null;
            }

            return new OnlineUserDto()
            {
                DisplayName = v_user.DisplayName,
                ExpiryTime = v_user.ExpiryTime,
                Fk_RoleID = v_user.RoleID,
                Fk_UserID = v_user.UserID,
                IPAddress = tcpSession.RemoteIPEndPoint.Address.ToString(),
                LoginTime = DateTime.Now,
                RoleName = v_user.RoleName
            };
        }

        private UserInfo V_User2UserInfo(V_User v_user)
        {
            if (v_user == null)
            {
                return null;
            }

            UserInfo userInfo = new UserInfo()
            {
                UserID = v_user.UserID,
                DisplayName = v_user.DisplayName,
                LastLoginIP = v_user.LastLoginIP,
                LastLoginTime = v_user.LastLoginTime.GetValueOrDefault(),
                RoleID = v_user.RoleID,
                RoleName = v_user.RoleName,
                ExpiryTime = v_user.ExpiryTime.GetValueOrDefault()
            };
            return userInfo;
        }

        /// <summary>
        /// 发送自动更新配置
        /// </summary>
        /// <param name="xmlUrl"></param>
        private void SendAutoUpdaterConfig(string xmlUrl)
        {
            tcpSession.Send(new RSP_AutoUpdaterConfig() { XmlUrl = xmlUrl });
        }

        private void FirstLogin(REQ_Login loginInfo)
        {
            bool isExist = _standardAccountService.GetRecordCount("Fk_UserID=@Fk_UserID and PasswordHash=@PasswordHash",
                new { Fk_UserID = _currentUserID, loginInfo.PasswordHash }) == 1;
            if (isExist == false)
            {
                LoginFailed(null, "用户名或密码错误，请重新输入", LoginType.First);
                return;
            }

            CheckOther(LoginType.First);
        }

        private void Reconnect(REQ_Login loginInfo)
        {
            if (loginInfo.IsAuthorized == false)
            {
                LoginFailed(null, "未授权或授权已过期，请重新登录", LoginType.Reconnect);
                return;
            }
            else
            {
                CheckOther(LoginType.Reconnect);
            }
        }

        private void CheckOther(LoginType loginType)
        {
            var v_user = _userService.GetByUserId(_currentUserID);
            
            if(v_user.RoleID == (int)Role.BlacklistUser)
            {
                LoginFailed(v_user, "您已被禁止登录", loginType);
                return;
            }

            if (v_user.ExpiryTime <= DateTime.Now)
            {
                LoginFailed(v_user, "会员到期，请续费后登录", loginType);
                return;
            }

            var onlineUsers = _onlineUserService.GetById(_currentUserID, "Fk_UserID");
            int count = onlineUsers.Count;
            if(count == 0)
            {
            }
            else if (count < TcpServer.UserMaxLoginCount)
            {
                string ipAddress = onlineUsers.First().IPAddress;
                try
                {
                    IPAddress ip1 = IPAddress.Parse(ipAddress);
                    IPAddress ip2 = tcpSession.RemoteIPEndPoint.Address;
                    if (ip1.Equals(ip2) == false)
                    {
                        LoginFailed(v_user, "您已在其他设备登录了工具", loginType);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    throw new NetworkException("解析数据库已保存的IP地址错误，ip：" + ipAddress, ex);
                }
            }
            else if (count >= TcpServer.UserMaxLoginCount)
            {
                LoginFailed(v_user, string.Format("您最多在同一设备上登录 {0} 个相同账号", TcpServer.UserMaxLoginCount), loginType);
                return;
            }

            OnlineUserDto newOnlineUser = V_User2OnlineUser(v_user);
            newOnlineUser.Key = Guid.NewGuid().ToString();
            _onlineUserToken = newOnlineUser.Key;
            _onlineUserService.Add(newOnlineUser);
            LoginSuccess(v_user, loginType);
        }

        private void LoginSuccess(V_User v_user, LoginType loginType)
        {
            RSP_LoginResult loginResult = new RSP_LoginResult() 
            { 
                UserInfo = V_User2UserInfo(v_user),
                IsPopMessageBox = false,
                IsSuccess = true,
                LoginType = loginType,
                Message = "登录成功"
            };

            _userService.UpdateAny("LastLoginTime=@LastLoginTime,LastLoginIP=@LastLoginIP",
                "UserID=@UserID", new
                {
                    LastLoginTime = DateTime.Now,
                    LastLoginIP = tcpSession.RemoteIPEndPoint.Address.ToString(),
                    UserID = _currentUserID
                });

            tcpSession.Send(loginResult);

            _isAuthorized = true;

            Log.Info("客户登录成功，来自IP：{0}，用户ID：{1}", tcpSession.RemoteIPEndPoint, _currentUserID);
        }

        private void LoginFailed(V_User v_user, string message, LoginType loginType)
        {
            RSP_LoginResult loginResult = new RSP_LoginResult()
            {
                UserInfo = V_User2UserInfo(v_user),
                IsPopMessageBox = true,
                IsSuccess = false,
                LoginType = loginType,
                Message = message
            };

            tcpSession.Send(loginResult);

            if (loginType == LoginType.Reconnect)
            {
                CloseClient();
            }
        }
        #endregion

        #region 注册
        public void RegisterAccount(REQ_RegisterAccount account)
        {
            Log.Info("客户请求注册账号，来自IP：{0}，注册UserID：{1}", tcpSession.RemoteIPEndPoint, account.UserID);

            if (account.RegisterAccountType == RegisterAccountType.StandardAccount)
            {
                bool result = false;
                try
                {
                    result = _userService.InsertStandardAccount(
                        new SP_InsertStandardAccount_Params(account.UserID, account.PasswordHash,
                            account.DisplayName, DateTime.Now.AddDays(TcpServer.LoginGiveUseTime), (int)Role.NormalUser));
                    if (result == false)
                    {
                        goto RegisterFailed;
                    }
                    else
                    {
                        Log.Info("客户请求注册账号成功，来自IP：{0}，注册UserID：{1}", tcpSession.RemoteIPEndPoint, account.UserID);
                        PopMessageBox("注册成功！");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    goto RegisterFailed;
                }

            RegisterFailed:
                Log.Error("客户请求注册账号失败，来自IP：{0}，注册UserID：{1}", tcpSession.RemoteIPEndPoint, account.UserID);
                PopMessageBox("注册失败！");
            }
            // 预留QQ账户
        }
        #endregion
    }
}
