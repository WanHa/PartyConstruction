using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.LearnTest
{
    public partial class TestPaper_edit : Web.UI.ManagePage
    {
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private string id = string.Empty; // 编辑试卷时，需要编辑的试卷ID
        public string parentid = string.Empty; // 新建或编辑时，试卷所属的题库ID（用于试卷页面刷新使用）
        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            this.parentid = DTRequest.GetQueryString("parentid");
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryString("id");
                if (string.IsNullOrEmpty(id))
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new BLL.P_TestPaper().Exists(this.id))
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
        private void ShowInfo(string _id)
        {
            BLL.P_TestPaper bll = new BLL.P_TestPaper();
            Model.P_TestPaper model = bll.GetModel(_id);

            txtTitle.Text = model.P_TestPaperName;
            TextBox1.Text = model.P_Description;
            TextBox2.Text = model.P_AnswerTime.ToString();
            if (model.P_IsRepeat == 1)
            {
                cbIsLock.Checked = true;
            }

        }
        private bool DoAdd(string parentid)
        {
            Model.manager user = GetAdminInfo();

            Model.P_TestPaper model = new Model.P_TestPaper();
            BLL.P_TestPaper bll = new BLL.P_TestPaper();

            model.P_Id = Guid.NewGuid().ToString();
            model.P_QuestionBankId = "parentid";
            model.P_TestPaperName = txtTitle.Text;
            model.P_Description = TextBox1.Text;
            model.P_AnswerTime = int.Parse(TextBox2.Text.ToString().Trim() == "" ? "0" : TextBox2.Text.ToString());
            model.P_CreateTime = DateTime.Now;
            model.P_CreateUser = user.role_id.ToString();
            model.P_UpdateTime = DateTime.Now;
            model.P_UpdateUser = user.role_id.ToString();
            model.P_Status = 0;
            model.P_IsRepeat = 0;
            if (cbIsLock.Checked == true) {
                model.P_IsRepeat = 1;
            }
            string result = bll.Add(model, parentid);
            if (!string.IsNullOrEmpty(result))
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加试卷成功" + model.P_Id); //记录日志
                return true;
            }
            else
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加试卷失败" + model.P_TestPaperName); //记录日志
                return true;

            }
        }
        #region 修改操作=================================
        private bool DoEdit(string _id)
        {
            Model.manager user = GetAdminInfo();

            BLL.P_TestPaper bll = new BLL.P_TestPaper();
            Model.P_TestPaper model = bll.GetModel(_id);

            string old_name = model.P_TestPaperName;

            model.P_QuestionBankId = "parentid";
            model.P_TestPaperName = txtTitle.Text;
            model.P_UpdateTime = DateTime.Now;
            model.P_UpdateUser = user.role_id.ToString();
            model.P_Status = 0;
            model.P_IsRepeat = 0;
            if (cbIsLock.Checked == true)
            {
                model.P_IsRepeat = 1;
            }
            if (!bll.Update(model))
            {

                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改试卷失败" + model.P_TestPaperName + "原数据" + old_name); //记录日志
                return false;
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改试卷成功" + model.P_TestPaperName + "原数据" + old_name); //记录日志
            return true;
        }
        #endregion



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
                JscriptMsg("修改数据成功！", "TestPaper_list.aspx?id=" + this.parentid, "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd(this.parentid))
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("添加数据成功！", "TestPaper_list.aspx?id=" + this.parentid, "parent.loadMenuTree");
            }
        }
    }
}
