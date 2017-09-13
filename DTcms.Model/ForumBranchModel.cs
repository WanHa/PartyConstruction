using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class ForumBranchModel
    {
        /// <summary>
        /// 创建人Id
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 创建人头像
        /// </summary>
        public string user_avatar { get; set; }

        /// <summary>
        /// 用户所属组织
        /// </summary>
        public string user_group_name { get; set; }

        /// <summary>
        /// 文字内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 数据ID
        /// </summary>
        public string p_id { get; set; }

        /// <summary>
        /// 内容来源 0 新建 1 转发
        /// </summary>
        public int content_source { get; set; }

        /// <summary>
        /// 内容类型(视频、图片、文字、党建风采、党建要论、党建视频)
        /// </summary>
        public int content_type { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int comment_count { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string create_time { get; set; }

        /// <summary>
        /// 点赞数量
        /// </summary>
        public int thumb_count { get; set; }

        /// <summary>
        /// 是否点赞
        /// </summary>
        public int is_thumb { get; set; }

        /// <summary>
        /// 是否收藏
        /// </summary>
        public int is_collect { get; set; }

        /// <summary>
        /// 论坛发布图片
        /// </summary>
        public List<ContentPicture> pictures { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        public string vodeo_name { get; set; }

        /// <summary>
        /// 视频ID
        /// </summary>
        public string video_id { get; set; }

        /// <summary>
        /// 视频截图
        /// </summary>
        public string video_pic { get; set; }

        /// <summary>
        /// 视频url
        /// </summary>
        public string video_url { get; set; }

        /// <summary>
        /// 视频长度
        /// </summary>
        public int video_lenght { get; set; }

        /// <summary>
        /// 视频播放次数
        /// </summary>
        public int vodeo_play_count { get; set; }

        /// <summary>
        /// 视频课程ID
        /// </summary>
        public string course_id { get; set; }

        /// <summary>
        /// 转发数据ID(视频、党建风采、要论)
        /// </summary>
        public string forwarding_id { get; set; }

        /// <summary>
        /// 转发数据的标题
        /// </summary>
        public string forwarding_title { get; set; }

        /// <summary>
        /// 转发数据的内容
        /// </summary>
        public string forwarding_content { get; set; }

        /// <summary>
        /// 转发数据的图片
        /// </summary>
        public string forwarding_pic { get; set; }

        /// <summary>
        /// 分享图片url
        /// </summary>
        public string share_file_pic { get; set; }

        /// <summary>
        /// 分享标题
        /// </summary>
        public string share_file_title { get; set; }

        /// <summary>
        /// 分享文件的时间
        /// </summary>
        public string share_create_time { get; set; }

        /// <summary>
        /// 分享文件url
        /// </summary>
        public string share_file_url { get; set; }

        /// <summary>
        /// 分享人姓名
        /// </summary>
        public string share_user_name { get; set; }

        /// <summary>
        /// 分享人组织
        /// </summary>
        public string share_user_group { get; set; }

        /// <summary>
        /// 分享文件大小
        /// </summary>
        public Int64 share_file_size { get; set; }

        private List<AtPersonnel> _at_personnel;
        public List<AtPersonnel> at_personnel {
            get { return _at_personnel== null ? new List<AtPersonnel>(): _at_personnel; }
            set { _at_personnel = value; }
        }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string comment_content { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AtPersonnel {
        public string user_name { get; set; }
    }

    public class ContentPicture
    {
        public string picture_url { get; set; }
    }

    public class ForwardingArticleModel {
        /// <summary>
        /// 转发数据ID(视频、党建风采、要论)
        /// </summary>
        public string forwarding_id { get; set; }

        /// <summary>
        /// 转发数据的标题
        /// </summary>
        public string forwarding_title { get; set; }

        /// <summary>
        /// 转发数据的内容
        /// </summary>
        public string forwarding_content { get; set; }

        /// <summary>
        /// 转发数据的图片
        /// </summary>
        public string forwarding_pic { get; set; }
    }

}
