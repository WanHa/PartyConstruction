using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class GroupZTreeBizDal
    {
        /// <summary>
        /// 获取组织ztree列表数据
        /// </summary>
        /// <returns></returns>
        public List<ZTreeModel> GetGroupZTreeData() {

            string sql = String.Format(@"select dt_user_groups.id,
                dt_user_groups.pid as pId,
                dt_user_groups.title as name from dt_user_groups
                where dt_user_groups.pid is null");

            DataSet ds = DbHelperSQL.Query(sql);
            DataSetToModelHelper<ZTreeModel> helper = new DataSetToModelHelper<ZTreeModel>();
            List<ZTreeModel> fristGroup = helper.FillModel(ds);
            if (fristGroup != null && fristGroup.Count > 0) {
                List<ZTreeModel> result = new List<ZTreeModel>();
                foreach (ZTreeModel item in fristGroup)
                {
                    result.Add(item);
                    GetChild(item.id,result);
                }
                return result;
            }
            return new List<ZTreeModel>();
        }

        private void GetChild(int pid, List<ZTreeModel> result) {

            string sql = String.Format(@"select dt_user_groups.id,
                dt_user_groups.pid as pId,
                dt_user_groups.title as name from dt_user_groups
                where dt_user_groups.pid = {0}", pid);
            DataSet ds = DbHelperSQL.Query(sql);
            DataSetToModelHelper<ZTreeModel> helper = new DataSetToModelHelper<ZTreeModel>();
            List<ZTreeModel> childList = helper.FillModel(ds);
            if (childList != null && childList.Count > 0) {
                foreach (ZTreeModel item in childList)
                {
                    result.Add(item);
                    GetChild(item.id,result);
                }
            }
        }

        /// <summary>
        /// 获取编辑党委通知组织ztree列表数据
        /// </summary>
        /// <returns></returns>
        public List<ZTreeModel> GetEditGroupZTreeData(string id) {
            string sql = String.Format(@"select dt_user_groups.id,
                dt_user_groups.pid as pId,
                dt_user_groups.title as name from dt_user_groups
                where dt_user_groups.pid is null");

            DataSet ds = DbHelperSQL.Query(sql);
            DataSetToModelHelper<ZTreeModel> helper = new DataSetToModelHelper<ZTreeModel>();
            List<ZTreeModel> fristGroup = helper.FillModel(ds);
            if (fristGroup != null && fristGroup.Count > 0)
            {
                List<ZTreeModel> result = new List<ZTreeModel>();
                foreach (ZTreeModel item in fristGroup)
                {
                    result.Add(item);
                    GetChild(item.id, result);
                }
                foreach (ZTreeModel item in result)
                {
                    GetIsChecked(id,item);
                }
                return result;
            }
            return new List<ZTreeModel>();
        }

        private void GetIsChecked(string id,ZTreeModel item) {
            // 组织下的人数
            string userSql = String.Format(@"select count(*) from dt_users where dt_users.group_id like '%,{0},%'", item.id);
            int user_count = Convert.ToInt32(DbHelperSQL.GetSingle(userSql));
            string Sql = String.Format(@"select count(*) From P_PartyCommitteeNotify
                    where P_ToUser in (
                    select dt_users.id from dt_users where dt_users.group_id like '%,{0},%'
                    ) and P_Relation = {1}", item.id, id);
            int data_count = Convert.ToInt32(DbHelperSQL.GetSingle(Sql));
            if (user_count > 0 && user_count == data_count) {
                item.@checked = true;
            }
        }

        /// <summary>
        /// 获取编辑活动风采组织ztree列表数据
        /// </summary>
        /// <returns></returns>
        public List<ZTreeModel> GetActivityEditGroupZTreeData(string id)
        {
            string sql = String.Format(@"select dt_user_groups.id,
                dt_user_groups.pid as pId,
                dt_user_groups.title as name from dt_user_groups
                where dt_user_groups.pid is null");

            DataSet ds = DbHelperSQL.Query(sql);
            DataSetToModelHelper<ZTreeModel> helper = new DataSetToModelHelper<ZTreeModel>();
            List<ZTreeModel> fristGroup = helper.FillModel(ds);
            if (fristGroup != null && fristGroup.Count > 0)
            {
                List<ZTreeModel> result = new List<ZTreeModel>();
                foreach (ZTreeModel item in fristGroup)
                {
                    result.Add(item);
                    GetChild(item.id, result);
                }
                foreach (ZTreeModel item in result)
                {
                    GetActivityIsChecked(id,item);
                }
                return result;
            }
            return new List<ZTreeModel>();
        }

        private void GetActivityIsChecked(string id, ZTreeModel item) {
            // 组织下的人数
            string userSql = String.Format(@"select count(*) from dt_users where dt_users.group_id like '%,{0},%'", item.id);
            int user_count = Convert.ToInt32(DbHelperSQL.GetSingle(userSql));
            string activitySql = String.Format(@"select count(*) from P_ActivityStyleSublist
                    where P_ActivityStyleSublist.P_Participant in (
                    select id from dt_users where dt_users.group_id like '%,{0},%'
                    ) and P_ActivityStyleSublist.P_Relation = '{1}'", item.id, id);
            int data_count = Convert.ToInt32(DbHelperSQL.GetSingle(activitySql));
            if (user_count > 0 && user_count == data_count)
            {
                item.@checked = true;
            }
        }

        public List<int> GetUserGroups(int userid) {
            List<int> result = new List<int>();
            string sql = String.Format(@"select t.value from F_Split((
                select group_id from dt_users where dt_users.id = {0}
                ),',') t 
                where t.value != ''", userid);

            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    result.Add(Convert.ToInt32(item["value"].ToString()));
                }
            }

            return result;
        }

    }
}
