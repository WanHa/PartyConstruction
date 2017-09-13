using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class UserGroupHelper
    {
        /// <summary>
        /// 获取用户所属最小党组织ID
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public int GetUserMinimumGroupId(int userId)
        {

            string sql = String.Format(@"select TOP 1 t.value from F_Split(
                    (select dt_users.group_id from dt_users where dt_users.id = {0}),',') as t
                    left join dt_user_groups on dt_user_groups.id = t.value
                    where t.value is not null and dt_user_groups.id is not null
                    order by dt_user_groups.grade DESC", userId);

            object data = DbHelperSQL.GetSingle(sql);

            int minimumGroup = data == null ? 0 : Convert.ToInt32(data);

            return minimumGroup;
        }
    }
}
