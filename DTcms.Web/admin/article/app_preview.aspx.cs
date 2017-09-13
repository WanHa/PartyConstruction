using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.article
{
    public partial class app_preview : System.Web.UI.Page
    {
        public string article_id = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            article_id = DTRequest.GetQueryString("article_id");
            ShowInfo();
        }

        private void ShowInfo() {
            BLL.article bll = new BLL.article();
            Model.article model = bll.GetModel(Convert.ToInt32(article_id));
            if (model != null && !String.IsNullOrEmpty(model.content))
            {
                string content = model.content.Replace("<img", "<input type=\"image\" style=\"width: 100%;\"");
                preview.InnerHtml = content;
            }
        }
    }
}