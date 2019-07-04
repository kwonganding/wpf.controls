using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Principal;
using System.Web.Security;

namespace System.Utility.Security
{
    /// <summary>
    /// Web安全服务（登陆相关）
    /// </summary>
    public class WebSecurityService
    {
        #region 配置信息
        /// <summary>
        /// 域的URL地址
        /// </summary>Mintine
        public static string DomainUrl = string.Empty;
        /// <summary>
        /// 用户身份有效分钟数（默认60*24分钟）
        /// </summary>
        public static int ExpireMinutes = 1440;
        /// <summary>
        /// 凭证是否持久化
        /// </summary>
        public static bool IsPersistent = false;
        #endregion

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
        public static UserInfo UserInfo
        {
            get
            {
                if (Principal != null)
                {
                    return Principal.UserInfo;
                }
                return new UserInfo();
            }
        }
        #endregion

        #region IsAuthenticated：当前用户的是否通过身份验证
        /// <summary>
        /// 是否通过身份验证
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                if (Principal != null && Principal.Identity != null)
                {
                    return Principal.Identity.IsAuthenticated;
                }
                return false;
            }
        }
        #endregion

        #region Principal：获取当前用户的 WEB安全信息
        /// <summary>
        /// WEB安全信息
        /// </summary>
        public static Principal Principal
        {
            get
            {
                var user = HttpContext.Current.User;
                if (user != null)
                {
                    return user as Principal;
                }
                return null;
            }
        }
        #endregion

        #endregion

        #region SignIn：登陆保存票据信息
        /// <summary>
        /// 登陆保存票据信息
        /// </summary>
        public static void SignIn(string userId, string userName, int roleId = 0, bool IsAddDomain = false)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
            {
                return;
            }
            //生成身份验证票的Cookie
            DateTime expire = DateTime.Now.AddMinutes(ExpireMinutes);
            string data = new UserInfo(userId, userName, roleId).ToString();
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now,
                expire, IsPersistent, data, "/");
            //将身份验证票加密序列化成一个字符串
            string hashTicket = FormsAuthentication.Encrypt(ticket);
            //生成cookie
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
            if (ticket.IsPersistent)
            {
                cookie.Expires = expire;
            }
            //添加域
            if (IsAddDomain)
            {
                cookie.Domain = DomainUrl;
            }
            //将身份验证票Cookie输出到客户端
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }
        #endregion

        #region SignOut：退出登陆
        /// <summary>
        /// 退出登陆，清除身份票据信息
        /// </summary>
        public static void SignOut()
        {
            FormsAuthentication.SignOut();
        }
        #endregion

        #region Clear：清除票据（使身份凭证信息过期）
        /// <summary>
        /// 清除票据（使身份凭证信息过期）
        /// </summary>
        /// <param name="IsAddDomain"></param>
        public static void Clear(bool IsAddDomain)
        {
            //获取cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            if (IsAddDomain)
            {
                cookie.Domain = DomainUrl;
            }
            cookie.Expires = DateTime.Now.AddHours(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        #endregion

        #region LoadPrincipal：加载身份票据信息
        /// <summary>
        /// 加载身份票据信息
        /// </summary>
        public static Principal LoadPrincipal()
        {
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null)
            {
                return null;
            }
            //从cookie中获取票据信息
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            if (ticket == null)
            {
                return null;
            }
            FormsIdentity formIdentity = new FormsIdentity(ticket);
            Identity identity = new Identity();
            identity.Name = formIdentity.Ticket.Name;
            identity.IsAuthenticated = true;
            var principal = new Principal(identity);
            //读取cookie中数据
            string data = formIdentity.Ticket.UserData;
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            principal.UserInfo = new Security.UserInfo(data);
            HttpContext.Current.User = principal;
            return principal;
        }
        #endregion
    }
}
