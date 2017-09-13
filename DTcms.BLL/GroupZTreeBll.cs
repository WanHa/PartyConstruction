using DTcms.DAL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class GroupZTreeBll
    {
        /// <summary>
        /// 组织ztree列表
        /// </summary>
        /// <returns></returns>
        public List<ZTreeModel> GetGroupZTreeData() {
            return new GroupZTreeBizDal().GetGroupZTreeData();
        }

        /// <summary>
        /// 党委通知编辑时，组织ztree列表
        /// </summary>
        /// <returns></returns>
        public List<ZTreeModel> GetEditGroupZTreeData(string id)
        {
            return new GroupZTreeBizDal().GetEditGroupZTreeData(id);
        }

        /// <summary>
        /// 活动风采编辑时，组织ztree列表
        /// </summary>
        /// <returns></returns>
        public List<ZTreeModel> GetActivityEditGroupZTreeData(string id)
        {
            return new GroupZTreeBizDal().GetActivityEditGroupZTreeData(id);
        }

        /// <summary>
        /// 根据人员ID获取组织列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<int> GetUserGroups(int userId) {
            return new GroupZTreeBizDal().GetUserGroups(userId);
        }
    }
}
