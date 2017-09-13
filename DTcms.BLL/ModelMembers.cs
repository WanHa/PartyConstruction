using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using DTcms.Common;
using DTcms.DAL;

namespace DTcms.BLL
{
    public partial class ModelMembers
    {
        private readonly Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(); //获得站点配置信息
        private readonly DAL.ModelMember mm;

        public ModelMembers()
        {
            mm = new DAL.ModelMember();
        }
        /// <summary>
        /// 返回数据总数
        /// </summary>
        public int GetCount(string strWhere)
        {
            return mm.GetCount(strWhere);
        }

        public bool Exists(string id)
        {
            return mm.Exists(id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Model.P_ModelPartyMember model)
        {
            return mm.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.P_ModelPartyMember model)
        {
            return mm.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id)
        {
            return mm.Delete(id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.P_ModelPartyMember GetModel(string id)
        {
            return mm.GetModel(id);
        }

        public DataRow GetEditInfo(string id)
        {
            return mm.GetEditInfo(id);
        }

        public string GetUserId(string name)
        {
            return mm.GetUserId(name);
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return mm.GetList(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return mm.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }

        public List<Name> GetUserNameList(string key)
        {
            return mm.GetUserNameList(key);
        }

        public bool NewRole(string name,string userid)
        {
            return mm.NewRole(name,userid);
        }

        public bool UpsetRole(string id, string name, string des,string userid)
        {
            return mm.UpdateRoleInfo(id, name, des,userid);
        }

        public bool CheckRepeat(string name)
        {
            return mm.CheckRepeat(name);
        }
    }
}
