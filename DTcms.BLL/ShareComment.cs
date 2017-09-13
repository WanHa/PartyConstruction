using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTcms.DAL.PartyBranchManagement;

namespace DTcms.BLL
{
    public class ShareComment
    {
        PartyBranchManagement PBM = new PartyBranchManagement();
        public Boolean sendcomment(discuss model)
        {
            return PBM.Senddiscuss(model);
        }

        public List<commentlist> GetList(int userid,string shareid, int page, int rows)
        {
            return PBM.GetCommentList(userid,shareid, page, rows );
        }

        public List<sharelist> GetContentList(int userid, int groupid, int page, int rows)
        {


            return PBM.GetShareList(userid, groupid,page,rows);

        }


    }
}
