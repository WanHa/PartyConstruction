using DTcms.Common;
using DTcms.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.MeetingManage
{
    public partial class meetingManage_edit : Web.UI.ManagePage
    {
        public string action = DTEnums.ActionEnum.Add.ToString();//操作类型
        public string id = string.Empty;
        public string userId = string.Empty;
        public string webApiDomain = string.Empty;

        #region 页面加载=================================
        protected void Page_Load(object sender, EventArgs e)
        {
            string aa = DTRequest.GetQueryString("tojson");
            action = DTRequest.GetQueryString("action");
            this.id = DTRequest.GetQueryString("id");
            this.userId = GetAdminInfo().role_id.ToString();
            this.webApiDomain = ConfigHelper.GetAppSettings("webApiDomain");
            //data.Value = aa;
            //if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            //{
            //    this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
            //    this.id = DTRequest.GetQueryString("id");
            //    if (string.IsNullOrEmpty(id))
            //    {
            //        JscriptMsg("传输参数不正确！", "back");
            //        return;
            //    }
            //    if(!new BLL.P_MeetingAdmin().Exists(this.id))
            //    {
            //        JscriptMsg("纪录不存在或已删除！", "back");
            //        return;
            //    }
            //}

            //if (!Page.IsPostBack)
            //{
            //    ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.View.ToString());//检查权限

            //    if(action == DTEnums.ActionEnum.Edit.ToString())
            //    {
            //        ShowInfo(this.id);
            //    }
            //}
        }
        #endregion


        #region 赋值操作=================================
        private void ShowInfo(string _id)
        {
            BLL.P_MeetingAdmin bll = new BLL.P_MeetingAdmin();
            Model.P_MeetingAdmin model = bll.GetModel(_id);
   
            txtTitle.Text = model.P_Title;
            txtStartTime.Text = model.P_StartTime.ToString();
            txtEndTime.Text = model.P_EndTime.ToString();
            txtSite.Text = model.P_MeePlace;
            txtNumber.Text = model.P_PeopleAmount.ToString();
            
            //TextContent.Text = model.P_MeeContent;
        }
          #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            Model.manager user = GetAdminInfo();

            Model.P_MeetingAdmin model = new Model.P_MeetingAdmin();
            BLL.P_MeetingAdmin bll = new BLL.P_MeetingAdmin();
            model.P_Id = Guid.NewGuid().ToString();
            model.P_Title = txtTitle.Text;
            model.P_StartTime =Convert.ToDateTime(txtStartTime.Text);
            model.P_EndTime = Convert.ToDateTime(txtEndTime.Text);
            model.P_MeePlace = txtSite.Text;
            model.P_PeopleAmount =Convert.ToInt32(txtNumber.Text);

            model.P_MeeContent = txtContent.Value;
            //foreach (var item in asd[].Text)
            //{
               
            //}
            model.P_CreateTime = DateTime.Now;
            model.P_CreateUser = user.role_id.ToString();
            model.P_UpdateTime = DateTime.Now;
            model.P_UpdateUser = user.role_id.ToString();
            model.P_Status = 0;

            string result = bll.Add(model);
            if (!string.IsNullOrEmpty(result))
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加会议成功" + model.P_Id);//记录日志
                return true;
            }
            else
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加会议失败" + model.P_Title);
                return true;
            }
        }
        #endregion


        #region 修改操作=================================
        private bool DoEdit(string _id)
        {
            Model.manager user = GetAdminInfo();

            BLL.P_MeetingAdmin bll = new BLL.P_MeetingAdmin();
            Model.P_MeetingAdmin model = bll.GetModel(_id);

            string old_name = model.P_Title;

            model.P_Title = txtTitle.Text;
            model.P_StartTime = Convert.ToDateTime(txtStartTime.Text);
            model.P_EndTime = Convert.ToDateTime(txtEndTime.Text);
            model.P_MeePlace = txtSite.Text;
            model.P_PeopleAmount = Convert.ToInt32(txtNumber.Text);

            model.P_MeeContent = txtContent.Value;
            model.P_UpdateTime = DateTime.Now;
            model.P_UpdateUser = user.role_id.ToString();
            model.P_Status = 0;
            if (!bll.Update(model))
            {
                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改会议失败" + model.P_Title + "原数据" + old_name);//记录日志
                return false;
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改会议成功" + model.P_Title + "原数据" + old_name);//纪录日志
            return true;
        }
        #endregion


        #region 保存操作=================================
        protected void btnSubmit_Click(object sender,EventArgs e)
        {
            if(action == DTEnums.ActionEnum.Edit.ToString())
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Edit.ToString());

                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("修改数据成功！", "meetingManage_list.aspx", "parent.loadMenuTree");
            }
            else
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Add.ToString());
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("添加数据成功！", "meetingManage_list.aspx", "parent.loadMenuTree");
            }
        }

        #endregion

    }
}