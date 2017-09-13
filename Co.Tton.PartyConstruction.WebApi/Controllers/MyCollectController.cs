using DTcms.BLL;
using DTcms.DAL;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("mcc/col")]
    public class MyCollectController : ApiControllerBase
    {
        private MyCollecter bll = new MyCollecter();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        [Route("img/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetMyCollect(int userid, int rows, int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                
                List<MyCollectModel> result = bll.GetMyCollect(userid,rows, page);
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
        /// 修改用户的头像
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="imgname"></param>
        /// <returns></returns>
        [Route("up/avatar"), AcceptVerbs("GET")]
        public HttpResponseMessage UpAvatar(int userid,string imgname)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = bll.UpAvatar(userid,imgname);
                if (data)
                {
                    message = RenderMessage(true, "修改成功.", "修改成功", 1);
                }
                else
                {
                    message = RenderMessage(false, "修改失败.");

                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
                throw;
            }
            return message;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="imgname"></param>
        /// <returns></returns>
        [Route("up/word"), AcceptVerbs("GET")]
        public HttpResponseMessage UpPassWord(int userid, string pass)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = bll.UpPassWord(userid, pass);
                if (data)
                {
                    message = RenderMessage(true, "修改成功.", "修改成功", 1);
                }
                else
                {
                    message = RenderMessage(false, "修改失败.");

                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
                throw;
            }
            return message;
        }
        /// <summary>
        /// 个人中心主页列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("get/pc"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPersonalCenter(int userid, int rows, int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<ContentModel> result = bll.GetPersonalCenter(userid, rows,page);
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
                throw;
            }
            return message;
        }
        /// <summary>
        /// 个人中心列表删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("del/gpc"), AcceptVerbs("POST")]
        public HttpResponseMessage GetDeleteContent(DeleteCenterFromBody fromBody)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean result = bll.GetDeleteContent(fromBody.data);
                if (result)
                {
                    message = RenderMessage(true, "删除成功.", "删除成功", 1);
                }
                else
                {
                    message = RenderMessage(false, "删除失败.");

                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
                throw;
            }
            return message;
        }
    }
}