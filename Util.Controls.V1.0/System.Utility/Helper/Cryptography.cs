using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace System.Utility.Helper
{
    /// <summary>
    /// 加密处理公共辅助类
    /// </summary>
    public class Cryptography
    {
        /// <summary>
        /// 默认的KEY值
        /// </summary>
        public const string DESKEY = "#s^XLY_DESKEY_1986,11+15";

        /// <summary>
        /// 用以导出密钥的密钥 salt。
        /// </summary>
        private const string AESSALT = "gsf4jvkyhye5/d7k8OrLgM==";

        /// <summary>
        /// 用于对称算法的初始化向量。
        /// </summary>
        private const string AESRGBIV = "Rkb4jvUy/ye7Cd7k89QQgQ==";

        /// <summary>
        /// 一次处理的明文字节数
        /// </summary>
        private const int ENCRYPTSIZE =802400;

        /// <summary>
        /// 一次处理的密文字节数
        /// </summary>
        private const int DECRYPTSIZE = ENCRYPTSIZE + 16;

        #region MD5Encrypt：MD5的32位加密
        /// <summary>
        /// MD5的32位加密，不可逆
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] source = value.ToBytes(Encoding.Unicode);
            byte[] target = md5.ComputeHash(source);
            StringBuilder sb = new StringBuilder();
            int len = target.Length;
            for (int i = 0; i < len; i++)
            {
                sb.Append(target[i].ToString("x").PadLeft(2, '0'));
            }
            md5.Clear();
            return sb.ToString();
        }
        #endregion

        #region DES加密
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="text">要加密的文本</param>
        /// <param name="key">24位的密钥，范例：#s^un2ye31fcn%|aoXpR,+vh</param>
        public static string EncodeDES(string text, string key = DESKEY)
        {
            //如果文本为空，则返回
            if (!text.IsValid())
            {
                return string.Empty;
            }

            //如果密钥的长度不是24位则返回
            if (key.Length != 24)
            {
                return string.Empty;
            }

            try
            {
                //创建DES加密服务提供程序
                TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();

                //提供加密键
                provider.Key = Encoding.ASCII.GetBytes(key);

                //提供加密模式
                provider.Mode = CipherMode.ECB;

                //创建加密器
                ICryptoTransform transform = provider.CreateEncryptor();

                //加密文本
                byte[] bytes = Encoding.Default.GetBytes(text);
                byte[] result = transform.TransformFinalBlock(bytes, 0, bytes.Length);

                //释放资源
                transform.Dispose();

                //返回文本结果
                return Convert.ToBase64String(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DES解密
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="text">要解密的文本</param>
        /// <param name="key">24位的密钥，范例：#s^un2ye31fcn%|aoXpR,+vh</param>
        public static string DecodeDES(string text, string key = DESKEY)
        {
            //如果文本为空，则返回
            if (!text.IsValid())
            {
                return string.Empty;
            }

            //如果密钥的长度不是24位则返回
            if (key.Length != 24)
            {
                return string.Empty;
            }

            //创建DES加密服务提供程序
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();

            //提供加密键
            provider.Key = Encoding.ASCII.GetBytes(key);

            //提供加密模式
            provider.Mode = CipherMode.ECB;

            //设置填充模式
            provider.Padding = PaddingMode.PKCS7;

            //创建解密器
            ICryptoTransform transform = provider.CreateDecryptor();

            try
            {
                //解密
                byte[] bytes = Convert.FromBase64String(text);
                byte[] result = transform.TransformFinalBlock(bytes, 0, bytes.Length);

                //返回文本结果
                return Encoding.Default.GetString(result);
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                //释放资源
                transform.Dispose();
            }
        }
        #endregion
        
        #region AES加解密服务 wangxi 2015-04-30 16:09:33

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AESEncrypt(string encryptString, string key)
        {
            var srouceByte = Encoding.Default.GetBytes(encryptString);
            // 加密
            var encryptByte = AESEncrypt(srouceByte, key);
            // 将密文转换成BASE64编码;
            return Convert.ToBase64String(encryptByte);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="encryptByte">加密明文</param>
        /// <param name="key">加密key</param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] encryptByte, string key)
        {
            return CreateICrypto(encryptByte, key, CryptoType.Encrypt);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AESDecrypt(string encryptString, string key)
        {
            var srouceByte = Convert.FromBase64String(encryptString);
            // 加密
            var encryptByte = AESDecrypt(srouceByte, key);
            // 将密文转换成BASE64编码;
            return Encoding.Default.GetString(encryptByte);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptByte">加密密文</param>
        /// <param name="key">解密key</param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] encryptByte, string key)
        {
            return CreateICrypto(encryptByte, key, CryptoType.Decrypt);
        }

        /// <summary>
        /// 创建加解密服务
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="iCrypto"></param>
        /// <returns></returns>
        private static byte[] CreateICrypto(byte[] source, string key, CryptoType iCrypto)
        {
            if (source.IsInvalid())
            {
                throw (new Exception("加解密数据源不可以为空!"));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw (new Exception("密钥不可以为空"));
            }

            byte[] strSource;
            var aesProvider = Rijndael.Create();
            try
            {

                var mStream = new MemoryStream();
                var pdb = new PasswordDeriveBytes(key, Convert.FromBase64String(AESSALT));
                ICryptoTransform transform = null;
                if (iCrypto == CryptoType.Encrypt)
                {
                    transform = aesProvider.CreateEncryptor(pdb.GetBytes(32), Convert.FromBase64String(AESRGBIV));
                }
                if (iCrypto == CryptoType.Decrypt)
                {
                    transform = aesProvider.CreateDecryptor(pdb.GetBytes(32), Convert.FromBase64String(AESRGBIV));
                }

                var mCsstream = new CryptoStream(mStream, transform, CryptoStreamMode.Write);
                mCsstream.Write(source, 0, source.Length);
                mCsstream.FlushFinalBlock();
                strSource = mStream.ToArray();

                mStream.Close();
                mStream.Dispose();

                mCsstream.Close();
                mCsstream.Dispose();

            }

            catch (IOException ex)
            {
                throw ex;
            }

            catch (CryptographicException ex)
            {
                throw ex;
            }

            catch (ArgumentException ex)
            {
                throw ex;
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                aesProvider.Clear();
            }

            return strSource;
        }
        
        /// <summary>
        /// AES加密文件
        /// </summary>
        /// <param name="encryptpath">加密文件路径</param>
        /// <param name="key">加密Key</param>
        /// <param name="isHiddenTempFile">是否隐藏当前加密的文件</param>
        public static string AESEncryptFile(string encryptpath, string key,bool isHiddenTempFile=false)
        {
            // 加密后的文件路径
            var encryptFullname = encryptpath + ".xly";
            if (System.IO.File.Exists(encryptFullname))
            {
                System.IO.File.Delete(encryptFullname);
            }
            try
            {
                using (var fs = new FileStream(encryptpath, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length == 0)
                    {
                        throw new Exception("不允许对无内容文件加密!");
                    }
                    using (var fsnew = new FileStream(encryptFullname, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        if(isHiddenTempFile)
                        {
                            if(File.IsValid(encryptFullname))
                            {
                                IO.File.SetAttributes(encryptFullname, FileAttributes.Hidden);
                            }
                        }
                        // 计算要做几次加密处理,大文件分几次操作
                        var blockCount = ((int) fs.Length - 1)/ENCRYPTSIZE + 1;
                        for (var i = 0; i < blockCount; i++)
                        {
                            var size = ENCRYPTSIZE;
                            if (i == blockCount - 1)
                            {
                                size = (int) (fs.Length - i*ENCRYPTSIZE);
                            }
                            var buffer = new byte[size];
                            fs.Read(buffer, 0, size);
                            var result = AESEncrypt(buffer, key);
                            fsnew.Write(result, 0, result.Length);
                            fsnew.Flush();
                        }
                        fsnew.Close();
                        fsnew.Dispose();
                    }
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return encryptFullname;
        }

        /// <summary>
        /// AES解密文件
        /// </summary>
        /// <param name="decryptpath">解密文件的路径</param>
        /// <param name="key">解密Key</param>
        /// <returns></returns>
        public static string AESDecryptFile(string decryptpath, string key)
        {
            var decryptFile = decryptpath.TrimEnd(".xly");
            try
            {
               // int a = 0;
               // decryptFile=counter(decryptFile,a);
                using (var fs = new FileStream(decryptpath, FileMode.Open, FileAccess.Read))
                {
                    if (fs.Length > 0)
                    {
                        using (var fsnew = new FileStream(decryptFile, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            int blockCount = ((int)fs.Length - 1) / DECRYPTSIZE + 1;
                            for (int i = 0; i < blockCount; i++)
                            {
                                int size = DECRYPTSIZE;
                                if (i == blockCount - 1) size = (int)(fs.Length - i * DECRYPTSIZE);
                                var bArr = new byte[size];
                                fs.Read(bArr, 0, size);
                                byte[] result = AESDecrypt(bArr, key);
                                fsnew.Write(result, 0, result.Length);
                                fsnew.Flush();
                            }
                            fsnew.Close();
                            fsnew.Dispose();
                        }
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }

        /////<summary>
        ///// 文件判断是否重复递归
        ///// </summary>
        //public static string counter(string decryptFile,int a)
        //{
        //    if (File.IsValid(decryptFile))
        //    {
        //        a = a+1;
        //        string p = File.GetFilePath(decryptFile);
        //        var name= File.GetFileName(decryptFile);
        //        var k = File.GetExtension(decryptFile);
        //        int b = a - 1;
        //        if (name.Contains("("+b.ToSafeString()+")"))
        //        {
        //            var c=name.TrimEnd("(" + b.ToSafeString() + ")"+".txt");
        //            name = c + ".txt";
        //        }
        //        name = name.TrimEnd(".txt") + "(" + a.ToSafeString() + ")" + "."+k;
        //        decryptFile = p + name;
        //        return counter(decryptFile,a);
        //    }
        //    else
        //    {
        //        return decryptFile;
        //    }
        // }
        
        /// <summary>
        /// 加解密类型
        /// </summary>
        public enum CryptoType
        {
            /// <summary>
            /// 加密
            /// </summary>
            Encrypt,
            /// <summary>
            /// 解密
            /// </summary>
            Decrypt
        }
        
        #endregion
    }
}
