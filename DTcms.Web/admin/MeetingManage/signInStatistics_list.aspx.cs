using DTcms.Common;
using DTcms.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.MeetingManage
{
    public partial class signInStatistics_list : Web.UI.ManagePage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected int site_id;
        protected string id;
        protected string keywords = string.Empty;
        public string webApiDomain = string.Empty;

        #region 页面加载=================================
        protected void Page_Load(object sender, EventArgs e)
        {
            //string action = DTRequest.GetQueryString("action");
            this.id = DTRequest.GetQueryString("id");
            this.keywords = DTRequest.GetQueryString("keywords");
            this.pageSize = GetPageSize(10); //每页数量
            this.webApiDomain = ConfigHelper.GetAppSettings("webApiDomain");
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.View.ToString()); //检查权限

                RptBind(CombSqlTxt(id, keywords), "P_UpdateTime desc,P_CreateTime desc");
            }
        }
        #endregion

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;
            BLL.signlnStatistics bll = new BLL.signlnStatistics();
            this.rptList.DataSource = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("signInStatistics_list.aspx", "id={0}&keywords={1}&page={2}", this.id,this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion


        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string id, string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();

            strTemp.Append(" P_Status= 0");
            strTemp.Append(" and P_MeeId ='" + id + "'");
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                //strTemp.Append(" and P_UserId like  '%" + _keywords + "%'");
                strTemp.Append(" and user_name like  '%" + _keywords + "%'");
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


        #region 关健字查询=============================
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            RptBind(CombSqlTxt(id, txtKeywords.Text), "P_UpdateTime desc,P_CreateTime desc");
        }
        #endregion


        #region 分页数量=================================
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
            Response.Redirect(Utils.CombUrlTxt("signInStatistics_list.aspx", "id={0}&site_id={0}&keywords={1}", this.id,this.site_id.ToString(), this.keywords));
        }
        #endregion

        [WebMethod]
        public static getbool UpdateType(string id)
        {
            BLL.signlnStatistics bll = new BLL.signlnStatistics();
            bool gettype = bll.UpdateType(id);
            getbool model = new getbool();
            model.result = gettype;
            return model;

            //string type = JsonConvert.SerializeObject(model);
            //return type;
        }
    }

    public class getbool
    {
        public bool result { get; set; }
    }

}