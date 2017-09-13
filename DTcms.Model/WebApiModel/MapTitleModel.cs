using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class MapTitleModel
    {
        /// <summary>
        /// 党员数量
        /// </summary>
        public int party_member_count { get; set; }

        /// <summary>
        /// 党组织数量
        /// </summary>
        public int party_organization_count { get; set; }

        /// <summary>
        /// 党员服务组织数量
        /// </summary>
        public int service_organization_count { get; set; }
    }
}
