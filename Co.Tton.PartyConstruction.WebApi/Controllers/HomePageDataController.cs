using DTcms.BLL;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.homepage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("homepagedata/get")]
    public class HomePageDataController : ApiControllerBase
    {
        private homePageSexAndMeeting homePage = new homePageSexAndMeeting();

        /// <summary>
        /// 获取党员性别比例
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        //[Route("sexproportion/list"), AcceptVerbs("GET")]
        //public HttpResponseMessage GetQuestionBankList([FromUri]string groupid)
        //{
        //    HttpResponseMessage message = new HttpResponseMessage();  
        //    try
        //    {
        //        List<SexProportionModel> result = homePage.getSexProportion(groupid);

        //        if (result != null && result.Count > 0)
        //        {
        //            message = RenderListTrueMessage(result, result.Count);
        //        }
        //        else
        //        {
        //            message = RenderListFalseMessage();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        message = RenderErrorMessage(ex);
        //    }

        //    return message;
        //}


        /// <summary>
        /// 获取收入来源
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        //[Route("getincomeproportion/list"), AcceptVerbs("GET")]
        //public HttpResponseMessage GetIncomeTypeNumList([FromUri]string groupid)
        //{
        //    HttpResponseMessage message = new HttpResponseMessage();
        //    try
        //    {
        //        List<IncomeSourceModel> result = homePage.getIncomeProportion(groupid);

        //        if (result != null && result.Count > 0)
        //        {
        //            message = RenderListTrueMessage(result, result.Count);
        //        }
        //        else
        //        {
        //            message = RenderListFalseMessage();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = RenderErrorMessage(ex);
        //    }
        //    return message;
        //}

        /// <summary>
        /// 获取正式党员，预备党员数量
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("getpartymembercount/list"), AcceptVerbs("GET")]
        public HttpResponseMessage getPartyMemberCount([FromUri]string groupid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                PartyMemberKind result = homePage.getPartyMemberCount(groupid);

                if (result != null)
                {
                    message = RenderMessage(true, "获取数据成功.", result, 1);
                }
                else
                {
                    message = RenderMessage(false, "获取数据失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }


        /// <summary>
        /// 获取参与会议个数
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("getmeetingcount/list"), AcceptVerbs("GET")]
        public HttpResponseMessage getMeetingNumber([FromUri]string groupid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<int> result = homePage.getMeetingNumber(groupid);

                if (result != null)
                {
                    message = RenderMessage(true, "获取数据成功.", result, 1);
                }
                else
                {
                    message = RenderMessage(false, "获取数据失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }


        /// <summary>
        /// 获取党员诉求数量
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("getappeal/list"), AcceptVerbs("GET")]
        public HttpResponseMessage getAppealInfo([FromUri]string groupid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                AppealChartModel result = homePage.getAppealInfo(groupid);

                if (result != null)
                {
                    message = RenderMessage(true, "获取数据成功.", result, 1);
                }
                else
                {
                    message = RenderMessage(false, "获取数据失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }




        /// <summary>
        /// 获取党费缴纳详情
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("getpay/list"), AcceptVerbs("GET")]
        public HttpResponseMessage getGroupPayInfo([FromUri]string groupid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                PayChartModel result = homePage.getGroupPayInfo(groupid);

                if (result != null)
                {
                    message = RenderMessage(true, "获取数据成功.", result, 1);
                }
                else
                {
                    message = RenderMessage(false, "获取数据失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        [Route("map/title"),AcceptVerbs("GET")]
        public HttpResponseMessage GetMapTitle() {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                MapTitleModel result = new homePageSexAndMeeting().GetMapTitle();
                message = RenderMessage(true, "获取数据成功", result, 1);
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        /// <summary>
        /// 获取首页地图组织列表
        /// </summary>
        /// <returns></returns>
        [Route("org/location"),AcceptVerbs("GET")]
        public HttpResponseMessage GetMapOrganizationLocation() {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<MapOrgLocationModel> result = new homePageSexAndMeeting().GetMapOrganizationLocation();
                message = RenderMessage(true, "获取数据成功", result, 1);
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;

        }

        /// <summary>
        /// 获取首页地图组织详情信息
        /// </summary>
        /// <param name="groupId">组织ID</param>
        /// <returns></returns>
        [Route("map/group-info")]
        public HttpResponseMessage GetGroupInfo(string groupId) {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                MapGroupInfoModel result = new homePageSexAndMeeting().GetGroupInfo(groupId);
                message = RenderMessage(true, "获取数据成功", result, 1);
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }


            return message;
        }

        /// <summary>
        /// 获取首页地图用户信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        [Route("map/user-info"),AcceptVerbs("GET")]
        public HttpResponseMessage GetMapUserInfo([FromUri]string userid) {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                MapUserInfoModel result = new homePageSexAndMeeting().GetMapUserInfo(userid);
                message = RenderMessage(true, "成功", result, 1);
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        /// <summary>
        /// 获取首页地图组织活动信息列表
        /// </summary>
        /// <param name="groupid">组织ID</param>
        /// <returns></returns>
        [Route("map/group-activity"),AcceptVerbs("GET")]
        public HttpResponseMessage GetGroupActivity([FromUri]string groupid) {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<MapActivityModel> result = new homePageSexAndMeeting().GetGroupActivity(groupid);
                message = RenderMessage(true, "成功", result, result.Count);
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

    }
}
