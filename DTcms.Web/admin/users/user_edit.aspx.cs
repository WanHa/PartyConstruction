using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Common;
using DTcms.Model;
using DTcms.BLL;
using Qiniu.Util;
using Qiniu.IO.Model;

namespace DTcms.Web.admin.users
{
    public partial class user_edit : Web.UI.ManagePage
    {
        string defaultpassword = "0|0|0|0"; //默认显示密码
        protected string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;
        protected string qiniu_uptoken;
        protected string qiniu_domain;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            GetQiNiuUpToken();
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryInt("id");
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new BLL.users().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("user_list", DTEnums.ActionEnum.View.ToString()); //检查权限
                TreeBind("is_lock=0"); //绑定类别
                                       //ListBind();//先拉菜单绑定
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }


        #region 绑定类别=================================
        private void TreeBind(string strWhere)
        {
            BLL.user_groups bll = new BLL.user_groups();
            DataTable dt = bll.GetList(0, strWhere, "grade asc,id asc").Tables[0];

            this.ddlGroupId.Items.Clear();
            this.ddlGroupId.Items.Add(new ListItem("请选择支部...", ""));
            foreach (DataRow dr in dt.Rows)
            {
                this.ddlGroupId.Items.Add(new ListItem(dr["title"].ToString(), dr["id"].ToString()));
            }
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            BLL.users bll = new BLL.users();
            Model.users model = bll.GetModel(_id);
            Model.P_Image img = bll.GetImageModel(_id);

            ddlGroupId.SelectedValue = model.group_id.ToString();
            // 2017-08-02 因业务需求人员状态不可修改。
            //rblStatus.SelectedValue = model.status.ToString();
            txtUserName.Text = model.user_name;
            //txtUserName.ReadOnly = true;
            //txtUserName.Attributes.Remove("ajaxurl");
            if (!string.IsNullOrEmpty(model.password))
            {
                txtPassword.Attributes["value"] = txtPassword1.Attributes["value"] = defaultpassword;
            }
            txtEmail.Text = model.email;
            txtNickName.Text = model.nick_name;
            if(img != null)
            {
                txtImgUrl.Text = img.P_PictureName;
            }
            rblSex.SelectedValue = model.sex;
            if (model.birthday != null)
            {
                txtBirthday.Text = model.birthday.GetValueOrDefault().ToString("yyyy-M-d");
            }
            txtTelphone.Text = model.telphone;
            txtMobile.Text = model.mobile;
            txtQQ.Text = model.qq;
            txtwechat.Text = model.wechat == null ? "" : model.wechat;
            txtMsn.Text = model.msn;
            txtAddress.Text = model.address;
            identity.Text = model.id_card.ToString() == null ? "" : model.id_card.ToString();
            txtnationId.Text = model.nation.ToString() == null ? "" : model.nation.ToString();
            maritalStatus.SelectedValue = model.marital_status.ToString();

            if (model.new_class_id == "否")
            {
                new1.Checked = true;
            }
            else if (model.new_class_id == "1001")
            {
                new2.Checked = true;
                new_class.Text = "1";
            }
            else if (model.new_class_id == "1002")
            {
                new2.Checked = true;
                new_class.Text = "2";
            }
            else if (model.new_class_id == "1003")
            {
                new2.Checked = true;
                new_class.Text = "3";
            }
            else if (model.new_class_id == "1004")
            {
                new2.Checked = true;
                new_class.Text = "4";
            }
            else if (model.new_class_id == "1005")
            {
                new2.Checked = true;
                new_class.Text = "5";
            }
            else if (model.new_class_id == "1006")
            {
                new2.Checked = true;
                new_class.Text = "6";
            }
            else if (model.new_class_id == "1007")
            {
                new2.Checked = true;
                new_class.Text = "7";
            }


            if (model.administration_rank_id == "1001")
            {
                rank1.Checked = true;
            }
            else if (model.administration_rank_id == "1002")
            {
                rank2.Checked = true;
            }
            else if (model.administration_rank_id == "1003")
            {
                rank3.Checked = true;
            }
            else if (model.administration_rank_id == "1004")
            {
                rank4.Checked = true;
            }
            else if (model.administration_rank_id == "1005")
            {
                rank5.Checked = true;
            }
            else if (model.administration_rank_id == "1006")
            {
                rank6.Checked = true;
            }
            else if (model.administration_rank_id == "1007")
            {
                rank7.Checked = true;
            }
            else if (model.administration_rank_id == "1008")
            {
                rank8.Checked = true;
            }
            else if (model.administration_rank_id == "1009")
            {
                rank9.Checked = true;
            }
            else if (model.administration_rank_id == "1010")
            {
                rank10.Checked = true;
            }
            else if (model.administration_rank_id == "1011")
            {
                rank11.Checked = true;
            }
            else if (model.administration_rank_id == "1012")
            {
                rank12.Checked = true;
            }

            if (model.military_rank_id == "1001")
            {
                military1.Checked = true;
            }
            else if (model.military_rank_id == "1002")
            {
                military2.Checked = true;
            }
            else if (model.military_rank_id == "1003")
            {
                military3.Checked = true;
            }
            else if (model.military_rank_id == "1004")
            {
                military4.Checked = true;
            }
            else if (model.military_rank_id == "1005")
            {
                military5.Checked = true;
            }
            else if (model.military_rank_id == "1006")
            {
                military6.Checked = true;
            }
            else if (model.military_rank_id == "1007")
            {
                military7.Checked = true;
            }
            else if (model.military_rank_id == "1008")
            {
                military8.Checked = true;
            }
            else if (model.military_rank_id == "1009")
            {
                military9.Checked = true;
            }
            else if (model.military_rank_id == "1010")
            {
                military10.Checked = true;
            }
            else if (model.military_rank_id == "1011")
            {
                military11.Checked = true;
            }
            else if (model.military_rank_id == "1012")
            {
                military12.Checked = true;
            }
            else if (model.military_rank_id == "1013")
            {
                military13.Checked = true;
            }
            else if (model.military_rank_id == "1014")
            {
                military14.Checked = true;
            }


            if (model.police_rank_id == "1001")
            {
                police1.Checked = true;
            }
            else if (model.police_rank_id == "1002")
            {
                police2.Checked = true;
            }
            else if (model.police_rank_id == "1003")
            {
                police3.Checked = true;
            }
            else if (model.police_rank_id == "1004")
            {
                police4.Checked = true;
            }
            else if (model.police_rank_id == "1005")
            {
                police5.Checked = true;
            }
            else if (model.police_rank_id == "1006")
            {
                police6.Checked = true;
            }
            else if (model.police_rank_id == "1007")
            {
                police7.Checked = true;
            }
            else if (model.police_rank_id == "1008")
            {
                police8.Checked = true;
            }
            else if (model.police_rank_id == "1009")
            {
                police9.Checked = true;
            }
            else if (model.police_rank_id == "1010")
            {
                police10.Checked = true;
            }
            else if (model.police_rank_id == "1011")
            {
                police11.Checked = true;
            }
            else if (model.police_rank_id == "1012")
            {
                police12.Checked = true;
            }
            else if (model.police_rank_id == "1013")
            {
                police13.Checked = true;
            }

            if (model.children_info == 1)
            {

                children_info.SelectedValue = "1";
            }
            else if (model.children_info == 2)
            {
                children_info.SelectedValue = "2";
            }
            else if (model.children_info == 3)
            {
                children_info.SelectedValue = "3";
            }

            if (model.only_child_award == 1)
            {
                only_child_award.SelectedValue = "1";
            }
            else if (model.only_child_award == 2)
            {
                only_child_award.SelectedValue = "2";
            }


            if (model.passport_info_id == "1001")
            {
                passport1.Checked = true;
            }
            else if (model.passport_info_id == "1002")
            {
                passport2.Checked = true;
            }
            else if (model.passport_info_id == "1003")
            {
                passport3.Checked = true;
            }
            else if (model.passport_info_id == "1004")
            {
                passport4.Checked = true;
            }
            else if (model.passport_info_id == "1005")
            {
                passport5.Checked = true;
            }
            else if (model.passport_info_id == "1006")
            {
                passport6.Checked = true;
            }

            if(model.entitled_group_id != null)
            {
                if (model.entitled_group_id.Equals("1001"))
                {
                    Entitled1.Checked = true;
                }
                else if (model.entitled_group_id.Equals("1002"))
                {
                    Entitled2.Checked = true;
                }
                else if (model.entitled_group_id.Equals("1003"))
                {
                    Entitled3.Checked = true;
                }
                else if (model.entitled_group_id.Equals("1004"))
                {
                    Entitled4.Checked = true;
                }
                else if (model.entitled_group_id.Equals("1005"))
                {
                    Entitled5.Checked = true;
                }
                else if (model.entitled_group_id.Equals("1006"))
                {
                    Entitled6.Checked = true;
                }
                else if (model.entitled_group_id.Equals("1007"))
                {
                    Entitled7.Checked = true;
                }
                else if (model.entitled_group_id.Equals("1008"))
                {
                    Entitled8.Checked = true;
                }
                else if (model.entitled_group_id.Equals("1009"))
                {
                    Entitled9.Checked = true;
                }
                else if (model.entitled_group_id.Equals("1010"))
                {
                    Entitled10.Checked = true;
                }
                else if (model.entitled_group_id.Equals("1011"))
                {
                    Entitled11.Checked = true;
                }
            }
    

            if (model.income_source_id == "1001")
            {
                i1001.Checked = true;
            }
            else if (model.income_source_id == "1002")
            {
                i1002.Checked = true;
            }
            else if (model.income_source_id == "1003")
            {
                i1003.Checked = true;
            }
            else if (model.income_source_id == "1004")
            {
                i1004.Checked = true;
            }
            else if (model.income_source_id == "1005")
            {
                i1005.Checked = true;
            }
            else if (model.income_source_id == "1006")
            {
                i1006.Checked = true;
            }
            else if (model.income_source_id == "1007")
            {
                i1007.Checked = true;
            }

            nativePlace.Text = model.native_place == null ? "" : model.native_place;
            livePlace.Text = model.live_place == null ? "" : model.live_place;
            shool.Text = model.graduate_school == null ? "" : model.graduate_school;
            if (model.graduate_time != null)
            {
                bytime.Text = model.graduate_time.GetValueOrDefault().ToString("yyyy-M-d");
            }


            if (model.education_info_id == "1001")
            {
                e1.Checked = true;
            }
            else if (model.education_info_id == "1002")
            {
                e2.Checked = true;
            }
            else if (model.education_info_id == "1003")
            {
                e3.Checked = true;
            }
            else if (model.education_info_id == "1004")
            {
                e4.Checked = true;
            }
            else if (model.education_info_id == "1005")
            {
                e5.Checked = true;
            }
            else if (model.education_info_id == "1006")
            {
                e6.Checked = true;
            }
            else if (model.education_info_id == "1007")
            {
                e7.Checked = true;
            }
            else if (model.education_info_id == "1008")
            {
                e8.Checked = true;
            }
            else if (model.education_info_id == "1009")
            {
                e9.Checked = true;
            }
            else if (model.education_info_id == "1010")
            {
                e10.Checked = true;
            }
            else if (model.education_info_id == "1011")
            {
                e11.Checked = true;
            }
            degree_info.Text = model.degree_info.ToString();
            if (model.join_party_time != null)
            {
                joinPartyTime.Text = model.join_party_time.GetValueOrDefault().ToString("yyyy-M-d");
            }
            peoname.Text = model.party_membership;
            aftergroup.Text = model.first_branch;
            nowjob.Text = model.party_job.ToString();
            nowgroup.Text = model.now_organiz;

            if (model.group_type_id == "1001")
            {
                type1.Checked = true;
            }
            else if (model.group_type_id == "1002")
            {
                type2.Checked = true;
            }
            else if (model.group_type_id == "1003")
            {
                type3.Checked = true;
            }
            else if (model.group_type_id == "1004")
            {
                type4.Checked = true;
            }
            else if (model.group_type_id == "1005")
            {
                type5.Checked = true;
            }
            else if (model.group_type_id == "1006")
            {
                type6.Checked = true;
            }
            else if (model.group_type_id == "1007")
            {
                type7.Checked = true;
            }
            else if (model.group_type_id == "1008")
            {
                type8.Checked = true;
            }
            else if (model.group_type_id == "1009")
            {
                type9.Checked = true;
            }

            //党员进社区情况
            if (model.community_info == "未参加")
            {
                community1.Checked = true;
            }
            else
            {
                community2.Checked = true;
                txtContent.InnerText = model.community_info == null ? "" : model.community_info;
            }

            //参加组织生活情况
            if (model.group_live == "未曾参加")
            {
                live1.Checked = true;
            }
            else
            {
                live2.Checked = true;
                txtlive.InnerText = model.group_live;
            }

            //奖惩信息
            Model.u_reward_punishment rew = bll.getrewardpunishment(_id);
            if (rew != null)
            {
                title.Text = rew.title == null ? "" : rew.title;
                txtreason.Text = rew.reason == null ? "" : rew.reason;
                txtauthority.Text = rew.approval_authority == null ? "" : rew.approval_authority;
                txtlevel.Text = rew.office_level == null ? "" : rew.office_level;
            }

            //现单位信息
            Model.u_company_type nowcom = bll.getNowCompany(_id);
            if(nowcom != null)
            {
                nowCompanyName.Text = nowcom.name == null ? "" : nowcom.name;
                nowpeoplecount.Text = nowcom.employee_count.ToString() == null ? "" : nowcom.employee_count.ToString();

                //组织关系所在单位
                if (nowcom.relation_com == "现工作单位")
                {
                    com1.Checked = true;
                }
                else if (nowcom.relation_com == "其他")
                {
                    com2.Checked = true;
                }

                //现工作单位类型
                if (nowcom.com_type_id == "1001")
                {
                    c1010.Checked = true;
                }
                else if (nowcom.com_type_id == "1002")
                {
                    c1011.Checked = true;
                }
                else if (nowcom.com_type_id == "1003")
                {
                    c1012.Checked = true;
                }
                else if (nowcom.com_type_id == "1004")
                {
                    c1013.Checked = true;
                }
                else if (nowcom.com_type_id == "1005")
                {
                    c1014.Checked = true;
                }
                else if (nowcom.com_type_id == "1006")
                {
                    c1015.Checked = true;
                }
                else if (nowcom.com_type_id == "1007")
                {
                    c1016.Checked = true;
                }
                else if (nowcom.com_type_id == "1008")
                {
                    c1017.Checked = true;
                }
                else if (nowcom.com_type_id == "1009")
                {
                    c1018.Checked = true;
                }
                else if (nowcom.com_type_id == "1010")
                {
                    c1019.Checked = true;
                }
                else if (nowcom.com_type_id == "1011")
                {
                    c1020.Checked = true;
                }
                else if (nowcom.com_type_id == "1012")
                {
                    c1021.Checked = true;
                }
                else if (nowcom.com_type_id == "1013")
                {
                    c1022.Checked = true;
                }
                else if (nowcom.com_type_id == "1014")
                {
                    c1023.Checked = true;
                }
                else if (nowcom.com_type_id == "1015")
                {
                    c1024.Checked = true;
                }
                else if (nowcom.com_type_id == "1016")
                {
                    c1025.Checked = true;
                }
                else if (nowcom.com_type_id == "1017")
                {
                    c1026.Checked = true;
                }
                else if (nowcom.com_type_id == "1018")
                {
                    c1027.Checked = true;
                }
                else if (nowcom.com_type_id == "1019")
                {
                    c1028.Checked = true;
                }
                else if (nowcom.com_type_id == "1020")
                {
                    c1029.Checked = true;
                }
                else if (nowcom.com_type_id == "1021")
                {
                    c1030.Checked = true;
                }
                else if (nowcom.com_type_id == "1022")
                {
                    c1031.Checked = true;
                }
                else if (nowcom.com_type_id == "1023")
                {
                    c1032.Checked = true;
                }

                //现工作岗位类型
                if (nowcom.post_type_id == "1001")
                {
                    c2010.Checked = true;
                }
                else if (nowcom.post_type_id == "1002")
                {
                    c2011.Checked = true;
                }
                else if (nowcom.post_type_id == "1003")
                {
                    c2012.Checked = true;
                }
                else if (nowcom.post_type_id == "1004")
                {
                    c2013.Checked = true;
                }
                else if (nowcom.post_type_id == "1005")
                {
                    c2014.Checked = true;
                }
                else if (nowcom.post_type_id == "1006")
                {
                    c2015.Checked = true;
                }
                else if (nowcom.post_type_id == "1007")
                {
                    c2016.Checked = true;
                }
                else if (nowcom.post_type_id == "1008")
                {
                    c2017.Checked = true;
                }
                else if (nowcom.post_type_id == "1009")
                {
                    c2018.Checked = true;
                }
                else if (nowcom.post_type_id == "1010")
                {
                    c2019.Checked = true;
                }
                else if (nowcom.post_type_id == "1011")
                {
                    c2020.Checked = true;
                }
                else if (nowcom.post_type_id == "1012")
                {
                    c2021.Checked = true;
                }
                else if (nowcom.post_type_id == "1013")
                {
                    c2022.Checked = true;
                }
                else if (nowcom.post_type_id == "1014")
                {
                    c2023.Checked = true;
                }
                else if (nowcom.post_type_id == "1015")
                {
                    c2024.Checked = true;
                }
                else if (nowcom.post_type_id == "1016")
                {
                    c2025.Checked = true;
                }
                else if (nowcom.post_type_id == "1017")
                {
                    c2026.Checked = true;
                }
                else if (nowcom.post_type_id == "1018")
                {
                    c2027.Checked = true;
                }
                else if (nowcom.post_type_id == "1019")
                {
                    c2028.Checked = true;
                }
                else if (nowcom.post_type_id == "1020")
                {
                    c2029.Checked = true;
                }
                else if (nowcom.post_type_id == "1021")
                {
                    c2030.Checked = true;
                }
                else if (nowcom.post_type_id == "1022")
                {
                    c2031.Checked = true;
                }
                else if (nowcom.post_type_id == "1023")
                {
                    c2032.Checked = true;
                }
                else if (nowcom.post_type_id == "1024")
                {
                    c2033.Checked = true;
                }
                else if (nowcom.post_type_id == "1025")
                {
                    c2034.Checked = true;
                }
                else if (nowcom.post_type_id == "1026")
                {
                    c2035.Checked = true;
                }
                else if (nowcom.post_type_id == "1027")
                {
                    c2036.Checked = true;
                }
            }
     

            //原单位信息
            Model.u_company_type formercom = bll.getFormerCompany(_id);
            if(formercom != null)
            {
                originalComName.Text = formercom.name == null ? "" : formercom.name;
                originalpeocount.Text = formercom.employee_count.ToString() == null ? "" : formercom.employee_count.ToString();

                //组织关系所在单位
                if (formercom.relation_com == "现工作单位")
                {
                    originalcom1.Checked = true;
                }
                else if (formercom.relation_com == "其他")
                {
                    originalcom2.Checked = true;
                }

                //原工作单位类型
                if (formercom.com_type_id == "1001")
                {
                    c3010.Checked = true;
                }
                else if (formercom.com_type_id == "1002")
                {
                    c3011.Checked = true;
                }
                else if (formercom.com_type_id == "1003")
                {
                    c3012.Checked = true;
                }
                else if (formercom.com_type_id == "1004")
                {
                    c3013.Checked = true;
                }
                else if (formercom.com_type_id == "1005")
                {
                    c3014.Checked = true;
                }
                else if (formercom.com_type_id == "1006")
                {
                    c3015.Checked = true;
                }
                else if (formercom.com_type_id == "1007")
                {
                    c3016.Checked = true;
                }
                else if (formercom.com_type_id == "1008")
                {
                    c3017.Checked = true;
                }
                else if (formercom.com_type_id == "1009")
                {
                    c3018.Checked = true;
                }
                else if (formercom.com_type_id == "1010")
                {
                    c3019.Checked = true;
                }
                else if (formercom.com_type_id == "1011")
                {
                    c3020.Checked = true;
                }
                else if (formercom.com_type_id == "1012")
                {
                    c3021.Checked = true;
                }
                else if (formercom.com_type_id == "1013")
                {
                    c3022.Checked = true;
                }
                else if (formercom.com_type_id == "1014")
                {
                    c3023.Checked = true;
                }
                else if (formercom.com_type_id == "1015")
                {
                    c3024.Checked = true;
                }
                else if (formercom.com_type_id == "1016")
                {
                    c3025.Checked = true;
                }
                else if (formercom.com_type_id == "1017")
                {
                    c3026.Checked = true;
                }
                else if (formercom.com_type_id == "1018")
                {
                    c3027.Checked = true;
                }
                else if (formercom.com_type_id == "1019")
                {
                    c3028.Checked = true;
                }
                else if (formercom.com_type_id == "1020")
                {
                    c3029.Checked = true;
                }
                else if (formercom.com_type_id == "1021")
                {
                    c3030.Checked = true;
                }
                else if (formercom.com_type_id == "1022")
                {
                    c3031.Checked = true;
                }
                else if (formercom.com_type_id == "1023")
                {
                    c3032.Checked = true;
                }

                //原工作岗位类型
                if (formercom.post_type_id == "1001")
                {
                    c3041.Checked = true;
                }
                else if (formercom.post_type_id == "1002")
                {
                    c3042.Checked = true;
                }
                else if (formercom.post_type_id == "1003")
                {
                    c3043.Checked = true;
                }
                else if (formercom.post_type_id == "1004")
                {
                    c3044.Checked = true;
                }
                else if (formercom.post_type_id == "1005")
                {
                    c3045.Checked = true;
                }
                else if (formercom.post_type_id == "1006")
                {
                    c3046.Checked = true;
                }
                else if (formercom.post_type_id == "1007")
                {
                    c3047.Checked = true;
                }
                else if (formercom.post_type_id == "1008")
                {
                    c3048.Checked = true;
                }
                else if (formercom.post_type_id == "1009")
                {
                    c3049.Checked = true;
                }
                else if (formercom.post_type_id == "1010")
                {
                    c3050.Checked = true;
                }
                else if (formercom.post_type_id == "1011")
                {
                    c3051.Checked = true;
                }
                else if (formercom.post_type_id == "1012")
                {
                    c3052.Checked = true;
                }
                else if (formercom.post_type_id == "1013")
                {
                    c3053.Checked = true;
                }
                else if (formercom.post_type_id == "1014")
                {
                    c3054.Checked = true;
                }
                else if (formercom.post_type_id == "1015")
                {
                    c3055.Checked = true;
                }
                else if (formercom.post_type_id == "1016")
                {
                    c3056.Checked = true;
                }
                else if (formercom.post_type_id == "1017")
                {
                    c3057.Checked = true;
                }
                else if (formercom.post_type_id == "1018")
                {
                    c3058.Checked = true;
                }
                else if (formercom.post_type_id == "1019")
                {
                    c3059.Checked = true;
                }
                else if (formercom.post_type_id == "1020")
                {
                    c3060.Checked = true;
                }
                else if (formercom.post_type_id == "1021")
                {
                    c3061.Checked = true;
                }
                else if (formercom.post_type_id == "1022")
                {
                    c3062.Checked = true;
                }
                else if (formercom.post_type_id == "1023")
                {
                    c3063.Checked = true;
                }
                else if (formercom.post_type_id == "1024")
                {
                    c3064.Checked = true;
                }
                else if (formercom.post_type_id == "1025")
                {
                    c3065.Checked = true;
                }
                else if (formercom.post_type_id == "1026")
                {
                    c3066.Checked = true;
                }
                else if (formercom.post_type_id == "1027")
                {
                    c3067.Checked = true;
                }
            }
      

            if (model.is_badly_off == 0)
            {
                badly_of.SelectedValue = "0";
            }
            else if (model.is_badly_off == 1)
            {
                badly_of.SelectedValue = "1";
            }

            if (model.is_organiz_identity == 0)
            {
                is_identity.SelectedValue = "0";
            }
            else if (model.is_organiz_identity == 1)
            {
                is_identity.SelectedValue = "1";
            }


            if (model.financial_situation_id == "1001")
            {
                financial1.Checked = true;
            }
            else if (model.financial_situation_id == "1002")
            {
                financial2.Checked = true;
            }
            else if (model.financial_situation_id == "1003")
            {
                financial3.Checked = true;
            }
            else if (model.financial_situation_id == "1004")
            {
                financial4.Checked = true;
            }

            //身体健康情况
            if (model.healthy_condition_id == "1001")
            {
                healthy1.Checked = true;
            }
            else if (model.healthy_condition_id == "1002")
            {
                healthy2.Checked = true;
            }
            else if (model.healthy_condition_id == "1003")
            {
                healthy3.Checked = true;
            }
            else if (model.healthy_condition_id == "1004")
            {
                healthy4.Checked = true;
            }
            else if (model.healthy_condition_id == "1005")
            {
                healthy5.Checked = true;
            }
            else if (model.healthy_condition_id == "1006")
            {
                healthy6.Checked = true;
            }
            else if (model.healthy_condition_id == "1007")
            {
                healthy7.Checked = true;
            }
            else if (model.healthy_condition_id == "1008")
            {
                healthy8.Checked = true;
            }

            //生活困难原因
            if (model.badly_off_reason_id == "1001")
            {
                reason1.Checked = true;
            }
            else if (model.badly_off_reason_id == "1002")
            {
                reason2.Checked = true;
            }
            else if (model.badly_off_reason_id == "1003")
            {
                reason3.Checked = true;
            }
            else if (model.badly_off_reason_id == "1004")
            {
                reason4.Checked = true;
            }
            else if (model.badly_off_reason_id == "1005")
            {
                reason5.Checked = true;
            }
            else if (model.badly_off_reason_id == "1006")
            {
                reason6.Checked = true;
            }
            else if (model.badly_off_reason_id == "1007")
            {
                reason7.Checked = true;
            }
            else if (model.badly_off_reason_id == "1008")
            {
                reason8.Checked = true;
            }

            info5.Text = model.badly_off_describe == null ? "" : model.badly_off_describe;

            //享受帮扶形式
            if (model.enjoy_help_id == null)
            {
                help1.Checked = true;
            }
            else if (model.enjoy_help_id != null)
            {
                help2.Checked = true;
                Model.u_enjoy_help enjoy = bll.getEnjoyHelp(_id);

                if(enjoy != null)
                {
                    TextBox3.Text = enjoy.start_time.GetValueOrDefault().ToString("yyyy-M-d") == null ? "" : enjoy.start_time.GetValueOrDefault().ToString("yyyy-M-d");

                    TextBox4.Text = enjoy.end_time.GetValueOrDefault().ToString("yyyy-M-d");

                    TextBox5.Text = enjoy.help_way_id == null ? "" : enjoy.help_way_id;
           
                    TextBox6.Text = enjoy.remark == null ? "" : enjoy.remark;
                }
            }

            Model.u_float_commie commie = bll.getFloatCommie(_id);
            if(commie != null)
            {
                TextBox7.Text = commie.flow_type == null ? "" : commie.flow_type;
                TextBox8.Text = commie.linkman == null ? "" : commie.linkman;
                TextBox9.Text = commie.flow_reason == null ? "" : commie.flow_reason;
                TextBox10.Text = commie.contact == null ? "" : commie.contact;
                TextBox11.Text = commie.id_number == null ? "" : commie.id_number;
                TextBox12.Text = commie.group_linkman == null ? "" : commie.group_linkman;
                TextBox13.Text = commie.discharge_place == null ? "" : commie.discharge_place;
                TextBox14.Text = commie.group_contact == null ? "" : commie.group_contact;
            }
           
            //查找最近登录信息
            Model.user_login_log logModel = new BLL.user_login_log().GetLastModel(model.user_name);
            if (logModel != null)
            {
                //lblLastTime.Text = logModel.login_time.ToString();
                //lblLastIP.Text = logModel.login_ip;
            }
        }

        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = false;
            Model.users model = new Model.users();
            Model.user_groups group = new Model.user_groups();
            BLL.users bll = new BLL.users();

