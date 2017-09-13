using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTcms.DAL.Partystyle_logic;

namespace DTcms.BLL
{
    public partial class Style
    {
        private readonly DAL.Partystyle_logic dal;
        public Style()
        {
            dal = new DAL.Partystyle_logic();
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            return dal.Exists(id);
        }
        public List<Model.P_ActivityStyle> GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return dal.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }
        public Webstyle GetDetail(string id)
        {
            return dal.GetDetail(id);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool Delete(string id, string userid)
        {
            return dal.Delete(id, userid);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool WebEdit(WebAdd model)
        {
            return dal.WebEdit(model);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public Boolean StyleAdd(WebAdd model)
        {
            return dal.StyleAdd(model);
        }

        /// <summary>
        /// 获取组织内人员列表
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        public List<ZTreeModel> GetMem(ZTreeUserFromBody fromBody)
        {
            return dal.GetMem(fromBody);
        }

        /// <summary>
        /// 获取组织以及下属组织所有人员列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<ZTreeModel> GetGroupAllUsers(string groupId)
        {
            return dal.GetGroupAllUsers(groupId);
        }
    }
}
