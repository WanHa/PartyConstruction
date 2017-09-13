using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel.FromBody
{
    /// <summary>
    /// 用户登录信息
    /// </summary>
   public class UserLoginFormBody
    {
        /// <summary>
        /// 电话号
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }


        /// <summary>
        /// 个推clientid
        /// </summary>
        public string clientid { get; set; }
    }
}