            model.group_id = ddlGroupId.SelectedValue;
            // 2017-08-02 因业务需求用户状态不可修改。
            //model.status = int.Parse(rblStatus.SelectedValue);
            // 2017-08-02 因业务需求新建人员是用户状态都为  1-->待审核
            model.status = 1;
            //检测用户名是否重复
            //if (bll.Exists(txtUserName.Text.Trim()))
            //{
            //    return false;
            //}
            model.user_name = Utils.DropHTML(txtUserName.Text.Trim());
            //获得6位的salt加密字符串
            model.salt = Utils.GetCheckCode(6);
            //以随机生成的6位字符串做为密钥加密
            model.password = DESEncrypt.Encrypt(txtPassword.Text.Trim(), model.salt);
            model.email = Utils.DropHTML(txtEmail.Text);
            model.nick_name = Utils.DropHTML(txtNickName.Text);
            model.avatar = Utils.DropHTML(txtImgUrl.Text);


            model.sex = rblSex.SelectedValue;
            DateTime _birthday;
            if (DateTime.TryParse(txtBirthday.Text.Trim(), out _birthday))
            {
                model.birthday = _birthday;
            }
            model.telphone = Utils.DropHTML(txtTelphone.Text.Trim());
            model.mobile = Utils.DropHTML(txtMobile.Text.Trim());
            model.qq = Utils.DropHTML(txtQQ.Text);
            model.wechat = Utils.DropHTML(txtwechat.Text.Trim());
            model.msn = Utils.DropHTML(txtMsn.Text);
            model.address = Utils.DropHTML(txtAddress.Text.Trim());
            //model.amount = decimal.Parse(txtAmount.Text.Trim());
            //model.point = int.Parse(txtPoint.Text.Trim());
            //model.exp = int.Parse(txtExp.Text.Trim());
            model.reg_time = DateTime.Now;
            model.reg_ip = DTRequest.GetIP();
            model.id_card = identity.Text.Trim();//身份证号
            model.nation = txtnationId.Text.Trim(); //民族		
            model.marital_status = int.Parse(maritalStatus.Text.Trim());    //婚姻状态	
            //新阶层人员
            if (new1.Checked)
            {
                model.party_job = "否";
            }
            if (new2.Checked)
            {
                if (new_class.Text == "1")
                {
                    model.new_class_id = "1001";
                }
                if (new_class.Text == "2")
                {
                    model.new_class_id = "1002";
                }
                if (new_class.Text == "3")
                {
                    model.new_class_id = "1003";
                }
                if (new_class.Text == "4")
                {
                    model.new_class_id = "1004";
                }
                if (new_class.Text == "5")
                {
                    model.new_class_id = "1005";
                }
                if (new_class.Text == "6")
                {
                    model.new_class_id = "1006";
                }
                if (new_class.Text == "7")
                {
                    model.new_class_id = "1007";
                }
            }

