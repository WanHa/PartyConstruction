using DTcms.BLL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;
using static DTcms.DAL.PartyGroupForumShare;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
	[RoutePrefix("v3/partygroupforum")]
	public class PartyGroupForumShareController : ApiControllerBase
	{
		private PartyGroupForumShare bll = new PartyGroupForumShare();
		#region 分享
		/// <summary>
		/// 分享文字、视频、图片（0：文字，1、图片，2：视频）
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>		
		[Route("share/forum"), AcceptVerbs("POST")]
		public HttpResponseMessage shareMessage([FromBody]ForumShare model)
		{
			HttpResponseMessage message = new HttpResponseMessage();
			try
			{
				if (model.tpye == 0)
				{
					int data = bll.shareMessage(model);
					if (data > 0)
					{
						message = RenderMessage(true, "分享成功.", "分享成功.", 1);
					}
					else
					{
						message = RenderMessage(false, "分享失败.");
					}
				}
				if (model.tpye == 1)
				{
					int data = bll.shareImg(model);
					if (data > 0)
					{
						message = RenderMessage(true, "分享成功.", "分享成功.", 1);
					}
					else
					{
						message = RenderMessage(false, "分享失败.");
					}
				}
				if (model.tpye == 2)
				{
					int data = bll.shareVideo(model);
					if (data > 0)
					{
						message = RenderMessage(true, "分享成功.", "分享成功.", 1);
					}
					else
					{
						message = RenderMessage(false, "分享失败.");
					}
				}

			}
			catch (Exception ex)
			{
				message = RenderErrorMessage(ex);
			}
			return message;
		}
        #endregion

        #region 手动签入
        [Route("sign/in"), AcceptVerbs("POST")]
        public HttpResponseMessage signIn([FromBody]IdModel id)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                DTcms.BLL.signlnStatistics bll = new DTcms.BLL.signlnStatistics();
                bool gettype = bll.UpdateType(id.id);
                if (gettype)
                {
                    message = RenderMessage(gettype, "签到成功.", "签到成功.", 1);
                }
                else
                {
                    message = RenderMessage(gettype, "签到失败.", "签到失败.", 1);
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }

        #endregion

        #region 手机签到
        [Route("phone/signin"), AcceptVerbs("POST")]
        public HttpResponseMessage PhoneSignIn([FromBody]PhoneSingModel singInData)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                DTcms.BLL.signlnStatistics bll = new DTcms.BLL.signlnStatistics();
                bool gettype = bll.PhoneSingIn(singInData.meettingid, singInData.userid);
                if (gettype)
                {
                    message = RenderMessage(true, "签到成功.", "签到成功.", 1);
                }
                else
                {
                    message = RenderMessage(false, "签到失败.");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }

        #endregion

        #region 删除党员
        /// <summary>
        /// 删除党员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("dropMember/forum"), AcceptVerbs("POST")]
		public HttpResponseMessage dropMember([FromBody]deleteMember model)
		{
			HttpResponseMessage message = new HttpResponseMessage();
			try
			{
				int data = bll.dorpMember(model);
				if (data > 0)
				{
					message = RenderMessage(true, "删除成功.", "删除成功.", 1);
				}
				else
				{
					message = RenderMessage(false, "删除失败.");
				}
			}
			catch (Exception ex)
			{
				message = RenderErrorMessage(ex);
			}
			return message;
		}
		#endregion

		#region 屏蔽党员
		/// <summary>
		/// 屏蔽党员
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Route("shieldMember/forum"), AcceptVerbs("POST")]
		public HttpResponseMessage shieldMember([FromBody]shieldMemberModel model)
		{
			HttpResponseMessage message = new HttpResponseMessage();
			try
			{
				Boolean data = bll.shieldMember(model);
				if (data)
				{
					message = RenderMessage(true, "屏蔽成功.", "屏蔽成功.", 1);
				}
				else
				{
					message = RenderMessage(false, "屏蔽失败.");
				}
			}
			catch (Exception ex)  
			{
				message = RenderErrorMessage(ex);
			}
			return message;
		}
		#endregion

		#region 点赞
		/// <summary>
		/// 点赞
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Route("like/forum"), AcceptVerbs("POST")]
		public HttpResponseMessage like([FromBody]likeModel model)
		{
			HttpResponseMessage message = new HttpResponseMessage();
			try
			{
				int data = bll.like(model);
				if (data > 0)
				{
					message = RenderMessage(true, "点赞成功.", "点赞成功.", 1);
				}
				else
				{
					message = RenderMessage(false, "点赞失败.");
				}
			}
			catch (Exception ex)
			{
				message = RenderErrorMessage(ex);
			}
			return message;
		}
		#endregion

		#region 评论
		[Route("getComment/forum"), AcceptVerbs("POST")]
		public HttpResponseMessage getFeedForCommentsModel([FromBody]postComment model)
		{
			HttpResponseMessage message = new HttpResponseMessage();
			try
			{
				int data = bll.getFeedForCommentsModel(model);
				if (data > 0)
				{
					message = RenderMessage(true, "评论成功.", "评论成功.", 1);
				}
				else
				{
					message = RenderMessage(false, "评论失败.");
				}
			}
			catch (Exception ex)
			{
				message = RenderErrorMessage(ex);
			}
			return message;
		}

		[Route("setComment/forum"), AcceptVerbs("GET")]
		public HttpResponseMessage setFeedForComments([FromUri]string userId,[FromUri]string forumShareId, [FromUri] int rows, [FromUri] int page)
		{
			HttpResponseMessage message = new HttpResponseMessage();
			try
			{
				List<acceptComment> result = bll.setFeedForCommentsModel(userId,forumShareId, rows, page);
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
		#endregion

		#region 获取党员列表
		/// <summary>
		/// 获取组员列表
		/// </summary>
		/// <param name = "forumId" ></ param >
		/// < param name="userId"></param>
		/// <param name = "rows" ></ param >
		/// < param name="page"></param>
		/// <returns></returns>
		[Route("setMembersList/forum"), AcceptVerbs("GET")]
		public HttpResponseMessage getMembersList([FromUri]string partyGroupId, [FromUri] int rows, [FromUri] int page)
		{
			HttpResponseMessage message = new HttpResponseMessage();			
			try
			{
				List<MembersListModel> result = bll.getMembersList(partyGroupId, rows, page); 
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
		#endregion

		#region 分享列表
		/// <summary>
		/// 分享列表获取
		/// </summary>
		/// <param name="groupForumId"></param>
		/// <param name="userId"></param>
		/// <param name="rows"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		[Route("shareList/forum"), AcceptVerbs("GET")]
		public HttpResponseMessage GetShareList([FromUri]string forumid, [FromUri] string userid, [FromUri] int rows, [FromUri] int page)
		{
			HttpResponseMessage message = new HttpResponseMessage();
			try
			{
				List<shareListModel> result = bll.GetShareList(forumid, userid, rows, page);
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
		#endregion

	}
}
