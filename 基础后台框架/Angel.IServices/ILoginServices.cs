using Angel.Model.Query;
using System;
using System.Threading.Tasks;

namespace Angel.IServices
{
    /// <summary>
    /// 账号服务
    /// </summary>
    public interface ILoginServices
    {
        /// <summary>
        /// 注册账号是否存在
        /// </summary>
        /// <param name="LoginCode"></param>
        /// <returns></returns>
        Task<bool> UserExist(string LoginCode);

        /// <summary>
        /// 账号登陆验证
        /// </summary>
        /// <param name="loginQuery"></param>
        /// <returns></returns>
        Task<bool> CheckUser(LoginQuery loginQuery);

        /// <summary>
        /// 账号注册
        /// </summary>
        /// <param name="loginQuery"></param>
        /// <returns></returns>
        Task<bool> Register(LoginQuery loginQuery);

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="loginQuery"></param>
        /// <returns></returns>
        int PhoneCode(LoginQuery loginQuery);
        
    }
}