            if (children_info.SelectedValue == "1")
            {
                model.children_info = 1;
            }
            else if (children_info.SelectedValue == "2")
            {
                model.children_info = 2;
            }
            else if (children_info.SelectedValue == "3")
            {
                model.children_info = 3;
            }

            if (only_child_award.SelectedValue == "1")
            {
                model.only_child_award = 1;
            }
            else if (only_child_award.SelectedValue == "2")
            {
                model.only_child_award = 2;
            }

            //行政级别
            if (rank1.Checked)
            {
                model.administration_rank_id = "1001";
            }
            if (rank2.Checked)
            {
                model.administration_rank_id = "1002";
            }
            if (rank3.Checked)
            {
                model.administration_rank_id = "1003";
            }
            if (rank4.Checked)
            {
                model.administration_rank_id = "1004";
            }
            if (rank5.Checked)
            {
                model.administration_rank_id = "1005";
            }
            if (rank6.Checked)
            {
                model.administration_rank_id = "1006";
            }
            if (rank7.Checked)
            {
                model.administration_rank_id = "1007";
            }
            if (rank8.Checked)
            {
                model.administration_rank_id = "1008";
            }
            if (rank9.Checked)
            {
                model.administration_rank_id = "1009";
            }
            if (rank10.Checked)
            {
                model.administration_rank_id = "1010";
            }
            if (rank11.Checked)
            {
                model.administration_rank_id = "1011";
            }
            if (rank12.Checked)
            {
                model.administration_rank_id = "1012";
            }

