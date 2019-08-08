using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DbAccess
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public interface ISqlConfig
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        string SqlConnectionString { get; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        DbType DbType { get; }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DbType
    {
        /// <summary>
        /// 
        /// </summary>
        Mysql = 0,
        /// <summary>
        /// 
        /// </summary>
        Mssql = 1,
        /// <summary>
        /// 
        /// </summary>
        Pgsql = 2
    }
    /// <summary>
    /// mysql配置实现
    /// </summary>
    public class MsSqlConfig : ISqlConfig
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public MsSqlConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 数据库连接
        /// </summary>
        public string SqlConnectionString { get => _configuration.GetConnectionString("DefaultConnection"); }
        /// <summary>
        /// 
        /// </summary>
        public DbType DbType { get => DbType.Mysql; }
    }

}
