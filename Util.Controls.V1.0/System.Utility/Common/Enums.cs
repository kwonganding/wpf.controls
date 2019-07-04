using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace System
{
    #region EnumHttpMode（请求方式）

    /// <summary>
    /// 请求方式
    /// </summary>
    public enum EnumHttpMode
    {
        /// <summary>
        /// Web Post提交数据
        /// </summary>
        [Description("POST")]
        Post,

        /// <summary>
        /// Web Get提交数据
        /// </summary>
        [Description("GET")]
        Get,

        /// <summary>
        /// TCP提交请求
        /// </summary>
        [Description("TCP")]
        TCP,
    }

    #endregion

    #region EnumResponseMode（应答方式）

    /// <summary>
    /// 应答方式
    /// </summary>
    public enum EnumResponseMode
    {
        /// <summary>
        /// 服务后台模式
        /// </summary>
        ServerBack = 0x1,

        /// <summary>
        /// URL地址跳转
        /// </summary>
        UrlRedirect = 0x2,
    }

    #endregion

    #region EnumEncodingType（字符编码方式）

    /// <summary>
    /// 字符编码方式
    /// </summary>
    public enum EnumEncodingType
    {
        /// <summary>
        /// UTF_8
        /// </summary>
        [Description("utf-8")]
        UTF_8 = 0x1,

        /// <summary>
        /// GB2312
        /// </summary>
        [Description("gb2312")]
        GB2312 = 0x2,

        /// <summary>
        /// GBK
        /// </summary>
        [Description("GBK")]
        GBK = 3,

        /// <summary>
        /// GB18030
        /// </summary>
        [Description("GB18030")]
        GB18030 = 4
    }

    #endregion

    #region EnumSignType（数据签名算法）

    /// <summary>
    /// 数据签名算法
    /// </summary>
    public enum EnumSignType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// RSA
        /// </summary>
        [Description("RSA")]
        RSA,

        /// <summary>
        /// MD5
        /// </summary>
        [Description("MD5")]
        MD5
    }

    #endregion

    #region EnumResFormat（应答数据格式）

    /// <summary>
    /// 应答数据格式
    /// </summary>
    public enum EnumResFormat
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// HTML
        /// </summary>
        [Description("HTML")]
        HTML,

        /// <summary>
        /// XML
        /// </summary>
        [Description("XML")]
        XML,
    }
    #endregion

    #region EnumDBSource
    /// <summary>
    /// 
    /// </summary>
    public enum EnumDBSource
    {
        /// <summary>
        /// 默认的SQLIte系统数据库
        /// </summary>
        DefaultSQLite = 1,

        /// <summary>
        /// 多个SQLite数据源
        /// </summary>
        SQLiteSet = 2,

        /// <summary>
        /// MYSQL
        /// </summary>
        MYSQL = 3
    }
    #endregion

    #region EnumPageType 页面类型

    public enum EnumPageType
    {
        [Description("模版html")]
        LayOut,

        [Description("头部html")]
        Header,

        [Description("菜单html")]
        Menu,

        [Description("内容html")]
        Content
    }
    #endregion
}
