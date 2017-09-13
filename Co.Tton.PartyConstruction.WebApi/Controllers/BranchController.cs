using DTcms.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;
using static DTcms.DAL.BranchDAL;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
   
    [RoutePrefix("f1/party_group")]
    public class BranchController : ApiControllerBase
    {
        private BranchBLL bll = new BranchBLL();
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
        [Route("setList/branch"), AcceptVerbs("GET")]//获取列表
        public HttpResponseMessage getList([FromUri]string group_id, [FromUri] int rows, [FromUri] int page)
        {
            
        HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                List<ListModel> result = bll.getList(group_id, rows, page);
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
        //申请
        [Route("Applyfor/branch"), AcceptVerbs("GET")]
        public HttpResponseMessage Applyfor([FromUri]string P_ApplyUserId, [FromUri] string P_ApplyContent)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                Boolean data = bll.Applyfor(P_ApplyUserId, P_ApplyContent);
                if (data)
                {
                    message = RenderMessage(true, "申请成功.");
                }
                else
                {
                    message = RenderMessage(false, "未能申请成功");
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