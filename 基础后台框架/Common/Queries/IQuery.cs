using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
namespace Common.Queries
{
    /// <summary>
    /// 查询接口
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// 启始页
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 查询对象(用对dapper构建参数时用)
        /// </summary>
        object Param { get; set; }
        /// <summary>
        ///获取前多少条
        /// </summary>
        int? Top { get; set; }
    }

    /// <summary>
    /// 查询接口
    /// </summary>
    public class QueryBase : IQuery
    {

        public QueryBase()
        {
            PageIndex = 1;
            PageSize = 15;
        }
        /// <summary>
        /// 启始页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 查询对象(用对dapper构建参数时用)
        /// </summary>
        public object Param { get; set; }
        /// <summary>
        ///获取前多少条
        /// </summary>
        public int? Top { get; set; }
    }
}
