using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class PartyGroupForum
    {
        private DAL.PartyGroupForum dal = new DAL.PartyGroupForum();

        /// <summary>
        /// 创建党小组论坛
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean AddForum(PartyGroupForumModel model)
        {
            return dal.AddForum(model);
        }

        /// <summary>
        /// 获取我所在的/所有的党小组论坛列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<GroupList> GetGroupList(int userid,int type,int rows, int page)
        {
            return dal.GetGroupList(userid,type,rows,page);
        }

        /// <summary>
        /// 获取论坛介绍
        /// </summary>
        /// <param name="groupforumId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ForumInfo GetForumInfo(string groupforumId)
        {
            return dal.GetForumInfo(groupforumId);
        }

        /// <summary>
        /// 党小组申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean CommitApply(CommitApply model)
        {
            return dal.CommitApply(model);
        }

        /// <summary>
        /// 申请列表
        /// </summary>
        /// <param name="groupforumId"></param>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<ApplyList> GetApplyList(string userid, int rows, int page)
        {
            return dal.GetApplyList(userid, rows, page);
        }

        /// <summary>
        /// 党小组论坛申请审批
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean CheckApply(CheckApply model)
        {
            return dal.CheckApply(model);
        }

        /// <summary>
        /// 退出党小组论坛
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean DelGroupForum(DelGroupForum model)
        {
            return dal.DelGroupForum(model);
        }

        /// <summary>
        /// 举报
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean Report(Report model)
        {
            return dal.Report(model);
        }
    }
}
