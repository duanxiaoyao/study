﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using Common.Queries;
using Common.Result;
using Common.Helper;
using Common;
using MySql.Data.MySqlClient;
using Common.Context;
using System.Text;

namespace Common.DbAccess
{
    /// <summary>
    /// 数据访问
    /// </summary>
    public interface ISqlQuery
    {
        /// <summary>
        /// 开启数据库
        /// </summary>
        SqlQuery Begin();
        /// <summary>
        /// 初始化where（1=1）
        /// </summary>
        string AppendWhere { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        IDbConnection DbConnection { get; set; }

        /// <summary>
        /// 查询首个数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        TResult First<TResult>(string sql, IDictionary<string, object> param = null);
        /// <summary>
        /// 查询首个数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        Task<TResult> FirstAsync<TResult>(string sql, IDictionary<string, object> param = null);
        /// <summary>
        /// 查询首个数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        TResult First<TResult>(string sql, object param = null);
        /// <summary>
        /// 查询首个数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        Task<TResult> FirstAsync<TResult>(string sql, object param = null);

        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        List<TResult> Query<TResult>(string sql, IDictionary<string, object> param = null);
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        List<TResult> Query<TResult>(string sql, object param = null);
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, IDictionary<string, object> param = null);
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null);
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        Task ExecuteAsync(string sql, object param = null);

        /// <summary>
        /// 异步查询分页数据对象
        /// </summary>
        /// <param name="selectSql">截至到from</param>
        /// <param name="whereSql">where</param>
        /// <param name="orderbySql">排序</param>
        /// <param name="query">查询对象,PageIndex起始页,每页数量PageSize,Param参数格式L new {A="1",B="2"}</param>
        /// <returns>结果集合</returns>
        Task<Paged<TResult>> QueryPageAsync<TResult>(string selectSql, string whereSql, string orderbySql, IQuery query);

