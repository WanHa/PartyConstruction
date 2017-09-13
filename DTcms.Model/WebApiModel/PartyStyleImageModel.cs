using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class PartyStyleImageModel
    {
        /// <summary>
        /// 文章id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 图片URL
        /// </summary>
        public string imageUrl { get; set; }

        /// <summary>
        /// 是否点赞
        /// </summary>
        public int thumbs { get; set; }
    }
}
