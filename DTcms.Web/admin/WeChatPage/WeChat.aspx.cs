using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.WeChatPage
{
    public partial class WeChat : System.Web.UI.Page
    {
        public string webApiDomain = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.webApiDomain = ConfigHelper.GetAppSettings("webApiDomain");
        }
    }
}