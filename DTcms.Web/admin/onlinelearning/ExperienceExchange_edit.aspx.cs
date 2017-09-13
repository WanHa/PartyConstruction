using DTcms.BLL;
using DTcms.Common;
using DTcms.DAL;
using DTcms.DBUtility;
using NPOI.POIFS.NIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static DTcms.DAL.ExperienceExchange;

namespace DTcms.Web.admin.onlinelearning
{
    public partial class ExperienceExchange_edit : Web.UI.ManagePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BLL.ExperienceExchange bll = new BLL.ExperienceExchange();
            string id = DTRequest.GetQueryString("id");
            LearnExchangeModel model = bll.SelLearnExchange(id);
            Textcontent.InnerText = model.content;
            if (Textcontent.InnerText == "")
            {
                Textcontent.Visible = false;
            }
            else
            {
                Textcontent.Visible = true;
            }
            p1.InnerText = model.title;
            p2.InnerText = Convert.ToString(model.createtime);
            p3.InnerText = model.branch;
            p4.InnerText = model.username;

            List<LearnExchangeImage> imageurl = bll.SelLearnExchangeImg(id);
            if(imageurl != null)
            {
                field_tab_content.Visible = true;
                int i = 0;
                foreach (var item in imageurl)
                {
                    HtmlGenericControl imgSelectFile = new HtmlGenericControl("input");
                    imgSelectFile.Attributes.Add("type", "image");
                    imgSelectFile.Attributes.Add("id", "img" + i++);
                    imgSelectFile.Attributes.Add("style", "width: 400px;margin-left:355px;");
                    //imgSelectFile.Attributes.Add("align", "center");
                    imgSelectFile.Attributes.Add("src", item.imageurl);

                    field_tab_content.Controls.Add(imgSelectFile);
                    field_tab_content.Attributes.Remove("style");
                }
            }
            else
            {
                field_tab_content.Visible = false;
            }
     

            int aa = bll.SelStatus(id);
            switch (aa)
            {
                case 0:
                    btnSubmit.Visible = true;
                    editSubmit.Visible = true;
                    break;
                case 1:
                    btnSubmit.Visible = false;
                    editSubmit.Visible = false;
                    break;
                case 2:
                    btnSubmit.Visible = false;
                    editSubmit.Visible = false;
                    break;
            }
        }
        //执行通过操作
        protected void btnPass_Click(object sender, EventArgs e)
        {
            BLL.ExperienceExchange bll = new BLL.ExperienceExchange();
            string id = DTRequest.GetQueryString("id");
            bool result = bll.Pass(id);
            if (result == true)
            {
                JscriptMsg("审批成功！", "ExperienceExchange_list.aspx", "parent.loadMenuTree");
            }
            else
            {
                JscriptMsg("审批过程中发生错误！", "");
                return;
            }
        }
        //执行拒绝操作
        protected void btnRefuse_Click(object sender, EventArgs e)
        {
            BLL.ExperienceExchange bll = new BLL.ExperienceExchange();
            string id = DTRequest.GetQueryString("id");
            bool result = bll.Refuse(id);
            if (result == true)
            {
                JscriptMsg("审批成功！", "ExperienceExchange_list.aspx", "parent.loadMenuTree");
            }
            else
            {
                JscriptMsg("审批过程中发生错误！", "");
                return;
            }
        }
    }
}