using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class Branch
    {
        private Branchmanagement ptybranch = new Branchmanagement();

        /// <summary>
        /// 获取党组织列表
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<Banch> GetBranchList(GroupManagement group)
        {
            return ptybranch.GetBranchList(group);
        }
        /// <summary>
        /// 个人介绍接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public Details GetDetails(int userid,int groupid)
        {
            return ptybranch.GetDetails(userid,groupid);
        }
        /// <summary>
        /// 发布文字
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean Addchar(Release model)
        {
            return ptybranch.Addchar(model);
        }
        /// <summary>
        /// 发布文字图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean AddImage(Release model)
        {
            return ptybranch.AddImage(model);
        }
        /// <summary>
        /// 发布视频文字
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean AddVideo(Release model)
        {
            return ptybranch.AddVideo(model);
        }
        /// <summary>
        /// 获取组员列表
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<PersonList> GetMembersList(string groupid, int rows, int page)
        {
            return ptybranch.GetMembersList(groupid, rows, page);
        }
        /// <summary>
        /// 获取党组织简介接口
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public BranchLists GetBranch(string groupid)
        {
            return ptybranch.GetBranch(groupid);
        }
        /// <summary>
        /// 搜索党支部接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<BLists> SeachBranch(Where model)
        {
            return ptybranch.SeachBranch(model);
        }
    }
}
