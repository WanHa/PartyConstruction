using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class PushMessageHelper
    {
        public static void PushMessages(string messageId, string push_title, List<int> personnels, int userId, int pushType) {
            if (personnels != null && personnels.Count > 0)
            {
                StringBuilder whereStr = new StringBuilder();

                foreach (int item in personnels)
                {
                    whereStr.Append(item);
                    whereStr.Append(",");
                }
                // 获取被@人员个推clientid
                string sql = String.Format(@"select dt_users.client_id from dt_users
                        where dt_users.id in ({0}) and dt_users.client_id is not null", whereStr.Remove(whereStr.Length - 1, 1).ToString());

                DataSet ds = DbHelperSQL.Query(sql);
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GeTuiPushModel pushData = new GeTuiPushModel()
                        {
                            push_type = pushType,
                            push_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            push_title = push_title,
                            user_id = userId,
                            message_id = messageId
                        };
                        // 推送信息
                        PushMessage.PushMessageToSingle(dr["client_id"].ToString(), JsonHelper.ObjectToJSON(pushData));
                    }
                }

            }
        }


        public static void PushMessages(string messageId, string push_title, List<string> personnels, int userId, int pushType)
        {
            if (personnels != null && personnels.Count > 0)
            {
                StringBuilder whereStr = new StringBuilder();

                foreach (string item in personnels)
                {
                    whereStr.Append(item);
                    whereStr.Append(",");
                }
                // 获取被@人员个推clientid
                string sql = String.Format(@"select dt_users.client_id from dt_users
                        where dt_users.id in ({0}) and dt_users.client_id is not null", whereStr.Remove(whereStr.Length - 1, 1).ToString());

                DataSet ds = DbHelperSQL.Query(sql);
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        GeTuiPushModel pushData = new GeTuiPushModel()
                        {
                            push_type = pushType,
                            push_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            push_title = push_title,
                            user_id = userId,
                            message_id = messageId
                        };
                        // 推送信息
                        PushMessage.PushMessageToSingle(dr["client_id"].ToString(), JsonHelper.ObjectToJSON(pushData));
                    }
                }

            }
        }



    }
}
