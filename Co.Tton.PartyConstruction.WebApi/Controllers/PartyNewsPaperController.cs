using DTcms.BLL;
using DTcms.DAL;
using DTcms.Model;
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
    [RoutePrefix("pnp/news")]
    public class PartyNewsPaperController : ApiControllerBase
    {
        private PartyNewsPaper newspaper = new PartyNewsPaper();
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
        /// 当点击转发获取党小组的接口
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("cli/trank"), AcceptVerbs("GET")]
        public HttpResponseMessage ClickTranck([FromUri]int userid)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                GetGroups data = newspaper.GetGroupsRelation(userid);
               if(data != null)
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
        /// 提交转发接口
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="content"></param>
        /// <param name="id"></param>
        /// <param name="organizeid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("cli/sub"), AcceptVerbs("POST")]
        public HttpResponseMessage SubmitTranck([FromBody]Trum transmit)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Boolean data = newspaper.SubTranck(transmit);
                if (data)
                {
                    message = RenderMessage(true, "转发成功.","转发成功",1);
                }
                else
                {
                    message = RenderMessage(false, "转发失败");
                }
            }
            catch (Exception ex)
            {

                message = RenderErrorMessage(ex);
            }


            return message;
        }

        [Route("at/personnels"),AcceptVerbs("GET")]
        public HttpResponseMessage GetAtPersonnels([FromUri]string userid, [FromUri]int page, [FromUri]int rows) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<AtPersonnelModel> result = new PartyNewsPaper().GetAtPersonnel(userid, page, rows);
                if (result != null && result.Count > 0) {
                    message = RenderListTrueMessage(result, result.Count);
                }
                else {
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

    }
}