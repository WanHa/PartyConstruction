using DTcms.BLL;
using DTcms.Common;
using DTcms.Model;
using Qiniu.IO.Model;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.PartyCloud
{
    public partial class PartyCloud : Web.UI.ManagePage
    {
        protected int channel_id;
        protected int totalCount;
        protected int page;
        protected int pageSize;

        protected int category_id;
        protected string channel_name = string.Empty;
        protected string property = string.Empty;
        protected string keywords = string.Empty;
        protected string prolistview = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.channel_id = DTRequest.GetQueryInt("channel_id");
            this.category_id = DTRequest.GetQueryInt("category_id");
            this.keywords = DTRequest.GetQueryString("keywords");
            this.property = DTRequest.GetQueryString("property");

            this.channel_name = new BLL.channel().GetChannelName(this.channel_id); //取得频道名称
            this.pageSize = GetPageSize(10); //每页数量
            this.prolistview = Utils.GetCookie("article_list_view"); //显示方式
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("channel_" + this.channel_name + "_list", DTEnums.ActionEnum.View.ToString()); //检查权限
                TreeBind(this.channel_id); //绑定类别
                RptBind(this.channel_id, this.category_id, CombSqlTxt(this.keywords, this.property));
            }
        }

        #region 绑定类别=================================
        private void TreeBind(int _channel_id)
        {
            BLL.article_category bll = new BLL.article_category();
            DataTable dt = bll.GetList(0, _channel_id);

            foreach (DataRow dr in dt.Rows)
            {
                string Id = dr["id"].ToString();
                int ClassLayer = int.Parse(dr["class_layer"].ToString());

                string Title = dr["title"].ToString().Trim();

            }
        }
        #endregion

        #region 数据绑定=================================
        private void RptBind(int _channel_id, int _category_id, Tuple<string, string> _strWhere)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            //if (this.category_id > 0)
            //{
            //    this.ddlCategoryId.SelectedValue = _category_id.ToString();
            //}
            this.ddlProperty.SelectedValue = this.property;
            this.txtKeywords.Text = this.keywords;
            //图表或列表显示
            BLL.PartyCloud bll = new BLL.PartyCloud();
            switch (this.prolistview)
            {
                case "Txt":
                    this.rptList2.Visible = false;
                    this.rptList1.DataSource = bll.listFiles(this.pageSize, this.page, _strWhere,out this.totalCount);
                    this.rptList1.DataBind();
                    break;
                default:
                    this.rptList1.Visible = false;
                    this.rptList2.DataSource = bll.listFiles(this.pageSize, this.page, _strWhere,out this.totalCount);
                    this.rptList2.DataBind();
                    break;
            }
            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("PartyCloud_list.aspx", "channel_id={0}&category_id={1}&keywords={2}&property={3}&page={4}",
                _channel_id.ToString(), _category_id.ToString(), this.keywords, this.property, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected Tuple<string,string> CombSqlTxt(string _keywords, string _property)
        {
            string where = string.Empty;
            string type = string.Empty;
            if (!string.IsNullOrEmpty(_keywords))
            {
                //模糊查询关键字
                where = _keywords;
            }
            if (!string.IsNullOrEmpty(_property))
            {
                //获取前台所选类别，类别value值是从七牛云返回的mimetype里截取的，方便查询。
                type = _property;
            }
            //返回查询条件的元组
            var tuples = new Tuple<string, string>(where, type);
            return tuples;
        }
        #endregion

        #region 返回图文每页数量=========================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("article_page_size", "DTcmsPage"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion

        //设置操作
        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ChkAdminLevel("channel_" + this.channel_name + "_list", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            string id = ((HiddenField)e.Item.FindControl("hidId")).Value;
            this.RptBind(this.channel_id, this.category_id, CombSqlTxt(this.keywords, this.property));
        }

        //关健字查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("PartyCloud_list.aspx", "channel_id={0}&category_id={1}&keywords={2}&property={3}",
                this.channel_id.ToString(), this.category_id.ToString(), txtKeywords.Text, this.property));
        }


        //筛选属性
        protected void ddlProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("PartyCloud_list.aspx", "channel_id={0}&category_id={1}&keywords={2}&property={3}",
               this.channel_id.ToString(), this.category_id.ToString(), this.keywords, ddlProperty.SelectedValue));
        }


        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("article_page_size", "DTcmsPage", _pagesize.ToString(), 43200);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("PartyCloud_list.aspx", "channel_id={0}&category_id={1}&keywords={2}&property={3}",
                this.channel_id.ToString(), this.category_id.ToString(), this.keywords, this.property));
        }

        //保存排序
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("channel_" + this.channel_name + "_list", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            BLL.article bll = new BLL.article();
            Repeater rptList = new Repeater();
            switch (this.prolistview)
            {
                case "Txt":
                    rptList = this.rptList1;
                    break;
                default:
                    rptList = this.rptList2;
                    break;
            }
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                int sortId;
                if (!int.TryParse(((TextBox)rptList.Items[i].FindControl("txtSortId")).Text.Trim(), out sortId))
                {
                    sortId = 99;
                }
                bll.UpdateField(id, "sort_id=" + sortId.ToString());
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存" + this.channel_name + "频道内容排序"); //记录日志
            JscriptMsg("保存排序成功！", Utils.CombUrlTxt("PartyCloud_list.aspx", "channel_id={0}&category_id={1}&keywords={2}&property={3}",
                this.channel_id.ToString(), this.category_id.ToString(), this.keywords, this.property));
        }


        //批量删除(暂时不要，后续你们再研究吧)
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("channel_" + this.channel_name + "_list", DTEnums.ActionEnum.Delete.ToString()); //检查权限
            int sucCount = 0; //成功数量
            int errorCount = 0; //失败数量
            BLL.PartyCloud bll = new BLL.PartyCloud();
            Repeater rptList = new Repeater();
            switch (this.prolistview)
            {
                case "Txt":
                    rptList = this.rptList1;
                    break;
                default:
                    rptList = this.rptList2;
                    break;
            }
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                string id = ((HiddenField)rptList.Items[i].FindControl("hidId")).Value;
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    if (bll.delete(id))
                    {
                        sucCount++;
                    }
                    else
                    {
                        errorCount++;
                    }
                }
            }
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "删除" + this.channel_name + "频道内容成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("PartyCloud_list.aspx", "channel_id={0}&category_id={1}&keywords={2}&property={3}",
                this.channel_id.ToString(), this.category_id.ToString(), this.keywords, this.property));
        }

        /// <summary>
        /// 下载记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void download_Click(object sender, EventArgs e)
        {
            P_QiNiuInfo info = new QiNiu().GetQiNiuConfigInfo();
            string url = info == null ? "" : info.P_RootUrl;
            string AK = info == null ? "" : info.P_AK;
            string SK = info == null ? "" : info.P_SK;
            //获取点击文件的key值
            string key =((LinkButton)sender).CommandArgument;
            Mac mac = new Mac(AK, SK);
            Auth auth = new Auth(mac);
            url = url + key;
            //获取七牛token
            string token = auth.CreateDownloadToken(url);
            //拼接七牛下载url
            string newurl = url + "?attname=&token=" + token;
            string userid = GetAdminInfo().id.ToString();
            BLL.PartyCloud bll = new BLL.PartyCloud();
            //在数据库进行记录
            if (bll.DownloadRecord(userid, key))
            {
                //打开新页面，保留原页面
                Response.Write("<script>window.open('" + newurl + "','_blank')</script>");
            }
        }

        /// <summary>
        /// 查看记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void view_Click(object sender, EventArgs e)
        {
            P_QiNiuInfo info = new QiNiu().GetQiNiuConfigInfo();
            string url = info == null ? "" : info.P_RootUrl;

            string key = ((LinkButton)sender).CommandArgument;
            url = url + key;
            string userid = GetAdminInfo().id.ToString();
            BLL.PartyCloud bll = new BLL.PartyCloud();
            if (bll.VisitRecord(userid, key))
            {
                Response.Write("<script>window.open('" + url + "','_blank')</script>");
            }
        }
    }
}