            //军职级别
            if (military1.Checked)
            {
                model.military_rank_id = "1001";
            }
            if (military2.Checked)
            {
                model.military_rank_id = "1002";
            }
            if (military3.Checked)
            {
                model.military_rank_id = "1003";
            }
            if (military4.Checked)
            {
                model.military_rank_id = "1004";
            }
            if (military5.Checked)
            {
                model.military_rank_id = "1005";
            }
            if (military6.Checked)
            {
                model.military_rank_id = "1006";
            }
            if (military7.Checked)
            {
                model.military_rank_id = "1007";
            }
            if (military8.Checked)
            {
                model.military_rank_id = "1008";
            }
            if (military9.Checked)
            {
                model.military_rank_id = "1009";
            }
            if (military10.Checked)
            {
                model.military_rank_id = "1010";
            }
            if (military11.Checked)
            {
                model.military_rank_id = "1011";
            }
            if (military12.Checked)
            {
                model.military_rank_id = "1012";
            }
            if (military13.Checked)
            {
                model.military_rank_id = "1013";
            }
            if (military14.Checked)
            {
                model.military_rank_id = "1014";
            }


            //警衔级别
            if (police1.Checked)
            {
                model.police_rank_id = "1001";
            }
            if (police2.Checked)
            {
                model.police_rank_id = "1002";
            }
            if (police3.Checked)
            {
                model.police_rank_id = "1003";
            }
            if (police4.Checked)
            {
                model.police_rank_id = "1004";
            }
            if (police5.Checked)
            {
                model.police_rank_id = "1005";
            }
            if (police6.Checked)
            {
                model.police_rank_id = "1006";
            }
            if (police7.Checked)
            {
                model.police_rank_id = "1007";
            }
            if (police8.Checked)
            {
                model.police_rank_id = "1008";
            }
            if (police9.Checked)
            {
                model.police_rank_id = "1009";
            }
            if (police10.Checked)
            {
                model.police_rank_id = "1010";
            }
            if (police11.Checked)
            {
                model.police_rank_id = "1011";
            }
            if (police12.Checked)
            {
                model.police_rank_id = "1012";
            }
            if (police13.Checked)
            {
                model.police_rank_id = "1013";
            }


            //优抚对象
            if (Entitled1.Checked)
            {
                model.entitled_group_id = "1001";
            }
            if (Entitled2.Checked)
            {
                model.entitled_group_id = "1002";
            }
            if (Entitled3.Checked)
            {
                model.entitled_group_id = "1003";
            }
            if (Entitled4.Checked)
            {
                model.entitled_group_id = "1004";
            }
            if (Entitled5.Checked)
            {
                model.entitled_group_id = "1005";
            }
            if (Entitled6.Checked)
            {
                model.entitled_group_id = "1006";
            }
            if (Entitled7.Checked)
            {
                model.entitled_group_id = "1007";
            }
            if (Entitled8.Checked)
            {
                model.entitled_group_id = "1008";
            }
            if (Entitled9.Checked)
            {
                model.entitled_group_id = "1009";
            }
            if (Entitled10.Checked)
            {
                model.entitled_group_id = "1010";
            }
            if (Entitled11.Checked)
            {
                model.entitled_group_id = "1011";
            }

            if (passport1.Checked)
            {
                model.passport_info_id = "1001";
            }
            else if (passport2.Checked)
            {
                model.passport_info_id = "1002";
            }
            else if (passport3.Checked)
            {
                model.passport_info_id = "1003";
            }
            else if (passport4.Checked)
            {
                model.passport_info_id = "1004";
            }
            else if (passport5.Checked == true)
            {
                model.passport_info_id = "1005";
            }
            else if (passport6.Checked == true)
            {
                model.passport_info_id = "1006";
            }
            //收入来源
            if (i1001.Checked)
            {
                model.income_source_id = "1001";
            }
            if (i1002.Checked)
            {
                model.income_source_id = "1002";
            }
            if (i1003.Checked)
            {
                model.income_source_id = "1003";
            }
            if (i1004.Checked)
            {
                model.income_source_id = "1004";
            }
            if (i1005.Checked)
            {
                model.income_source_id = "1005";
            }
            if (i1006.Checked)
            {
                model.income_source_id = "1006";
            }
            if (i1007.Checked)
            {
                model.income_source_id = "1007";
            }

            model.native_place = nativePlace.Text.Trim();

            model.live_place = livePlace.Text.Trim();

            model.graduate_school = shool.Text.Trim();

            DateTime _graduate_time;
            if (DateTime.TryParse(bytime.Text.Trim(), out _graduate_time))
            {
                model.graduate_time = _graduate_time;
            }

            //学历情况
            if (e1.Checked)
            {
                model.education_info_id = "1001";
            }
            if (e2.Checked)
            {
                model.education_info_id = "1002";
            }
            if (e3.Checked)
            {
                model.education_info_id = "1003";
            }
            if (e4.Checked)
            {
                model.education_info_id = "1004";
            }
            if (e5.Checked)
            {
                model.education_info_id = "1005";
            }
            if (e6.Checked)
            {
                model.education_info_id = "1006";
            }
            if (e7.Checked)
            {
                model.education_info_id = "1007";
            }
            if (e8.Checked)
            {
                model.education_info_id = "1008";
            }
            if (e9.Checked)
            {
                model.education_info_id = "1009";
            }
            if (e10.Checked)
            {
                model.education_info_id = "1010";
            }
            if (e11.Checked)
            {
                model.education_info_id = "1011";
            }

            model.degree_info = int.Parse(degree_info.Text.Trim());

            //入党时间
            DateTime _joinPartyTime;
            if (DateTime.TryParse(joinPartyTime.Text.Trim(), out _joinPartyTime))
            {
                model.join_party_time = _joinPartyTime;
            }

            model.party_membership = peoname.Text.Trim();

            model.first_branch = aftergroup.Text.Trim();

            model.party_job = nowjob.Text.Trim();

            model.now_organiz = nowgroup.Text.Trim();

            //进入现支部类型
            if (type1.Checked)
            {
                model.group_type_id = "1001";
            }
            if (type2.Checked)
            {
                model.group_type_id = "1002";
            }
            if (type3.Checked)
            {
                model.group_type_id = "1003";
            }
            if (type4.Checked)
            {
                model.group_type_id = "1004";
            }
            if (type5.Checked)
            {
                model.group_type_id = "1005";
            }
            if (type6.Checked)
            {
                model.group_type_id = "1006";
            }
            if (type7.Checked)
            {
                model.group_type_id = "1007";
            }
            if (type8.Checked)
            {
                model.group_type_id = "1008";
            }
            if (type9.Checked)
            {
                model.group_type_id = "1009";
            }

            //党员进社区情况
            if (community1.Checked)
            {
                model.community_info = "未参加";
            }
            if (community2.Checked)
            {
                model.community_info = txtContent.InnerText.Trim();
            }

            //参加组织生活情况
            if (live1.Checked)
            {
                model.group_live = "未曾参加";
            }
            if (live2.Checked)
            {
                model.group_live = txtlive.InnerText.Trim();
            }

