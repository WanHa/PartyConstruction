using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class PartyGroupForumModel
    {
        /// <summary>
        /// 创建人id
        /// </summary>
        public string creater { get; set; }
        /// <summary>
        /// 论坛名称
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 论坛简介
        /// </summary>
        public string intro { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string imagename { get; set; }
    }

    public class Image
    {
        public string image { get; set; }
    }


    public class GroupList
    {
        //创建人id
        public string createid { get; set; }
        //论坛id
        public string forumid { get; set; }
        //论坛名称
        public string title { get; set; }
        //图片地址
        public string imageurl { get; set; }
        //论坛封面
        public string background { get; set; }
        //论坛人数
        public int count { get; set; }
        //状态
        public int status { get; set; }
    }

    public class ForumShare
    {
        //分享人
        public int createrid { get; set; }
        //发布时间
        private string createtime { get; set; }
        //所属支部
        public string groupname { get; set; }
        //文字内容
        public string content { get; set; }
        //图片
        public List<Image> imageurl { get; set; }
        //视频地址
        public string videourl { get; set; }
        //分享id
        public string id { get; set; }

		public int tpye { get; set; }
		//视频时长
		public string vidTime { get; set; }
        /// <summary>
        /// @人员ID列表
        /// </summary>
        public List<AtPersonnelFrombody> at_personnels { get; set; }
    }

    public class ForumInfo
    {
        //论坛图片
        public string img { get; set; }
        //论坛名称
        public string title { get; set; }
        //负责人名字
        public string username { get; set; }
        //组员数量
        public int groupmembers { get; set; }
        //简介内容
        public string content { get; set; }
    }


    public class CommitApply
    {
        //党小组论坛id
        public string groupforumid { get; set; }
        //申请人
        public int userid { get; set; }
        //申请说明
        public string content { get; set; }
    }
    
    public class ApplyList
    {
        //申请人id
        public string applyuserid { get; set; }
        //头像
        public string useravatar { get; set; }
        //用户名字
        public string username { get; set; }
        //论坛id
        public string forumid { get; set; }
        //党小组论坛名称
        public string forumname { get; set; }
        //内容
        public string content { get; set; }
        //日期
        public string time { get; set; }
        //类型 0被审批；1审批
        public int type { get; set; }
    }

    public class CheckApply
    {
        //论坛id
        public string groupforumid { get; set; }
        //审核状态
        public int isverify { get; set; }
        //用户id
        public string userid { get; set; }
    }
    
    public class DelGroupForum
    {
        //用户id
        public string userid { get; set; }
        //组织id
        public string groupforumid { get; set; }
        //类型--0退出，1解散
        public int type { get; set; }
    }

    public class Report
    {
        //举报人id
        public string userid { get; set; }
        //关联id
        public string relevancyid { get; set; }
        //举报内容
        public string content { get; set; }
        //证据截图
        public List<Image> imagename { get; set; }
        //分类
        public int type { get; set; }
    }

    public class IdModel
    {
        public string id { set; get; }
    }

    /// <summary>
    /// 手机端签到模型
    /// </summary>
    public class PhoneSingModel
    {
        public string meettingid { get; set; }

        public string userid { get; set; }
    }

}
