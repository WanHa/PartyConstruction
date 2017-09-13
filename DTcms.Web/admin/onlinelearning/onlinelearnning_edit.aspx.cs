using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.onlinelearning
{
    public partial class onlinelearnning_edit : Web.UI.ManagePage
    {
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private string id = string.Empty;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");

            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryString("id");
                if (string.IsNullOrEmpty(id))
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new BLL.P_OnlineLearn().Exists(this.id))
                {
                    JscriptMsg("记录不存在或已删除！", "back");
                    return;
                }
            }
           
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.View.ToString()); //检查权限
              
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
               
            }
        }

        #region 赋值操作=================================
        private void ShowInfo(string _id)
        {
            BLL.P_OnlineLearn bll = new BLL.P_OnlineLearn();
            Model.P_OnlineLearn model = bll.GetModel(_id);

            txtTitle.Text = model.P_LearnUrl;
         
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            Model.manager user = GetAdminInfo();

            Model.P_OnlineLearn model = new Model.P_OnlineLearn();
            BLL.P_OnlineLearn bll = new BLL.P_OnlineLearn();

            model.P_Id = Guid.NewGuid().ToString();
            model.P_LearnUrl = txtTitle.Text;
            model.P_CreateTime = DateTime.Now;
            model.P_CreateUser = user.role_id.ToString();
            model.P_UpdateTime = DateTime.Now;
            model.P_UpdateUser = user.role_id.ToString();
            model.P_Status = 0;

            string result = bll.Add(model);
            if (!string.IsNullOrEmpty(result))
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加在线学习成功" + model.P_Id); //记录日志
                return true;
            }
            else
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加在线学失败" + model.P_LearnUrl); //记录日志
                return true;

            }
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(string _id)
        {
            Model.manager user = GetAdminInfo();

            BLL.P_OnlineLearn bll = new BLL.P_OnlineLearn();
            Model.P_OnlineLearn model = bll.GetModel(_id);

            string old_name = model.P_LearnUrl;

            model.P_LearnUrl = txtTitle.Text;
            model.P_UpdateTime = DateTime.Now;
            model.P_UpdateUser = user.role_id.ToString();
            model.P_Status = 0;
            if (!bll.Update(model))
            {

                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改在线学习失败" + model.P_LearnUrl + "原数据" + old_name); //记录日志
                return false;
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改在线学习成功" + model.P_LearnUrl + "原数据" + old_name); //记录日志
            return true;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string url = txtTitle.Text.ToString().Trim();
            if (!String.IsNullOrEmpty(txtTitle.Text))
            {
                string Pattern = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&$%\$#\=~])*$";
                Regex r = new Regex(Pattern);
                Match m = r.Match(url);
                if (!m.Success)
                {
                    JscriptMsg("输入在线学习网址不正确,请重新输入!", "");
                    return;
                }
            }
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("修改数据成功！", "onlinelearnning_list.aspx", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("添加数据成功！", "onlinelearnning_list.aspx", "parent.loadMenuTree");
            }
        }

    }
}