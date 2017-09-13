using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class AppealChartModel
    {
        /// <summary>
        /// 全部
        /// </summary>
        public List<int> all { get; set; }

        /// <summary>
        /// 未处理
        /// </summary>
        public List<int> untreated { get; set; }

        /// <summary>
        /// 已处理
        /// </summary>
        public List<int> processed { get; set; }
    }
}
