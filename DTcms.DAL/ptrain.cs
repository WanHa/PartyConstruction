using DTcms.Common;
using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class ptrain
    {
        /// <summary>
        /// 查找诉求总数
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DemandCount GetDetails(int year)
        {
            DemandCount model = new DemandCount();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(P_Id) as sum from P_UserDemand");
            strSql.Append(" where YEAR(P_CreateTime) ='" + year + @"' GROUP BY MONTH(P_CreateTime)");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            DataSetToModelHelper<DemandCount> helper = new DataSetToModelHelper<DemandCount>();
            if (ds.Tables[0].Rows.Count>0)
            {
                model = helper.FillToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// 按照月份查找处理中数量
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DemandCount GetPending(int year)
        {
            DemandCount model = new DemandCount();
            StringBuilder str = new StringBuilder();       
            str.Append("select count(*)as sum from (select P_UserDemand.P_Id as id,");
            str.Append("P_UserDemand.P_CheckStatus as checkstatus,CONVERT(varchar(100), P_UserDemand.P_CreateTime, 23) as createtime,");
            str.Append("(select count(P_UserDemandSublist.P_ID) from P_UserDemandSublist where P_UserDemandSublist.P_UDId = P_UserDemand.P_Id) as bbbb");
            str.Append(" from P_UserDemand LEFT JOIN dt_users on P_UserDemand.P_CreateUser = dt_users.id");
            str.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id =(select TOP 1 t.value from F_Split(");
            str.Append("(select dt_users.group_id from dt_users where dt_users.id = P_UserDemand.P_CreateUser),',') as t ");
            str.Append(" left join dt_user_groups on dt_user_groups.id = t.value");
            str.Append(" where t.value != '' order by dt_user_groups.grade DESC)) as b");
            str.Append(" where (b.checkstatus = -1 and b.bbbb = 0) or (b.checkstatus = 1 and b.bbbb = 2)");
            str.Append(" and year(b.createtime)='" + year + @"' GROUP BY month(b.createtime)");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<DemandCount> helper = new DataSetToModelHelper<DemandCount>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                model = helper.FillToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                model = null;
            }
            return model;
        }
        public DemandCount GetFinishDemand(int year)
        {
            DemandCount model = new DemandCount();
            StringBuilder str = new StringBuilder();
            str.Append("select count(*) as sum from (select P_UserDemand.P_Id as id,CONVERT(varchar(100), P_UserDemand.P_CreateTime, 23) as createtime,");
            str.Append(" P_UserDemand.P_CheckStatus as checkstatus,(select count(P_UserDemandSublist.P_ID) ");
            str.Append(" from P_UserDemandSublist where P_UserDemandSublist.P_UDId = P_UserDemand.P_Id) as counts");
            str.Append(" from P_UserDemand LEFT JOIN dt_users on P_UserDemand.P_CreateUser = dt_users.id");
            str.Append(" LEFT JOIN dt_user_groups on dt_user_groups.id =(select TOP 1 table1.value from F_Split( ");
            str.Append(" (select dt_users.group_id from dt_users where dt_users.id = P_UserDemand.P_CreateUser),',') as table1");
            str.Append(" left join dt_user_groups on dt_user_groups.id = table1.value where table1.value != '' order by dt_user_groups.grade DESC)) as t");
            str.Append(" where (t.checkstatus = 0 and t.counts =1) or (t.checkstatus = -1 and t.counts = 1) or (t.checkstatus = 1 and t.counts = 3)");
            str.Append(" and year(t.createtime)='2017' GROUP BY month(t.createtime)");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<DemandCount> helper = new DataSetToModelHelper<DemandCount>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                model = helper.FillToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                model = null;
            }
            return model;
        }
    }
    public class DemandCount
    {
        public int sum { get; set; }
    }
}
