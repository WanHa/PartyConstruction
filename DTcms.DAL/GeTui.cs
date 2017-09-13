using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class GeTui
    {
        public static string GetClientId(int userid)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select dt_users.client_id as clientid from dt_users where dt_users.id=" + userid + " ");
            string ds = Convert.ToString(DbHelperSQL.GetSingle(strsql.ToString()));
            if(string.IsNullOrEmpty(ds))
            {
                return null;
                    
            }

            return ds;

        }
    }
}
