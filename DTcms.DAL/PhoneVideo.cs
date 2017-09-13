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
    /// <summary>
    /// 获取视频记录列表
    /// </summary>
    public class PhoneVideo
    {

        public List<videomodel> GetVideoList(int userid, int rows, int page)
        {
            
            videomodel model = new videomodel();
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select P_Video.P_Id as videoid,P_Video.P_ParentId as courseid,P_Video.P_VideoName as videoname,P_Video.P_VideoPic as pic,P_Video.P_Url as url,P_Video.P_Number as number,P_Video.P_VideoLength as videolength,");
            strsql.Append("P_VideoRecord.P_LastPlaybackTime as lastplaytime,P_VideoRecord.P_MaxPlaybackTime as maxplaytime ");
            strsql.Append("from P_Video ");
            strsql.Append("left join P_VideoRecord on P_VideoRecord.P_VideoId=P_Video.P_Id ");
            strsql.Append("where P_Video.P_Source is NULL and P_VideoRecord.P_UserId='" + userid + "'");

            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_VideoRecord.P_CreateTime desc"));
            DataSetToModelHelper<videomodel> video = new DataSetToModelHelper<videomodel>();

            List <videomodel> result = video.FillModel(ds);

            if (result != null && result.Count > 0) {
                foreach (videomodel item in result)
                {
                    if (item.videolength == 0) {
                        item.videolength = (int)new QiNiuHelper().GetVideoLength(item.url);
                    }
                }
            }

            return result;
        }

        public timemodel Getmaxtime(int userid)
        {
            timemodel time = new timemodel();
            StringBuilder strsql = new StringBuilder();
            
            strsql.Append("select sum(P_MaxPlaybackTime) as maxplaytime from P_VideoRecord where P_VideoRecord.P_QuestionBankId != 0 and  P_VideoRecord.P_UserId='" + userid + "' ");
            DataSet ds = DbHelperSQL.Query(strsql.ToString());
            DataSetToModelHelper<timemodel> model = new DataSetToModelHelper<timemodel>();
            if (ds.Tables[0].Rows[0] != null)
            {
                return model.FillToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 个人中心信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public personalmodel GetPersonalInfo(int userid)
        {
            StringBuilder st = new StringBuilder();
            st.Append("SELECT P_Image.P_ImageUrl AS imageurl,a.P_ImageUrl AS backgroundimage,dt_users.user_name as username FROM dt_users ");
            st.Append("LEFT JOIN P_Image on P_Image.P_ImageId =CONVERT(VARCHAR,dt_users.id) and P_Image.P_ImageType='20011' ");
            st.Append("LEFT JOIN P_Image a on a.P_CreateUser =CONVERT(VARCHAR,dt_users.id) and a.P_ImageType='20013' ");
            st.Append("WHERE dt_users.id = '"+userid+"'");
            DataSet dt = DbHelperSQL.Query(st.ToString());
            DataSetToModelHelper<personalmodel> model = new DataSetToModelHelper<personalmodel>();
            if (dt.Tables[0].Rows[0] != null)
            {
                return model.FillToModel(dt.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
            
        }





    }
    /// <summary>
    /// 个人中心信息
    /// </summary>
    public class personalmodel
    {
        public string username { get; set; }
        public string imageurl { get; set; }
        public string backgroundimage { get; set; }

    }


    public class timemodel
    {
        public int maxplaytime { get; set; }
       

    }

    public class videomodel
    {
        
        public string videoid { get; set; }
        
        public string videoname { get; set; }
        public string pic { get; set; }
        public string url { get; set; }
        public int number { get; set; }


        public int videolength { get; set; }
        //public int playtime { get; set; }
        /// <summary>
        /// 最近一次观看时间
        /// </summary>
        public int lastplaytime { get; set; }

        /// <summary>
        /// 最大观看时间
        /// </summary>
        public int maxplaytime { get; set; }
        public string courseid { get; set; }
    }
  
   

}