            //党员奖惩信息
            u_reward_punishment reward = new u_reward_punishment();
            string puid = Guid.NewGuid().ToString();
            reward.id = puid;
            model.reward_punishment_id = puid;
            reward.title = title.Text;
            reward.reason = txtreason.Text;
            reward.approval_authority = txtauthority.Text;
            reward.office_level = txtlevel.Text;
            bool rew = bll.addreward(reward);

            //现工作单位信息
            u_company_type company = new u_company_type();
            string nowid = Guid.NewGuid().ToString();
            company.id = nowid;
            model.now_company_id = nowid;
            company.name = nowCompanyName.Text.Trim();
            company.employee_count = nowpeoplecount.Text.Trim() == "" ? 0 : int.Parse(nowpeoplecount.Text.Trim());
            if (com1.Checked)
            {
                company.relation_com = "现工作单位";
            }
            else if (com2.Checked)
            {
                company.relation_com = "其他";
            }
            //现工作单位类型
            if (c1010.Checked)
            {
                company.com_type_id = "1001";
            }
            else if (c1011.Checked)
            {
                company.com_type_id = "1002";
            }
            else if (c1012.Checked)
            {
                company.com_type_id = "1003";
            }
            else if (c1013.Checked)
            {
                company.com_type_id = "1004";
            }
            else if (c1014.Checked)
            {
                company.com_type_id = "1005";
            }
            else if (c1015.Checked)
            {
                company.com_type_id = "1006";
            }
            else if (c1016.Checked)
            {
                company.com_type_id = "1007";
            }
            else if (c1017.Checked)
            {
                company.com_type_id = "1008";
            }
            else if (c1018.Checked)
            {
                company.com_type_id = "1009";
            }
            else if (c1019.Checked)
            {
                company.com_type_id = "1010";
            }
            else if (c1020.Checked)
            {
                company.com_type_id = "1011";
            }
            else if (c1021.Checked)
            {
                company.com_type_id = "1012";
            }
            else if (c1022.Checked)
            {
                company.com_type_id = "1013";
            }
            else if (c1023.Checked)
            {
                company.com_type_id = "1014";
            }
            else if (c1024.Checked)
            {
                company.com_type_id = "1015";
            }
            else if (c1025.Checked)
            {
                company.com_type_id = "1016";
            }
            else if (c1026.Checked)
            {
                company.com_type_id = "1017";
            }
            else if (c1027.Checked)
            {
                company.com_type_id = "1018";
            }
            else if (c1028.Checked)
            {
                company.com_type_id = "1019";
            }
            else if (c1029.Checked)
            {
                company.com_type_id = "1020";
            }
            else if (c1030.Checked)
            {
                company.com_type_id = "1021";
            }
            else if (c1031.Checked)
            {
                company.com_type_id = "1022";
            }
            else if (c1032.Checked)
            {
                company.com_type_id = "1023";
            }

            //现工作岗位类型
            if (c2010.Checked)
            {
                company.post_type_id = "1001";
            }
            else if (c2011.Checked)
            {
                company.post_type_id = "1002";
            }
            else if (c2012.Checked)
            {
                company.post_type_id = "1003";
            }
            else if (c2013.Checked)
            {
                company.post_type_id = "1004";
            }
            else if (c2014.Checked)
            {
                company.post_type_id = "1005";
            }
            else if (c2015.Checked)
            {
                company.post_type_id = "1006";
            }
            else if (c2016.Checked)
            {
                company.post_type_id = "1007";
            }
            else if (c2017.Checked)
            {
                company.post_type_id = "1008";
            }
            else if (c2018.Checked)
            {
                company.post_type_id = "1009";
            }
            else if (c2019.Checked)
            {
                company.post_type_id = "1010";
            }
            else if (c2020.Checked)
            {
                company.post_type_id = "1011";
            }
            else if (c2021.Checked)
            {
                company.post_type_id = "1012";
            }
            else if (c2022.Checked)
            {
                company.post_type_id = "1013";
            }
            else if (c2023.Checked)
            {
                company.post_type_id = "1014";
            }
            else if (c2024.Checked)
            {
                company.post_type_id = "1015";
            }
            else if (c2025.Checked)
            {
                company.post_type_id = "1016";
            }
            else if (c2026.Checked)
            {
                company.post_type_id = "1017";
            }
            else if (c2027.Checked)
            {
                company.post_type_id = "1018";
            }
            else if (c2028.Checked)
            {
                company.post_type_id = "1019";
            }
            else if (c2029.Checked)
            {
                company.post_type_id = "1020";
            }
            else if (c2030.Checked)
            {
                company.post_type_id = "1021";
            }
            else if (c2031.Checked)
            {
                company.post_type_id = "1022";
            }
            else if (c2032.Checked)
            {
                company.post_type_id = "1023";
            }
            else if (c2033.Checked)
            {
                company.post_type_id = "1024";
            }
            else if (c2034.Checked)
            {
                company.post_type_id = "1025";
            }
            else if (c2035.Checked)
            {
                company.post_type_id = "1026";
            }
            else if (c2036.Checked)
            {
                company.post_type_id = "1027";
            }
            bool com = bll.addcompany(company);

            //原工作单位信息
            u_company_type ycompany = new u_company_type();
            string originalid = Guid.NewGuid().ToString();
            ycompany.id = originalid;
            model.former_company_id = originalid;
            ycompany.name = originalComName.Text.Trim();
            ycompany.employee_count = originalpeocount.Text.Trim() == "" ? 0 : int.Parse(originalpeocount.Text.Trim());
            if (originalcom1.Checked)
            {
                ycompany.relation_com = "现工作单位";
            }
            else if (originalcom2.Checked)
            {
                ycompany.relation_com = "其他";
            }

            //原工作单位类型
            if (c3010.Checked)
            {
                ycompany.com_type_id = "1001";
            }
            else if (c3011.Checked)
            {
                ycompany.com_type_id = "1002";
            }
            else if (c3012.Checked)
            {
                ycompany.com_type_id = "1003";
            }
            else if (c3013.Checked)
            {
                ycompany.com_type_id = "1004";
            }
            else if (c3014.Checked)
            {
                ycompany.com_type_id = "1005";
            }
            else if (c3015.Checked)
            {
                ycompany.com_type_id = "1006";
            }
            else if (c3016.Checked)
            {
                ycompany.com_type_id = "1007";
            }
            else if (c3017.Checked)
            {
                ycompany.com_type_id = "1008";
            }
            else if (c3018.Checked)
            {
                ycompany.com_type_id = "1009";
            }
            else if (c3019.Checked)
            {
                ycompany.com_type_id = "1010";
            }
            else if (c3020.Checked)
            {
                ycompany.com_type_id = "1011";
            }
            else if (c3021.Checked)
            {
                ycompany.com_type_id = "1012";
            }
            else if (c3022.Checked)
            {
                ycompany.com_type_id = "1013";
            }
            else if (c3023.Checked)
            {
                ycompany.com_type_id = "1014";
            }
            else if (c3024.Checked)
            {
                ycompany.com_type_id = "1015";
            }
            else if (c3025.Checked)
            {
                ycompany.com_type_id = "1016";
            }
            else if (c3026.Checked)
            {
                ycompany.com_type_id = "1017";
            }
            else if (c3027.Checked)
            {
                ycompany.com_type_id = "1018";
            }
            else if (c3028.Checked)
            {
                ycompany.com_type_id = "1019";
            }
            else if (c3029.Checked)
            {
                ycompany.com_type_id = "1020";
            }
            else if (c3030.Checked)
            {
                ycompany.com_type_id = "1021";
            }
            else if (c3031.Checked)
            {
                ycompany.com_type_id = "1022";
            }
            else if (c3032.Checked)
            {
                ycompany.com_type_id = "1023";
            }

            //原工作岗位类型
            if (c3041.Checked)
            {
                ycompany.post_type_id = "1001";
            }
            else if (c3042.Checked)
            {
                ycompany.post_type_id = "1002";
            }
            else if (c3043.Checked)
            {
                ycompany.post_type_id = "1003";
            }
            else if (c3044.Checked)
            {
                ycompany.post_type_id = "1004";
            }
            else if (c3045.Checked)
            {
                ycompany.post_type_id = "1005";
            }
            else if (c3046.Checked)
            {
                ycompany.post_type_id = "1006";
            }
            else if (c3047.Checked)
            {
                ycompany.post_type_id = "1007";
            }
            else if (c3048.Checked)
            {
                ycompany.post_type_id = "1008";
            }
            else if (c3049.Checked)
            {
                ycompany.post_type_id = "1009";
            }
            else if (c3050.Checked)
            {
                ycompany.post_type_id = "1010";
            }
            else if (c3051.Checked)
            {
                ycompany.post_type_id = "1011";
            }
            else if (c3052.Checked)
            {
                ycompany.post_type_id = "1012";
            }
            else if (c3053.Checked)
            {
                ycompany.post_type_id = "1013";
            }
            else if (c3054.Checked)
            {
                ycompany.post_type_id = "1014";
            }
            else if (c3055.Checked)
            {
                ycompany.post_type_id = "1015";
            }
            else if (c3056.Checked)
            {
                ycompany.post_type_id = "1016";
            }
            else if (c3057.Checked)
            {
                ycompany.post_type_id = "1017";
            }
            else if (c3058.Checked)
            {
                ycompany.post_type_id = "1018";
            }
            else if (c3059.Checked)
            {
                ycompany.post_type_id = "1019";
            }
            else if (c3060.Checked)
            {
                ycompany.post_type_id = "1020";
            }
            else if (c3061.Checked)
            {
                ycompany.post_type_id = "1021";
            }
            else if (c3062.Checked)
            {
                ycompany.post_type_id = "1022";
            }
            else if (c3063.Checked)
            {
                ycompany.post_type_id = "1023";
            }
            else if (c3064.Checked)
            {
                ycompany.post_type_id = "1024";
            }
            else if (c3065.Checked)
            {
                ycompany.post_type_id = "1025";
            }
            else if (c3066.Checked)
            {
                ycompany.post_type_id = "1026";
            }
            else if (c3067.Checked)
            {
                ycompany.post_type_id = "1027";
            }

