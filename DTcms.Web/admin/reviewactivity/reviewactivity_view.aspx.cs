using DTcms.BLL;
using DTcms.Common;
using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.reviewactivity
{
    public partial class reviewactivity_view : Web.UI.ManagePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = DTRequest.GetQueryString("id");
            Hidden.Value = id;
        }
        [WebMethod]
        public static string GetReviewactity(string id)
        {
            PartyReview view = new PartyReview();
            List<usercount> ds = view.GetReview(id);
            string json = JsonHelper.ObjectToJSON(ds);
            return json;
        }



    }
}
