using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class Meeting
    {
        private MeetingMent dal = new MeetingMent();
        /// <summary>
        /// 报名的会议管理列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<MeetingModel> GetMeetingList(int userid, int rows, int page)
        {
            return dal.GetMeetingList(userid, rows, page);
        }
    }
}
