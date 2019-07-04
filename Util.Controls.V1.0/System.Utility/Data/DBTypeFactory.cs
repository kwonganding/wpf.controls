using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace System.Data
{
    public static class DBTypeFactory
    {
        /// <summary>
        /// 类型映射字典
        /// </summary>
        private static readonly HybridDictionary _Types;

        static DBTypeFactory()
        {
            _Types = new HybridDictionary();
            //一般类型
            _Types[typeof(string)] = DbType.String;
            _Types[typeof(DateTime)] = DbType.DateTime;
            _Types[typeof(bool)] = DbType.Boolean;

            _Types[typeof(byte)] = DbType.Byte;
            _Types[typeof(sbyte)] = DbType.SByte;
            _Types[typeof(decimal)] = DbType.Decimal;
            _Types[typeof(double)] = DbType.Double;
            _Types[typeof(float)] = DbType.Single;

            _Types[typeof(int)] = DbType.Int32;
            _Types[typeof(uint)] = DbType.UInt32;
            _Types[typeof(long)] = DbType.Int64;
            _Types[typeof(ulong)] = DbType.UInt64;
            _Types[typeof(short)] = DbType.Int16;
            _Types[typeof(ushort)] = DbType.UInt16;

            _Types[typeof(Guid)] = DbType.Guid;
            _Types[typeof(byte[])] = DbType.Binary;
            _Types[typeof(Enum)] = DbType.Int32;
            //可空类型
            _Types[typeof(Nullable<bool>)] = DbType.Boolean;
            _Types[typeof(Nullable<byte>)] = DbType.Byte;
            _Types[typeof(Nullable<DateTime>)] = DbType.DateTime;
            _Types[typeof(Nullable<decimal>)] = DbType.Decimal;
            _Types[typeof(Nullable<double>)] = DbType.Double;
            _Types[typeof(Nullable<float>)] = DbType.Single;
            _Types[typeof(Nullable<int>)] = DbType.Int32;
            _Types[typeof(Nullable<long>)] = DbType.Int64;
            _Types[typeof(Nullable<sbyte>)] = DbType.SByte;
            _Types[typeof(Nullable<short>)] = DbType.Int16;
            _Types[typeof(Nullable<uint>)] = DbType.UInt32;
            _Types[typeof(Nullable<ulong>)] = DbType.UInt64;
            _Types[typeof(Nullable<ushort>)] = DbType.UInt16;
            _Types[typeof(Nullable<Guid>)] = DbType.Guid;
        }


        /// <summary>
        /// 将指定.NET类型获取ADO.NET参数的数据类型
        /// </summary>
        public static DbType GetDataType(Type type)
        {
            if (type.IsEnum)
            {
                type = typeof(Enum);
            }

            if (_Types.Contains(type))
            {
                return (DbType)_Types[type];
            }

            throw new ArgumentOutOfRangeException(type.ToString());
        }
    }
}
