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
    /// <summary>
    /// 论坛、支部、个人中心、收藏列表bll
    /// </summary>
    public class ForumBranchBll
    {
        /// <summary>
        /// 支部和论坛发布信息时,获取需要@的人员 列表
        /// </summary>
        /// <param name="groupId">支部或论坛ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="type">区分是支部还是论坛发布信息0-->支部，1-->论坛</param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<AtPersonnelModel> GetNewInfoAtPersonnels(string groupId, string userId, int type, int page, int rows) {
            return new ForumOrBranchDal().GetNewInfoAtPersonnels(groupId, userId, type, page, rows);
        }

        /// <summary>
        /// 获取论坛信息列表
        /// </summary>
        /// <param name="forumId">论坛ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="page">分页页数</param>
        /// <param name="rows">分页条数</param>
        /// <returns></returns>
        public List<ForumBranchModel> GetForumList(string forumId, string userId, int page, int rows) {
            return new ForumOrBranchDal().GetForumList(forumId, userId, page, rows);
        }

        /// <summary>
        /// 获取支部信息列表
        /// </summary>
        /// <param name="branchId">支部ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="page">分页页数</param>
        /// <param name="rows">分页行数</param>
        /// <returns></returns>
        public List<ForumBranchModel> GetBranchList(string branchId, string userId, int page, int rows) {
            return new ForumOrBranchDal().GetbranchList(branchId, userId, page, rows);
        }

        /// <summary>
        /// 获取个人中心主页
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<ForumBranchModel> GetPersonalCenter(string userId, int page, int rows) {
            return new ForumOrBranchDal().GetPersonalCenter(userId, page, rows);
        }

        /// <summary>
        /// 获取个人中心收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<ForumBranchModel> GetPersonalCenterCollect(string userId, int page, int rows) {
            return new ForumOrBranchDal().GetPersonalCenterCollect(userId, page, rows);
        }

        /// <summary>
        /// 获取@我的信息列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<ForumBranchModel> GetAtMyInfoList(string userId, int page, int rows) {
            return new ForumOrBranchDal().GetAtMyInfoList(userId, page, rows);
        }

        /// <summary>
        /// 获取个人中心评论列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<ForumBranchModel> GetCommentsList(string userId, int page, int rows) {
            return new ForumOrBranchDal().GetCommentsList(userId, page, rows);
        }

        /// <summary>
        /// 获取个人中心点赞列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<ForumBranchModel> GetThumbUpList(string userId, int page, int rows) {
            return new ForumOrBranchDal().GetThumbUpList(userId, page, rows);
        }
    }
}
