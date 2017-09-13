using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.PartyMap
{
    public partial class PartyMap : Web.UI.ManagePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetPositionList()
        {
            BLL.PartyMap pm = new BLL.PartyMap();
            string json = pm.MapGetPositionList();

            return json;
        }
    }
}