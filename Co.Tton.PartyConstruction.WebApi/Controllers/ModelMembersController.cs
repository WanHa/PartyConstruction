using DTcms.BLL;
using DTcms.Common;
using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("mmc/mome")]
    public class ModelMembersController : ApiControllerBase
    {
        private Members members = new Members();
        // GET: api/ModelMembers
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ModelMembers/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ModelMembers
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ModelMembers/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ModelMembers/5
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 获取党员列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("dym/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetModelList([FromUri]int userid, [FromUri]int rows, [FromUri]int page)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<MeModel> result = members.GetModelList(userid, rows, page);
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
        /// 模范党员详情接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("det/ali"), AcceptVerbs("GET")]
        public HttpResponseMessage ModelXiangQing([FromUri]string id, [FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                DetailMeModel data = members.ModelXiangQing(id,userid);
                if (data != null)
                {
                    message = RenderMessage(true, "获取数据成功.", data, 1);
                }
                else
                {
                    message = RenderEntityFalseMessage();
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [Route("denglu/user"), AcceptVerbs("GET")]
        public HttpResponseMessage GetDengLuEnroll([FromUri]string mobile, [FromUri]string password)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                var salt = Utils.GetCheckCode(6);
                var password1 = DESEncrypt.Encrypt(password, salt);
                Boolean data = members.GetDengLuEnroll(mobile, salt, password1);
                if (data)
                {
                    message = RenderMessage(true, "登录成功.", "登录成功", 1);
                }
                else
                {
                    message = RenderMessage(false, "登录失败");
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
