using DTcms.Common;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.DailyLog
{
    public partial class DailyLog_list : Web.UI.ManagePage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;

        protected int site_id;
        protected string keywords = string.Empty;
        protected string status = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = DTRequest.GetQueryString("keywords");
            this.status = DTRequest.GetQueryString("status");
            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.View.ToString()); //检查权限

                RptBind(CombSqlTxt(keywords, status), "P_UpdateTime desc,P_CreateTime desc");
            }

        }
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;
            selType.Text = this.status;
            BLL.P_DailyLogWord bll = new BLL.P_DailyLogWord();
            this.rptList.DataSource = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("DailyLog_list.aspx", "keywords={0}&status={1}&page={2}", this.keywords,this.status,"__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
           
            RptBind(CombSqlTxt(txtKeywords.Text, selType.Text), "P_UpdateTime desc,P_CreateTime desc");
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
            Response.Redirect(Utils.CombUrlTxt("DailyLog_list.aspx", "site_id={0}&keywords={1}", this.site_id.ToString(), this.keywords));
        }

        //内容筛选
        protected void selType_SelecteTypeChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("DailyLog_list.aspx", "status={0}", selType.SelectedValue, this.status));
        }

        //关键字查询
        protected string CombSqlTxt(string _keywords,string _status)
        {
            StringBuilder strTemp = new StringBuilder();

            strTemp.Append("P_Status= 0");

            _keywords = _keywords.Replace("'", "");
            _status = _status.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                 strTemp.Append(" and (P_title like  '%" + _keywords + "%')");
            }
            if (!string.IsNullOrEmpty(_status))
            {
                switch (_status)
                {
                    case "1":
                        strTemp.Append(" and (P_TypeId = 1)");
                        break;
                    case "2":
                        strTemp.Append(" and (P_TypeId = 2)");
                        break;
                    case "3":
                        strTemp.Append(" and (P_TypeId = 3)");
                        break;
                }           
            }
            return strTemp.ToString();
        }

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
        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Model.manager user = GetAdminInfo();

            ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Delete.ToString()); //检查权限
            int sucCount = 0;
            int errorCount = 0;
            BLL.P_DailyLogWord bll = new BLL.P_DailyLogWord();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                string id = ((HiddenField)rptList.Items[i].FindControl("hidId")).Value;
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {

                    DetailWoedJournal model = bll.SelPartyNewsPeper(id);
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
            AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除工作日志成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
            Utils.CombUrlTxt("DailyLog_list.aspx", "keywords={0}", this.keywords), "parent.loadMenuTree");
        }
        
    }
}