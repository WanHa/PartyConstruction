using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class PayDal
    {
        /// <summary>
        /// 获取支付订单信息
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        public string GetPayOrder(PayFromBody fromBody) {

            string result = "";

            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder deleteSql = new StringBuilder();
                        deleteSql.Append(@"delete from P_PartyPayMentRecord where P_PartyPayMentRecord.P_OutTradeNo = @P_OutTradeNo");
                        SqlParameter[] deletePar = {
                        new SqlParameter("@P_OutTradeNo", SqlDbType.NVarChar, 50)
                    };
                        deletePar[0].Value = fromBody.pay_id;
                        DbHelperSQL.ExecuteSql(conn, trans, deleteSql.ToString(), deletePar);

                        StringBuilder insertSql = new StringBuilder();
                        insertSql.Append("insert into P_PartyPayMentRecord (");
                        insertSql.Append("P_Id,P_OutTradeNo,P_Content,P_CreateUser,P_Status,P_PayType)");
                        insertSql.Append("values (@P_Id,@P_OutTradeNo,@P_Content,@P_CreateUser,@P_Status,@P_PayType)");
                        SqlParameter[] insertPar = {
                        new SqlParameter("@P_Id",SqlDbType.NVarChar, 50),
                        new SqlParameter("@P_OutTradeNo",SqlDbType.NVarChar, 50),
                        new SqlParameter("@P_Content",SqlDbType.NText, 255),
                        new SqlParameter("@P_CreateUser",SqlDbType.NVarChar, 50),
                        new SqlParameter("@P_Status",SqlDbType.Int, 0),
                        new SqlParameter("@P_PayType",SqlDbType.Int, 0)
                    };
                        insertPar[0].Value = Guid.NewGuid().ToString();
                        insertPar[1].Value = fromBody.pay_id;
                        insertPar[2].Value = fromBody.content;
                        insertPar[3].Value = fromBody.user_id;
                        insertPar[4].Value = 0; // 支付状态 0-->未支付 1-->已支付
                        insertPar[5].Value = fromBody.pay_type; // 支付方式 0-->支付宝 1--> 微信
                        DbHelperSQL.ExecuteSql(conn, trans, insertSql.ToString(), insertPar);
                        trans.Commit();
                        if (fromBody.pay_type == 1)
                        {   // 获取微信
                            WechatPay wx = new WechatPay();
                            result = wx.GetWxPayOrder(fromBody.pay_id);
                        }
                        else
                        {   // 获取支付宝
                            result = GetALiPayOrderStr(fromBody.pay_id);
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// 获取支付宝订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetALiPayOrderStr(string id) {

            StringBuilder sql = new StringBuilder();
            sql.Append("select P_PartyPayMent.P_Title,P_PartyPayMentPeople.P_Money from P_PartyPayMentPeople");
            sql.Append(" left join P_PartyPayMent on P_PartyPayMent.P_Id = P_PartyPayMentPeople.P_PayMentId");
            sql.Append(" where P_PartyPayMentPeople.P_ID = @P_ID");
            SqlParameter[] parameter = {
                new SqlParameter("@P_ID", SqlDbType.NVarChar, 50)
            };
            parameter[0].Value = id;
            DataSet ds = DbHelperSQL.Query(sql.ToString(), parameter);

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                string privateKey = ConfigHelper.GetAppSettings("ALiPayPrivateKey");
                string publicKey = ConfigHelper.GetAppSettings("ALiPayPublicKey");
                string appId = ConfigHelper.GetAppSettings("ALiAppId");
                IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do",
                appId, privateKey, "json", "1.0", "RSA2", publicKey, "utf-8", false);
                AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
                AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
                model.Body = ds.Tables[0].Rows[0]["P_Title"].ToString();
                model.Subject = ds.Tables[0].Rows[0]["P_Title"].ToString();
                model.TotalAmount = ds.Tables[0].Rows[0]["P_Money"].ToString();
                model.ProductCode = "QUICK_MSECURITY_PAY";
                model.OutTradeNo = id;
                model.TimeoutExpress = "15d";
                request.SetBizModel(model);
                request.SetNotifyUrl("http://api.dj.ttonservice.com/v1/pay/ali");
                //这里和普通的接口调用不同，使用的是sdkExecute
                AlipayTradeAppPayResponse response = client.SdkExecute(request);
                return response.Body;
            }

            return "";
        }

        private string GetWeiXinPayOrderStr() {

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        public Boolean ALiPayCalBackFun(ALiPayCallBackModel fromBody) {

            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("update P_PartyPayMentRecord set ");
                        sql.Append("P_PartyPayMentRecord.P_TradeNo = @P_TradeNo,");
                        sql.Append("P_PartyPayMentRecord.P_PaySum = @P_PaySum,");
                        sql.Append("P_PartyPayMentRecord.P_PayTime = @P_PayTime,");
                        sql.Append("P_PartyPayMentRecord.P_Status = @P_Status ");
                        sql.Append("where P_PartyPayMentRecord.P_OutTradeNo = @P_OutTradeNo");
                        SqlParameter[] parameter = {
                        new SqlParameter("@P_TradeNo", SqlDbType.NVarChar, 50),
                        new SqlParameter("@P_PaySum", SqlDbType.Decimal, 2),
                        new SqlParameter("@P_PayTime", SqlDbType.DateTime, 50),
                        new SqlParameter("@P_Status", SqlDbType.Int, 0),
                        new SqlParameter("@P_OutTradeNo", SqlDbType.NVarChar, 50)
                    };
                        parameter[0].Value = fromBody.trade_no;
                        parameter[1].Value = fromBody.receipt_amount;
                        parameter[2].Value = DateTime.Now;
                        parameter[3].Value = 1;
                        parameter[4].Value = fromBody.out_trade_no;

                        DbHelperSQL.ExecuteSql(conn, trans, sql.ToString(), parameter);
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return true;
        }

    }
}
