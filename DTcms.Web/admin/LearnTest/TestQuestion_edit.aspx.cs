using DTcms.Common;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace DTcms.Web.admin.LearnTest
{
    public partial class TestQuestion_edit : Web.UI.ManagePage
    {
        private string[] serialNumber = { "A.", "B.", "C.", "D.", "E.", "F.", "", "", "", "", "", "", "", };
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        protected string id = string.Empty;
        public string parentid = string.Empty;
        protected string answerHtmlStr ="";
        protected int answerCount = 0;
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
                if (!new BLL.P_TestQuestion().Exists(this.id))
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
            BLL.P_TestQuestion bll = new BLL.P_TestQuestion();
            Model.P_TestQuestion model = bll.GetModel(_id);

            txtTitle.Text = model.P_QuestionStem;
            TextType.Text = model.P_Type;
            TextScroe.Text = model.P_Score.ToString();

            if (model.questions_list != null && model.questions_list.Count > 0) {
                this.answerCount = model.questions_list.Count;
                for (int i = 0; i < model.questions_list.Count; i++)
                {
                    P_TestList item = model.questions_list[i];
                    string isanswer = item.P_Correct.ToString();
                    string ischeck = "";
                    if (item.P_Correct == 1) {
                        ischeck = "checked='" + item.P_Correct + "'";
                    }
                    answerHtmlStr += "<tr>" + "<td style='padding-left: 5px; padding-top: 5px;'>"
                        + "<input id='isSelected" + i + "' type='checkbox'"
                        + ischeck
                        + " onclick='SelectedFun(" + i + ")' style='width: 20px; height: 20px;'>"
                        + "</input>"
                        + "<input id='isanswer" + i + "' name='isanswer' type='text' value='"
                        + isanswer
                        + "' style='display: none'></input>"
                        + "</td>" + "<td style='padding-left: 5px; padding-top: 5px; font-size: 18px''>" +
                        serialNumber[i]
                        + "</td>"
                        + "<td style='padding-left: 5px; padding-top: 5px;' class='formValue'>" + "<input name='answer" + "' type='text' value='"
                        + item.P_Choices + "' style='width: 500px; height: 30px;'>" + "</input>" + "</td>"
                        + "<td style='padding-left: 5px; padding-top: 5px;'>" + "<input style='height: 30px;' type='button' onclick='DeleteTableRow("
                        + i + ")' value='删除'></input>" + "</td>"
                        + "</tr>";
                }
                this.answerHtmlSt.Value = answerHtmlStr;
            }
        }


        private bool DoAdd(string parentid)
        {
            string[] isanswer = Request.Form.GetValues("isanswer");
            string[] answer = Request.Form.GetValues("answer");

            Model.manager user = GetAdminInfo();

            Model.P_TestQuestion model = new Model.P_TestQuestion();
            BLL.P_TestQuestion bll = new BLL.P_TestQuestion();

            model.P_Id = Guid.NewGuid().ToString();
            model.P_TestPaperId = this.parentid;
            model.P_QuestionStem = txtTitle.Text;
            //model.P_Type = TextType.Text;
            model.P_Type = "单选";
            model.P_Score = int.Parse(TextScroe.Text);
            model.P_Status = 0;
            model.P_CreateUser = GetAdminInfo().id.ToString();
            model.P_CreateTime = DateTime.Now;
            List<P_TestList> answerList = new List<P_TestList>();
            for (int i = 0; i < isanswer.Length; i++)
            {
                Model.P_TestList answerItem = new Model.P_TestList()
                {
                    P_Id = Guid.NewGuid().ToString(),
                    P_Choices = answer[i],
                    P_Correct = int.Parse(isanswer[i]),
                    P_Status = 0,
                    P_CreateTime = DateTime.Now,
                    P_TestQuestionId = model.P_Id,
                    P_CreateUser = GetAdminInfo().id.ToString(),
                    P_Sequence = serialNumber[i]
                };
                answerList.Add(answerItem);
            }
            model.questions_list = answerList;
            string result = bll.Add(model, id);
            if (!string.IsNullOrEmpty(result))
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加试题成功" + model.P_Id); //记录日志
                return true;
            }
            else
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加试题失败" + model.P_TestPaperId); //记录日志
                return true;

            }
        }

        private bool DoEdit(string _id)
        {
            string[] isanswer = Request.Form.GetValues("isanswer");
            string[] answer = Request.Form.GetValues("answer");

            Model.manager user = GetAdminInfo();

            BLL.P_TestQuestion bll = new BLL.P_TestQuestion();
            Model.P_TestQuestion model = bll.GetModel(_id);

            string old_name = model.P_QuestionStem;

            model.P_QuestionStem = txtTitle.Text;
            model.P_Type = TextType.Text;
            model.P_Score = int.Parse(TextScroe.Text);
            model.P_UpdateTime = DateTime.Now;
            model.P_UpdateUser = user.id.ToString();

            List<P_TestList> answerList = new List<P_TestList>();
            for (int i = 0; i < isanswer.Length; i++)
            {
                Model.P_TestList answerItem = new Model.P_TestList()
                {
                    P_Id = Guid.NewGuid().ToString(),
                    P_Choices = answer[i],
                    P_Correct = int.Parse(isanswer[i]),
                    P_Status = 0,
                    P_CreateTime = DateTime.Now,
                    P_TestQuestionId = model.P_Id,
                    P_CreateUser = GetAdminInfo().id.ToString(),
                    P_Sequence = serialNumber[i]
                };
                answerList.Add(answerItem);
            }
            model.questions_list = answerList;
            if (!bll.Update(model))
            {

                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改试题失败" + model.P_TestPaperId + "原数据" + old_name); //记录日志
                return false;
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改试题成功" + model.P_TestPaperId + "原数据" + old_name); //记录日志
            return true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string[] isanswer = Request.Form.GetValues("isanswer");
            string[] answer = Request.Form.GetValues("answer");
            if (answer == null) {
                JscriptMsg("请录入答案", "");
                return;
            }
            if (isanswer != null && isanswer.Length > 0)
            {
                int length = isanswer.Length;
                Boolean isExistsAnswer = false;
                for (int i = 0; i < length; i++)
                {
                    if (isanswer[i] == "1")
                    {
                        isExistsAnswer = true;
                    }
                }
                if (!isExistsAnswer)
                {
                    JscriptMsg("请填写正确答案", "");
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
                JscriptMsg("修改数据成功！", "TestQuestion_list.aspx?id=" + this.parentid, "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd(this.parentid))
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("添加数据成功！", "TestQuestion_list.aspx?id=" + this.parentid, "parent.loadMenuTree");
            }
        }

    }
}