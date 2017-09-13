using DTcms.Common;
using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.UserDemand
{
    public partial class UserDemand_reply : Web.UI.ManagePage
    {
        private string action = DTEnums.ActionEnum.Reply.ToString(); //操作类型
        private string id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");

            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Reply.ToString())
            {
                this.action = DTEnums.ActionEnum.Reply.ToString();
                this.id = DTRequest.GetQueryString("id");
                if (string.IsNullOrEmpty(id))
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new BLL.P_UserDemandSublist().Exists(this.id))
                {
                    JscriptMsg("记录不存在或已删除！", "back");
                    return;
                }
            }
            BLL.P_UserDemandSublist bll = new BLL.P_UserDemandSublist();
            //Model.P_UserDemandSublist model = bll.GetModel(this.id);
            Userdemand model = bll.GetToModel(this.id);
            List<Model.P_UserDemandSublist> list = bll.Getlist(this.id);
            if(model !=null)
            {
                switch(model.checkstatus)
                {
                    case -1: par.InnerText = "未处理";
                        break;
                    case 0: par.InnerText = "满意";
                        break;
                    case 1: par.InnerText = "不满意";
                        break;
                }
                p1.InnerText = model.content;
                switch(model.sum)
                {
                    case 0:
                        txt.Visible = false;
                        txt1.Visible = false;
                        txt2.Visible = false;
                        break;
                    case 2:
                        txt2.Visible = false;
                        break;
                }
            }          
            else
            {
                par.InnerText = "";
                p1.InnerText = "";
            }
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            p2.InnerText = list[i].P_Content == null ? "" : list[i].P_Content.ToString();
                            break;
                        case 1:
                            p3.InnerText = list[i].P_Content == null ? "" : list[i].P_Content.ToString();
                            break;
                        case 2:
                            p4.InnerText = list[i].P_Content == null ? "" : list[i].P_Content.ToString();
                            break;
                    }
                }               
            }
            List<Ims> imageurl = bll.GetUrl(this.id);
            if(imageurl!= null)
            {
                image.Visible = true;
                int i = 0;
                foreach (var item in imageurl)
                {
                    HtmlGenericControl imgSelectFile = new HtmlGenericControl("input");
                    imgSelectFile.Attributes.Add("type", "image");
                    imgSelectFile.Attributes.Add("id", "img" + i++);
                    imgSelectFile.Attributes.Add("style", "width: 240px;height: 240px;margin-left:100px;");
                    //imgSelectFile.Attributes.Add("align", "center");
                    imgSelectFile.Attributes.Add("src", item.imgurl);

                    image.Controls.Add(imgSelectFile);
                    image.Attributes.Remove("style");
                }
            }
            //if (Page.IsPostBack)
            //{
            //    ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Reply.ToString()); //检查权限

            //    if (action == DTEnums.ActionEnum.Reply.ToString())
            //    {
            //        ShowInfo(this.id);
            //    }
            //}
        }
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    RptBind(CombSqlTxt(txtKeywords.Text), "P_UpdateTime desc,P_CreateTime desc");
        //}
        #region 赋值操作=================================
        private void ShowInfo(string _id)
        {
            BLL.P_UserDemandSublist bll = new BLL.P_UserDemandSublist();
            //Model.P_UserDemandSublist model = bll.GetModel(_id);
            List<Model.P_UserDemandSublist> list = bll.Getlist(_id);
            //TextBox1.Text = model.P_Content;
            //txtStartTime.Text = model.P_StartTime.ToString();
            //txtEndTime.Text = model.P_EndTime.ToString();

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            p2.InnerText = list[i].P_Content == null ? "" : list[i].P_Content.ToString();
                            break;
                        case 1:
                            p3.InnerText = list[i].P_Content == null ? "" : list[i].P_Content.ToString();
                            break;
                        case 2:
                            p4.InnerText = list[i].P_Content == null ? "" : list[i].P_Content.ToString();
                            break;
                    }
                }
            }
            else
            {
                JscriptMsg("无数据", "UserDemand_reply.aspx");
                //return;
            }
        }
        #endregion
        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Reply.ToString()); //检查权限
            Model.manager user = GetAdminInfo();

            Model.P_UserDemandSublist model = new Model.P_UserDemandSublist();
            BLL.P_UserDemandSublist bll = new BLL.P_UserDemandSublist();

            model.P_ID = Guid.NewGuid().ToString();
            if(txtcontent.Text != null && txtcontent.Text !="")
            {
                model.P_Content = txtcontent.Text;
            }
            else
            {
                JscriptMsg("请输入内容！", "");
                return;
            }
            model.P_UDId = "id";
            //model.P_Verifier = txtVer.Text;
            model.P_CreateTime = DateTime.Now;
            model.P_CreateUser = user.role_id.ToString();
            model.P_Status = 0;

            string result = bll.Add(model, id);
            //if (!string.IsNullOrEmpty(result))
            //{
            //    AddAdminLog(DTEnums.ActionEnum.Reply.ToString(), "添加成功" ); 
            //}
            //else
            //{
            //    AddAdminLog(DTEnums.ActionEnum.Reply.ToString(), "添加失败" );
            //}
            JscriptMsg("回复成功！", "UserDemand_Main.aspx?id=" + id, "parent.loadMenuTree");
            //}
            //ShowInfo(id);

        }


        #region 增加操作=================================
    }
}
#endregion