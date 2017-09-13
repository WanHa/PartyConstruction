using DTcms.BLL;
using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("v1/partycloud")]
    public class PartyCloudController : ApiControllerBase
    {
        private Cloud cloud = new Cloud();
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="rows">分页行数</param>
        /// <param name="page">分页页数</param>
        /// <returns></returns>
        [Route("partycloud/submit"), AcceptVerbs("POST")]
        public HttpResponseMessage Submit([FromBody]CloudModel model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = cloud.Submit(model);
                if (data)
                {
                    message = RenderMessage(true, "上传数据成功", "上传数据成功",1);
                }
                else
                {
                    message = RenderMessage(false, "未能获取到列表数据");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 党建云列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("cloud/list"), AcceptVerbs("POST")]
        public HttpResponseMessage GetCloudList(BranchList list)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<Cloudlist> result = cloud.GetCloudList(list);
                if (result != null && result.Count > 0)
                {
                    message = RenderListTrueMessage(result, result.Count);
                }
                else
                {
                    message = RenderListFalseMessage();
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 分享党组织接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("cloud/share"), AcceptVerbs("POST")]
        public HttpResponseMessage GetBranch([FromBody]BranchList model)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = cloud.GetBranch(model);
                if (data)
                {
                    message = RenderMessage(true, "分享数据成功");
                }
                else
                {
                    message = RenderMessage(false, "未能获取到列表数据");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 点击分享获取党组织的接口
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("click/share"), AcceptVerbs("GET")]
        public HttpResponseMessage ClickShare([FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                GetGroup data = cloud.ClickShare(userid);
                if (data != null)
                {
                    message = RenderMessage(true, "获取数据成功.", data, 1);
                }
                else
                {
                    message = RenderMessage(true, "获取数据失败.");
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 点击党建云获取数量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("click/count"), AcceptVerbs("GET")]
        public HttpResponseMessage GetSum()
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<Count> data = cloud.GetSum();
                if (data != null)
                {
                    message = RenderMessage(true, "获取数据成功.", data, 1);
                }
                else
                {
                    message = RenderMessage(true, "获取数据失败.");
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 检索查询数据列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        //[Route("click/type"), AcceptVerbs("POST")]
        //public HttpResponseMessage GetTypeList(BranchList list)
        //{
        //    HttpResponseMessage message = new HttpResponseMessage();
        //    try
        //    {
        //        List<Cloudlist> result = cloud.GetTypeList(list);
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
    }
}
