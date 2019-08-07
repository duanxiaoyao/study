using Angel.IServices.ILogin;
using Angel.Model.Query;
using Common.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Angel.Controllers.Login
{
    /// <summary>
    /// 账号服务
    /// </summary>
    [Route("api/[controller]")]
    public class LoginController : BaseApiController
    {
        private readonly ILoginServices _loginServices;

        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="loginServices"></param>
        public LoginController(ILoginServices loginServices)
        {
            _loginServices = loginServices;
        }

        /// <summary>
        /// 注册账号是否存在
        /// </summary>
        /// <param name="LoginCode">账号</param>
        /// <returns></returns>
        [HttpPost("userExist")]
        public async Task<IActionResult> UserExist(string LoginCode)
        {
            var data = await _loginServices.UserExist(LoginCode);
            return Success(data);
        }

        /// <summary>
        /// 账号登陆验证
        /// </summary>
        /// <param name="loginQuery"></param>
        /// <returns></returns>
        [HttpPost("checkUser")]
        public async Task<IActionResult> CheckUser(LoginQuery loginQuery)
        {
            var data = await _loginServices.CheckUser(loginQuery);
            return Success(data);
        }

        /// <summary>
        /// 账号注册
        /// </summary>
        /// <param name="loginQuery"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginQuery loginQuery)
        {
            var data = await _loginServices.Register(loginQuery);
            return Success(data);
        }

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="loginQuery"></param>
        /// <returns></returns>
        [HttpPost("phoneCode")]
        public IActionResult PhoneCode(LoginQuery loginQuery)
        {
            var data = _loginServices.PhoneCode(loginQuery);
            return Success(data);
        }
    }
}
