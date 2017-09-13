using DTcms.DAL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DTcms.BLL
{
    public class LearningExchange
    {
        private LearnExchange learn = new LearnExchange();

        /// <summary>
        /// 学习交流列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<LearnModel> GetList(int userid, int rows, int page)
        {
            return learn.GetLearningList(userid, rows, page);
        }

        /// <summary>
        /// 学习交流新增接口
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        //public DataSet GetXinZeng(int id,int user_id)
        //{
        //    return learn.GetXinZeng(id,user_id);
        //}

        /// <summary>
        /// 学习交流提交接口
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public Boolean TiJiao(Aticle article)
        {
            return learn.TiJiao(article);
        }

        /// <summary>
        /// 学习交流详情页面接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailLearnModel GetXiangQing(string id)
        {
            return learn.GetXiangQing(id);
        } 
    }
}
