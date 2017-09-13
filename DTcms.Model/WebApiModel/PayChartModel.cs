using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class PayChartModel
    {
        /// <summary>
        /// 1-12月已提交数量
        /// </summary>
        public List<int> submit { get; set; }

        /// <summary>
        /// 1-12月未提交数量
        /// </summary>
        public List<int> unsubmit { get; set; }
    }
}
