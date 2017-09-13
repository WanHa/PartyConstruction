using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class TokenResult
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string portraitUri { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public string token { get; set; }
    }
}