            bool comp = bll.addOrigCompany(ycompany);

            model.is_badly_off = int.Parse(badly_of.Text);

            model.is_organiz_identity = int.Parse(is_identity.Text);

            if (financial1.Checked)
            {
                model.financial_situation_id = "1001";
            }
            else if (financial2.Checked)
            {
                model.financial_situation_id = "1002";
            }
            else if (financial3.Checked)
            {
                model.financial_situation_id = "1003";
            }
            else if (financial4.Checked)
            {
                model.financial_situation_id = "1004";
            }

            if (healthy1.Checked)
            {
                model.healthy_condition_id = "1001";
            }
            else if (healthy2.Checked)
            {
                model.healthy_condition_id = "1002";
            }
            else if (healthy3.Checked)
            {
                model.healthy_condition_id = "1003";
            }
            else if (healthy4.Checked)
            {
                model.healthy_condition_id = "1004";
            }
            else if (healthy5.Checked)
            {
                model.healthy_condition_id = "1005";
            }
            else if (healthy6.Checked)
            {
                model.healthy_condition_id = "1006";
            }
            else if (healthy7.Checked)
            {
                model.healthy_condition_id = "1007";
            }
            else if (healthy8.Checked)
            {
                model.healthy_condition_id = "1008";
            }

            if (reason1.Checked)
            {
                model.badly_off_reason_id = "1001";
            }
            else if (reason2.Checked)
            {
                model.badly_off_reason_id = "1002";
            }
            else if (reason3.Checked)
            {
                model.badly_off_reason_id = "1003";
            }
            else if (reason4.Checked)
            {
                model.badly_off_reason_id = "1004";
            }
            else if (reason5.Checked)
            {
                model.badly_off_reason_id = "1005";
            }
            else if (reason6.Checked)
            {
                model.badly_off_reason_id = "1006";
            }
            else if (reason7.Checked)
            {
                model.badly_off_reason_id = "1007";
            }
            else if (reason8.Checked)
            {
                model.badly_off_reason_id = "1008";
            }

            model.badly_off_describe = info5.Text.Trim();


            if (help1.Checked)
            {
                model.enjoy_help_id = "";
            }
            if (help2.Checked)
            {
                u_enjoy_help eh = new u_enjoy_help();
                string helpid = Guid.NewGuid().ToString();
                eh.id = helpid;
                model.enjoy_help_id = helpid;
                eh.start_time = Convert.ToDateTime(TextBox3.Text);
                eh.end_time = Convert.ToDateTime(TextBox4.Text);
                eh.help_way_id = TextBox5.Text;
                eh.remark = TextBox6.Text.ToString().Trim();

                bool enjoy = bll.addEnjoyHelp(eh);
            }

            u_float_commie commie = new u_float_commie();
            string commieid = Guid.NewGuid().ToString();
            model.float_commie_id = commieid;
            commie.id = commieid;
            commie.flow_type = TextBox7.Text.ToString();
            commie.linkman = TextBox8.Text.ToString();
            commie.flow_reason = TextBox9.Text.ToString();
            commie.contact = TextBox10.Text.ToString();
            commie.id_number = TextBox11.Text.ToString();
            commie.group_linkman = TextBox12.Text.ToString();
            commie.discharge_place = TextBox13.Text.ToString();
            commie.group_contact = TextBox14.Text.ToString();

            bool comm = bll.addFloatCommie(commie);

            //保存数据
            if (bll.Add(model) > 0)
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加用户:" + model.user_name); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            BLL.users bll = new BLL.users();
            Model.users model = bll.GetModel(_id);
            model.group_id = ddlGroupId.SelectedValue;
            // 2017-08-02 因业务需求人员状态不可修改。
            //model.status = int.Parse(rblStatus.SelectedValue);
            model.id = _id;
            model.user_name = Utils.DropHTML(txtUserName.Text.Trim());
            //判断密码是否更改
            if (txtPassword.Text.Trim() != defaultpassword)
            {
                //获取用户已生成的salt作为密钥加密
                model.password = DESEncrypt.Encrypt(txtPassword.Text.Trim(), model.salt);
            }
            model.email = Utils.DropHTML(txtEmail.Text);
            model.nick_name = Utils.DropHTML(txtNickName.Text);
            model.avatar = Utils.DropHTML(txtImgUrl.Text);
            model.sex = rblSex.SelectedValue;
            DateTime _birthday;
            if (DateTime.TryParse(txtBirthday.Text.Trim(), out _birthday))
            {
                model.birthday = _birthday;
            }
            model.telphone = Utils.DropHTML(txtTelphone.Text.Trim());
            model.mobile = Utils.DropHTML(txtMobile.Text.Trim());
            model.qq = Utils.DropHTML(txtQQ.Text);
            model.wechat = Utils.DropHTML(txtwechat.Text.Trim());
            model.msn = Utils.DropHTML(txtMsn.Text);
            model.address = Utils.DropHTML(txtAddress.Text.Trim());
            model.reg_time = DateTime.Now;
            model.reg_ip = DTRequest.GetIP();
            model.id_card = identity.Text.Trim();//身份证号
            model.nation = txtnationId.Text.Trim(); //民族		
            model.marital_status = int.Parse(maritalStatus.Text.Trim());    //婚姻状态	
            //新阶层人员
            if (new1.Checked)
            {
                model.party_job = "否";
            }
            if (new2.Checked)
            {
                if (new_class.Text == "1")
                {
                    model.new_class_id = "1001";
                }
                if (new_class.Text == "2")
                {
                    model.new_class_id = "1002";
                }
                if (new_class.Text == "3")
                {
                    model.new_class_id = "1003";
                }
                if (new_class.Text == "4")
                {
                    model.new_class_id = "1004";
                }
                if (new_class.Text == "5")
                {
                    model.new_class_id = "1005";
                }
                if (new_class.Text == "6")
                {
                    model.new_class_id = "1006";
                }
                if (new_class.Text == "7")
                {
                    model.new_class_id = "1007";
                }
            }

            if (children_info.SelectedValue == "1")
            {
                model.children_info = 1;
            }
            else if (children_info.SelectedValue == "2")
            {
                model.children_info = 2;
            }
            else if (children_info.SelectedValue == "3")
            {
                model.children_info = 3;
            }

            if (only_child_award.SelectedValue == "1")
            {
                model.only_child_award = 1;
            }
            else if (only_child_award.SelectedValue == "2")
            {
                model.only_child_award = 2;
            }

            //行政级别
            if (rank1.Checked)
            {
                model.administration_rank_id = "1001";
            }
            if (rank2.Checked)
            {
                model.administration_rank_id = "1002";
            }
            if (rank3.Checked)
            {
                model.administration_rank_id = "1003";
            }
            if (rank4.Checked)
            {
                model.administration_rank_id = "1004";
            }
            if (rank5.Checked)
            {
                model.administration_rank_id = "1005";
            }
            if (rank6.Checked)
            {
                model.administration_rank_id = "1006";
            }
            if (rank7.Checked)
            {
                model.administration_rank_id = "1007";
            }
            if (rank8.Checked)
            {
                model.administration_rank_id = "1008";
            }
            if (rank9.Checked)
            {
                model.administration_rank_id = "1009";
            }
            if (rank10.Checked)
            {
                model.administration_rank_id = "1010";
            }
            if (rank11.Checked)
            {
                model.administration_rank_id = "1011";
            }
            if (rank12.Checked)
            {
                model.administration_rank_id = "1012";
            }

            //军职级别
            if (military1.Checked)
            {
                model.military_rank_id = "1001";
            }
            if (military2.Checked)
            {
                model.military_rank_id = "1002";
            }
            if (military3.Checked)
            {
                model.military_rank_id = "1003";
            }
            if (military4.Checked)
            {
                model.military_rank_id = "1004";
            }
            if (military5.Checked)
            {
                model.military_rank_id = "1005";
            }
            if (military6.Checked)
            {
                model.military_rank_id = "1006";
            }
            if (military7.Checked)
            {
                model.military_rank_id = "1007";
            }
            if (military8.Checked)
            {
                model.military_rank_id = "1008";
            }
            if (military9.Checked)
            {
                model.military_rank_id = "1009";
            }
            if (military10.Checked)
            {
                model.military_rank_id = "1010";
            }
            if (military11.Checked)
            {
                model.military_rank_id = "1011";
            }
            if (military12.Checked)
            {
                model.military_rank_id = "1012";
            }
            if (military13.Checked)
            {
                model.military_rank_id = "1013";
            }
            if (military14.Checked)
            {
                model.military_rank_id = "1014";
            }


            //警衔级别
            if (police1.Checked)
            {
                model.police_rank_id = "1001";
            }
            if (police2.Checked)
            {
                model.police_rank_id = "1002";
            }
            if (police3.Checked)
            {
                model.police_rank_id = "1003";
            }
            if (police4.Checked)
            {
                model.police_rank_id = "1004";
            }
            if (police5.Checked)
            {
                model.police_rank_id = "1005";
            }
            if (police6.Checked)
            {
                model.police_rank_id = "1006";
            }
            if (police7.Checked)
            {
                model.police_rank_id = "1007";
            }
            if (police8.Checked)
            {
                model.police_rank_id = "1008";
            }
            if (police9.Checked)
            {
                model.police_rank_id = "1009";
            }
            if (police10.Checked)
            {
                model.police_rank_id = "1010";
            }
            if (police11.Checked)
            {
                model.police_rank_id = "1011";
            }
            if (police12.Checked)
            {
                model.police_rank_id = "1012";
            }
            if (police13.Checked)
            {
                model.police_rank_id = "1013";
            }


