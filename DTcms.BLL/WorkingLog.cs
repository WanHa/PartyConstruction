using DTcms.DAL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTcms.DAL.WorkLog;

namespace DTcms.BLL
{
    public class WorkingLog
    {
        private WorkLog worklog = new WorkLog();
        //public ManagerModel GetManager(int userid)
        //{
        //    return worklog.GetManager(userid);
        //}
        public Boolean GetWorkLog(WorkLogModel model)
        {
            return worklog.GetWorkLog(model );
        }
        public DataSet GetType(int userid)
        {
            return worklog.GetType(userid);
        }

        public List<WKmodel> GetWorkLogList(int userid,int rows, int page)
        {
            return worklog.GetWorkLogList(userid,rows, page);
        }

        /// <summary>
        /// 获取工作日志详情接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public DetailWorkLogModel DetailWorkLog(int userid,string id)
        {
            return worklog.DetailWorkLog(userid, id);
        }
        /// <summary>
        /// 获得当月更新日志
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<WorkUpdateModel> Monthupdate(int userid, int rows, int page,int type)
        {

            return worklog.monthupdate(userid,rows,page,type);

        }
        /// <summary>
        /// 标题本月/周/日更新多少篇
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Workcount> GetlogCont(int userid)
        {

            return worklog.GetlogCont(userid);

        }

    }
}
