using DTcms.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("home/page")]
    public class HomeController : ApiControllerBase
    {
        private home page = new home();
        /// <summary>
        /// 获取组织年龄
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("age"), AcceptVerbs("GET")]
        public HttpResponseMessage GetAge([FromUri]string groupid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<DTcms.DAL.Left_home.Ages> result = page.GetAge(groupid);

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
        /// 获取组织性别
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("sex/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetSex([FromUri]string groupid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                DTcms.DAL.Left_home.Gender result = page.GetSex(groupid);

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
        /// 组织学习时长
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("learn/time"), AcceptVerbs("GET")]
        public HttpResponseMessage GetGrouptime([FromUri]string groupid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<DTcms.DAL.Left_home.Learntime> result = page.GetGrouptime(groupid);

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
        /// 主要经济来源
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("econ/money"), AcceptVerbs("GET")]
        public HttpResponseMessage GetMoney([FromUri]string groupid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<DTcms.DAL.Left_home.Economic> result = page.GetMoney(groupid);

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
    }
}