﻿using DTcms.Common;
using DTcms.DAL;
using DTcms.DBUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.PartyConstruction
{
    public partial class ModelMember : Web.UI.ManagePage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected int site_id;
        protected string keywords = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.site_id = DTRequest.GetQueryInt("site_id");
            this.keywords = DTRequest.GetQueryString("keywords");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.View.ToString()); //检查权限
                TreeBind(); //绑定类别
                RptBind("P_Status=0" + CombSqlTxt(site_id, keywords), "P_CreateTime desc");
            }
        }

        #region 绑定类别=================================
        private void TreeBind()
        {
            BLL.ModelMembers bll = new BLL.ModelMembers();
            DataTable dt = bll.GetList(0, "", "P_CreateTime desc").Tables[0];

            //this.ddlSiteId.Items.Clear();
            //this.ddlSiteId.Items.Add(new ListItem("所有站点", ""));
            //foreach (DataRow dr in dt.Rows)
            //{
            //    this.ddlSiteId.Items.Add(new ListItem(dr["P_UserId"].ToString(), dr["P_Id"].ToString()));
            //}
        }
        #endregion

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            //ddlSiteId.SelectedValue = this.site_id.ToString();
            txtKeywords.Text = this.keywords;
            BLL.ModelMembers bll = new BLL.ModelMembers();
            this.rptList.DataSource = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("ModelMember.aspx", "site_id={0}&keywords={1}&page={2}", this.site_id.ToString(), this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(int _site_id, string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();
            if (_site_id > 0)
            {
                strTemp.Append(" and site_id=" + _site_id);
            }
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" and (b.user_name like  '%" + _keywords + "%')");
            }

            return strTemp.ToString();
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("channel_page_size", "DTcmsPage"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion

        [WebMethod]
        public static string GetNameList(string key)
        {
            BLL.ModelMembers mm = new BLL.ModelMembers();
            List<Name> list = mm.GetUserNameList(key);
            string a = JsonConvert.SerializeObject(list);
            return a;
        }


        
        //动态搜索
        protected void btnSearchProject_Click(object sender, EventArgs e)
        {
            //Response.Redirect(Utils.CombUrlTxt("ModelMember.aspx", "site_id={0}&keywords={1}", this.site_id.ToString(), txtKeywords.Text));
        }

        //关健字查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("ModelMember.aspx", "site_id={0}&keywords={1}", this.site_id.ToString(), txtKeywords.Text));
        }

        //筛选类型
        protected void ddlSiteId_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Response.Redirect(Utils.CombUrlTxt("channel_list.aspx", "site_id={0}&keywords={1}", ddlSiteId.SelectedValue, this.keywords));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("channel_page_size", "DTcmsPage", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("ModelMember.aspx", "site_id={0}&keywords={1}", this.site_id.ToString(), this.keywords));
        }

        //保存排序
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            BLL.channel bll = new BLL.channel();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                int sortId;
                if (!int.TryParse(((TextBox)rptList.Items[i].FindControl("txtSortId")).Text.Trim(), out sortId))
                {
                    sortId = 99;
                }
                bll.UpdateSort(id, sortId);
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存频道排序"); //记录日志
            JscriptMsg("保存排序成功！", Utils.CombUrlTxt("ModelMember.aspx", "site_id={0}&keywords={1}", this.site_id.ToString(), this.keywords), "parent.loadMenuTree");
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Delete.ToString()); //检查权限
            int sucCount = 0;
            int errorCount = 0;
            BLL.ModelMembers bll = new BLL.ModelMembers();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                string id = ((HiddenField)rptList.Items[i].FindControl("hidId")).Value;
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    if (bll.Delete(id))
                    {
                        sucCount += 1;
                    }
                    else
                    {
                        errorCount += 1;
                    }
                }
            }
            AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除数据成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
                Utils.CombUrlTxt("ModelMember.aspx", "site_id={0}&keywords={1}", this.site_id.ToString(), this.keywords), "parent.loadMenuTree");
        }
    }
}