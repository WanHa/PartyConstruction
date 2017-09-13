using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public partial class PartyCloud
    {
        private readonly Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(); //获得站点配置信息
        private readonly DAL.PartyCloud pc;

        public PartyCloud()
        {
            pc = new DAL.PartyCloud();
        }

        public List<Item> listFiles(int pageSize,int page,Tuple<string, string> strWhere,out int totalCount)
        {
            return pc.listFiles(pageSize,page,strWhere,out totalCount);
        }

        public bool delete(string key)
        {
            return pc.delete(key);
        }

        public bool VisitRecord(string userid, string fileid)
        {
            return pc.VisitRecord(userid, fileid);
        }

        public bool DownloadRecord(string userid, string fileid)
        {
            return pc.DownloadRecord(userid, fileid);
        }
    }
}