            //优抚对象
            if (Entitled1.Checked)
            {
                model.entitled_group_id = "1001";
            }
            if (Entitled2.Checked)
            {
                model.entitled_group_id = "1002";
            }
            if (Entitled3.Checked)
            {
                model.entitled_group_id = "1003";
            }
            if (Entitled4.Checked)
            {
                model.entitled_group_id = "1004";
            }
            if (Entitled5.Checked)
            {
                model.entitled_group_id = "1005";
            }
            if (Entitled6.Checked)
            {
                model.entitled_group_id = "1006";
            }
            if (Entitled7.Checked)
            {
                model.entitled_group_id = "1007";
            }
            if (Entitled8.Checked)
            {
                model.entitled_group_id = "1008";
            }
            if (Entitled9.Checked)
            {
                model.entitled_group_id = "1009";
            }
            if (Entitled10.Checked)
            {
                model.entitled_group_id = "1010";
            }
            if (Entitled11.Checked)
            {
                model.entitled_group_id = "1011";
            }

            if (passport1.Checked)
            {
                model.passport_info_id = "1001";
            }
            else if (passport2.Checked)
            {
                model.passport_info_id = "1002";
            }
            else if (passport3.Checked)
            {
                model.passport_info_id = "1003";
            }
            else if (passport4.Checked)
            {
                model.passport_info_id = "1004";
            }
            else if (passport5.Checked)
            {
                model.passport_info_id = "1005";
            }
            else if (passport6.Checked)
            {
                model.passport_info_id = "1006";
            }
            //收入来源
            if (i1001.Checked)
            {
                model.income_source_id = "1001";
            }
            if (i1002.Checked)
            {
                model.income_source_id = "1002";
            }
            if (i1003.Checked)
            {
                model.income_source_id = "1003";
            }
            if (i1004.Checked)
            {
                model.income_source_id = "1004";
            }
            if (i1005.Checked)
            {
                model.income_source_id = "1005";
            }
            if (i1006.Checked)
            {
                model.income_source_id = "1006";
            }
            if (i1007.Checked)
            {
                model.income_source_id = "1007";
            }

            model.native_place = nativePlace.Text.Trim();

            model.live_place = livePlace.Text.Trim();

            model.graduate_school = shool.Text.Trim();

            DateTime _graduate_time;
            if (DateTime.TryParse(bytime.Text.Trim(), out _graduate_time))
            {
                model.graduate_time = _graduate_time;
            }

            //学历情况
            if (e1.Checked)
            {
                model.education_info_id = "1001";
            }
            if (e2.Checked)
            {
                model.education_info_id = "1002";
            }
            if (e3.Checked)
            {
                model.education_info_id = "1003";
            }
            if (e4.Checked)
            {
                model.education_info_id = "1004";
            }
            if (e5.Checked)
            {
                model.education_info_id = "1005";
            }
            if (e6.Checked)
            {
                model.education_info_id = "1006";
            }
            if (e7.Checked)
            {
                model.education_info_id = "1007";
            }
            if (e8.Checked)
            {
                model.education_info_id = "1008";
            }
            if (e9.Checked)
            {
                model.education_info_id = "1009";
            }
            if (e10.Checked)
            {
                model.education_info_id = "1010";
            }
            if (e11.Checked)
            {
                model.education_info_id = "1011";
            }

            model.degree_info = int.Parse(degree_info.Text.Trim());

            //入党时间
            DateTime _joinPartyTime;
            if (DateTime.TryParse(joinPartyTime.Text.Trim(), out _joinPartyTime))
            {
                model.join_party_time = _joinPartyTime;
            }

            model.party_membership = peoname.Text.Trim();

            model.first_branch = aftergroup.Text.Trim();

            model.party_job = nowjob.Text.Trim();

            model.now_organiz = nowgroup.Text.Trim();

            //进入现支部类型
            if (type1.Checked)
            {
                model.group_type_id = "1001";
            }
            if (type2.Checked)
            {
                model.group_type_id = "1002";
            }
            if (type3.Checked)
            {
                model.group_type_id = "1003";
            }
            if (type4.Checked)
            {
                model.group_type_id = "1004";
            }
            if (type5.Checked)
            {
                model.group_type_id = "1005";
            }
            if (type6.Checked)
            {
                model.group_type_id = "1006";
            }
            if (type7.Checked)
            {
                model.group_type_id = "1007";
            }
            if (type8.Checked)
            {
                model.group_type_id = "1008";
            }
            if (type9.Checked)
            {
                model.group_type_id = "1009";
            }

            //党员进社区情况
            if (community1.Checked)
            {
                model.community_info = "未参加";
            }
            if (community2.Checked)
            {
                model.community_info = txtContent.InnerText.Trim();
            }

            //参加组织生活情况
            if (live1.Checked)
            {
                model.group_live = "未曾参加";
            }
            if (live2.Checked)
            {
                model.group_live = txtlive.InnerText.Trim();
            }

            //党员奖惩信息
            u_reward_punishment reward = new u_reward_punishment();
            string puid = String.IsNullOrEmpty(model.reward_punishment_id) ? Guid.NewGuid().ToString(): model.reward_punishment_id;
            reward.id = puid;
            reward.title = title.Text;
            reward.reason = txtreason.Text;
            reward.approval_authority = txtauthority.Text;
            reward.office_level = txtlevel.Text;
            if (!String.IsNullOrEmpty(model.reward_punishment_id)) {
                bool rew = bll.updatereward(reward);
            }
            else {
                Boolean rew1 = bll.addreward(reward);
                model.reward_punishment_id = puid;
            }

            //现工作单位信息
            u_company_type company = new u_company_type();
            string nowid = String.IsNullOrEmpty(model.now_company_id) ? Guid.NewGuid().ToString() : model.now_company_id;
            company.id = nowid;
            company.name = nowCompanyName.Text.Trim();
            company.employee_count = nowpeoplecount.Text.Trim() == "" ? 0 : int.Parse(nowpeoplecount.Text.Trim());
            if (com1.Checked)
            {
                company.relation_com = "现工作单位";
            }
            else if (com2.Checked)
            {
                company.relation_com = "其他";
            }

            //现工作单位类型
            if (c1010.Checked)
            {
                company.com_type_id = "1001";
            }
            else if (c1011.Checked)
            {
                company.com_type_id = "1002";
            }
            else if (c1012.Checked)
            {
                company.com_type_id = "1003";
            }
            else if (c1013.Checked)
            {
                company.com_type_id = "1004";
            }
            else if (c1014.Checked)
            {
                company.com_type_id = "1005";
            }
            else if (c1015.Checked)
            {
                company.com_type_id = "1006";
            }
            else if (c1016.Checked)
            {
                company.com_type_id = "1007";
            }
            else if (c1017.Checked)
            {
                company.com_type_id = "1008";
            }
            else if (c1018.Checked)
            {
                company.com_type_id = "1009";
            }
            else if (c1019.Checked)
            {
                company.com_type_id = "1010";
            }
            else if (c1020.Checked)
            {
                company.com_type_id = "1011";
            }
            else if (c1021.Checked)
            {
                company.com_type_id = "1012";
            }
            else if (c1022.Checked)
            {
                company.com_type_id = "1013";
            }
            else if (c1023.Checked)
            {
                company.com_type_id = "1014";
            }
            else if (c1024.Checked)
            {
                company.com_type_id = "1015";
            }
            else if (c1025.Checked)
            {
                company.com_type_id = "1016";
            }
            else if (c1026.Checked)
            {
                company.com_type_id = "1017";
            }
            else if (c1027.Checked)
            {
                company.com_type_id = "1018";
            }
            else if (c1028.Checked)
            {
                company.com_type_id = "1019";
            }
            else if (c1029.Checked)
            {
                company.com_type_id = "1020";
            }
            else if (c1030.Checked)
            {
                company.com_type_id = "1021";
            }
            else if (c1031.Checked)
            {
                company.com_type_id = "1022";
            }
            else if (c1032.Checked)
            {
                company.com_type_id = "1023";
            }

            //现工作岗位类型
            if (c2010.Checked)
            {
                company.post_type_id = "1001";
            }
            else if (c2011.Checked)
            {
                company.post_type_id = "1002";
            }
            else if (c2012.Checked)
            {
                company.post_type_id = "1003";
            }
            else if (c2013.Checked)
            {
                company.post_type_id = "1004";
            }
            else if (c2014.Checked)
            {
                company.post_type_id = "1005";
            }
            else if (c2015.Checked)
            {
                company.post_type_id = "1006";
            }
            else if (c2016.Checked)
            {
                company.post_type_id = "1007";
            }
            else if (c2017.Checked)
            {
                company.post_type_id = "1008";
            }
            else if (c2018.Checked)
            {
                company.post_type_id = "1009";
            }
            else if (c2019.Checked)
            {
                company.post_type_id = "1010";
            }
            else if (c2020.Checked)
            {
                company.post_type_id = "1011";
            }
            else if (c2021.Checked)
            {
                company.post_type_id = "1012";
            }
            else if (c2022.Checked)
            {
                company.post_type_id = "1013";
            }
            else if (c2023.Checked)
            {
                company.post_type_id = "1014";
            }
            else if (c2024.Checked)
            {
                company.post_type_id = "1015";
            }
            else if (c2025.Checked)
            {
                company.post_type_id = "1016";
            }
            else if (c2026.Checked)
            {
                company.post_type_id = "1017";
            }
            else if (c2027.Checked)
            {
                company.post_type_id = "1018";
            }
            else if (c2028.Checked)
            {
                company.post_type_id = "1019";
            }
            else if (c2029.Checked)
            {
                company.post_type_id = "1020";
            }
            else if (c2030.Checked)
            {
                company.post_type_id = "1021";
            }
            else if (c2031.Checked)
            {
                company.post_type_id = "1022";
            }
            else if (c2032.Checked)
            {
                company.post_type_id = "1023";
            }
            else if (c2033.Checked)
            {
                company.post_type_id = "1024";
            }
            else if (c2034.Checked)
            {
                company.post_type_id = "1025";
            }
            else if (c2035.Checked)
            {
                company.post_type_id = "1026";
            }
            else if (c2036.Checked)
            {
                company.post_type_id = "1027";
            }

