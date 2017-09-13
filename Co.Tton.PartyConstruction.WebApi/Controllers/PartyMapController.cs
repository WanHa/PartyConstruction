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
	[RoutePrefix("v1/mapp")]
	public class PartyMapController : ApiControllerBase
    {
		PartyMap bll = new PartyMap();
		/// <summary>
		/// 检索
		/// </summary>
		/// <param name="party"></param>
		/// <param name="rows"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		[Route("map/getPartyMassage"), AcceptVerbs("GET")]
		public HttpResponseMessage getPartyMassage([FromUri]string party, [FromUri]int rows, [FromUri]int page)
		{
			HttpResponseMessage message = new HttpResponseMessage();
			try
			{
				
		        List<PartyMassageModel> result = bll.getPartyMassage(party, rows, page);
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
        /// 获取党组织详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("map/getPartyMassageDetails"), AcceptVerbs("GET")]
		public HttpResponseMessage getPartyMassageDetails([FromUri]string id)
		{
			HttpResponseMessage message = new HttpResponseMessage();
			try
			{
				PartyMassageDetailsModel result = bll.getPartyMassageDetails(id);
				if (result != null)
				{
					message = RenderMessage(true, "获取数据成功.", result, 1);
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
		/// 获取职务、人员列表
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		[Route("map/getPartyMember"), AcceptVerbs("GET")]
		public HttpResponseMessage getPartyMember([FromUri]string groupId, [FromUri]int rows, [FromUri]int page)
		{
			HttpResponseMessage message = new HttpResponseMessage();
			try
			{
				List<PartyList> result = bll.getPartyMember(groupId,rows,page);
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
		/// 传入经纬度与距离返回所传经纬度相应距离的位置
		/// </summary>
		/// <param name="lat">纬度</param>
		/// <param name="lng">精度</param>
		/// <param name="dist">距离</param>
		/// <returns></returns>
		[Route("map/getLocation"), AcceptVerbs("GET")]
		public HttpResponseMessage getLocation([FromUri]double lat, [FromUri]double lng, [FromUri] string party)
		{
			HttpResponseMessage message = new HttpResponseMessage();
			try
			{
				List<BranchL> result = bll.getLocation(lat, lng, party);
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
