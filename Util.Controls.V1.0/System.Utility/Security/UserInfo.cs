using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Utility.Security
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo()
        {
            this.UserId = string.Empty;
            this.UserName = string.Empty;
            this.RoleId = 0;
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo(string userId, string userName, int roleId)
            : this()
        {
            this.UserId = userId;
            this.UserName = userName;
            this.RoleId = roleId;
        }

        /// <summary>
        /// 用户信息，使用cookie数据初始化
        /// </summary>
        public UserInfo(string data)
            : this()
        {
            if (string.IsNullOrEmpty(data))
            {
                return;
            }
            var datas = data.Split('|');
            if (datas.Length >= 3)
            {
                this.UserId = datas[0];
                this.UserName = datas[1];
                this.RoleId = datas[2].ToSafeInt();
            }
        }

        /// <summary>
        /// 转换为字符串（cookie存储）
        /// </summary>
        public override string ToString()
        {
            return string.Concat(this.UserId, "|", this.UserName, "|", this.RoleId);
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户角色编号
        /// </summary>
        public int RoleId { get; set; }
    }
}
