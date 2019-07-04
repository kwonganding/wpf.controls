using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Utility.Security
{
    /// <summary>
    /// windows程序的安全服务，主要是登录信息的管理
    /// </summary>
    public class WinSecurityService
    {
        #region Properties

        #region UserId：当前用户编号
        /// <summary>
        /// UserId：当前用户编号
        /// </summary>
        public static string UserId
        {
            get
            {
                return UserInfo.UserId;
            }
        }
        #endregion

        #region UserName：当前用户名
        /// <summary>
        /// 当前用户名
        /// </summary>
        public static string UserName
        {
            get
            {
                return UserInfo.UserName;
            }
        }
        #endregion

        #region RoleId：当前用户角色编号
        /// <summary>
        /// 当前用户角色编号
        /// </summary>
        public static int RoleId
        {
            get
            {
                return UserInfo.RoleId;
            }
        }
        #endregion

        #region UserInfo：当前用户信息
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public static UserInfo UserInfo { get; private set; }
        #endregion

        #region IsAuthenticated：当前用户的是否通过身份验证
        /// <summary>
        /// 是否通过身份验证
        /// </summary>
        public static bool IsAuthenticated = false;
        #endregion

        #endregion

        static WinSecurityService()
        {
            WinSecurityService.UserInfo = new UserInfo();
        }

        #region SignIn：登陆保存票据信息
        /// <summary>
        /// 登陆保存票据信息
        /// </summary>
        public static void SignIn(string userId, string userName, int roleId = 0)
        {
            UserInfo.UserId = userId;
            UserInfo.UserName = userName;
            UserInfo.RoleId = roleId;
            IsAuthenticated = true;
        }
        #endregion

        #region SignOut：退出登陆
        /// <summary>
        /// 退出登陆，清除身份票据信息
        /// </summary>
        public static void SignOut()
        {
            if (IsAuthenticated)
            {
                UserInfo.UserId = string.Empty;
                UserInfo.UserName = string.Empty;
                UserInfo.RoleId = 0;
                IsAuthenticated = false;
            }
        }
        #endregion
    }
}
