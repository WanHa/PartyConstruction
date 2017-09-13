using DTcms.DAL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class RongCloundConfig
    {
        /// <summary>
        /// 获取融云配置
        /// </summary>
        /// <returns></returns>
        public RongCloudModel GetRongCloudConfig()
        {
            return new RongCloud().GetRongCloudConfig();
        }
    }
}
