using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Utility.Logger;

namespace System.Data
{
    /// <summary>
    /// Sqlite数据库恢复助手。
    /// </summary>
    public static class SqliteRecoveryHelper
    {
        #region  全局字段

        //删除内容
        private const int DeleteData = 1;

        //正常内容
        private const int NormalData = 2;

        //扫描碎片数据
        private const int ScanData = 4;

        /// <summary>
        /// 数据模型
        /// </summary>
        private static byte _DataMode;

        private const string NewColumnName = "XLY_DataType";

        private static string licenseFile = "sqliteKey.dat";

        //sqlite 回调
        //private static SqliteGeneralCallBack _CallBack;

        private static List<List<SqliteColumnObject>> _AllNewRowData;

        private static Encoding _CurrentEncoding = Encoding.UTF8;

        #endregion

        #region 共有方法

        /// <summary>
        /// 数据恢复。
        /// </summary>
        /// <param name="sourcedb">源数据库db文件路径。</param>
        /// <param name="tableNames">多个表之间请按照","分隔，如 t1,t2,t3</param>
        /// <param name="isScanDebris">是否扫描碎片</param>
        /// <returns>恢复成功，则返回新数据库db文件。(极端)失败则返回源来的数据库db文件。</returns>
        public static string DataRecovery(string sourcedb, string tableNames, bool isScanDebris = false)
        {
            return DataRecovery(sourcedb, string.Empty, tableNames, isScanDebris);
        }

