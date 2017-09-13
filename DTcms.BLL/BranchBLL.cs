using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTcms.DAL.BranchDAL;

namespace DTcms.BLL
{
    public class BranchBLL
    {
        DAL.BranchDAL dal = new DAL.BranchDAL();

        //获取列表
        public List<ListModel> getList(string group_id, int rows, int page)
        {
            return dal.getList(group_id, rows, page);
        }
        //申请支部
        public Boolean Applyfor(string P_ApplyUserId, string P_ApplyContent)
        {
            return dal.Applyfor(P_ApplyUserId, P_ApplyContent);
        }



    }
}
