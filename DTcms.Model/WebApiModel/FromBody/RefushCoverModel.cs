using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel.FromBody
{
    public class RefushCoverModel
    {
        /// <summary>
        /// 0支部，1论坛
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 党支部ID
        /// </summary>
        public string groupId { get; set; }

        /// <summary>
        /// 图片名字
        /// </summary>
        public string imageName { get; set; }
    }
}
