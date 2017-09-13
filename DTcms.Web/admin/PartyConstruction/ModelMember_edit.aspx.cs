using DTcms.Common;
using DTcms.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.PartyConstruction
{
    public partial class ModelMember_edit : Web.UI.ManagePage
    {
        protected string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private string id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryString("id");
                if (string.IsNullOrEmpty(this.id))
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new BLL.ModelMembers().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                //ChkAdminLevel("user_list", DTEnums.ActionEnum.View.ToString()); //检查权限
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }

        private void ShowInfo(string _id)
        {
            BLL.ModelMembers bll = new BLL.ModelMembers();
            DataRow row = bll.GetEditInfo(_id);
            Hidden.Value = row["user_name"].ToString();
            manager.Value = row["user_name"].ToString();
            des.Value = row["P_Description"].ToString();
        }

        private bool DoAdd()
        {
            bool result = false;
            BLL.ModelMembers bll = new BLL.ModelMembers();
            string userid = bll.GetUserId(manager.Value.Trim());
            Model.P_ModelPartyMember model = new Model.P_ModelPartyMember();
            model.P_Id = Guid.NewGuid().ToString("N");
            model.P_UserId = userid;
            model.P_Description = des.Value.Trim();
            model.P_CreateUser = GetAdminInfo().id.ToString();
            model.P_CreateTime = DateTime.Now;
            model.P_Status = 0;

            if (bll.Add(model))
            {
                //AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加用户:" + model.user_name); //记录日志
                result = true;
            }
            return result;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string a = manager.Value;
            string b = des.Value;
            string id = this.id = DTRequest.GetQueryString("id");
            string userid = GetAdminInfo().id.ToString();
            BLL.ModelMembers mm = new BLL.ModelMembers();

            if (action == DTEnums.ActionEnum.Edit.ToString())
            {
                bool bol = mm.UpsetRole(id, a, b, userid);
                if (bol)
                {
                    JscriptMsg("修改成功",
                    Utils.CombUrlTxt("ModelMember.aspx", "site_id=0&keywords={1}", "0", ""), "parent.loadMenuTree");
                }
            }
            else
            {
                //查询新增模范党员是否已存在
                if (mm.CheckRepeat(a))
                {
                    bool add = DoAdd();
                    if (add)
                    {
                        JscriptMsg("添加成功",
                        Utils.CombUrlTxt("ModelMember.aspx", "site_id=0&keywords={1}", "0", ""), "parent.loadMenuTree");
                    }
                }
                else
                {
                    JscriptMsg("未选择或所选党员已存在，添加失败",
                            Utils.CombUrlTxt("ModelMember_edit.aspx", "site_id=0&keywords={1}", "0", ""), "parent.loadMenuTree");
                }
            }

        }

        [WebMethod]
        public static string GetNameList(string key)
        {
            BLL.ModelMembers mm = new BLL.ModelMembers();
            List<Name> list = mm.GetUserNameList(key);
            string a = JsonConvert.SerializeObject(list);
            return a;
        }
    }
}