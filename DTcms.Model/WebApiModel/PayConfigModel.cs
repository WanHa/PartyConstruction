using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class PayConfigModel
    {
        /// <summary>
        /// 支付宝AppId
        /// </summary>
        public string ALiAppId { get; set; }

        /// <summary>
        /// 支付宝公钥
        /// </summary>
        public string ALiPayPublicKey { get; set; }

        /// <summary>
        /// 支付宝私钥
        /// </summary>
        public string ALiPayPrivateKey { get; set; }

        /// <summary>
        /// 微信AppId
        /// </summary>
        public string WeiXinAppId { get; set; }

        /// <summary>
        /// 微信公钥
        /// </summary>
        public string WeiXinPublicKey { get; set; }

        /// <summary>
        /// 微信私钥
        /// </summary>
        public string WeiXinPrivateKey { get; set; }
    }
}