        /// <summary>
        /// 查询分页数据对象
        /// </summary>
        /// <param name="selectSql">截至到from</param>
        /// <param name="whereSql">where</param>
        /// <param name="orderbySql">排序</param>
        /// <param name="query">查询对象,PageIndex起始页,每页数量PageSize,Param参数格式L new {A="1",B="2"}</param>
        /// <returns>结果集合</returns>
        Paged<TResult> QueryPage<TResult>(string selectSql, string whereSql, string orderbySql, IQuery query);
    }
    /// <summary>
    /// Sql数据库访问
    /// </summary>
    public class SqlQuery : ISqlQuery, IDisposable
    {
        #region 属性变量

        /// <summary>
        /// 初始化where 1=1
        /// </summary>
        public string AppendWhere { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public IDbConnection DbConnection { get; set; }
        /// <summary>
        /// 连接配置
        /// </summary>
        private ISqlConfig _sqlConfig { get; set; }
        /// <summary>
        /// 用户上下文
        /// </summary>
        private IUserContext _userContext { get; set; }
        /// <summary>
        /// 日志
        /// </summary>
        private NLog.Logger _log = NLog.LogManager.GetLogger("SqlTraceLog");

        #endregion 

        #region 构造
        /// <summary>
        /// Sql数据库访问
        /// </summary>
        /// <param name="sqlConfig">数据库配置</param>
        /// <param name="userContext">用户上下文</param>
        public SqlQuery(ISqlConfig sqlConfig, IUserContext userContext)
        {
            AppendWhere = " where 1=1 ";
            _sqlConfig = sqlConfig;
            _userContext = userContext;
        }

        #endregion

        #region  数据库操作

        /// <summary>
        /// 查询首个数据对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public TResult First<TResult>(string sql, object param = null)
        {
            return First<TResult>(sql, ToDictionary(param));
        }
        /// <summary>
        /// 查询首个数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public async Task<TResult> FirstAsync<TResult>(string sql, object param = null)
        {
            return await FirstAsync<TResult>(sql, ToDictionary(param));
        }
        /// <summary>
        /// 查询首个数据对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public TResult First<TResult>(string sql, IDictionary<string, object> param = null)
        {
            this.CreateDbConnection();
            this.Open();
            _log.Trace("查询开始：" + DateTime.Now);
            WriteTraceLog(sql, param);
            var query = DbConnection.QueryFirstOrDefault<TResult>(sql, param);
            _log.Trace("查询结束：" + DateTime.Now);
            return query;
        }
        /// <summary>
        /// 查询首个数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public async Task<TResult> FirstAsync<TResult>(string sql, IDictionary<string, object> param = null)
        {
            this.CreateDbConnection();
            this.Open();
            _log.Trace("查询开始：" + DateTime.Now);
            WriteTraceLog(sql, param);
            var query = await DbConnection.QueryFirstOrDefaultAsync<TResult>(sql, param);
            _log.Trace("查询结束：" + DateTime.Now);
            return query;
        }

        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public List<TResult> Query<TResult>(string sql, IDictionary<string, object> param = null)
        {
            this.CreateDbConnection();
            this.Open();
            _log.Trace("查询开始：" + DateTime.Now);
            WriteTraceLog(sql, param);
            var query = DbConnection.Query<TResult>(sql, param).ToList();
            _log.Trace("查询结束：" + DateTime.Now);
            return query;
        }
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public List<TResult> Query<TResult>(string sql, object param = null)
        {
            return this.Query<TResult>(sql, ToDictionary(param));
        }
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, IDictionary<string, object> param = null)
        {
            this.CreateDbConnection();
            this.Open();
            _log.Trace("查询开始：" + DateTime.Now);
            WriteTraceLog(sql, param);
            var query = await DbConnection.QueryAsync<TResult>(sql, param);
            _log.Trace("查询结束：" + DateTime.Now);
            return query;
        }

        /// <summary>
        /// 执行完成执行sql
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public async Task ExecuteAsync(string sql, object param = null)
        {
            this.CreateDbConnection();
            this.Open();
            _log.Trace("执行开始：" + DateTime.Now);
            WriteTraceLog(sql, ToDictionary(param));
            await DbConnection.ExecuteAsync(sql, param);
            _log.Trace("执行结束：" + DateTime.Now);
        }
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null)
        {
            return await this.QueryAsync<TResult>(sql, ToDictionary(param));
        }
        /// <summary>
        /// 查询分页数据对象
        /// </summary>
        /// <param name="selectSql">截至到from之前</param>
        /// <param name="whereSql">where</param>
        /// <param name="orderbySql">排序(必须传入)</param>
        /// <param name="query">查询对象,PageIndex起始页,每页数量PageSize,Param参数格式L new {A="1",B="2"}</param>
        /// <returns>结果集合</returns>
        public Paged<TResult> QueryPage<TResult>(string selectSql, string whereSql, string orderbySql, IQuery query)
        {
            this.CreateDbConnection();
            this.Open();
            Paged<TResult> pagedResult = new Paged<TResult>();
            var parameters = ToDictionary(query.Param);
            var dataSql = selectSql.Add(", ROW_NUMBER() over (" + orderbySql + ") as NUMBER");
            dataSql = dataSql.Add(whereSql);
            dataSql = "select *   from ( " + dataSql + " ) Temp_M where NUMBER<@end and NUMBER>@start";
            var countSql = whereSql.PageCount();
            parameters.Add("start", (query.PageIndex - 1) * query.PageSize);
            parameters.Add("end", query.PageSize * query.PageIndex + 1);
            _log.Trace("分页查询开始：" + DateTime.Now);
            WriteTraceLogPage(dataSql, dataSql, parameters);
            var count = DbConnection.QueryFirst<int>(countSql, parameters);
            var data = DbConnection.Query<TResult>(dataSql, parameters);
            pagedResult.PageIndex = query.PageIndex;
            pagedResult.TotalCount = count;
            pagedResult.Result = data.ToList();
            _log.Trace("分页查询结束：" + DateTime.Now);
            return pagedResult;
        }

        /// <summary>
        /// 查询分页数据对象
        /// </summary>
        /// <param name="selectSql">截至到from之前</param>
        /// <param name="whereSql">where</param>
        /// <param name="orderbySql">排序(必须传入)</param>
        /// <param name="query">查询对象,PageIndex起始页,每页数量PageSize,Param参数格式L new {A="1",B="2"}</param>
        /// <returns>结果集合</returns>
        public async Task<Paged<TResult>> QueryPageAsync<TResult>(string selectSql, string whereSql, string orderbySql, IQuery query)
        {
            this.CreateDbConnection();
            this.Open();
            Paged<TResult> pagedResult = new Paged<TResult>();
            var parameters = ToDictionary(query.Param);
            var dataSql = selectSql.Add(", ROW_NUMBER() over (" + orderbySql + ") as NUMBER");
            dataSql = dataSql.Add(whereSql);
            dataSql = "select *   from ( " + dataSql + " ) Temp_M where NUMBER<@end and NUMBER>@start";
            var countSql = whereSql.PageCount();
            parameters.Add("start", (query.PageIndex - 1) * query.PageSize);
            parameters.Add("end", query.PageSize * query.PageIndex + 1);
            _log.Trace("分页查询开始：" + DateTime.Now);
            WriteTraceLogPage(dataSql, dataSql, parameters);
            var count = await DbConnection.QueryFirstAsync<int>(countSql, parameters);
            var data = await  DbConnection.QueryAsync<TResult>(dataSql, parameters);
            pagedResult.PageIndex = query.PageIndex;
            pagedResult.TotalCount = count;
            pagedResult.Result = data.ToList();
            _log.Trace("分页查询结束：" + DateTime.Now);
            return pagedResult;
        }
        #endregion

        #region 基础操作

        /// <summary>
        /// 设置数据库连接
        /// </summary>
        protected void CreateDbConnection()
        {
            if (DbConnection == null)
            {
                if (_sqlConfig.DbType == DbType.Mysql)
                {
                    DbConnection = new MySqlConnection(_sqlConfig.SqlConnectionString);
                }
                if (_sqlConfig.DbType == DbType.Mssql)
                {
                    DbConnection = new SqlConnection(_sqlConfig.SqlConnectionString);
                }
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数</param>
        protected void WriteTraceLog(string sql, IDictionary<string, object> param)
        {
            var debugSql = sql;
            foreach (var parameter in param)
            {
                debugSql = debugSql.Replace("@" + parameter.Key, SqlHelper.GetParamLiterals(parameter.Value));
            }
            //_log.Content("请求:" + "浏览器：" + Web.Browser + "  请求地址：" + Web.Url)
            var sqlLog = new StringBuilder();
            sqlLog.AppendLine("原始Sql:");
            sqlLog.AppendLine(sql);
            sqlLog.AppendLine("调试Sql：");
            sqlLog.AppendLine(debugSql);
            SqlParams(debugSql, sqlLog, param);
            _log.Trace(sqlLog.ToString());
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="countsql">总条数Sql语句</param>
        /// <param name="param">参数</param>
        protected void WriteTraceLogPage(string sql, string countsql, IDictionary<string, object> param)
        {
            var debugSql = sql;
            foreach (var parameter in param)
            {
                debugSql = debugSql.Replace("@" + parameter.Key, SqlHelper.GetParamLiterals(parameter.Value));
            }
            var sqlLog = new StringBuilder();
            sqlLog.AppendLine("原始Sql:");
            sqlLog.AppendLine(sql);
            sqlLog.AppendLine("调试Sql：");
            sqlLog.AppendLine(debugSql);
            _log.Trace(sqlLog.ToString());
        }
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        protected void Open()
        {
            try
            {
                if (DbConnection == null)
                {
                    CreateDbConnection();
                    if (DbConnection.State == ConnectionState.Closed)
                    {
                        DbConnection.Open();
                    }
                }
                if (DbConnection.State == ConnectionState.Closed)
                {
                    DbConnection.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>

        protected void Close()
        {
            try
            {
                if (DbConnection == null)
                {
                    return;
                }
                if (DbConnection.State == ConnectionState.Open)
                {
                    DbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Close();
            DbConnection?.Dispose();
        }


        /// <summary>
        /// 设置Sql参数
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="value">值</param>
        /// <param name="args">变量值</param>
        private static void SqlParams(string value, StringBuilder sb, params object[] args)
        {
            value.AppendLine(sb, value, args);
        }


        /// <summary>
        /// 设置Sql参数
        /// </summary>
        /// <param name="log">日志操作</param>
        /// <param name="dictionary">字典</param>
        /// <returns></returns>
        public static void SqlParams(string value, StringBuilder sb, IDictionary<string, object> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return;
            }
            SqlParams(value, sb, dictionary.Select(t => $"{t.Key} : {SqlHelper.GetParamLiterals(t.Value)}").Join());
        }
        #endregion

        #region  私有方法

        /// <summary>
        /// 转换为字典类型
        /// </summary>
        /// <param name="param">对象</param>
        /// <returns></returns>
        private static IDictionary<string, object> ToDictionary(object param)
        {
            if (param == null)
                return null;
            var properties = param.GetType().GetProperties();
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var item in properties)
            {
                parameters.Add(item.Name, item.GetValue(param));
            }
            return parameters;
        }

        #endregion

        #region Log


        /// <summary>
        /// 获取Sql
        /// </summary>
        private static string GetSql(string sql, string sqlParams)
        {
            var parameters = GetSqlParameters(sqlParams);
            foreach (var parameter in parameters)
            {
                var regex = new System.Text.RegularExpressions.Regex($@"{parameter.Key}\b", RegexOptions.Compiled);
                sql = regex.Replace(sql, parameter.Value);
            }
            return sql;
        }

        /// <summary>
        /// 获取Sql参数字典
        /// </summary>
        /// <param name="sqlParams">Sql参数</param>
        private static IDictionary<string, string> GetSqlParameters(string sqlParams)
        {
            var result = new Dictionary<string, string>();
            var paramName = GetParamName(sqlParams);
            if (string.IsNullOrWhiteSpace(paramName))
                return result;
            string pattern = $@",\s*?{paramName}";
            var parameters = Common.Helper.Regex.Split(sqlParams, pattern);
            foreach (var parameter in parameters)
                AddParameter(result, parameter, paramName);
            return result;
        }

        /// <summary>
        /// 获取参数名
        /// </summary>
        private static string GetParamName(string sqlParams)
        {
            string pattern = $@"([@].*?)\d+=";
            return Common.Helper.Regex.GetValue(sqlParams, pattern, "$1");
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        private static void AddParameter(Dictionary<string, string> result, string parameter, string paramName)
        {
            string pattern = $@"(@={paramName})?(\d+)='(.*)'(.*)";
            var values = Common.Helper.Regex.GetValues(parameter, pattern, new[] { "$1", "$2", "$3" }).Select(t => t.Value).ToList();
            if (values.Count != 3)
                return;
            result.Add($"{paramName}{values[0]}", GetValue(values[1], values[2]));
        }
        /// <summary>
        /// 添加Sql参数
        /// </summary>
        private static void AddSqlParameter(Dictionary<string, string> result, string parameter)
        {
            var items = parameter.Split('=');
            if (items.Length < 2)
                return;
            result.Add(items[0].Trim(), GetValue(parameter, items[1]));
        }

        /// <summary>
        /// 获取值
        /// </summary>
        private static string GetValue(string parameter, string value)
        {
            value = value.SafeString();
            parameter = parameter.SafeString();
            if (string.IsNullOrWhiteSpace(value) && parameter.Contains("DbType = Guid"))
                return "null";
            return $"'{value}'";
        }

        /// <summary>
        /// 添加字典内容
        /// </summary>
        private void AddDictionary(IDictionary<string, string> dictionary)
        {
            AddElapsed(GetValue(dictionary, "elapsed"));
            var sqlParams = GetValue(dictionary, "parameters");
            AddSql(GetValue(dictionary, "commandText"), sqlParams);
            AddSqlParams(sqlParams);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        private string GetValue(IDictionary<string, string> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            return string.Empty;
        }

        /// <summary>
        /// 添加执行时间
        /// </summary>
        private void AddElapsed(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            //_log.Content($"执行时间: {value} 毫秒");
        }

        /// <summary>
        /// 添加Sql
        /// </summary>
        private void AddSql(string sql, string sqlParams)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return;
            //_log.Sql("原始Sql: ").Sql($"{sql}{Environment.NewLine}");
            //sql = sql.Replace("SET NOCOUNT ON;", "");
            //_log.Sql($"调试Sql: {GetSql(sql, sqlParams)}{Environment.NewLine}");
        }

        /// <summary>
        /// 添加Sql参数
        /// </summary>
        private void AddSqlParams(string value)
        {
            //if (string.IsNullOrWhiteSpace(value))
            //    return;
            //_log.SqlParams(value);
        }

        /// <summary>
        /// 启动开始
        /// </summary>
        /// <returns></returns>
        public SqlQuery Begin()
        {
            this.CreateDbConnection();
            this.Open();
            return this;
        }

        #endregion
    }
}