            if (!String.IsNullOrEmpty(model.now_company_id)) {
                bool com = bll.updatecompany(company);
                model.now_company_id = nowid;
            }
            else {
                Boolean com1 = bll.addcompany(company);
                model.now_company_id = nowid;
            }


            //原工作单位信息
            u_company_type ycompany = new u_company_type();
            string originalid = String.IsNullOrEmpty(model.former_company_id) ? Guid.NewGuid().ToString() : model.former_company_id;
            ycompany.id = originalid;
            ycompany.name = originalComName.Text.Trim();
            ycompany.employee_count = originalpeocount.Text.Trim() == "" ? 0 : int.Parse(originalpeocount.Text.Trim());
            if (originalcom1.Checked)
            {
                ycompany.relation_com = "现工作单位";
            }
            else if (originalcom2.Checked)
            {
                ycompany.relation_com = "其他";
            }

            //原工作单位类型
            if (c3010.Checked)
            {
                ycompany.com_type_id = "1001";
            }
            else if (c3011.Checked)
            {
                ycompany.com_type_id = "1002";
            }
            else if (c3012.Checked)
            {
                ycompany.com_type_id = "1003";
            }
            else if (c3013.Checked)
            {
                ycompany.com_type_id = "1004";
            }
            else if (c3014.Checked)
            {
                ycompany.com_type_id = "1005";
            }
            else if (c3015.Checked)
            {
                ycompany.com_type_id = "1006";
            }
            else if (c3016.Checked)
            {
                ycompany.com_type_id = "1007";
            }
            else if (c3017.Checked)
            {
                ycompany.com_type_id = "1008";
            }
            else if (c3018.Checked)
            {
                ycompany.com_type_id = "1009";
            }
            else if (c3019.Checked)
            {
                ycompany.com_type_id = "1010";
            }
            else if (c3020.Checked)
            {
                ycompany.com_type_id = "1011";
            }
            else if (c3021.Checked)
            {
                ycompany.com_type_id = "1012";
            }
            else if (c3022.Checked)
            {
                ycompany.com_type_id = "1013";
            }
            else if (c3023.Checked)
            {
                ycompany.com_type_id = "1014";
            }
            else if (c3024.Checked)
            {
                ycompany.com_type_id = "1015";
            }
            else if (c3025.Checked)
            {
                ycompany.com_type_id = "1016";
            }
            else if (c3026.Checked)
            {
                ycompany.com_type_id = "1017";
            }
            else if (c3027.Checked)
            {
                ycompany.com_type_id = "1018";
            }
            else if (c3028.Checked)
            {
                ycompany.com_type_id = "1019";
            }
            else if (c3029.Checked)
            {
                ycompany.com_type_id = "1020";
            }
            else if (c3030.Checked)
            {
                ycompany.com_type_id = "1021";
            }
            else if (c3031.Checked)
            {
                ycompany.com_type_id = "1022";
            }
            else if (c3032.Checked)
            {
                ycompany.com_type_id = "1023";
            }

            //原工作岗位类型
            if (c3041.Checked)
            {
                ycompany.post_type_id = "1001";
            }
            else if (c3042.Checked)
            {
                ycompany.post_type_id = "1002";
            }
            else if (c3043.Checked)
            {
                ycompany.post_type_id = "1003";
            }
            else if (c3044.Checked)
            {
                ycompany.post_type_id = "1004";
            }
            else if (c3045.Checked)
            {
                ycompany.post_type_id = "1005";
            }
            else if (c3046.Checked)
            {
                ycompany.post_type_id = "1006";
            }
            else if (c3047.Checked)
            {
                ycompany.post_type_id = "1007";
            }
            else if (c3048.Checked)
            {
                ycompany.post_type_id = "1008";
            }
            else if (c3049.Checked)
            {
                ycompany.post_type_id = "1009";
            }
            else if (c3050.Checked)
            {
                ycompany.post_type_id = "1010";
            }
            else if (c3051.Checked)
            {
                ycompany.post_type_id = "1011";
            }
            else if (c3052.Checked)
            {
                ycompany.post_type_id = "1012";
            }
            else if (c3053.Checked)
            {
                ycompany.post_type_id = "1013";
            }
            else if (c3054.Checked)
            {
                ycompany.post_type_id = "1014";
            }
            else if (c3055.Checked)
            {
                ycompany.post_type_id = "1015";
            }
            else if (c3056.Checked)
            {
                ycompany.post_type_id = "1016";
            }
            else if (c3057.Checked)
            {
                ycompany.post_type_id = "1017";
            }
            else if (c3058.Checked)
            {
                ycompany.post_type_id = "1018";
            }
            else if (c3059.Checked)
            {
                ycompany.post_type_id = "1019";
            }
            else if (c3060.Checked)
            {
                ycompany.post_type_id = "1020";
            }
            else if (c3061.Checked)
            {
                ycompany.post_type_id = "1021";
            }
            else if (c3062.Checked)
            {
                ycompany.post_type_id = "1022";
            }
            else if (c3063.Checked)
            {
                ycompany.post_type_id = "1023";
            }
            else if (c3064.Checked)
            {
                ycompany.post_type_id = "1024";
            }
            else if (c3065.Checked)
            {
                ycompany.post_type_id = "1025";
            }
            else if (c3066.Checked)
            {
                ycompany.post_type_id = "1026";
            }
            else if (c3067.Checked)
            {
                ycompany.post_type_id = "1027";
            }

            if (!String.IsNullOrEmpty(model.former_company_id)) {
                bool comp = bll.updateOrigCompany(ycompany);
                model.former_company_id = originalid;
            }
            else {
                bool comp1 = bll.addOrigCompany(ycompany);
                model.former_company_id = originalid;
            }


            model.is_badly_off = int.Parse(badly_of.Text);

            model.is_organiz_identity = int.Parse(is_identity.Text);

            if (financial1.Checked)
            {
                model.financial_situation_id = "1001";
            }
            else if (financial2.Checked)
            {
                model.financial_situation_id = "1002";
            }
            else if (financial3.Checked)
            {
                model.financial_situation_id = "1003";
            }
            else if (financial4.Checked)
            {
                model.financial_situation_id = "1004";
            }

            if (healthy1.Checked)
            {
                model.healthy_condition_id = "1001";
            }
            else if (healthy2.Checked)
            {
                model.healthy_condition_id = "1002";
            }
            else if (healthy3.Checked)
            {
                model.healthy_condition_id = "1003";
            }
            else if (healthy4.Checked)
            {
                model.healthy_condition_id = "1004";
            }
            else if (healthy5.Checked)
            {
                model.healthy_condition_id = "1005";
            }
            else if (healthy6.Checked)
            {
                model.healthy_condition_id = "1006";
            }
            else if (healthy7.Checked)
            {
                model.healthy_condition_id = "1007";
            }
            else if (healthy8.Checked)
            {
                model.healthy_condition_id = "1008";
            }

            if (reason1.Checked)
            {
                model.badly_off_reason_id = "1001";
            }
            else if (reason2.Checked)
            {
                model.badly_off_reason_id = "1002";
            }
            else if (reason3.Checked)
            {
                model.badly_off_reason_id = "1003";
            }
            else if (reason4.Checked)
            {
                model.badly_off_reason_id = "1004";
            }
            else if (reason5.Checked)
            {
                model.badly_off_reason_id = "1005";
            }
            else if (reason6.Checked)
            {
                model.badly_off_reason_id = "1006";
            }
            else if (reason7.Checked)
            {
                model.badly_off_reason_id = "1007";
            }
            else if (reason8.Checked)
            {
                model.badly_off_reason_id = "1008";
            }

            model.badly_off_describe = info5.Text.Trim();


            if (help1.Checked)
            {
                model.enjoy_help_id = "";
            }
            if (help2.Checked)
            {
                u_enjoy_help eh = new u_enjoy_help();
                string helpid = String.IsNullOrEmpty(model.enjoy_help_id) ? Guid.NewGuid().ToString() : model.enjoy_help_id;
                eh.id = helpid;
                eh.start_time = Convert.ToDateTime(TextBox3.Text);
                eh.end_time = Convert.ToDateTime(TextBox4.Text);
                eh.help_way_id = TextBox5.Text;
                eh.remark = TextBox6.Text.ToString().Trim();
                if (!String.IsNullOrEmpty(model.enjoy_help_id)) {
                bool enjoy = bll.updateEnjoyHelp(eh);
                }
                else {
                    Boolean enjoy1 = bll.addEnjoyHelp(eh);
                    model.enjoy_help_id = helpid;
                }
            }

            u_float_commie commie = new u_float_commie();
            string commieid = String.IsNullOrEmpty(model.float_commie_id) ? Guid.NewGuid().ToString() : model.float_commie_id;
            commie.id = commieid;
            commie.flow_type = TextBox7.Text.ToString();
            commie.linkman = TextBox8.Text.ToString();
            commie.flow_reason = TextBox9.Text.ToString();
            commie.contact = TextBox10.Text.ToString();
            commie.id_number = TextBox11.Text.ToString();
            commie.group_linkman = TextBox12.Text.ToString();
            commie.discharge_place = TextBox13.Text.ToString();
            commie.group_contact = TextBox14.Text.ToString();

            if (!String.IsNullOrEmpty(model.float_commie_id)) {
                bool comm = bll.updateFloatCommie(commie);
            }
            else {
                Boolean comm1 = bll.addFloatCommie(commie);
                model.float_commie_id = commieid;
            }


            if (bll.Update(model) == true)
            {
                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改用户信息:" + model.user_name); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("user_list", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("修改党员成功！", "user_list.aspx");
            }
            else //添加
            {
                ChkAdminLevel("user_list", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误！", "");
                    return;
                }
                JscriptMsg("添加党员成功！", "user_list.aspx");
            }
        }

        /// <summary>
        /// 获取七牛云基本数据
        /// </summary>
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


    }
}

