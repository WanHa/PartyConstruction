using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.Partypayment
{
    public partial class Partypayment_details : Web.UI.ManagePage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected int site_id;
        protected string keywords = string.Empty;
        public string id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = DTRequest.GetQueryString("keywords");
            this.id = DTRequest.GetQueryString("id");
            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.View.ToString()); //检查权限

                RptBind(CombSqlTxt(keywords, id), "P_UpdateTime desc,P_CreateTime desc");
            }
        }
        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;
            BLL.PartyPayMentPeople bll = new BLL.PartyPayMentPeople();
            this.rptList.DataSource = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
           this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("Partypayment_details.aspx", "id={0}&keywords={1}&page={2}", this.id,this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords, string id)
        {
            StringBuilder strTemp = new StringBuilder();

            strTemp.Append("P_Status= 0");
            strTemp.Append(" and (P_PayMentId = '" + id + "' )");
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" and (P_UserID like  '%" + _keywords + "%')");
            }
            //if (!string.IsNullOrEmpty(id))
            //{
            //    strTemp.Append(" and (P_PayMentId = '" + id + "' )");
            //}
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
        //关健字查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Response.Redirect(Utils.CombUrlTxt("Partypayment_details.aspx", "&keywords={0}", txtKeywords.Text));
            RptBind(CombSqlTxt(txtKeywords.Text,id), "P_UpdateTime desc,P_CreateTime desc");
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
            Response.Redirect(Utils.CombUrlTxt("Partypayment_details.aspx", "id={0}&site_id={0}&keywords={1}", id, this.site_id.ToString(), this.keywords));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Model.manager user = GetAdminInfo();

            ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Delete.ToString()); //检查权限
            int sucCount = 0;
            int errorCount = 0;
            BLL.P_PartyPayMent bll = new BLL.P_PartyPayMent();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                string id = ((HiddenField)rptList.Items[i].FindControl("hidId")).Value;
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {

                    Model.P_PartyPayMent model = bll.GetModel(id);
                    if (bll.Delete(id, user.role_id.ToString()))
                    {
                        sucCount += 1;
                    }
                    else
                    {
                        errorCount += 1;
                    }
                }
            }
            AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除在线学习成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
                Utils.CombUrlTxt("Partypayment_details.aspx", "id={0}&site_id={0}&keywords={0}", id, this.site_id.ToString(), this.keywords), "parent.loadMenuTree");
        }
    }
}