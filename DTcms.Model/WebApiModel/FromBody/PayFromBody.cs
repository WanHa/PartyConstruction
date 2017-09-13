using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel.FromBody
{
    public class PayFromBody
    {
        /// <summary>
        /// 支付ID
        /// </summary>
        public string pay_id { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public int pay_type { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int user_id { get; set; }
    }
}
