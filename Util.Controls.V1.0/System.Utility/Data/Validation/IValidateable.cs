namespace System.Utility.Data.Validation
{
    /// <summary>
    /// 可支持验证的验证接口
    /// </summary>
    public interface IValidateable
    {
        /// <summary>
        /// 验证数据合法性
        /// 验证通过返回true；验证失败返回false
        /// </summary>
        /// <returns></returns>
        bool IsValid();

        /// <summary>
        /// 错误信息
        /// </summary>
        string ErrorMsg { get; }
    }
}
