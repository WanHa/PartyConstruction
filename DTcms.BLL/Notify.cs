using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{ 
    public class Notify
    {
        private DAL.Notify dal = new DAL.Notify();

        public Boolean AddNotify(Model.ztree model)
        {
            return dal.AddNotify(model);
        }

        public Model.ztree GetNotify(int id)
        {
            return dal.GetNotify(id);
        }


        public Boolean EditNotify(Model.ztree model)
        {
            return dal.EditNotify(model);
        }


        public List<ZTreeModel> GetGroup()
        {
            return new DAL.Notify().GetGroup();
        }
    }
}
