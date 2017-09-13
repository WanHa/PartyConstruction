using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel.FromBody
{
    /// <summary>
    /// 删除个人中心数据
    /// </summary>
    public class DeleteCenterFromBody
    {
        ///// <summary>
        ///// 数据ID
        ///// </summary>
        //public string p_id { get; set; }

        ///// <summary>
        ///// 删除的数据类型
        ///// </summary>
        //public int type { get; set; }

        public List<DeleteCenter> data { get; set; }
    }

    public class DeleteCenter {
        /// <summary>
        /// 数据ID
        /// </summary>
        public string p_id { get; set; }

        /// <summary>
        /// 删除的数据类型
        /// </summary>
        public int type { get; set; }
    }
}
