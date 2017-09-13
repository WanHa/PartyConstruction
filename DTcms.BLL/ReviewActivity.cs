using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class ReviewActivity
    {
        private ReviewActivion dal = new ReviewActivion();
        /// <summary>
        /// 评选活动列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<ReviewModel> GetReView(int userid, int rows, int page,int asstatus)
        {
            return dal.GetReView(userid, rows, page,asstatus);
        }
       
        /// <summary>
        /// 创建评选活动接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean getAddReView(ActivityModel model)
        {
            return dal.getAddReView(model);
        }
        /// <summary>
        /// 评选活动详情接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ReviewActivionModel GetReviewModel(string id, int userid)
        {
            return dal.GetReviewModel(id,userid);
        }
        /// <summary>
        /// 已经结束的投票结果
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ModelToAction EndReviewModel(string id, int userid)
        {
            return dal.EndReviewModel(id,userid);
        }
        /// <summary>
        /// 投票接口
        /// </summary>
        /// <param name="froum"></param>
        /// <returns></returns>
        public Boolean GetFroumCount(FroumCount froum)
        {
            return dal.GetFroumCount(froum);
        }
        public ReviewActivionToModel DetailReviewModel(string id, int userid)
        {
            return dal.DetailReviewModel(id,userid);
        }
        public ModelReview GetReview(string id, int userid)
        {
            return dal.GetReview(id,userid);
        }

    }
}
