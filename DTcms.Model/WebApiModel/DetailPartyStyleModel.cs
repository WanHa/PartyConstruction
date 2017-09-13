using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class DetailPartyStyleModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string organ { get; set; }
        public string imgurl { get; set; }
        public string content { get; set; }
        public string createtime { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int comcount { get; set; }
       
    }
    public class Imgurl
    {
        public string imageurl { get; set; }
    }
    public class comment
    {
        public int id { get; set; }
        public string username { get; set; }
        public string content { get; set; }
        public DateTime createtime { get; set; }
        /// <summary>
        /// 点赞个数
        /// </summary>
        public int trumcount { get; set; }
        /// <summary>
        /// 判断登录人是否点赞 0没/1点赞
        /// </summary>
        public int trumuser { get; set; }
        public string avatar { get; set; }
    }

}
