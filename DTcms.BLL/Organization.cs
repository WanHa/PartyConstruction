using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DTcms.BLL
{
    public partial class Organization
    {
        private readonly Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(); //获得站点配置信息
        private readonly DAL.Organization o;

        public Organization()
        {
            o = new DAL.Organization();
        }

        public int GetCount(string strWhere)
        {
            return o.GetCount(strWhere);
        }

        public bool Add(Model.user_groups model)
        {
            return o.Add(model);
        }

        public bool Update(Model.user_groups model)
        {
            return o.Update(model);
        }

        public bool Delete(int id)
        {
            return o.Delete(id);
        }

        public Model.user_groups GetModel(int id)
        {
            return o.GetModel(id);
        }

        public Model.user_groups DataRowToModel(DataRow row)
        {
            return o.DataRowToModel(row);
        }

        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return o.GetList(Top, strWhere, filedOrder);
        }

        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return o.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }

        public bool Exists(int id)
        {
            return o.Exists(id);
        }

        public DataRow GetEditInfo(int id)
        {
            return o.GetEditInfo(id);
        }

        public bool UpdateInfo(int id, string name, string pname, string manager, string userid,string position,string managerid)
        {
            return o.UpdateInfo(id, name, pname, manager, userid, position, managerid);
        }

        public ArrayList PnameList()
        {
            return o.PnameList();
        }

        public int? GetPid(string pname)
        {
            return o.GetPid(pname);
        }

        public bool CheckRepeat(string name, string pname,string managerid)
        {
            return o.CheckRepeat(name, pname, managerid);
        }

        public string PositionList()
        {
            return o.PositionList();
        }
    }
}
