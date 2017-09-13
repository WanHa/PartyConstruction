using DTcms.Common;
using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using WxPayAPI;

namespace DTcms.DAL
{
    public class WechatPay 
    {
        /// <summary>
        /// 统一下单接口返回结果
        /// </summary>
        public WxPayData unifiedOrderResult { get; set; }

        public string GetWxPayOrder(string id)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select P_PartyPayMent.P_Title,P_PartyPayMentPeople.P_Money from P_PartyPayMentPeople");
            sql.Append(" left join P_PartyPayMent on P_PartyPayMent.P_Id = P_PartyPayMentPeople.P_PayMentId");
            sql.Append(" where P_PartyPayMentPeople.P_ID = @P_ID");
            SqlParameter[] parameter = {
                new SqlParameter("@P_ID", SqlDbType.NVarChar, 50)
            };
            parameter[0].Value = id;
            DataSet ds = DbHelperSQL.Query(sql.ToString(), parameter);

            string trade_no = "";
            trade_no = WxPayApi.GenerateOutTradeNo();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                WxPayData model = new WxPayData();
                model.SetValue("body", ds.Tables[0].Rows[0]["P_Title"].ToString());
                model.SetValue("attach", "两吨科技有限公司");
                model.SetValue("out_trade_no", "11111111112222222222333333333310");
                model.SetValue("total_fee", 1);
                model.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
                model.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
                model.SetValue("goods_tag", "党费缴纳");
                model.SetValue("trade_type", "JSAPI");
                model.SetValue("openid", "o6_bmjrPTIm6_2sgVt7hMZOPfL2M");
                //model.SetValue("notify_url","https://");
                WxPayData result = WxPayApi.UnifiedOrder(model);
                if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
                {
                    Log.Error(this.GetType().ToString(), "UnifiedOrder response error!");
                    throw new WxPayException("UnifiedOrder response error!");
                }
                unifiedOrderResult = result;
                return trade_no;
            }
            return "";
        }
    }

    public class WxPayConfig
    {
        //public const string APPID = "wxe311436461ba68b8";
        //public const string APPSECRET = "ac2af304f8db3a3ae4f6aa0b0224586d";
        public const string IP = "192.168.1.1";
        //public const string MCHID = "1233410002";
        //public const string KEY = "e10adc3849ba56abbe56e056f20f883e";

        public const string APPID = "wx2428e34e0e7dc6ef";
        public const string MCHID = "1233410002";
        public const string KEY = "E9A1087F5A0C5F8EBCF26F3C7A842022";
        public const string APPSECRET = "51c56b886b5be869567dd389b3e5d1d6";
    }
}
