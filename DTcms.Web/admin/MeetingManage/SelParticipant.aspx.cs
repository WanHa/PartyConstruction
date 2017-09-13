using DTcms.Common;
using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace DTcms.Web.admin.MeetingManage
{
    public partial class SelParticipant : Web.UI.ManagePage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected int site_id;
        protected string keywords = string.Empty;
        public int test = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = DTRequest.GetQueryString("keywords");
            //this.pageSize = GetPageSize(10); //每页数量
            var a = txta.Value;
            test++;
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.View.ToString()); //检查权限

                RptBind(CombSqlTxt(keywords,a), "reg_time desc");
            }
        }


        ///组合SQL语句查询
        private string CombSqlTxt(string _keywords,string sql)
        {
            StringBuilder strTemp = new StringBuilder();
            if (!string.IsNullOrEmpty(sql))
            {
                strTemp.Append("and dt_users.group_id like (select '%,'+CONVERT(nvarchar,dt_user_groups.id) +',%' from dt_user_groups where title='" + sql + "')");
                if (!string.IsNullOrEmpty(_keywords))
                {
                    strTemp.Append("  and  (user_name like  '%" + _keywords + "%')");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(_keywords))
                {
                    strTemp.Append("  and  (user_name like  '%" + _keywords + "%')");
                }
            }

            strTemp.Append("");
             _keywords = _keywords.Replace("'", "");
          
            return strTemp.ToString();
        }


        // 数据绑定
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            this.keywords = txtKeywords.Text;
            //txtKeywords.Text = this.keywords;
            BLL.SelParticipant bll = new BLL.SelParticipant();
            this.rptList.DataSource = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
            this.rptList.DataBind();

           // txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("SelParticipant.aspx", "keywords={0}&page={1}", this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }



        // 返回每页数量
        //private int GetPageSize(int _default_size)
        //{
        //    int _pagesize;
        //    if (int.TryParse(Utils.GetCookie("channel_page_size", "DTcmsPage"), out _pagesize))
        //    {
        //        if (_pagesize > 0)
        //        {
        //            return _pagesize;
        //        }
        //    }
        //    return _default_size;
        //}

        //设置分页数量
        //protected void txtPageNum_TextChanged(object sender, EventArgs e)
        //{
        //    int _pagesize;
        //    if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
        //    {
        //        if (_pagesize > 0)
        //        {
        //            Utils.WriteCookie("channel_page_size", "DTcmsPage", _pagesize.ToString(), 14400);
        //        }
        //    }
        //    Response.Redirect(Utils.CombUrlTxt("SelParticipant.aspx", "site_id={0}&keywords={1}", this.site_id.ToString(), this.keywords));
        //}


        //批量提交
        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    List<User11> list = new List<User11>();
        //    for (int i = 0; i < rptList.Items.Count; i++)
        //    {
        //        string hiduser = ((HiddenField)rptList.Items[i].FindControl("hiduser")).Value;
        //        string hidId = ((HiddenField)rptList.Items[i].FindControl("hidId")).Value;
        //        CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
             
        //        if (cb.Checked)
        //        {
        //            User11 cc = new User11();
        //            cc.id = hidId;
        //            cc.name = hiduser;
        //            list.Add(cc);
        //        }

        //    }
        //    string json = JsonHelper.ObjectToJSON(list);
        //    //JscriptMsg("提交成功", "meetingManage_edit.aspx?tojson="+ json, "parent.loadMenuTree");
        //    JscriptMsg("提交成功", Utils.CombUrlTxt("meetingManage_edit.aspx", "tojson={0}", json));
        //}


        // 关键字搜索
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            RptBind(CombSqlTxt(txtKeywords.Text, txta.Value), "reg_time desc");
        }


        [WebMethod]
        public static string GetOrganizeList()
        {
            BLL.SelParticipant dal = new BLL.SelParticipant();
            string list = dal.GetOrganizeNameList();
            return list;
        }
    }
    public class User11
    {
        public string id { get; set; }

        public string name { get; set; }
    }
}