        /// <summary>
        /// DFSqlite数据恢复。（应急存在，后面需要删除）
        /// </summary>
        /// <param name="sourcedb">源数据库db文件路径。</param>
        /// <param name="tableNames">多个表之间请按照","分隔，如 t1,t2,t3</param>
        /// <param name="isCopy">是否把源文件拷贝其他地方再做处理 </param>
        /// <param name="isScanDebris">是否扫描碎片</param>
        /// <returns>恢复成功，则返回新数据库db文件。(极端)失败则返回源来的数据库db文件。</returns>
        public static string DataRecovery(string sourcedb, bool isCopy, string tableNames, bool isScanDebris = false)
        {
            string tempfile;

            if (isCopy)
            {
                tempfile = System.Utility.Helper.File.ConnectPath(Path.GetTempPath(), DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_") + System.Utility.Helper.File.GetFileName(sourcedb));
                System.IO.File.Copy(sourcedb, tempfile, true);
            }
            else
            {
                tempfile = sourcedb;
            }

            return DataRecovery(tempfile, string.Empty, tableNames, isScanDebris);
        }

        /// <summary>
        /// 数据恢复。
        /// </summary>
        /// <param name="sourcedb">源数据库db文件路径。</param>
        /// <param name="charatorPath">特征库文件（可空）</param>
        /// <param name="tableNames">多个表之间请按照","分隔，如 t1,t2,t3</param>
        /// <param name="isScanDebris">是否扫描碎片数据</param>
        /// <returns>恢复成功，则返回新数据库db文件。(极端)失败则返回源来的数据库db文件。</returns>
        public static string DataRecovery(string sourcedb, string charatorPath, string tableNames, bool isScanDebris = false)
        {
            if (!System.Utility.Helper.File.IsValid(sourcedb))
            {
                LogHelper.Error(string.Format("Sqlite恢复文件【{0}】不存在，或者文件大小为0，无法处理。", sourcedb));
                return sourcedb;
            }

            if (isScanDebris)
            {
                _DataMode = NormalData + DeleteData + ScanData;
            }
            else
            {
                _DataMode = NormalData + DeleteData;
            }

            try
            {
                var name = System.Utility.Helper.File.GetFileName(sourcedb);
                var path = System.Utility.Helper.File.GetFilePath(sourcedb);
                var ext = System.Utility.Helper.File.GetExtension(sourcedb);
                var newfile = System.Utility.Helper.File.ConnectPath(path, string.Format("{0}_recovery.{1}", name.TrimEnd(ext).TrimEnd("."), ext)).TrimEnd(".");

                charatorPath = System.Utility.Helper.File.GetPhysicalPath(charatorPath);

                //表列表
                var tableArray = tableNames.Replace('，', ',').TrimEnd(new[] { ',', ' ' }).Split(',');

                //底层C++处理
                var res = ButtomDataRecovery(sourcedb, charatorPath, newfile, tableArray);

                //若底层处理不成功，采用上层数据处理。
                if (!res.IsSucess)
                {
                    LogHelper.Info(string.Format("Sqlite数据库恢复底层C++恢复文件【{0}】失败，系统进入上层C#（正常数据）恢复流程。", sourcedb));
                    return TopDataRecovery(sourcedb, newfile, tableArray);
                }

                return newfile;
            }
            catch (Exception ex)
            {
                string mes = string.Format("文件[{0}]执行数据恢复出错：{1}", sourcedb, ex.AllMessage());
                LogHelper.Error(mes, ex);
                return sourcedb;
            }
        }

        /// <summary>
        /// 是否存在某个表。
        /// </summary>
        /// <param name="context">SQLite上下文对象。</param>
        /// <param name="tableName">表名字（注意大小写）。</param>
        /// <returns>存在返回True，反之返回false。</returns>
        public static bool IsExistTable(SqliteContext context, string tableName)
        {
            return context.Exist(tableName);
        }

        /// <summary>
        /// 获取所有用户信息表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllTables(SqliteContext context)
        {
            try
            {
                var tables = context.Find(new SQLiteString("select * from sqlite_master where type = 'table'"));
                return tables.Select(table => DynamicConvert.ToSafeString(table.name)).Cast<string>().ToList();
            }
            catch
            {
                LogHelper.Error("上层获取数据库表信息失败. 转底层获取!");
                return ButtomGetAllTables(context.DataSource, "");
            }
        }

        public static Encoding GetSqliteEcoding(string sourceDb)
        {
            IntPtr dbBase = IntPtr.Zero;
            InitDb(sourceDb, string.Empty, ref dbBase);
            //清理资源
            DisposeSource(dbBase);
            return _CurrentEncoding;
        }

        /// <summary>
        /// 调用底层方法，获取所有用户信息
        /// </summary>
        /// <param name="sourceDb">源数据库路径</param>
        /// <param name="charatorPath">特征库文件路径</param>
        /// <returns></returns>
        public static List<string> ButtomGetAllTables(string sourceDb, string charatorPath)
        {
            var tableNames = new List<string>();
            IntPtr dbBase = IntPtr.Zero;
            var stackMsgBuilder = new StringBuilder();
            bool isSuccess = true;
            IntPtr tableArr = IntPtr.Zero;
            int tableCount = 0;

            //判断是否初始化，若初始化底层失败，则返回。
            if (!InitDb(sourceDb, charatorPath, ref dbBase))
            {
                stackMsgBuilder.AppendLine("SQLite底层DLl初始化失败。可能原因");
                stackMsgBuilder.AppendLine("1：程序未使用管理员权限运行。");
                stackMsgBuilder.AppendLine("2：底层DLL缺少必要的Key文件。");
                isSuccess = false;
            }

            if (isSuccess)
            {
                SqliteCoreDll.GetAllTableName(dbBase, ref tableArr, ref tableCount);
                tableNames = ConvertToArray(tableArr, tableCount);
            }
            else
            {
                LogHelper.Error(stackMsgBuilder.ToSafeString());
            }

            return tableNames;
        }

        /// <summary>
        /// 设置授权文件路径全路径
        /// 如果需要使用自己的指定授权文件，需要在数据恢复之前调用该方法
        /// </summary>
        /// <param name="path">license文件全路径</param>
        public static void SetLicenseFile(string path)
        {
            licenseFile = path;
        }

        #endregion

        #region 核心处理

        /// <summary>
        /// 上层C#数据处理。
        /// 上传恢复的数据只有删除的数据。
        /// </summary>
        /// <param name="sourcedb">数据源db。</param>
        /// <param name="newfile">新备份文件db。</param>
        /// <param name="tableArray">表列表。</param>
        /// <returns>返回新的备份文件db。</returns>
        private static string TopDataRecovery(string sourcedb, string newfile, IEnumerable<string> tableArray)
        {
            var oldcontext = new SqliteContext(sourcedb);
            System.IO.File.Copy(sourcedb, newfile, true);
            var newcontext = new SqliteContext(newfile);
            foreach (var tableName in tableArray)
            {
                //判断原库中是否含有表，若存在该表则处理，否则不处理。
                if (!IsExistTable(oldcontext, tableName))
                {
                    LogHelper.Warn(string.Format("Sqlite数据库恢复-上层C#恢复库【{0}】的表【{1}】失败，可能是表名拼写不准确（如大小写或表名存在特殊字符等）", sourcedb, tableName));
                    continue;
                }

                //添加新的一列
                string alterTablesql = string.Format("ALTER TABLE {0} ADD COLUMN {1} INTEGER default 2", tableName, NewColumnName);
                newcontext.ExecuteNonQuery(alterTablesql);
            }

            newcontext.Dispose();
            return newfile;
        }

        /// <summary>
        /// 底层c++处理数据库。
        /// 数据库处理后，请使用新数据库进行查找。
        /// </summary>
        /// <param name="sourceDb">源数据库路径。</param>
        /// <param name="charatorPath">特征库文件路径。</param>
        /// <param name="newDbPath">临时数据库路径。</param>
        /// <param name="tableNames">表名（特别注意表名拼写正确）集合,多个表之间请按照","分隔，如 t1,t2,t3</param>
        /// <returns>处理结果，成功则SqliteReturn对象的IsSucess = true，StackMsg为空。
        /// 注意若是多个表进行同时处理，只要有一个表处理成功，IsSucess= true。
        /// 其他异常错误信息，可从StackMsg属性中获取概要信息；出错时查看日志文件，可得到更多栈信息。</returns>
        private static SqliteReturn ButtomDataRecovery(string sourceDb, string charatorPath, string newDbPath, string[] tableNames)
        {
            var sqliteReturn = new SqliteReturn { IsSucess = false, StackMsg = string.Empty };
            if (sourceDb.IsNullOrEmpty() || newDbPath.IsNullOrEmpty() || tableNames.Length <= 0)
            {
                sqliteReturn.StackMsg = "传入参数【源数据库路径，备份路径，表列表】不能存在空值。";
                return sqliteReturn;
            }

            var context = new SqliteContext(newDbPath);
            bool isInit = false;
            IntPtr dbBase = IntPtr.Zero;
            var stackMsgBuilder = new StringBuilder();
            bool isSuccess = true;

            foreach (var tableName in tableNames)
            {
                //判断是否含有表,则直接跳过处理。
                if (IsExistTable(context, tableName))
                {
                    stackMsgBuilder.AppendLine(string.Format("副本数据库 【{0}】中已经存在表【{1}】，系统未对该表进行处理。", newDbPath, tableName));
                    continue;
                }

                //判断是否初始化，若初始化底层失败，则返回。
                if (isInit == false)
                {
                    if (InitDb(sourceDb, charatorPath, ref dbBase))
                    {
                        isInit = true;
                    }
                    else
                    {
                        stackMsgBuilder.AppendLine("SQLite底层DLl初始化失败。可能原因");
                        stackMsgBuilder.AppendLine("1：程序未使用管理员权限运行。");
                        stackMsgBuilder.AppendLine("2：底层DLL缺少必要的Key文件。");
                        isSuccess = false;
                        break;
                    }
                }

                //获取表定义
                IList<string> allColumnNames;
                IList<string> allColumnTypes;
                GetTableDefin(dbBase, tableName, out allColumnNames, out allColumnTypes);
                if (allColumnNames.Count == 0)
                {
                    isSuccess = false;
                    stackMsgBuilder.AppendLine(string.Format("无法获取表【{0}】的定义,可能原因：", tableName));
                    stackMsgBuilder.AppendLine(string.Format("1：原数据库中不存在表【{0}】，请注意表名大小写（用SQLite工具查看核实）。", tableName));
                    stackMsgBuilder.AppendLine("2：c++底层DLL存在错误。");
                    break;
                }

                #region 创建表，插入数据

                //创建表架构
                if (CreateTable(context, tableName, allColumnNames, allColumnTypes))
                {
                    //从底层获取当前表的数据。
                    GetTableAllData(dbBase, tableName);

                    //若底层获取了数据，则把底层的数据写入副本对应的表中。
                    if (_AllNewRowData.Count > 0)
                    {
                        //高效批量插入多条数据（采用事务机制）
                        // 高效事务批量插入数据库。
                        // 由于SQLite特殊性，它是文件存储的，每一次插入都是一次IO操作
                        //为了高效插入，引入事务机制，先在内存中插入，最后一次性提交到数据库。
                        try
                        {
                            context.UsingSafeTransaction(command =>
                            {
                                //获取插入表的SQL(多条)语句。
                                var notIdColumns = new StringBuilder();
                                allColumnNames.Skip(1).ForEach(name => notIdColumns.Append(name + ","));
                                notIdColumns.Append(NewColumnName);

                                var hasIdColumns = new StringBuilder();
                                // 过滤字段中特殊符号(`);
                                for (int i = 0; i < allColumnNames.Count; i++)
                                {
                                    allColumnNames[i] = allColumnNames[i].Replace("`", "");
                                }
                                allColumnNames.ForEach(name => hasIdColumns.Append(name + ","));

                                hasIdColumns.Append(NewColumnName);

                                //对数据进行分组合并，按正常、删除、碎片顺序存储。
                                var allRowData = GetAllRowData();

                                foreach (var row in allRowData)
                                {
                                    var parameters = new List<SQLiteParameter>();
                                    var valuesHead = string.Empty;
                                    StringBuilder columns;

                                    if (row[0].Value.ToString().IsNullOrEmpty())
                                    {//主键为空，一般为Integer的数字
                                        columns = notIdColumns;
                                    }
                                    else
                                    {//主键为存在
                                        columns = hasIdColumns;
                                        string primaryKeyId = "@" + allColumnNames[0];
                                        var parameter = new SQLiteParameter(primaryKeyId, DbType.String);
                                        if (row[0].Type == ColumnType.BLOB)
                                        {
                                            parameter.DbType = DbType.Binary;
                                        }

                                        parameter.Value = row[0].Value;
                                        valuesHead = primaryKeyId + ",";
                                        parameters.Add(parameter);
                                    }

                                    var valuesBody = new StringBuilder();
                                    valuesBody.Append(valuesHead);

                                    var formTwoDatas = row.Skip(1);

                                    int columnIndex = 1;
                                    foreach (var cell in formTwoDatas)
                                    {
                                        string paramName = "@" + allColumnNames[columnIndex];
                                        var parameter = new SQLiteParameter(paramName, DbType.String);
                                        if (cell.Type == ColumnType.BLOB)
                                        {
                                            parameter.DbType = DbType.Binary;
                                        }
                                        parameter.Value = cell.Value;
                                        parameters.Add(parameter);
                                        valuesBody.Append(paramName + ",");
                                        columnIndex++;
                                    }

                                    valuesBody.Append(row[0].DataState);
                                    var insertSql = string.Format("insert into {0}({1}) values({2})", tableName, columns, valuesBody);
                                    command.CommandText = insertSql;
                                    command.Parameters.AddRange(parameters.ToArray());

                                    try
                                    {
                                        command.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        var msg = string.Format("Sqlite插入表【{0}】发生异常 \n SQL语句为： {1}", tableName, insertSql);
                                        LogHelper.Warn(msg, ex);
                                    }
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            var msg = string.Format("Sqlite数据库恢复-Sqlite插入表【{0}】发生异常", tableName);
                            stackMsgBuilder.AppendLine(msg);
                            LogHelper.Warn(msg, ex);
                            isSuccess = false;
                            break;
                        }
                    }
                }
                else
                {
                    isSuccess = false;
                    stackMsgBuilder.AppendLine(string.Format("无法在备份库中创建表【{0}】，详细信息可在日志文件中查看。", tableName));
                    break;
                }

                #endregion
            }

            //清理资源
            DisposeSource(dbBase);

            sqliteReturn.IsSucess = isSuccess;
            sqliteReturn.StackMsg = stackMsgBuilder.ToString();
            return sqliteReturn;
        }

        #endregion

        #region 底层流程处理逻辑

        /// <summary>
        /// 获取表定义。
        /// 获取表列集合。
        /// 获取列数据类型集合。
        /// </summary>
        private static void GetTableDefin(IntPtr dbBase, string tableName, out IList<string> allColumnNames, out IList<string> allColumnTypes)
        {
            //获取表定义(包含列名和列类型)
            IntPtr columnNames = IntPtr.Zero;
            IntPtr columnTypes = IntPtr.Zero;
            int columnCount = 0;
            int gettabledefineCode = SqliteCoreDll.GetTableDefine(dbBase, tableName, ref columnNames, ref columnTypes, ref columnCount);
            if (gettabledefineCode != 0)
            {
                LogHelper.Error(string.Format("获取表【{0}】定义失败，错误码:{1}，可能原因：表名字错误", tableName, gettabledefineCode));
                allColumnNames = new List<string>();
                allColumnTypes = new List<string>();
                return;
            }

            allColumnNames = ConvertToArray(columnNames, columnCount);
            allColumnTypes = ConvertToArray(columnTypes, columnCount);
            int freeTableDefineCode = SqliteCoreDll.FreeTableDefine(dbBase, ref columnNames, ref columnTypes, ref columnCount);
            if (freeTableDefineCode != 0)
            {
                LogHelper.Warn(string.Format("Sqlite数据库恢复-释放表【{0}】定义失败，错误码:{1}", tableName, freeTableDefineCode));
            }
        }

        /// <summary>
        /// 分组存储数据。
        /// 由于返回的是错乱的结构，必须把正常的数据放在前面，因为他们有主键值。
        /// </summary>
        /// <returns>按照正常、删除、碎片结构返回。</returns>
        private static IEnumerable<List<SqliteColumnObject>> GetAllRowData()
        {
            var normalRowData = new List<List<SqliteColumnObject>>();
            var deleteRowData = new List<List<SqliteColumnObject>>();
            var scanRowData = new List<List<SqliteColumnObject>>();

            foreach (var rowData in _AllNewRowData)
            {
                int dataState = rowData[0].DataState;
                if (dataState == 2)
                {
                    normalRowData.Add(rowData);
                }
                else if (dataState == 1)
                {
                    deleteRowData.Add(rowData);
                }
                else
                {
                    scanRowData.Add(rowData);
                }
            }

            var allData = new List<List<SqliteColumnObject>>();
            allData.AddRange(normalRowData);
            allData.AddRange(deleteRowData);
            allData.AddRange(scanRowData);

            return allData;
        }

        /// <summary>
        /// 获取所有从Dll底层返回的数据。
        /// </summary>
        /// <param name="dbBase"></param>
        /// <param name="tableName"></param>
        private static void GetTableAllData(IntPtr dbBase, string tableName)
        {
            //获取表的正常和删除数据。
            _AllNewRowData = new List<List<SqliteColumnObject>>();

            //int getCotentCode = SqliteCoreDll.getTableContentGenearal(dbBase, _CallBack, tableName, _DataMode);
            // 这里可以直接调用 SqliteCallBack 为什么还要定义一个变量 _CallBack 呢？而且 _CallBack 也就这个地方使用;
            int getCotentCode = SqliteCoreDll.getTableContentGenearal(dbBase, SqliteCallBack, tableName, _DataMode);
            if (getCotentCode != 0)
            {
                LogHelper.Error(string.Format("Sqlite数据库恢复-Sqlite底层读取表[{0}]记录发生错误，错误码：{1}", tableName, getCotentCode));
            }
        }

        /// <summary>
        /// 创建表构架。
        /// </summary>
        private static bool CreateTable(SqliteContext context, string tableName, IList<string> allColumnNames, IList<string> allColumnTypes)
        {
            // 获取当前表中主键字段;
            var primaryKeys = GetPrimaryKey(context, tableName);
            //拼接创建表的SQL语句
            var fieldBuilder = new StringBuilder();
            for (int i = 0; i < allColumnNames.Count; i++)
            {
                //if (i == 0 && (allColumnNames[0].ToUpper().Equals("_ID") || allColumnNames[0].ToUpper().Equals("ID")) && allColumnTypes[0].Contains("INT"))
                if (primaryKeys.Contains(allColumnNames[0].ToUpper()))
                {
                    fieldBuilder.Append(allColumnNames[i] + " " + allColumnTypes[i] + " PRIMARY KEY,");
                }
                else
                {
                    fieldBuilder.Append(allColumnNames[i] + " " + allColumnTypes[i] + ",");
                }
            }
            fieldBuilder.Append(NewColumnName + " INTEGER");

            //创建数据表
            string createTableSql = string.Format("create table {0}({1})", tableName, fieldBuilder);

            try
            {
                context.ExecuteNonQuery(createTableSql);
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("创建表{0}构造失败,创建表的SQL语句：\n{1}", tableName, createTableSql), ex);
                return false;
            }

            return true;

        }

        /// <summary>
        /// 获取表中的主键信息;
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static List<string> GetPrimaryKey(SqliteContext context, string tableName)
        {
            var keyAll = new List<string>();
            try
            {
                var sqlite = new SqliteContext("") { DataSource = context.DataSource.Replace("_recovery", "") };
                var data =
                    sqlite.Find(
                        new SQLiteString("select * from sqlite_master where type = 'table' and tbl_name = '" + tableName +
                                         "';"));
                if (data == null || data.Count == 0)
                    return keyAll;
                var lists = Regex.Matches(DynamicConvert.ToSafeString(data[0].sql), @"\(.*\)");
                if (lists.Count == 0)
                    return keyAll;
                var columns = lists[0].Value.TrimStart('(').TrimEnd(')').Split(',');
                foreach (var column in columns)
                {
                    if (!column.Contains("PRIMARY KEY")) continue;
                    var result = Regex.Matches(column, "'.*'");
                    if (result.Count == 0 || result.Count > 1)
                        continue;
                    keyAll.Add(DynamicConvert.ToSafeString(result[0].Value).Replace("'", ""));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("获取表{0}主键信息失败", tableName), ex);
            }
            return keyAll;
        }

        /// <summary>
        /// 初始化Dll底层
        /// 类似与Mount。
        /// </summary>
        /// <param name="sourceDb">源数据库路径。</param>
        /// <param name="charatorPath">特征库路径。</param>
        /// <param name="dbBase">数据库句柄。</param>
        /// <returns>返回是否初始化成功。</returns>
        private static bool InitDb(string sourceDb, string charatorPath, ref IntPtr dbBase)
        {
            try
            {
                int initCode = SqliteCoreDll.Init(licenseFile);
                if (initCode != 0)
                {
                    LogHelper.Error(string.Format("Sqlite数据库恢复-Sqlite底层初始化错误，错误码：{0}", initCode));
                    return false;
                }

                int openCode = SqliteCoreDll.OpenSqliteData(ref dbBase, sourceDb, charatorPath);
                if (openCode != 0)
                {
                    LogHelper.Error(openCode == 9999
                                        ? string.Format("Sqlite数据库恢复-Sqlite 打开数据库失败,错误码：{0}.原因可能是没有注册或者没有管理员方式运行", openCode)
                                        : string.Format("Sqlite数据库恢复-Sqlite 打开数据库失败,错误码：{0}", openCode));

                    return false;
                }

                int formatCode = 0;
                int getCode = SqliteCoreDll.GetCodeFomart(dbBase, ref formatCode);
                if (getCode != 0)
                {
                    LogHelper.Error(string.Format("-Sqlite数据库恢复Sqlite获取数据库【{0}】编码失败，错误码：{1}", sourceDb, getCode));
                }

                _CurrentEncoding = GetFormatString(formatCode);
                //_CallBack = SqliteCallBack;

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("调用底层SQLite-dll发生异常",ex);
                return false;
            }

        }

        /// <summary>
        /// 清理资源，释放数据库句柄。
        /// 把所有数据清空。
        /// </summary>
        /// <param name="dbBase"></param>
        private static void DisposeSource(IntPtr dbBase)
        {
            if (dbBase != IntPtr.Zero)
            {
                int freeDataBase = SqliteCoreDll.CloseSqliteHandle(dbBase);
                if (freeDataBase != 0)
                {
                    LogHelper.Error(string.Format("Sqlite数据库恢复-释放数据库句柄失败，错误码：{0}", freeDataBase));
                }
            }

            _AllNewRowData = null;
        }

        /// <summary>
        /// Sqilte获取表数据的回调函数。
        /// </summary>
        /// <param name="count">返回表的行数。</param>
        /// <param name="objectPointer">列数组指针。</param>
        /// <param name="dataType">每一行的数据类型（正常，删除，扫描）</param>
        /// <returns>默认返回0。</returns>
        public static int SqliteCallBack(int count, IntPtr objectPointer, byte dataType)
        {
            var cellObjects = new List<SqliteColumnObject>();
            for (int i = 0; i < count; i++)
            {
                var columnObject = objectPointer.ToStruct<ColumnObject>();

                var sqliteColumn = new SqliteColumnObject();
                cellObjects.Add(sqliteColumn);

                sqliteColumn.DataState = dataType;
                sqliteColumn.Length = columnObject.Length;
                if (columnObject.Length == 0)
                {
                    sqliteColumn.Value = "";
                    objectPointer = objectPointer.Increment<ColumnObject>();
                    continue;
                }

                if (columnObject.ColumnType == 1)
                {//Integer
                    byte[] bytes = GetIntAndDoubleBytes(columnObject.Value, columnObject.Length);
                    sqliteColumn.Value = BitConverter.ToInt64(bytes, 0);
                    sqliteColumn.Type = ColumnType.INTEGER;
                }
                else if (columnObject.ColumnType == 2)
                {//Double
                    byte[] bytes = GetIntAndDoubleBytes(columnObject.Value, columnObject.Length);
                    sqliteColumn.Value = BitConverter.ToDouble(bytes, 0);
                    sqliteColumn.Type = ColumnType.DOUBLE;
                }
                else if (columnObject.ColumnType == 3)
                {//Text
                    var bytes = new byte[columnObject.Length];
                    Marshal.Copy(columnObject.Value, bytes, 0, columnObject.Length);
                    //string tempValue = Encoding.Default.GetString(Encoding.Convert(_CurrentEncoding, Encoding.Default, bytes));
                    //Modify：chenjing 为了民族团结，为了少数民族文化传承，for the horde
                    string tempValue = Encoding.UTF8.GetString(bytes);

                    if (bytes[columnObject.Length - 1] != 0)
                    {
                        tempValue = tempValue.TrimEnd("\0");
                    }

                    sqliteColumn.Value = tempValue;
                    sqliteColumn.Type = ColumnType.TEXT;
                }
                else if (columnObject.ColumnType == 4)
                {//Blob
                    var bytes = new byte[columnObject.Length];
                    Marshal.Copy(columnObject.Value, bytes, 0, columnObject.Length);

                    sqliteColumn.Value = bytes;
                    sqliteColumn.Type = ColumnType.BLOB;
                }
                else
                {//未知
                    sqliteColumn.Value = "";
                    sqliteColumn.Type = ColumnType.NONE;
                }


                objectPointer = objectPointer.Increment<ColumnObject>();

            }

            _AllNewRowData.Add(cellObjects);

            return 0;
        }

        #endregion

        #region 工具方法

        /// <summary>
        /// 获取数据库的编码。
        /// </summary>
        private static Encoding GetFormatString(int code)
        {
            // 1：utf-8 2:utf-16le 3:utf-16be 
            switch (code)
            {
                case 1:
                    return Encoding.UTF8;
                case 2:
                    return Encoding.GetEncoding("UTF-16LE");
                case 3:
                    return Encoding.GetEncoding("UTF-16BE");
                default:
                    return Encoding.UTF8;
            }
        }

        /// <summary>
        /// 获取Int和Double类型
        /// 从底层获取的指针为高位存储，需要转换为Windows能识别的低位存储。
        /// </summary>
        private static byte[] GetIntAndDoubleBytes(IntPtr intper, int count)
        {
            var bytes = new byte[8];
            Marshal.Copy(intper, bytes, 0, count);

            for (int i = 0; i < count / 2; i++)
            {
                byte temp = bytes[i];
                bytes[i] = bytes[count - 1 - i];
                bytes[count - 1 - i] = temp;
            }

            if ((bytes[count - 1] & 128) == 128)
            {
                for (int i = count; i < 8; i++)
                {
                    bytes[i] = 255;
                }
            }

            return bytes;
        }


        /// <summary>
        /// 转换数组指针为字符集合。
        /// </summary>
        /// <param name="stringPointer">数组指针</param>
        /// <param name="count">数组大小</param>
        /// <returns>泛型集合</returns>
        private static List<string> ConvertToArray(IntPtr stringPointer, int count)
        {
            var tbs = new IntPtr[count];
            Marshal.Copy(stringPointer, tbs, 0, count);
            var resultList = new List<string>();
            Array.ForEach(tbs, name => resultList.Add(Marshal.PtrToStringAnsi(name)));
            return resultList;
        }

        public static string GetKeyId()
        {
            IntPtr key = IntPtr.Zero;
            SqliteCoreDll.GetAuthorizeId(ref key);
            return key.ToAnsiString().Substring(0, 32);
        }

        #endregion
    }
}
