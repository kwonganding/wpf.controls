using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace System.Data
{
    #region SQLite参数
    /// <summary>
    /// SQLite参数，注意参数使用方法，sql中不带引号，@符号代表参数
    /// Example：select * from numberinfo where number =@number ； sql.Parameters.Add(new SqlParameter("@number", number));
    /// select longitude,latitude from location where province like @province；sql.Parameters.Add(new SqlParameter("@province", province+"%"));
    /// </summary>
    public class SqlParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public SqlParameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// 隐士转换为SQLiteParameter的重载扩展
        /// </summary>
        public SQLiteParameter ToSQLiteParameter()
        {
            SQLiteParameter para = new SQLiteParameter();
            para.Value = this.Value;
            para.ParameterName = this.Name;
            para.DbType = DBTypeFactory.GetDataType(this.Value.GetType());
            return para;
        }
    }
    #endregion
}
