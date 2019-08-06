using Angel.IServices;
using Angel.Model.Dto;
using Angel.Model.Query;
using Common.DbAccess;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Angel.Services
{
    /// <summary>
    /// 账号服务
    /// </summary>
    public class LoginServices : ILoginServices
    {
        /// <summary>
        /// 数据访问
        /// </summary>
        private readonly ISqlQuery _sqlQuery;

        /// <summary>
        /// 初始化注入
        /// </summary>
        /// <param name="sqlQuery"></param>
        public LoginServices(ISqlQuery sqlQuery)
        {
            _sqlQuery = sqlQuery;
        }

        /// <summary>
        /// 注册账号是否存在
        /// </summary>
        /// <param name="LoginCode"></param>
        /// <returns></returns>
        public async Task<bool> UserExist(string LoginCode)
        {
            var sql = "select * from Users where LoginCode=@LoginCode";
            var data = await _sqlQuery.FirstAsync<LoginDto>(sql, new { LoginCode = LoginCode });
            if (data == null)
                return false;
            return true;
        }

        /// <summary>
        /// 账号登陆验证
        /// </summary>
        /// <param name="loginQuery"></param>
        /// <returns></returns>
        public async Task<bool> CheckUser(LoginQuery loginQuery)
        {
            var sql = "select * from Users where LoginCode=@LoginCode and PassWord=@PassWord";
            var data = await _sqlQuery.FirstAsync<LoginDto>(sql, new { LoginCode = loginQuery.LoginCode, PassWord = loginQuery.PassWord });
            if (data == null)
                return false;
            return true;
        }

        /// <summary>
        /// 账号注册
        /// </summary>
        /// <param name="loginQuery"></param>
        /// <returns></returns>
        public async Task<bool> Register(LoginQuery loginQuery)
        {
            var id = Guid.NewGuid();
            var time = DateTime.Now;
            var sql = "INSERT INTO Users (Id,LoginCode,PassWord,Phone,DateTime) VALUES (@Id,@LoginCode,@PassWord,@Phone,@DateTime)";
            await _sqlQuery.ExecuteAsync(sql,
                new
                {
                    Id = id,
                    LoginCode = loginQuery.LoginCode,
                    PassWord = loginQuery.PassWord,
                    Phone = loginQuery.Phone,
                    DateTime = time
                });
            return true;
        }

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="loginQuery"></param>
        /// <returns></returns>
        public int PhoneCode(LoginQuery loginQuery)
        {
            string account = "C47841384";//查看用户名 登录用户中心->验证码通知短信>产品总览->API接口信息->APIID
            string password = "8a1134912cc88fa532f9a5d505112114"; //查看密码 登录用户中心->验证码通知短信>产品总览->API接口信息->APIKEY
            string PostUrl = "http://106.ihuyi.com/webservice/sms.php?method=Submit";
            string mobile = loginQuery.Phone;
            Random rad = new Random();
            int mobile_code = rad.Next(1000, 10000);
            string content = "您的验证码是：" + mobile_code + " 。请不要把验证码泄露给其他人。";

            //Session["mobile"] = mobile;
            //Session["mobile_code"] = mobile_code;

            string postStrTpl = "account={0}&password={1}&mobile={2}&content={3}";

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postData = encoding.GetBytes(string.Format(postStrTpl, account, password, mobile, content));

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(PostUrl);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = postData.Length;

            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(postData, 0, postData.Length);
            newStream.Flush();
            newStream.Close();

            return mobile_code;
        }
    }
}
