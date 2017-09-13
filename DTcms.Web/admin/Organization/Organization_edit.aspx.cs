using DTcms.Common;
using DTcms.DAL;
using DTcms.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.Organization
{
    public partial class Organization_edit : Web.UI.ManagePage
    {
        protected string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryInt("id");
                if (string.IsNullOrEmpty(this.id.ToString()))
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new BLL.Organization().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back");
                    return;
                }
            }
			//父级下拉列表的绑定
			if (!Page.IsPostBack)
            {
			    TreeBind("is_lock=0"); //绑定类别
                //ChkAdminLevel("user_list", DTEnums.ActionEnum.View.ToString()); //检查权限
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }
		private void TreeBind(string strWhere)
		{
			BLL.user_groups bll = new BLL.user_groups();
			DataTable dt = bll.GetList(0, strWhere, "grade asc,id asc").Tables[0];

			this.pname.Items.Clear();
			this.pname.Items.Add(new ListItem("请选择父级", ""));
			foreach (DataRow dr in dt.Rows)
			{
                string id = dr["id"].ToString();
                string title = dr["title"].ToString();
                this.pname.Items.Add(new ListItem(title,id));
			}
		}
        //页面绑定
		private void ShowInfo(int _id)
        {
			BLL.users bll = new BLL.users();
            BLL.Groups groups = new BLL.Groups();
            Model.user_groups model = groups.getGroups(_id);
            if(model.title!=null)
            {
                name.Text = model.title;//党支部名称
            }
            if(model.pid!=null)
            {
                pname.Text = model.pid.ToString();//父级id
            }
            if(model.manager!=null)
            {
                Hidden.Value = model.manager;//管理员
            }
            if(model.Manager_id!=null)
            {
                Hidden1.Value = model.Manager_id.ToString();//管理员id
            }
            if(model.location!=null)
            {
                position.Text = model.location;//党组织所在位置
            }
            if(model.position!=null)
            {
                longAndLat.Value = model.position;//定位
            }
            if(pname.Text!=null)
            {
                hipname.Value = pname.Text;
            }
            if(model.org_code!=null)
            {
                partyCode.Text = model.org_code;//党组织代码
            }
            if(model.create_time!=null)
            {
                createOrganizTime.Text = model.create_time.GetValueOrDefault().ToString("yyyy-M-d");//建立党组织日期
            }
            if(model.sort!=null)
            {
                partyGroupSort.SelectedValue = model.sort.ToString();//党组织类别
            }
            if(model.territory_relation!=null)
            {
                TextBox1.Text = model.territory_relation;//党组织属地关系
            }
            if(model.contact_address!=null)
            {
                communicationSite.Text = model.contact_address.ToString();//党组织通讯地址
            }
            if(model.phone_fax!=null)
            {
                phone.Text = model.phone_fax;//联系电话及传真
            }
            if(model.elected_date!=null)
            {
                dateTime1.Text = model.elected_date.GetValueOrDefault().ToString("yyyy-M-d");//领导班子当选日期
            }
            if(model.expiration_date!=null)
            {
                dateTime2.Text = model.expiration_date.GetValueOrDefault().ToString("yyyy-M-d");//领导班子届满日期
            }
            if(model.superior_org!=null)
            {
                superiorGroup.Text = model.superior_org;//上级党组织名称
            }
            //正式党员
            TextBox2.Text = model.official_male_count.ToString();
            TextBox3.Text = model.official_female_count.ToString();
            //预备党员
            TextBox4.Text = model.ready_male_count.ToString();
            TextBox5.Text = model.ready_female_count.ToString();
            //下辖党组织信息
            if (groups.getExists(_id) == "0")
			{
				s1001.Checked = true;
			}
			else
			{
				s1002.Checked = true;
				txtnumber.Text = groups.getCount(_id);
                textcontent.InnerText = groups.getInfo(_id);
			}
            if(model.intre!=null)
            {
                txtorganizIntro.Text = model.intre;//党组织简介
            }            
            if (model.P_CreateVoluntaryTeam == null)
			{
				t1001.Checked = true;
			}
			else if(model.P_CreateVoluntaryTeam !=null)
			{
				t1002.Checked = true;
				team.Text = model.P_CreateVoluntaryTeam;
			}
            //奖惩信息
            P_RewardsAndPunishment reward = groups.getRewardsAndPunishment(_id);
            if (reward != null)
			{
				RPtitle.Text = reward.P_Title.ToString() == null ? "" : reward.P_Title.ToString();
                if (reward.P_DateTime != null)
                {
                    dateTime.Text = reward.P_DateTime.GetValueOrDefault().ToString("yyyy-M-d");//领导班子当选日期
                }
                content.Text = reward.P_Content.ToString() == null ? "" : reward.P_Content.ToString();
				ratifyOrganiz.Text = reward.P_RatifyOrganiz.ToString() == null ? "" : reward.P_RatifyOrganiz.ToString();
			}
            //单位信息
            u_company_type ty = groups.unitinfo(_id);
            if(ty!=null)
            {
                if(ty.name!=null)
                {
                    companyName.Text = ty.name;//单位名称
                }
                peopleCount.Text = Convert.ToString(ty.employee_count);//单位人数
                if(ty.service_organiz==0)
                {
                    t1001.Checked =true;
                }
                else
                {
                    t1002.Checked = true;
                    team.Text = Convert.ToString(ty.service_organiz);//建立党员服务组织
                }
                //单位性质
                if (ty.com_nature == "1001")
                {
                    this.RadioButton1.Checked = true;
                }
                else if (ty.com_nature == "1002")
                {
                    this.RadioButton2.Checked = true;
                }
                else if (ty.com_nature == "1003")
                {
                    this.RadioButton3.Checked = true;
                }
                else if (ty.com_nature == "1004")
                {
                    this.RadioButton4.Checked = true;
                }
                else if (ty.com_nature == "1005")
                {
                    this.RadioButton5.Checked = true;
                }
                else if (ty.com_nature == "1006")
                {
                    this.RadioButton6.Checked = true;
                }
            }

            //领导班子信息
            u_lead_info infotion = groups.leadinfo(_id);
            if(infotion!=null)
            {
                if(infotion.name!=null)
                {
                    Textbox6.Text = infotion.name;//姓名
                }
                if(infotion.job!=null)
                {
                    Textbox7.Text = infotion.job;//职位
                }
                if(infotion.contact_way!=null)
                {
                    TextBox.Text = infotion.contact_way;//联系方式
                }
                if(infotion.remark!=null)
                {
                    Textbox9.Text = infotion.remark;//备注
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string a = name.Text;
            string b = pname.SelectedItem.ToString();            
            string m = string.Empty;
            string f;
            if (string.IsNullOrEmpty(manager.Value))
            {
                m = Hidden.Value;
                f = Hidden1.Value;
            }
            else
            {
                m = manager.Value;
                f = manager1.Value;
            }
            string userid = GetAdminInfo().id.ToString();
            string po = position.Text;
            BLL.Organization mm = new BLL.Organization();
            int mid;
            bool results;
            //修改
            if (action == DTEnums.ActionEnum.Edit.ToString())
            {
				int id = this.id = DTRequest.GetQueryInt("id");
                //BLL.users bll = new BLL.users();
                BLL.Groups groups = new BLL.Groups();
                Model.user_groups model = groups.getGroups(id);
                Model.user_groups group = new Model.user_groups();//主表
                u_company_type ty = new u_company_type();//工作单位信息表                
                u_lead_info info = new u_lead_info();//领导班子成员信息表
                Model.users user = new Model.users();

                group.id = id;//党组织id
                group.pid = groups.SearchParty(b);//父级id
                group.title = a;//党支部名称
                group.manager = m;//党委书记
                group.Manager_id = Convert.ToInt32(f);//党委书记id
                group.location = po;//组织位置
                group.position = longAndLat.Value;//经纬度
				group.org_code = partyCode.Text.Trim();//党组织代码
                DateTime _create_time;
				if (DateTime.TryParse(createOrganizTime.Text.Trim(), out _create_time))
				{
					group.create_time = _create_time;//建立党组织日期
                }
                group.sort = int.Parse(partyGroupSort.SelectedValue);//党组织类别
                group.territory_relation = TextBox1.Text.Trim();//党组织属地关系
                DateTime _elected_date;
                if (DateTime.TryParse(dateTime1.Text.Trim(), out _elected_date))
                {
                    group.elected_date = _elected_date;//领导班子当选日期
                }
                DateTime _expiration_date;
                if (DateTime.TryParse(dateTime2.Text.Trim(), out _expiration_date))
                {
                    group.expiration_date = _expiration_date;//领导班子届满日期
                }
                group.contact_address = communicationSite.Text.Trim();//党组织通讯地址
                group.phone_fax = phone.Text.Trim();//联系电话及传真
                group.superior_org = superiorGroup.Text.Trim();//上级党组织名称
                //下辖党组织信息
                string subordinateGroupInfoId = model.sub_org_id;
                if(subordinateGroupInfoId != null)
                {
                    if (s1001.Checked)//无
                    {
                       groups.subordinate(subordinateGroupInfoId, 0, int.Parse(txtnumber.Text.Trim() == "" ? "0" : txtnumber.Text.Trim()), textcontent.InnerText.Trim().ToString());
                       group.sub_org_id = subordinateGroupInfoId;
                    }
                   else if (s1002.Checked)//有
                   {
                       groups.subordinate(subordinateGroupInfoId, 1, int.Parse(txtnumber.Text.Trim() == "" ? "0" : txtnumber.Text.Trim()), textcontent.InnerText.Trim().ToString());
                       group.sub_org_id = subordinateGroupInfoId;
                   }
                }
                else if(subordinateGroupInfoId == null) 
                {
                    if (s1001.Checked)//无
                    {
                        subordinateGroupInfoId = Guid.NewGuid().ToString();
                        groups.setSubordinateGroupInfoId(subordinateGroupInfoId, 0, int.Parse(txtnumber.Text.Trim() == "" ? "0" : txtnumber.Text.Trim()), textcontent.InnerText.Trim().ToString());
                        group.sub_org_id = subordinateGroupInfoId;
                    }
                    else if (s1002.Checked)//有
                    {
                        subordinateGroupInfoId = Guid.NewGuid().ToString();
                        groups.setSubordinateGroupInfoId(subordinateGroupInfoId, 1, int.Parse(txtnumber.Text.Trim() == "" ? "0" : txtnumber.Text.Trim()), textcontent.InnerText.Trim().ToString());
                        group.sub_org_id = subordinateGroupInfoId;
                    }
                }
                //正式党员
                group.official_male_count = int.Parse(TextBox2.Text.Trim());//男
                group.official_female_count = int.Parse(TextBox3.Text.Trim());//女
                //预备党员
                group.ready_male_count = int.Parse(TextBox4.Text.Trim());//男
                group.ready_female_count = int.Parse(TextBox5.Text.Trim());//女
                group.intre = txtorganizIntro.Text.Trim();//党组织简介
                //单位性质
                if (RadioButton1.Checked)
                {
                    ty.com_nature = "1001";
                }
                else if (RadioButton2.Checked)
                {
                    ty.com_nature = "1002";
                }
                else if (RadioButton3.Checked)
                {
                    ty.com_nature = "1003";
                }
                else if (RadioButton4.Checked)
                {
                    ty.com_nature = "1004";
                }
                else if (RadioButton5.Checked)
                {
                    ty.com_nature = "1005";
                }
                else if (RadioButton6.Checked)
                {
                    ty.com_nature = "1006";
                }
                //建立党员服务组织
                string unitid = model.company_info_id;
                if (unitid!=null)
                {
                    if (t1001.Checked)
                    {
                        //group.P_CreateVoluntaryTeam = 0;
                        if(companyName.Text.Trim()!="")
                        {
                            bool unit = groups.changeunit(unitid, companyName.Text.Trim(), ty.com_nature, int.Parse(peopleCount.Text.Trim()),0);
                            group.company_info_id = unitid;
                        }                      
                        //group.lead_info_id = unitid;
                    }
                    else if (t1002.Checked)
                    {
                        //group.P_CreateVoluntaryTeam = team.Text.ToString().Trim();
                        bool unit = groups.changeunit(unitid, companyName.Text.Trim(), ty.com_nature, int.Parse(peopleCount.Text.Trim()),int.Parse(team.Text.Trim()));
                        group.company_info_id = unitid;
                    }
                }
                else if(unitid ==null)
                {
                    if (t1001.Checked)
                    {
                        //group.P_CreateVoluntaryTeam = "无";
                        if (companyName.Text.Trim() != "")
                        {
                            unitid = Guid.NewGuid().ToString();
                            groups.UnitInfo(unitid, companyName.Text.Trim(), ty.com_nature, int.Parse(peopleCount.Text.Trim()), 0);
                            group.company_info_id = unitid;
                        }
                    }
                    else if (t1002.Checked)
                    {
                        //group.P_CreateVoluntaryTeam = team.Text.ToString().Trim();
                        if (companyName.Text.Trim() != "")
                        {
                            unitid = Guid.NewGuid().ToString();
                            groups.UnitInfo(unitid, companyName.Text.Trim(), ty.com_nature, int.Parse(peopleCount.Text.Trim()),int.Parse(team.Text.ToString().Trim()));
                            group.company_info_id = unitid;
                        }
                    }
                }
                //奖惩信息
                string rewardPunishId = String.IsNullOrEmpty(model.rewards_punishment_id) ? Guid.NewGuid().ToString() : model.rewards_punishment_id;
                P_RewardsAndPunishment rewardPunish = new P_RewardsAndPunishment();
                rewardPunish.P_Id = rewardPunishId;
                group.rewards_punishment_id = rewardPunishId;
                rewardPunish.P_Title = RPtitle.Text.Trim();//奖惩名称
                DateTime _pDateTime;
				if (DateTime.TryParse(dateTime.Text.Trim(), out _pDateTime))
				{
					rewardPunish.P_DateTime = _pDateTime;//奖惩日期
                }
				rewardPunish.P_Content = content.Text.Trim();//奖惩说明
                rewardPunish.P_RatifyOrganiz = ratifyOrganiz.Text.Trim();//批准奖惩的党组织
                bool uprap = false;
                if (!String.IsNullOrEmpty(model.rewards_punishment_id))
                {
                    uprap = groups.changeRewardsAndPunishment(rewardPunish);
                }
                else {
                    uprap = groups.addRewardsAndPunishment(rewardPunish);
                }
                //领导班子成员信息
                string infoid = model.lead_info_id;
                if(infoid == null)
                {
                    //领导班子成员信息
                    string infoid2 = Guid.NewGuid().ToString();
                    group.lead_info_id = infoid2;
                    group.name = Textbox6.Text.Trim();//姓名
                    group.job = Textbox7.Text.Trim();//职务
                    group.contact_way = TextBox.Text.Trim();//联系方式
                    group.remark = Textbox9.Text.Trim();//备注
                }
                if(infoid != null)
                {
                info.id = infoid;
                group.lead_info_id = infoid;
                info.name = Textbox6.Text.Trim();//姓名
                info.job = Textbox7.Text.Trim();//职务
                info.contact_way = TextBox.Text.Trim();//联系方式
                info.remark = Textbox9.Text.Trim();//备注
                bool upinfo;

                upinfo = groups.changeInfo(info);
                }
                results = groups.updateUsersGroup(group);

				if (results ==true  && uprap==true)
				{
					JscriptMsg("修改成功",
					Utils.CombUrlTxt("Organization_list.aspx", "site_id=0&keywords={1}", "0", ""), "parent.loadMenuTree");
				}
                else
                {
                    JscriptMsg("修改失败",
                    Utils.CombUrlTxt("Organization_list.aspx", "site_id=0&keywords={1}", "0", ""), "parent.loadMenuTree");
                }
			}
            //添加
            else
            {
                if (!mm.CheckRepeat(a, b,f))
                {
                    BLL.Groups groups = new BLL.Groups();
					Model.user_groups group = new Model.user_groups();//主表
                    u_company_type ty = new u_company_type();//工作单位信息表
                    P_RewardsAndPunishment rewardPunish = new P_RewardsAndPunishment();//奖惩信息表
                    Model.users user = new Model.users();
                    int managerid;
                    group.manager = m;//党委书记
                    if(f!="")
                    {
                        group.Manager_id = Convert.ToInt32(f);//党委书记id                    
                    }
                    managerid = group.Manager_id;
                    group.location = po;//组织位置
                    group.position = longAndLat.Value;//经纬度          
                    group.title = a;//党支部名称
                    group.pid = groups.SearchParty(b);//父级id
                    group.org_code = partyCode.Text.Trim();//党组织代码
                    DateTime _create_time;
                    if (DateTime.TryParse(createOrganizTime.Text.Trim(), out _create_time))
                    {
                        group.create_time = _create_time;//建立党组织日期
                    }
                    group.sort = int.Parse(partyGroupSort.SelectedValue);//党组织类别
                    group.territory_relation = TextBox1.Text.Trim();//党组织属地关系 
                    group.contact_address = communicationSite.Text.Trim();//党组织通讯地址
                    group.phone_fax = phone.Text.Trim();//联系电话及传真
                    DateTime _elected_date;
                    if (DateTime.TryParse(dateTime1.Text.Trim(), out _elected_date))
                    {
                        group.elected_date = _elected_date;//领导班子当选日期
                    }
                    DateTime _expiration_date;
                    if (DateTime.TryParse(dateTime2.Text.Trim(), out _expiration_date))
                    {
                        group.expiration_date = _expiration_date;//领导班子届满日期
                    }
                    group.superior_org = superiorGroup.Text.Trim();//上级党组织名称
                    u_lead_info info = new u_lead_info();//领导班子成员信息表
                    //领导班子成员信息
                    string infoid = Guid.NewGuid().ToString();
                    group.lead_info_id = infoid;
                    group.name = Textbox6.Text.Trim();//姓名
                    group.job = Textbox7.Text.Trim();//职务
                    group.contact_way = TextBox.Text.Trim();//联系方式
                    group.remark = Textbox9.Text.Trim();//备注
                    //下辖党组织信息                    
                    if (s1001.Checked)
                    {
                        string subordinateGroupInfoId = Guid.NewGuid().ToString();
                        groups.setSubordinateGroupInfoId(subordinateGroupInfoId, 0, int.Parse(txtnumber.Text.Trim() == "" ? "0" : txtnumber.Text.Trim()), textcontent.InnerText.Trim().ToString());
                        group.sub_org_id = subordinateGroupInfoId;
                    }
                    else if (s1002.Checked)
                    {
                        string subordinateGroupInfoId = Guid.NewGuid().ToString();
                        groups.setSubordinateGroupInfoId(subordinateGroupInfoId, 1, int.Parse(txtnumber.Text.Trim() == "" ? "0" : txtnumber.Text.Trim()), textcontent.InnerText.Trim().ToString());
                        group.sub_org_id = subordinateGroupInfoId;
                    }
                    //正式党员
                    if (TextBox2.Text.Trim() != null)
                    {
                        group.official_male_count = int.Parse(TextBox2.Text.Trim());//男
                    }
                    if(TextBox3.Text.Trim()!=null)
                    {
                        group.official_female_count = int.Parse(TextBox3.Text.Trim());//女
                    }
                    //预备党员
                    if(TextBox4.Text.Trim()!=null)
                    {
                        group.ready_male_count = int.Parse(TextBox4.Text.Trim());//男
                    }
                    if(TextBox5.Text.Trim()!=null)
                    {
                        group.ready_female_count = int.Parse(TextBox5.Text.Trim());//女
                    }
                    if(txtorganizIntro.Text.Trim()!=null)
                    {
                        group.intre = txtorganizIntro.Text.Trim();//党组织简介
                    }                  
                 
                    //单位性质
                    if (RadioButton1.Checked)
                    {
                        ty.com_nature = "1001";
                    }
                    else if (RadioButton2.Checked)
                    {
                        ty.com_nature = "1002";
                    }
                    else if (RadioButton3.Checked)
                    {
                        ty.com_nature = "1003";
                    }
                    else if (RadioButton4.Checked)
                    {
                        ty.com_nature = "1004";
                    }
                    else if (RadioButton5.Checked)
                    {
                        ty.com_nature = "1005";
                    }
                    else if (RadioButton6.Checked)
                    {
                        ty.com_nature = "1006";
                    }
                    //建立党员服务组织
                    if (t1001.Checked)
                    {
                        string unitid = Guid.NewGuid().ToString();
                        //group.P_CreateVoluntaryTeam = 0;
                        groups.UnitInfo(unitid, companyName.Text.Trim(), ty.com_nature, int.Parse(peopleCount.Text.Trim()), 0);
                        group.company_info_id = unitid;
                    }
                    if (t1002.Checked)
                    {
                        string unitid = Guid.NewGuid().ToString();
                        //group.P_CreateVoluntaryTeam = team.Text.ToString().Trim();
                        groups.UnitInfo(unitid, companyName.Text.Trim(), ty.com_nature, int.Parse(peopleCount.Text.Trim()),int.Parse(team.Text.ToString().Trim()));
                        group.company_info_id = unitid;
                    }
                    //组织奖惩信息
                    string rewardPunishId = Guid.NewGuid().ToString();
                    group.rewards_punishment_id = rewardPunishId;
                    rewardPunish.P_Id= rewardPunishId;
                    rewardPunish.P_Title = RPtitle.Text.Trim();//奖惩名称
                    DateTime _pDateTime;
                    if (DateTime.TryParse(dateTime.Text.Trim(), out _pDateTime))
                    {
                        rewardPunish.P_DateTime = _pDateTime;//奖惩日期
                    }
                    rewardPunish.P_Content = content.Text.Trim();//奖惩说明
                    rewardPunish.P_RatifyOrganiz = ratifyOrganiz.Text.Trim();//批准奖惩的党组织

                    bool qa = groups.addRewardsAndPunishment(rewardPunish);
                    //组织mid
                    mid = groups.addGroup(group);
                    bool userform = groups.addUser(managerid,mid);

                    if (mid!=0 && qa == true && userform==true)
                    {
                        JscriptMsg("添加成功",
                        Utils.CombUrlTxt("Organization_list.aspx", "site_id=0&keywords={1}", "0", ""), "parent.loadMenuTree");
                    }
                    else
                    {
                        JscriptMsg("修改失败",
                        Utils.CombUrlTxt("Organization_list.aspx", "site_id=0&keywords={1}", "0", ""), "parent.loadMenuTree");
                    }
                }
                else
                {
                    JscriptMsg("支部名称重复，请重新编辑",
                            Utils.CombUrlTxt("Organization_list.aspx", "site_id=0&keywords={1}", "0", ""), "parent.loadMenuTree");
                }
            }
        }

        [WebMethod]
        public static string GetNameList(string key)
        {
            BLL.ModelMembers mm = new BLL.ModelMembers();
            List<Name> list = mm.GetUserNameList(key);
            string a = JsonConvert.SerializeObject(list);
            return a;
        }


        [WebMethod]
        public static string GetPositionList()
        {
            BLL.Organization o = new BLL.Organization();
            string json = o.PositionList();
            return json;
        }
    }
}