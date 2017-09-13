using DTcms.Common;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using static DTcms.DAL.Partystyle_logic;

namespace DTcms.Web.admin.PartyStyle
{
    public partial class Partystyle : Web.UI.ManagePage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string prolistview = string.Empty;
        protected int site_id;
        protected string keywords = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = DTRequest.GetQueryString("keywords");
            this.prolistview = Utils.GetCookie("article_list_view"); //显示方式
            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.View.ToString()); //检查权限

                RptBind(CombSqlTxt(keywords), "P_CreateTime desc");
            }
        }
        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;
            BLL.Style bll = new BLL.Style();
            this.rptLists.DataSource = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
            this.rptLists.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("Partystyle.aspx", "keywords={0}&page={1}", this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();

            strTemp.Append("P_Status=0");

            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" and (P_ActivityName like  '%" + _keywords + "%')");
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
        //关健字查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RptBind(CombSqlTxt(txtKeywords.Text), "P_UpdateTime desc,P_CreateTime desc");
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
            Response.Redirect(Utils.CombUrlTxt("Partystyle.aspx", "site_id={0}&keywords={1}", this.site_id.ToString(), this.keywords));
        }
        //删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Model.manager user = GetAdminInfo();

            ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Delete.ToString()); //检查权限
            int sucCount = 0;
            int errorCount = 0;
            BLL.Style bll = new BLL.Style();
            //BLL.P_UserDemand bll = new BLL.P_UserDemand();
            for (int i = 0; i < rptLists.Items.Count; i++)
            {
                string id = ((HiddenField)rptLists.Items[i].FindControl("hidId")).Value;
                CheckBox cb = (CheckBox)rptLists.Items[i].FindControl("chkId");
                if (cb.Checked)
                {

                    Webstyle model = bll.GetDetail(id);
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
            AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
                Utils.CombUrlTxt("Partystyle.aspx", "keywords={0}", this.keywords), "parent.loadMenuTree");
        }
    }
}