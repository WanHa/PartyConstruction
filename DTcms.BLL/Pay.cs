using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTcms.DAL;

namespace DTcms.BLL
{
    public class Pay
    {
        private Partyment partypayment = new Partyment();

        /// <summary>
        /// 党费缴纳详情
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public Detail GetPartypaymentList(int userid)
        {
            return partypayment.GetPartypaymentList(userid);
        }
        /// <summary>
        /// 党费缴纳提交接口
        /// </summary>
        /// <param name = "model" ></ param >
        /// < returns ></ returns >
        public Boolean Submit(PartyPaymentModel model)
        {
            return partypayment.Submit(model);
        }
        /// <summary>
        /// 获取党费缴纳记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<PartyPaymentRecordModel> GetPartypaymentRecord(int userid,int rows,int page)
        {
            return partypayment.GetPartypaymentRecord(userid,rows,page);
        }
    }
}
