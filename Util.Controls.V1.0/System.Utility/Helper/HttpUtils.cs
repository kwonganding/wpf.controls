using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace System.Utility.Helper
{
    /// <summary>
    /// HTTP相关处理辅助类
    /// </summary>
    /// User:Ryan  CreateTime:2012-10-15 22:02.
    public class HttpUtils
    {
        #region GetQueryString（从URL地址中获取指定参数值）

        /// <summary>
        /// 从指定URL地址中获取指定参数值
        /// </summary>
        /// <param name="strParaName">需要取值的参数名.</param>
        /// <param name="strUrl">str字符值</param>
        /// <param name="urlDecode">是否对值进行HTTP解码，默认不解密.</param>
        /// User:Ryan  CreateTime:2012-10-15 22:09.
        public static string GetQueryString(string strParaName, string strUrl, bool urlDecode = false)
        {
            string[] strArray = strUrl.Split(new char[] { '&' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].IndexOf(strParaName) >= 0)
                {
                    string value = strArray[i].Split(new char[] { '=' })[1];
                    return urlDecode ? HttpUtility.UrlDecode(value) : value;
                }
            }
            return "";
        }
        #endregion

        #region SendSyncRequest（发送异步HTTP请求）

        /// <summary>
        /// 发送http请求（默认编码法方式，默认Get方式提交）
        /// </summary>
        /// User:Ryan  CreateTime:2012-10-15 22:58.
        public static string SendSyncRequest(string url, string queryString)
        {
            return SendSyncRequest(url, queryString, EnumHttpMode.Get, Encoding.Default);
        }

        /// <summary>
        /// 发送http请求（默认编码法方式）
        /// </summary>
        /// User:Ryan  CreateTime:2012-10-15 22:58.
        public static string SendSyncRequest(string url, string queryString, EnumHttpMode httpMode)
        {
            return SendSyncRequest(url, queryString, httpMode, Encoding.Default);
        }

        /// <summary>
        /// 发送http请求（默认Get方式提交）
        /// </summary>
        /// User:Ryan  CreateTime:2012-10-15 22:58.
        public static string SendSyncRequest(string url, string queryString, Encoding contentEncoding)
        {
            return SendSyncRequest(url, queryString, EnumHttpMode.Get, contentEncoding);
        }

        #region SendSyncRequest （发送http请求）

        /// <summary>
        /// 发送http请求（支持HTTPS请求，需要指定证书文件及证书密码）
        /// </summary>
        /// <param name="url">提交的URL地址.</param>
        /// <param name="queryString">数据.</param>
        /// <param name="httpMode">Http请求模式.</param>
        /// <param name="contentEncoding">字符编码类型.</param>
        /// <returns>Return a data(or instance) of String.</returns>
        /// User:Ryan  CreateTime:2012-10-15 22:58.
        public static string SendSyncRequest(string url, string queryString, EnumHttpMode httpMode, Encoding contentEncoding
            , string certFile = "", string certPwd = "")
        {
            //1.validation parameters
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            if (string.IsNullOrEmpty(queryString))
            {
                queryString = queryString.TrimStart(new char[] { '?' });
            }
            if (contentEncoding == null)
            {
                throw new ArgumentNullException("contentEncoding");
            }
            //2.generate http request
            HttpWebRequest request = null;
            switch (httpMode)
            {
                case EnumHttpMode.Get:
                    request = (HttpWebRequest)WebRequest.Create(string.Format("{0}?{1}", url, queryString.TrimStart(new char[] { '?' })));
                    request.Method = "GET";
                    break;
                case EnumHttpMode.Post:
                    request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    byte[] bytes = contentEncoding.GetBytes(queryString);
                    request.ContentLength = bytes.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                    break;
                default:
                    throw new ArgumentException("不支持该类型的HTTP请求模式", "httpMode");
            }
            //2.2 Https request
            if (!string.IsNullOrEmpty(certFile) && !string.IsNullOrEmpty(certPwd))
            {
                request.ClientCertificates.Add(new X509Certificate2(certFile, certPwd));
            }

            //3.execute request
            try
            {
                return new StreamReader(((HttpWebResponse)request.GetResponse()).GetResponseStream(), contentEncoding).ReadToEnd();
            }
            //4.catch exception
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
        #endregion

        #endregion
    }
}
