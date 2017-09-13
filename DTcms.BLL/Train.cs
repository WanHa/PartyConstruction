using DTcms.DAL;
using DTcms.Model;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class Train
    {
        private Ptytrain ptytrn = new Ptytrain();

        /// <summary>
        /// 党员诉求列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<member> GetPartytrainList(int userid, int rows, int page, int asstatus)
        {
            return ptytrn.GetPartytrainList(userid, rows, page, asstatus);
        }
        /// <summary>
        /// 书记诉求列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<Trainlist> GetLeadList(int userid,int rows, int page, int asstatus)
        {
            return ptytrn.GetLeadList(userid,rows, page, asstatus);
        }
        /// <summary>
        /// 新增用户诉求接口
        /// </summary>
        /// <param name = "model" ></ param >
        /// < returns ></ returns >
        public Boolean Add(PartytrainModel model)
        {
            return ptytrn.Add(model);
        }
        /// <summary>
        /// 回复接口
        /// </summary>
        /// <param name = "model" ></ param >
        /// < returns ></ returns >
        public Boolean Reply(PartytrainModel model)
        {
            return ptytrn.Reply(model);
        }
        /// <summary>
        /// 加急接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartyUrgent GetPartyUrgent(int userid,string id)
        {
            return ptytrn.GetPartyUrgent(userid,id);
        }
        /// <summary>
        /// 满意状态接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartySatisfaction GetPartySatisfaction(int userid, string id)
        {
            return ptytrn.GetPartySatisfaction(userid, id);
        }
        public SumCount GetPendingCount(int userid)
        {
            return ptytrn.GetPendingCount(userid);
        }
    }
}
