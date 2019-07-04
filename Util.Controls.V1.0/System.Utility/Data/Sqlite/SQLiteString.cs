using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace System.Data
{
    /// <summary>
    /// 执行SQLitel的Sql命令定义
    /// </summary>
    public class SQLiteString
    {
        public string SqlText { get; set; }
        public IList<SqlParameter> Parameters;

        public SQLiteString()
        {
            this.Parameters = new List<SqlParameter>();
        }

        public SQLiteString(string sqlText)
            : this()
        {
            this.SqlText = sqlText;
        }

        public void SetCommand(SQLiteCommand com)
        {
            Guard.ArgumentNotNull(com, "SQLiteCommand");
            com.CommandText = this.SqlText;
            if (this.Parameters != null && this.Parameters.Count > 0)
            {
                this.Parameters.ForEach(s =>
                    {
                        com.Parameters.Add(s.ToSQLiteParameter());
                    });
            }
        }
    }
}
