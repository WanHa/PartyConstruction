using DTcms.DAL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DTcms.BLL
{
    public class PartysStyle
    {
        private PartyStyle ps = new PartyStyle();
        /// <summary>
        /// 获取列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<partyStyleModel> GetPartyStyleList(int userid, int rows, int page)
        {
            return ps.GetPartyStyleList(userid,rows,page);
        }
        /// <summary>
        /// 获取详情和评论列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailPartyStyleModel DetailPartyStyle(int id)
        {
            return ps.DetailPartyStyle(id);
        }
        /// <summary>
        /// 添加评论接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean InsertComment(PartyCommentModel model)
        {
            return ps.InsertComment(model);
        }
        /// <summary>
        /// 点赞接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Boolean GetThumbUp(int userid, string id, int familytype)
        {
            return ps.GetThumbUp(userid,id,familytype);
        }
        /// <summary>
        /// 删除点赞接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean DelThumbUp(int userid, string id,int familytype)
        {
            return ps.DelThumbUp(userid, id, familytype);
        }
        /// <summary>
        /// 评论接口分页
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<commentModel> GetComment(int userid, int id, int rows, int page)
        {
            return ps.GetComment(userid,id,rows,page);
        }

        public List<PartyStyleImageModel> GetArticleImage(string userId)
        {
            return ps.GetArticleImage(userId);
        }
    }
}
