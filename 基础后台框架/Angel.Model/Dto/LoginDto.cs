using System;
using System.Collections.Generic;
using System.Text;

namespace Angel.Model.Dto
{
    /// <summary>
    /// 账号实体
    /// </summary>
    public class LoginDto
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
        /// <summary>
        /// 验证码
        /// </summary>
        public int Code { get; set; }
    }
}
