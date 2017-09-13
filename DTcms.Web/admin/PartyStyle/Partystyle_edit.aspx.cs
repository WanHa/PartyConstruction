using DTcms.BLL;
using DTcms.Common;
using DTcms.Model;
using Qiniu.IO.Model;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using static DTcms.DAL.Partystyle_logic;

namespace DTcms.Web.admin.PartyStyle
{
    public partial class Partystyle_edit : Web.UI.ManagePage
    {
        public string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        public string id = string.Empty;
        protected string qiniu_uptoken;
        protected string qiniu_domain;
        public string userId = string.Empty;
        public string webApiDomain = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            action = DTRequest.GetQueryString("action");
            this.id = DTRequest.GetQueryString("id");
            this.userId = GetAdminInfo().role_id.ToString();
            this.webApiDomain = ConfigHelper.GetAppSettings("webApiDomain");

            if (!string.IsNullOrEmpty(action) && action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryString("id");
                if (string.IsNullOrEmpty(id))
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new BLL.Style().Exists(this.id))
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
        //页面初始化事件
        protected void Page_Init(object sernder, EventArgs e)
        {
            GetQiNiuUpToken();
        }
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
        private void ShowInfo(string _id)
        {
            //BLL.Style bll = new BLL.Style();
            //Webstyle model = bll.GetDetail(_id);
            //activityname.Text = model.P_ActivityName;
            //activitysite.Text = model.P_ActivitySite;
            //sponsor.Text = model.P_Sponsor;//主办单位
            //starttime.Text = model.P_ActivityStartTime.ToString("yyyy-MM-dd HH:mm:ss");
            //endtime.Text = model.P_ActivityEndTime.ToString("yyyy-MM-dd HH:mm:ss");
            //txtImgUrl.Text = model.img_url;
            //txtContent.Value = model.P_Particular;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("修改失败!", "");
                    return;
                }
                JscriptMsg("修改数据成功！", "Partystyle.aspx", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("添加失败", "");
                    return;
                }
                else
                {
                    JscriptMsg("添加数据成功！", "Partystyle.aspx", "parent.loadMenuTree");
                }
            }
        }
        private bool DoAdd()
        {
        //    Model.manager user = GetAdminInfo();

        //    Model.P_ActivityStyle model = new Model.P_ActivityStyle();
        //    BLL.Style bll = new BLL.Style();
        //    model.P_Id = Guid.NewGuid().ToString();
        //    model.P_ActivityName = activityname.Text;
        //    model.P_ActivitySite = activitysite.Text;
        //    model.P_Sponsor = sponsor.Text;
        //    model.P_Particular = txtContent.Value;
        //    model.P_ActivityStartTime = Convert.ToDateTime(starttime.Text);
        //    model.P_ActivityEndTime = Convert.ToDateTime(endtime.Text);
        //    model.P_CreateTime = DateTime.Now;
        //    model.P_CreateUser = user.role_id.ToString();
        //    model.img_url = txtImgUrl.Text;
        //    model.P_Status = 0;

        //    bool result = bll.StyleAdd(model);
        //    if (result)
        //    {
        //        if (!string.IsNullOrEmpty(result))
        //        {
        //            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加成功" + model.P_Id); //记录日志
        //            return true;
        //        }
        //        else
        //        {
        //            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加失败" + model.P_ActivityName); //记录日志
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加成功" + model.P_ActivityName); //记录日志
                return true;
        //    }
        }
        private bool DoEdit(string _id)
        {
            Model.manager user = GetAdminInfo();

            BLL.Style bll = new BLL.Style();
            //Model.P_ActivityStyle model = bll.GetModel(_id);
            //string old_name = model.P_ActivityName;

            //model.P_ActivityName = activityname.Text;
            //model.P_ActivitySite = activitysite.Text;
            //model.P_Sponsor = sponsor.Text;
            //model.P_Particular = txtContent.Value;
            //model.P_ActivityStartTime= Convert.ToDateTime(starttime.Text);
            //model.P_ActivityEndTime= Convert.ToDateTime(endtime.Text);
            //model.P_UpdateTime = DateTime.Now;
            //model.P_UpdateUser = user.role_id.ToString();

            //if (bll.Update(model))
            //{
            //    AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改成功" + model.P_ActivityName + "原数据" + old_name); //记录日志
            //    return true;
            //}
            //else
            //{
            //    AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改失败" + model.P_ActivityName + "原数据" + old_name); //记录日志
            //    return false;
            //}
            return true;
        }
    }
}