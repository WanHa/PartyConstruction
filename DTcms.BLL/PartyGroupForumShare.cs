using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTcms.DAL.PartyGroupForumShare;

namespace DTcms.BLL
{
        #region 分享
	public partial class PartyGroupForumShare
	{
		DAL.PartyGroupForumShare dal = new DAL.PartyGroupForumShare();
		/// <summary>
		/// 分享文字
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int shareMessage(ForumShare model)
		{
			return dal.shareMessage(model);
		}

		/// <summary>
		/// 分享图片文字
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int shareImg(ForumShare model)
		{
			return dal.shareImg(model);
		}

		/// <summary>
		/// 分享视频文字
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int shareVideo(ForumShare model)
		{
			return dal.shareVideo(model);
		}
		#endregion

		#region 删除组员
		/// <summary>
		/// 删除组员
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int dorpMember(deleteMember model)
		{
			return dal.dorpMember(model);
		}
        #endregion

		#region 点赞
		/// <summary>
		/// 点赞
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int like(likeModel model)
		{
			return dal.like(model);
		}
#endregion

		#region 屏蔽党员
		/// <summary>
		/// 屏蔽党员
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public Boolean shieldMember(shieldMemberModel model)
		{
			return dal.shieldMember(model);
		}
		#endregion

		#region 评论
		/// <summary>
		/// 发布评论
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int getFeedForCommentsModel(postComment model)
		{
			return dal.getFeedForCommentsModel(model);
		}

		/// <summary>
		/// 获取评论列表
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="rows"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		public List<acceptComment> setFeedForCommentsModel(string userId,string forumShareId, int rows, int page)
		{
			return dal.setFeedForCommentsModel(userId,forumShareId, rows, page);
		}
			#endregion

		#region 组员列表
			/// <summary>
			/// 获取组员列表
			/// </summary>
			/// <param name="model"></param>
			/// <returns></returns>
			public List<MembersListModel> getMembersList(string partyGroupId, int rows, int page)
		{
			return dal.getMembersList(partyGroupId,rows,page);
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
		public List<shareListModel> GetShareList(string forumid, string userid, int rows, int page)
		{
			return dal.GetShareList(forumid, userid, rows, page);
		}
        #endregion
	}
}