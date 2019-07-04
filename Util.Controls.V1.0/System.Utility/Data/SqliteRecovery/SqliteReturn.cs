namespace System.Data
{
    /// <summary>
    /// Sqlite执行返回实体。
    /// </summary>
    public class SqliteReturn
    {
        /// <summary>
        /// 是否处理成功。
        /// </summary>
        public bool IsSucess { get; set; }

        /// <summary>
        /// 栈信息。
        /// 当正常处理时，可能为空。
        /// </summary>
        public string StackMsg { get; set; }
    }
}
