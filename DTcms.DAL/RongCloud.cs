using DTcms.Common;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class RongCloud
    {
        /// <summary>
        /// 获取融云配置
        /// </summary>
        /// <returns></returns>
        public RongCloudModel GetRongCloudConfig()
        {
            RongCloudModel result = new RongCloudModel();
            result.AppKey = ConfigHelper.GetAppSettings("RongAppKey");
            result.AppSecret = ConfigHelper.GetAppSettings("RongAppSecret");
            return result;
        }
    }
}
