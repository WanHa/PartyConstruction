using DTcms.Common;
using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("test")]
    public class GetuiController : ApiController
    {
        [Route("pushmessage")]
        [AcceptVerbs("GET")]
        public HttpResponseMessage testpushmessage(string clientId,string content)
        {
             
             GeTui.GetClientId(1036);
            PushMessage.PushMessageToSingle(clientId, content);

            return null;
        }

       


    }
}
