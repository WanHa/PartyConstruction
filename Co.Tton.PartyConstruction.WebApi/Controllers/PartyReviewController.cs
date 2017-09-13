using DTcms.BLL;
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
    [RoutePrefix("par/revi")]
    public class PartyReviewController : ApiControllerBase
    {
        private PartyReview view = new PartyReview();
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
        /// 根据评选活动获得图表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Route("view/list"), AcceptVerbs("GET")]

        //public HttpResponseMessage GetPartyStyleList([FromUri]string  id)
        //{
        //    HttpResponseMessage message = new HttpResponseMessage();
        //    try
        //    {
        //        DataSet data = view.GetReview(id);
        //        if (data != null)
        //        {
        //            message = RenderMessage(true, "获取数据成功.", data.Tables[0], data.Tables[0].Rows.Count);
        //        }
        //        else
        //        {
        //            message = RenderMessage(false, "未能获取到数据");
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