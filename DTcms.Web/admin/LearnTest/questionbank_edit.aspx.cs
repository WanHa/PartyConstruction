using DTcms.BLL;
using DTcms.Common;
using DTcms.Model;
using Qiniu.IO.Model;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.LearnTest
{
    public partial class questionbank_edit : Web.UI.ManagePage
    {
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private string id = string.Empty;
        protected string qiniu_uptoken;
        protected string qiniu_domain;

        protected void Page_Load(object sender, EventArgs e)
        {
            GetQiNiuUpToken();
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
                if (!new BLL.P_QuestionBank().Exists(this.id))
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

        #region 初始化七牛云参数=========
        private void GetQiNiuUpToken()
        {
            P_QiNiuInfo info = new QiNiu().GetQiNiuConfigInfo();

            this.qiniu_domain = info == null ? "" : info.P_RootUrl;
            string AK = info == null ? "" : info.P_AK;
            string SK = info == null ? "" : info.P_SK;
            string scope = info == null ? "" : info.P_Scope;

            Dictionary<string, string> dic = new Dictionary<string, string>();

            Mac mac = new Mac(AK, SK);
            Auth auth = new Auth(mac);
            PutPolicy putPolicy = new PutPolicy();

            putPolicy.Scope = scope;
            putPolicy.SetExpires(3600);
            putPolicy.InsertOnly = 0;
            qiniu_uptoken = auth.CreateUploadToken(putPolicy.ToJsonString());

        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(string _id)
        {
            BLL.P_QuestionBank bll = new BLL.P_QuestionBank();
            Model.P_QuestionBank model = bll.GetModel(_id);

            txtTitle.Text = model.P_QuestionBankName;
            txtImage.Text = model.P_ImageId;
            txtContent.Text = model.P_Description;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            Model.manager user = GetAdminInfo();

            Model.P_QuestionBank model = new Model.P_QuestionBank();
            BLL.P_QuestionBank bll = new BLL.P_QuestionBank();

            model.P_Id = Guid.NewGuid().ToString();
            model.P_QuestionBankName = txtTitle.Text;
            model.P_ImageId = txtImage.Text;
            model.P_Description = txtContent.Text;
            model.P_CreateTime = DateTime.Now;
            model.P_CreateUser = user.role_id.ToString();
            model.P_UpdateTime = DateTime.Now;
            model.P_UpdateUser = user.role_id.ToString();
            model.P_Status = 0;

            string result = bll.Add(model);
            if (!string.IsNullOrEmpty(result))
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加题库成功" + model.P_Id); //记录日志
                return true;
            }
            else
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加题库失败" + model.P_QuestionBankName); //记录日志
                return true;

            }
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(string _id)
        {
            Model.manager user = GetAdminInfo();

            BLL.P_QuestionBank bll = new BLL.P_QuestionBank();
            Model.P_QuestionBank model = bll.GetModel(_id);

            string old_name = model.P_QuestionBankName;

            model.P_QuestionBankName = txtTitle.Text;
            model.P_ImageId = txtImage.Text;
            model.P_Description = txtContent.Text;
            model.P_UpdateTime = DateTime.Now;
            model.P_UpdateUser = user.role_id.ToString();
            model.P_Status = 0;
            if (!bll.Update(model))
            {

                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改题库失败" + model.P_QuestionBankName + "原数据" + old_name); //记录日志
                return false;
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改题库成功" + model.P_QuestionBankName + "原数据" + old_name); //记录日志
            return true;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("修改数据成功！", "questionbank_list.aspx", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("添加数据成功！", "questionbank_list.aspx", "parent.loadMenuTree");
            }
        }

    }
}