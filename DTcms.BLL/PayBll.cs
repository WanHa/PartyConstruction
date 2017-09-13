using DTcms.DAL;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class PayBll
    {
        /// <summary>
        /// 支付订单
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        public string GetPayOrder(PayFromBody fromBody) {
            return new PayDal().GetPayOrder(fromBody);
        }

        /// <summary>
        /// 支付宝APP支付成功回调
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        public Boolean ALiPayCalBackFun(ALiPayCallBackModel fromBody) {
            return new PayDal().ALiPayCalBackFun(fromBody);
        }

    }
}
