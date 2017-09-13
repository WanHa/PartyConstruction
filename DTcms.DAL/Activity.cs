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
    public class Activity
    {
        /// <summary>
        /// 评选活动列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="asstatus"></param>
        /// <returns></returns>
        public List<ViewModel> GetView(int userid, int rows, int page)
        {
            List<ViewModel> mo = new List<ViewModel>();
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select P_ReviewActivity.P_Id as id,P_ReviewActivity.P_Title as title,CONVERT(varchar(100),P_ReviewActivity.P_CreateTime, 23) as starttime,");
            strsql.Append("P_ReviewActivity.P_VoteResult as voteresult,(case when P_ReviewActivity.P_EndTime > GETDATE() then 0 else 1 end) as status,");
            strsql.Append("CASE when (select count(P_VotePerson.P_Id) from P_VotePerson ");
            strsql.Append(" where P_VotePerson.P_VoteUserID='" + userid + @"' and P_VotePerson.P_ActivityId=P_ReviewActivity.P_Id )=0");
            strsql.Append(" then 0 ELSE 1 END as userfroum from P_ReviewActivity");
            strsql.Append(" LEFT JOIN P_VotePerson on P_VotePerson.P_ActivityId=P_ReviewActivity.P_Id");
            strsql.Append(" where P_VotePerson.P_VoteUserID = '" + userid + @"'");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_ReviewActivity.P_CreateTime"));
            DataSetToModelHelper<ViewModel> model = new DataSetToModelHelper<ViewModel>();
            if(dt.Tables[0].Rows.Count!=0)
            {
                mo = model.FillModel(dt);
            }
            else
            {
                mo = null;
            }
            return mo;
        }
        /// <summary>
        /// 已投票 任何人可见
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ViewActivionToModel DetailReviewModel(string id, int userid)
        {
            ViewActivionToModel model = new ViewActivionToModel();
            StringBuilder str = new StringBuilder();
            str.Append(" select P_ReviewActivity.P_Id as id,P_ReviewActivity.P_Title as title,");
            str.Append("(select user_name from dt_users where dt_users.id=P_ReviewActivity.P_CreateUser) as createuser,");
            str.Append(" (select top 1 P_VotePerson.P_NameStatus from P_VotePerson where P_VotePerson.P_VoteUserID='" + userid + @"' and P_VotePerson.P_ActivityId='" + id + @"') as namestatus,");
            str.Append(" (CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/(3600*24) as VARCHAR)+'天'+");
            str.Append(" CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/3600-");
            str.Append(" ((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/(3600*24)*24 as VARCHAR)+'时'+");
            str.Append(" CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%60))/60-");
            str.Append(" ((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/60 as VARCHAR)+'分') as time,");
            str.Append(" (select count(P_VotePerson.P_Id) from P_VotePerson where P_VotePerson.P_ActivityId='" + id + @"') as usercount");
            str.Append(" from P_ReviewActivity");
            str.Append(" where P_ReviewActivity.P_EndTime>GETDATE() and P_ReviewActivity.P_Id='" + id + @"' and P_ReviewActivity.P_status=0 ");
            DataSet dt = DbHelperSQL.Query(str.ToString());
            if (dt.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<ViewActivionToModel> review = new DataSetToModelHelper<ViewActivionToModel>();
                model = review.FillToModel(dt.Tables[0].Rows[0]);
                StringBuilder ss = new StringBuilder();
                ss.Append("select CONVERT(FLOAT,table1.a*(1.0) / table2.b*(1.0)) as schedcount,* from (");
                ss.Append("	SELECT ");
                ss.Append(" COUNT(P_VotePerson.P_Id) AS a,");
                ss.Append(" P_VoteOption.P_ActivityId , ");
                ss.Append("P_VoteOption.P_Option as optioncontent,P_Image.P_ImageUrl as imgurl");
                ss.Append(" from P_VoteOption");
                ss.Append(" left join P_VotePerson on P_VotePerson.P_ByVoteUserId = P_VoteOption.P_Id");
                ss.Append(" left join P_Image on P_Image.P_ImageId=P_VoteOption.P_Id and P_Image.P_ImageType='" + (int)ImageTypeEnum.评选活动 + @"' ");
                ss.Append(" where P_VoteOption.P_ActivityId ='" + id + @"'");
                ss.Append(" group by P_VoteOption.P_Id,P_VoteOption.P_Option,P_VoteOption.P_ActivityId,P_Image.P_ImageUrl");
                ss.Append(" )table1 ");
                ss.Append(" left join (");
                ss.Append(" select P_VoteOption.P_ActivityId,COUNT(P_VotePerson.P_Id) AS b ");
                ss.Append(" from P_VoteOption ");
                ss.Append(" left join P_VotePerson on P_VotePerson.P_ByVoteUserId = P_VoteOption.P_Id ");
                ss.Append(" where P_VoteOption.P_ActivityId = '" + id + @"'");
                ss.Append(" group by P_VoteOption.P_ActivityId ) table2 on table2.P_ActivityId = table1.P_ActivityId");
                DataSet dd = DbHelperSQL.Query(ss.ToString());
                if (dd.Tables[0].Rows.Count != 0)
                {
                    DataSetToModelHelper<VoteOpToModel> votemodel = new DataSetToModelHelper<VoteOpToModel>();
                    List<VoteOpToModel> voteoption = votemodel.FillModel(dd);
                    model.options = voteoption;
                }
                else
                {
                    model.options = null;
                }
            }
            else
            {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// 投完票 投票后可见
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ModelView GetView(string id, int userid)
        {
            ModelView model = new ModelView();
            StringBuilder strsql = new StringBuilder();
            strsql.Append(" select P_ReviewActivity.P_Id as id,P_ReviewActivity.P_Title as title,");
            strsql.Append("  (select top 1 P_VotePerson.P_NameStatus from P_VotePerson where P_VotePerson.P_VoteUserID='" + userid + @"' and P_VotePerson.P_ActivityId='" + id + @"') as namestatus,");
            strsql.Append(" (select user_name from dt_users where dt_users.id=P_ReviewActivity.P_CreateUser) as createuser, ");
            strsql.Append(" (CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/(3600*24) as VARCHAR)+'天'+");
            strsql.Append(" CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/3600- ");
            strsql.Append(" ((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/(3600*24)*24 as VARCHAR)+'时'+ ");
            strsql.Append(" CAST(((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%60))/60- ");
            strsql.Append(" ((DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime))-(DATEDIFF(second,GETDATE(),P_ReviewActivity.P_EndTime)%3600))/60 as VARCHAR)+'分') as time, ");
            strsql.Append(" (select count(P_VotePerson.P_Id) from P_VotePerson where P_VotePerson.P_ActivityId='" + id + @"' ) as usercount  ");
            strsql.Append(" from P_ReviewActivity ");
            strsql.Append(" where P_ReviewActivity.P_EndTime>GETDATE() and P_ReviewActivity.P_Id='" + id + @"' and P_ReviewActivity.P_status=0 ");
            DataSet dd = DbHelperSQL.Query(strsql.ToString());
            if (dd.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<ModelView> voteoptionmodel = new DataSetToModelHelper<ModelView>();
                model = voteoptionmodel.FillToModel(dd.Tables[0].Rows[0]);
                StringBuilder str = new StringBuilder();
                str.Append("select P_Image.P_ImageUrl as imgurl,P_VoteOption.P_Option as optioncontent,");
                str.Append(" CASE WHEN (SELECT COUNT(P_VotePerson.P_Id) from P_VotePerson where P_VotePerson.P_ByVoteUserId=P_VoteOption.P_Id ");
                str.Append("and P_VotePerson.P_VoteUserID='" + userid + @"')=0 THEN 0 ELSE 1 END as userselect ");
                str.Append(" from P_VoteOption ");
                str.Append(" LEFT JOIN P_Image on P_Image.P_ImageId=P_VoteOption.P_Id and P_Image.P_ImageType='" + (int)ImageTypeEnum.评选活动 + @"' ");
                str.Append(" where P_VoteOption.P_ActivityId='" + id + @"' and P_VoteOption.P_status=0 ");
                DataSet dt = DbHelperSQL.Query(str.ToString());
                if (dt != null)
                {
                    DataSetToModelHelper<VoteModelOpen> voteonmodel = new DataSetToModelHelper<VoteModelOpen>();
                    model.options = voteonmodel.FillModel(dt);
                }
                else
                {
                    model.options = null;
                }
            }
            else
            {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// 投票已完成页面接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public EndToAction EndViewModel(string id, int userid)
        {
            EndToAction model = new EndToAction();
            StringBuilder str1 = new StringBuilder();
            str1.Append("select P_ReviewActivity.P_Id as id,P_ReviewActivity.P_Title as title,");
            str1.Append("(select user_name from dt_users where dt_users.id=P_ReviewActivity.P_CreateUser) as createuser,");
            str1.Append("CONVERT(varchar(100), P_ReviewActivity.P_EndTime,120) as endtime,");
            str1.Append("(select count(P_VotePerson.P_Id) from P_VotePerson where P_VotePerson.P_ActivityId='" + id + @"') as usercount ");
            str1.Append(" from P_ReviewActivity where P_ReviewActivity.P_Id='" + id + @"' and P_ReviewActivity.P_status=0 ");
            DataSet dt1 = DbHelperSQL.Query(str1.ToString());
            if (dt1.Tables[0].Rows.Count != 0)
            {
                DataSetToModelHelper<EndToAction> reviewactionmodel = new DataSetToModelHelper<EndToAction>();
                model = reviewactionmodel.FillToModel(dt1.Tables[0].Rows[0]);
                StringBuilder ss1 = new StringBuilder();
                ss1.Append("select count(P_VotePerson.P_Id) as playcount,P_VoteOption.P_Option as optioncontent, ");
                ss1.Append(" P_Image.P_ImageUrl as imgurl from P_VoteOption ");
                ss1.Append(" LEFT JOIN P_VotePerson on P_VotePerson.P_ByVoteUserId=P_VoteOption.P_Id ");
                ss1.Append(" left JOIN P_Image on P_Image.P_ImageId=P_VoteOption.P_Id and P_Image.P_ImageType='" + (int)ImageTypeEnum.评选活动 + @"' ");
                ss1.Append(" where P_VoteOption.P_ActivityId='" + id + @"' and P_VoteOption.P_status=0 ");
                ss1.Append(" group by P_VoteOption.P_Id,P_VoteOption.P_Option,P_Image.P_Id,P_Image.P_ImageUrl ");
                ss1.Append(" ORDER BY count(P_VotePerson.P_Id) desc ");
                DataSet dd1 = DbHelperSQL.Query(ss1.ToString());
                if (dd1.Tables[0].Rows.Count != 0)
                {
                    DataSetToModelHelper<EndAction> voteoptionmodel1 = new DataSetToModelHelper<EndAction>();
                    List<EndAction> vote = voteoptionmodel1.FillModel(dd1);
                    model.actionmodel = vote;
                }
            }
            else
            {
                model = null;
            }
            return model;
        }
    }
    public class ViewModel
    {
        public int userfroum { get; set; }
        public int voteresult { get; set; }
        public string id { get; set; }
        public int status { get; set; }
        public string title { get; set; }
        private string _starttime;
        public string starttime
        {
            get
            {
                return DateTime.Parse(_starttime == null ? "" : _starttime).ToString("yyyy年MM月dd日");
            }
            set
            {
                _starttime = value;
            }
        }
    }
    public class ViewActivionToModel
    {
        public string id { get; set; }
        public string title { get; set; }
        public string createuser { get; set; }
        public string time { get; set; }
        public int usercount { get; set; }
        public int namestatus { get; set; }
        public List<VoteOpToModel> options { get; set; }

    }
    public class VoteOpToModel
    {
        public string imgurl { get; set; }
        public string optioncontent { get; set; }
        /// <summary>
        /// 投票的百分比
        /// </summary>
        public double schedcount { get; set; }
    }
    public class ModelView
    {
        public string id { get; set; }
        public string title { get; set; }
        public string createuser { get; set; }
        public string time { get; set; }
        public int usercount { get; set; }
        public int namestatus { get; set; }
        public List<VoteModelOpen> options { get; set; }
    }
    public class VoteModelOpen
    {
        public string optioncontent { get; set; }
        public string imgurl { get; set; }
        public int userselect { get; set; }
    }
    public class EndToAction
    {
        public string id { get; set; }
        public string title { get; set; }
        public string createuser { get; set; }
        public string endtime { get; set; }
        public int usercount { get; set; }
        public List<EndAction> actionmodel { get; set; }
    }
    public class EndAction
    {
        public string optioncontent { get; set; }
        public string imgurl { get; set; }
        public int playcount { get; set; }
    }
}

