using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel.FromBody
{
    public class UserRegisteredModel
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
        /// 省份证
        /// </summary>
        public string card { get; set; }

        /// <summary>
        /// 党组织ID
        /// </summary>
        public int groupid { get; set; }

        /// <summary>
        /// 个推
        /// </summary>
        public string clientid { get; set; }
    }
}
