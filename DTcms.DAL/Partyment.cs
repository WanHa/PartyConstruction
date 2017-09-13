using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class Partyment
    {
        /// <summary>
        /// 党费缴纳详情
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public Detail GetPartypaymentList(int userid)
        {
            Detail model = new Detail();

            string sql = String.Format(@"select dt_users.user_name as username,
                        dt_users.mobile,
                        dt_users.id_card as card,
                        P_PartyPayMentPeople.P_Money as money,
                        P_PartyPayMentPeople.P_ID as moneyid
                        from P_PartyPayMentPeople
                        left join P_PartyPayMent on P_PartyPayMent.P_Id = P_PartyPayMentPeople.P_PayMentId
                        left join dt_users on CONVERT(nvarchar,dt_users.id) = P_PartyPayMentPeople.P_UserID
                        left join P_PartyPayMentRecord on P_PartyPayMentRecord.P_OutTradeNo = P_PartyPayMentPeople.P_ID
                        where P_PartyPayMent.P_PayMentState = 0 and P_PartyPayMentPeople.P_UserID = '{0}' 
                        and (P_PartyPayMentRecord.P_Status is null or P_PartyPayMentRecord.P_Status = 0)", userid);

            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0]!= null && ds.Tables[0].Rows.Count > 0)
            {
                DataSetToModelHelper<Detail> helper = new DataSetToModelHelper<Detail>();
                model = helper.FillToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                model = null;
            }
            return model;
        }

        /// <summary>
        /// 党费缴纳提交接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean Submit(PartyPaymentModel model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //使用字表id查询数据
                        string sql = String.Format(@"select 
                            P_Money,
                            P_UserID,
                            P_Content
                             from P_PartyPayMentPeople
                            WHERE P_PartyPayMentPeople.P_ID = '{0}'", model.p_id);
                        DataSet ds = DbHelperSQL.Query(sql);

                        //通过字表ID更新缴费状态
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_PartyPayMentPeople set ");
                        strSql.Append("P_PayStatus=@P_PayStatus");
                        strSql.Append(" where P_ID=@P_Id ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_PayStatus", SqlDbType.Int,10),
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100)};
                        parameters[0].Value = 1;
                        parameters[1].Value = model.p_id;

                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务                       

                        string a = ds.Tables[0].Rows[0]["P_Money"].ToString();
                        string b = ds.Tables[0].Rows[0]["P_UserID"].ToString();
                        string c = ds.Tables[0].Rows[0]["P_Content"].ToString();

                        //生成数据
                        StringBuilder SstrSql = new StringBuilder();
                        SstrSql.Append("insert into P_PartyPayMentRecord(");
                        SstrSql.Append("P_Id,P_PaySum,P_UserID,P_Content,P_PayTime,P_CreateUser,P_UpdateTime,P_UpdateUser,P_Status)");
                        SstrSql.Append(" values (");
                        SstrSql.Append("@P_Id,@P_PaySum,@P_UserID,@P_Content,@P_PayTime,@P_CreateUser,@P_UpdateTime,@P_UpdateUser,@P_Status)");
                        SqlParameter[] parameters2 = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_PaySum", SqlDbType.Decimal),
                                new SqlParameter("@P_UserId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Content", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_PayTime",SqlDbType.DateTime,100),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                               };
                        parameters2[0].Value = Guid.NewGuid().ToString();
                        parameters2[1].Value = a;
                        parameters2[2].Value = b;
                        parameters2[3].Value = c;
                        parameters2[4].Value = DateTime.Now;
                        parameters2[5].Value = b;
                        parameters2[6].Value = DateTime.Now; ;
                        parameters2[7].Value = b;
                        parameters2[8].Value = 0;
                        
                        object obj1 = DbHelperSQL.GetSingle(conn, trans, SstrSql.ToString(), parameters2);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 获取党费缴纳记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public List<PartyPaymentRecordModel> GetPartypaymentRecord(int userid,int rows,int page)
        {
            List<PartyPaymentRecordModel> model = new List<PartyPaymentRecordModel>();
            string sql = String.Format(@"select dt_users.user_name as username,
                        P_PartyPayMentRecord.P_PaySum as paysum,
                        P_Image.P_ImageUrl as avatar,
                        P_PartyPayMentRecord.P_Content as content,
                        CONVERT(varchar(100), P_PartyPayMentRecord.P_PayTime, 8)as paytime
                        from P_PartyPayMentRecord
                        left join dt_users on CONVERT(nvarchar,dt_users.id) = P_PartyPayMentRecord.P_CreateUser
                        left join P_Image on P_Image.P_ImageId = P_PartyPayMentRecord.P_CreateUser and P_Image.P_ImageType = '{0}'
                        where P_PartyPayMentRecord.P_CreateUser = '{1}'", (int)ImageTypeEnum.头像, userid);
            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, sql, "P_PartyPayMentRecord.P_PayTime"));            
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<PartyPaymentRecordModel> helper = new DataSetToModelHelper<PartyPaymentRecordModel>();
                model = helper.FillModel(ds);
            }
            else
            {
                model = null;
            }
            return model;
        }
    }
}
