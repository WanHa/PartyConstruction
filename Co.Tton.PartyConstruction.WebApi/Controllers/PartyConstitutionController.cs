using DTcms.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;
using static DTcms.Model.WebApiModel.PartyConstitutionModel;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("p1/partyconstitution")]
    public class PartyConstitutionController : ApiControllerBase
    {
        private PartyConstitution bll = new PartyConstitution();

        /// <summary>
        /// 获取党规党章下的类别列表
        /// </summary>
        /// <param name="conid"></param>
        /// <returns></returns>
        [Route("get/list"),AcceptVerbs("GET")]
        public HttpResponseMessage GetPartyConstitution()
        {
            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                List<Constitution> result = bll.GetPartyConstitution();
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
    }
}
