using DTcms.DAL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class WorkingLog1
    {
        WorkLogS worklog = new WorkLogS();

        public Boolean AddFeedback(string logId,string userId, string feedback)
        {
            return worklog.AddFeedback(logId, userId, feedback);
        }

        public List<WorkLogsModel> GetUserLogList(int userid, int rows, int page)
        {
            return worklog.GetWorkLogList(userid, rows, page);
        }

        public WorkLogDetailModel GetWorkLogDetial(int userid, string id)
        {
            return worklog.GetWorkLogDetail(userid, id);
        }

    }
}
