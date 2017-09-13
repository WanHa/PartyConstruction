using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class SelectAty
    {
        private Activity dal = new Activity();
        /// <summary>
        /// 评选活动列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<ViewModel> GetView(int userid, int rows, int page)
        {
            return dal.GetView(userid, rows, page);
        }
        /// <summary>
        /// 已投票 任何人可见
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        public ViewActivionToModel DetailReviewModel(string id, int userid)
        {
            return dal.DetailReviewModel(id, userid);
        }
        /// <summary>
        /// 投完票 投票后可见
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        public ModelView GetView(string id, int userid)
        {
            return dal.GetView(id, userid);
        }
        /// <summary>
        /// 投票已完成页面接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public EndToAction EndViewModel(string id, int userid)
        {
            return dal.EndViewModel(id, userid);
        }
    }
}
