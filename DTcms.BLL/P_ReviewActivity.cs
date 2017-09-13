using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public partial class P_ReviewActivity
    {
        private readonly DAL.P_ReviewActivity dal;

        public P_ReviewActivity()
        {
            dal = new DAL.P_ReviewActivity();
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public List<Model.P_ReviewActivity> GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return dal.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.P_ReviewActivity GetModel(string id)
        {
            return dal.GetModel(id);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id,string userid)
        {
          
            return dal.Delete(id,userid);
        }
    }
}
