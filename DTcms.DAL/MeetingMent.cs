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
    public class MeetingMent
    {
        public List<MeetingModel> GetMeetingList(int userid, int rows, int page)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select P_MeetingAdmin.P_Id as id,P_Title as title,P_MeePlace as place,CONVERT(varchar(100), P_StartTime, 20)as starttime,");
            strsql.Append("(case when P_MeetingAdmin.P_EndTime > GETDATE() then 0 else 1 end) as status from P_MeetingAdmin");
            strsql.Append(" LEFT JOIN P_MeetingAdminSublist on P_MeetingAdminSublist.P_MeeID = P_MeetingAdmin.P_Id");
            strsql.Append(" where P_MeetingAdminSublist.P_UserId = '" + userid + @"'");
            DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strsql.ToString(), "P_MeetingAdmin.P_CreateTime"));
            DataSetToModelHelper<MeetingModel> model = new DataSetToModelHelper<MeetingModel>();
            return model.FillModel(dt);
        }
    }
    public class MeetingModel
    {
        public string id { get; set; }
        public string place { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        private string _starttime;
        public string starttime
        {
            get
            {
                return DateTime.Parse(_starttime == null ? "" : _starttime).ToString("yyyy年MM月dd日 HH:mm");
            }
            set
            {
                _starttime = value;
            }
        }
    }
}
