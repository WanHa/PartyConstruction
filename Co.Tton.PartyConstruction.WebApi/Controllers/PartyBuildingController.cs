using DTcms.BLL;
using DTcms.Common;
using DTcms.DAL;
using DTcms.Model.WebApiModel;
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
    [RoutePrefix("pbc/pri")]
    public class PartyBuildingController : ApiControllerBase
    {
        private PartysBuilding building = new PartysBuilding();
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
        /// <summary>
        /// 获取cos列表接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("news/list"), AcceptVerbs("POST")]
        public HttpResponseMessage GetNewsPaper(BuildModel buil)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<BuildingModel> result = building.GetPartyNewsList(buil);
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
        /// 获取cos详情接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("list/detail"), AcceptVerbs("GET")]
        public HttpResponseMessage DetailNewsPaper([FromUri]int userid, [FromUri]int id)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                DetailPaperModel data = building.SelPartyNewsPeper(userid, id);
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
        /// 账号注册加密接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("regist/enr"), AcceptVerbs("GET")]
        public HttpResponseMessage GetRegisterEnroll([FromUri]string mobile, [FromUri]string password)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                var salt = Utils.GetCheckCode(6);
                var password1 = DESEncrypt.Encrypt(password,salt);
                Boolean data = building.GetRegisterEnroll(mobile, salt, password1);
                if (data)
                {
                    message = RenderMessage(true, "注册成功.", "注册成功", 1);
                }
                else
                {
                    message = RenderMessage(false, "注册失败");
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }
            return message;
        }
        /// <summary>
        /// 收藏接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("coll/ect"), AcceptVerbs("GET")]
        public HttpResponseMessage SelCollect([FromUri]int userid, [FromUri]string id, [FromUri] string type)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            Boolean data = building.GetCollect(userid, id, type);
            if (data)
            {
                message = RenderMessage(true, "收藏成功.", "收藏成功", 1);
            }
            else
            {
                message = RenderMessage(false, "收藏失败");
            }

            return message;
        }
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("coll/del"), AcceptVerbs("GET")]
        public HttpResponseMessage DelectCollect([FromUri]int userid, [FromUri] string id,[FromUri]string type)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            Boolean data = building.DelectCollect(userid, id,type);
            if (data)
            {
                message = RenderMessage(true, "取消收藏成功.");
            }
            else
            {
                message = RenderMessage(false, "取消收藏失败");
            }

            return message;
        }
    }
}