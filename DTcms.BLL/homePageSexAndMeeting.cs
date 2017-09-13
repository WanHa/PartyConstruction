using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTcms.DAL;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.homepage;

namespace DTcms.BLL
{
    public class homePageSexAndMeeting
    {
        //public List<SexProportionModel> getSexProportion(string groupId)
        //{
        //    return GetSexProportion.getSexPro(groupId);
        //}

        //public List<IncomeSourceModel> getIncomeProportion(string groupId)
        //{
        //    return GetSexProportion.getIncomeTypeNum(groupId);
        //}

        /// <summary>
        /// 获取正式党员，预备党员数量
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public PartyMemberKind getPartyMemberCount(string groupid)
        {
            return new GetSexProportion().getPartyMemberCount(groupid);
        }

        /// <summary>
        /// 获取参与会议个数
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public List<int> getMeetingNumber(string groupid)
        {
            return new GetSexProportion().getMeetingNumber(groupid);
        }


        /// <summary>
        /// 获取党员诉求数量
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public AppealChartModel getAppealInfo(string groupid)
        {
            return new GetSexProportion().getAppealInfo(groupid);
        }


        /// <summary>
        /// 获取党费缴纳详情
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public PayChartModel getGroupPayInfo(string groupid)
        {
            return new GetSexProportion().getGroupPayInfo(groupid);
        }

        /// <summary>
        /// 获取地图标题信息
        /// </summary>
        /// <returns></returns>
        public MapTitleModel GetMapTitle() {
            return new GetSexProportion().GetMapTitle();
        }

        /// <summary>
        /// 获取首页地图组织列表
        /// </summary>
        /// <returns></returns>
        public List<MapOrgLocationModel> GetMapOrganizationLocation() {
            return new GetSexProportion().GetMapOrganizationLocation();
        }

        /// <summary>
        /// 获取首页地图组织详情信息
        /// </summary>
        /// <param name="groupId">组织ID</param>
        /// <returns></returns>
        public MapGroupInfoModel GetGroupInfo(string groupId) {
            return new GetSexProportion().GetGroupInfo(groupId);
        }

        /// <summary>
        /// 获取首页地图用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public MapUserInfoModel GetMapUserInfo(string userId) {
            return new GetSexProportion().GetMapUserInfo(userId);
        }

        /// <summary>
        /// 获取首页地图组织活动信息列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<MapActivityModel> GetGroupActivity(string groupId) {
            return new GetSexProportion().GetGroupActivity(groupId);
        }
    }
}
