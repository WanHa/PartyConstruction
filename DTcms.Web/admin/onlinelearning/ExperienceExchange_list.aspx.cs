﻿using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.onlinelearning
{
	public partial class ExperienceExchange_list : Web.UI.ManagePage
	{  protected int totalCount;
        protected int page;
        protected int pageSize;

        protected int site_id;
        protected string keywords = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            this.keywords = DTRequest.GetQueryString("keywords");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.View.ToString()); //检查权限
              
                RptBind(CombSqlTxt(keywords), "P_UpdateTime desc,P_CreateTime desc");
            }
        }

     

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
			txtkeywords.Text = this.keywords;
            BLL.ExperienceExchange bll = new BLL.ExperienceExchange();
            this.rptList.DataSource = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("ExperienceExchange_list.aspx", "keywords={0}&page={1}",this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();

			strTemp.Append("P_Status= 0");

			_keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" and (P_title like  '%" + _keywords + "%')");
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
			
			RptBind(CombSqlTxt(txtkeywords.Text), "P_UpdateTime desc,P_CreateTime desc");
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
			Response.Redirect(Utils.CombUrlTxt("channel_list.aspx", "site_id={0}&keywords={1}", this.site_id.ToString(), this.keywords));
		}
	

		//删除操作
		protected void btnDelete_Click(object sender, EventArgs e)
		{
			ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Delete.ToString()); //检查权限
			int sucCount = 0;
			int errorCount = 0;
			BLL.ExperienceExchange bll = new BLL.ExperienceExchange();
			for (int i = 0; i < rptList.Items.Count; i++)
			{
				string id = ((HiddenField)rptList.Items[i].FindControl("hidId")).Value;
				CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
				if (cb.Checked)
				{
					Model.P_LearnExchange model = bll.GetModel(id);
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
			AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
			JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
				Utils.CombUrlTxt("ExperienceExchange_list.aspx", "keywords={0}", this.keywords), "parent.loadMenuTree");
		}
	}
}