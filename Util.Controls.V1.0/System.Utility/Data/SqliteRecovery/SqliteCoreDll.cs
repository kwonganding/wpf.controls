using System;
using System.Runtime.InteropServices;
using System.Utility;
using System.Utility.Logger;

namespace System.Data
{
    internal static class SqliteCoreDll
    {

        private const string _SqliteDllName = "SqliteInterface.dll";

        public static int Init(string licensePath)
        {
            try
            {
                return InitPath("", licensePath);
            }
            catch
            {
                string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, _SqliteDllName);
                LogHelper.Error(string.Format("Sqlite数据库恢复底层DLL初始化( InitPath)失败，当前路径：{0}", path));
                return -1;
            }
        }

        //int __stdcall   initPath(char *pdllPath,char *pAuthorisedFilePath)//设置dll路径与授权文件路径
        [DllImport(_SqliteDllName, EntryPoint = "initPath")]
        public static extern int InitPath(string dllPath, string licensePath);

        //int __stdcall  openSqliteDATA(HANDLE  &pdataBase,char *pPath, char *pChractorPath)//打开文件得到句柄pdataBase，pPath为文件路径，pChractorPath特征数据库路径
        [DllImport(_SqliteDllName, EntryPoint = "openSqliteDATA")]
        public static extern int OpenSqliteData(ref IntPtr dataBase, string dbPath, string chratorPath);


        //打开sqlite数据pSqliteBuff得到句柄pdataBase,length 数据长度，pChractorPath为特征文件路径
        //int __stdcall openSqliteBuff(HANDLE  &pdataBase,char *pSqliteBuff,UINT64 length, char *pChractorPath);
        [DllImport(_SqliteDllName, EntryPoint = "openSqliteBuff")]
        public static extern int OpenSqliteBuff(ref IntPtr dataBase, string buffer, ulong length, string chratorPath);

        //int  __stdcall getCodeFormat(HANDLE pDatabase,int &codeType)   //  获取编码类型 1：utf-8 2:utf-16le 3:utf-16be 
        [DllImport(_SqliteDllName, EntryPoint = "getCodeFormat")]
        public static extern int GetCodeFomart(IntPtr dataBase, ref int codeType);

        //int   __stdcall   getAllTableName( HANDLE pDatabase,char ** &ptableNameGroup,int &tableCount) 
        [DllImport(_SqliteDllName, EntryPoint = "getAllTableName")]
        public static extern int GetAllTableName(IntPtr dataBase, ref  IntPtr arr, ref int tableCount);

        //int  __stdcall   freeALLTableName(HANDLE pDatabase,char ** ptabaleNameGroup, int tableCount)
        [DllImport(_SqliteDllName, EntryPoint = "freeALLTableName")]
        public static extern int FreeALLTableName(IntPtr dataBase, IntPtr tableNames, int tableCount);

        //int  __stdcall  getTableDefine(HANDLE pDatabase,const char * pTableName,char ** &pColumeName,char ** &pColumeType,int &columnCount)
        [DllImport(_SqliteDllName, EntryPoint = "getTableDefine")]
        public static extern int GetTableDefine(IntPtr dataBase, string tableName, ref IntPtr columnNames, ref IntPtr columnTypes, ref int columnCount);

        //int   __stdcall  freeTableDefine(HANDLE pDatabase,char ** &pColumeName,char ** &pColumeType,int &columnCount)
        [DllImport(_SqliteDllName, EntryPoint = "freeTableDefine")]
        public static extern int FreeTableDefine(IntPtr dataBase, ref IntPtr columnNames, ref IntPtr columnTypes, ref int columnCount);

        //int   __stdcall    closeSqliteDATA(HANDLE pDatabase) //关闭数据库 
        [DllImport(_SqliteDllName, EntryPoint = "getTableContentGenearal")]
        public static extern int getTableContentGenearal(IntPtr dataBase, SqliteGeneralCallBack callBack, string tableName, byte dataMode);

        [DllImport(_SqliteDllName, EntryPoint = "closeSqliteDATA")]
        public static extern int CloseSqliteHandle(IntPtr dataBase);

        //pAuthorizeID 获取到32位值，该id不同电脑不同
        //int   __stdcall  getAuthorizeID( char * &pAuthorizeID) //没有授权情况下，请使用该接口获取授权号，然后索取授权文件
        [DllImport(_SqliteDllName, EntryPoint = "getAuthorizeID")]
        public static extern int GetAuthorizeId(ref IntPtr authorizeId);
    }

    public delegate int SqliteGeneralCallBack(int count, IntPtr columnObject, byte dataType);

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColumnObject
    {
        public int Length;

        public byte ColumnType;

        public IntPtr Value;
    }

    public enum ColumnType
    {
        NONE = 0,
        INTEGER = 1,
        DOUBLE = 2,
        TEXT = 3,
        BLOB = 4
    }

    public class SqliteColumnObject
    {
        public byte DataState { get; set; }

        public ColumnType Type { get; set; }

        public int Length { get; set; }

        public object Value { get; set; }

        public SqliteColumnObject()
        {
            Type = ColumnType.NONE;
            DataState = 2;
        }
    }
}
