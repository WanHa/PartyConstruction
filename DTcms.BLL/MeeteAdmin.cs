using DTcms.DAL;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class MeeteAdmin
    {
        private MeetingAdmin dal = new MeetingAdmin();
        /// <summary>
        /// 报名的会议管理列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<meeteAdmin> GetMetingAdminList(int userid, int rows, int page)
        {
            return dal.GetMetingAdminList(userid,rows,page);
        }
        /// <summary>
        /// 报名的会议管理的详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DetailMeeting DeatilAdmin(string id, int userid)
        {
            return dal.DeatilAdmin(id,userid);
        }
        /// <summary>
        /// 用户报名的接口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Boolean SelAdminSubmit(string id, int userid)
        {
            return dal.SelAdminSubmit(id, userid);
        }
        /// <summary>
        /// 会议签到列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<StartEndAdmin> StartEndMeeting(int userid, int rows, int page)
        {
            return dal.StartEndMeeting(userid,rows,page);
        }
        public MeetingCount GetMeetingAdmin(string id)
        {
            return dal.GetMeetingAdmin(id);
        }
        /// <summary>
        /// 获得自己的状态和别人的状态 0 点击参会人员 / 1 已签到 / 2 未签到
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<InfoModel> GetInfoStatus(int type, int userid, string id, int rows, int page)
        {
            return dal.GetInfoStatus(type,userid,id,rows,page);
        }
        /// <summary>
        /// 用户签到的接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean UpdataInfo(int userid, string id)
        {
            return dal.UpdataInfo(userid,id);
        }
        /// <summary>
        /// 用户的会议签到的列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<UserMeetingModel> GetUserMeetingList(int userid, int rows, int page)
        {
            return dal.GetUserMeetingList(userid,rows,page);
        }

        public WebMeetingDetailModel WebMeetingDetail(string id) {
            return dal.WebMeetingDetail(id);
        }

        /// <summary>
        /// Web页面添加会议
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        public Boolean WebMeetingAdd(WebMeetingFromBody fromBody) {
            return dal.WebMeetingAdd(fromBody);
        }

        public Boolean WebMeetingEdit(WebMeetingFromBody fromBody) {
            return dal.WebMeetingEdit(fromBody);
        }
    }
}
