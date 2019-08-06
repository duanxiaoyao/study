using System;
using System.Collections.Generic;
using System.Text;

namespace Angel.Model.Query
{
    /// <summary>
    /// 账号查询对象
    /// </summary>
    public class LoginQuery
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 登陆账号
        /// </summary>
        public string LoginCode { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}
