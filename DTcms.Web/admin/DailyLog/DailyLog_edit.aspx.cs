using DTcms.BLL;
using DTcms.Common;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static DTcms.DAL.P_DailyLoWord;

namespace DTcms.Web.admin.DailyLog
{
    public partial class DailyLog_edit : Web.UI.ManagePage
    {
        private string id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            P_DailyLogWord bll = new P_DailyLogWord();
            this.id = DTRequest.GetQueryString("id");
            DetailWoedJournal model = bll.SelPartyNewsPeper(id);
            if(model.replycontent!=null)
            {
                par.Visible = true;
                par.InnerText = model.replycontent;
                txt3.Visible = false;
                txt4.Visible = false;
            }
            else
            {
                txt2.Visible = false;
            }
            title.InnerText = model.title;
            type.InnerText = model.type;
            site.InnerText = model.content;
            if (site.InnerText == "")
            {
                site.Visible = false;
            }
            else
            {
                site.Visible = true;
            }
            username.InnerText = model.username;
            senduser.InnerText = model.senduser;
            time.InnerText = model.createtime.ToString();

            List<DailyLogImage> imageurl = bll.SelDailyLogImage(id);
            if (imageurl != null)
            {
                field_tab_content.Visible = true;
                int i = 0;
                foreach (var item in imageurl)
                {
                    HtmlGenericControl imgSelectFile = new HtmlGenericControl("input");
                    imgSelectFile.Attributes.Add("type", "image");
                    imgSelectFile.Attributes.Add("id", "img" + i++);
                    imgSelectFile.Attributes.Add("style", "width: 400px;margin-left:355px;");
                    imgSelectFile.Attributes.Add("src", item.imageurl);

                    field_tab_content.Controls.Add(imgSelectFile);
                    field_tab_content.Attributes.Remove("style");
                }
            }
            else
            {
                field_tab_content.Visible = false;
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            P_AuditingFeedback model = new P_AuditingFeedback();
            BLL.Feedback bll = new Feedback();
            Model.manager user = GetAdminInfo();
            if (txtcontent.Text != null && txtcontent.Text != "")
            {
                model.P_AuditContent = txtcontent.Text;
            }
            else
            {
                JscriptMsg("请输入内容！", "DailyLog_list.aspx?id=" + id, "parent.loadMenuTree");
                return;
            }
            model.P_Id = Guid.NewGuid().ToString();
            model.P_LogId = "id";
            model.P_CreateUser = user.role_id.ToString();
            model.P_CreateTime = DateTime.Now;

            string result = bll.Add(model,id);
            if(result!=null)
            {
                JscriptMsg("回复成功！", "DailyLog_list.aspx?id=" + id, "parent.loadMenuTree");
            }
            else
            {
                JscriptMsg("回复失败！", "DailyLog_list.aspx?id=" + id, "parent.loadMenuTree");
            }
        }
    }
}