using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model
{
    public class userExcel
    {
        public userExcel()
        {}

        //组织奖惩信息表
        public string P_rId { get; set; }
        public string P_rTitle { get; set; }
        public DateTime P_rDateTime { get; set; }
        public string P_rContent { get; set; }
        public string P_rRatifyOrganiz { get; set; }
        public DateTime P_rCreateTime { get; set; }
        public string P_rCreateUser { get; set; }
        public DateTime P_rUpdateTime { get; set; }
        public string P_rUpdateUser { get; set; }
        public int P_rStatus { get; set; }
        //优抚对象信息表
        public string P_eId { get; set; }

        public string P_eEntitledGroups { get; set; }

        public DateTime P_eCreateTime { get; set; }

        public string P_eCreateUser { get; set; }
        public DateTime P_eUpdateTime { get; set; }
        public string P_eUpdateUser { get; set; }
        public int P_eStatus { get; set; }
        //新阶层人员
        public string u_id { get; set; }
        public string u_type { get; set; }
        public DateTime u_create_time { get; set; }
        public string u_create_user { get; set; }
        public DateTime u_update_time { get; set; }
        public string u_update_user { get; set; }
        public int u_status { get; set; }
        //行政级别
        public string u_aid { get; set; }
        public string u_atype { get; set; }
        public DateTime u_acreate_time { get; set; }
        public string u_acreate_user { get; set; }

        public DateTime u_aupdate_time { get; set; }
        public string u_aupdate_user { get; set; }
        public int u_astatus { get; set; }
        //军职级别
        public string u_mid { get; set; }
        public string u_mtype { get; set; }
        public DateTime u_mcreate_time { get; set; }
        public string u_mcreate_user { get; set; }

        public DateTime u_mupdate_time { get; set; }
        public string u_mupdate_user { get; set; }
        public int u_mstatus { get; set; }
        //警衔级别
        public string u_pid { get; set; }
        public string u_ptype { get; set; }
        public DateTime u_pcreate_time { get; set; }
        public string u_pcreate_user { get; set; }

        public DateTime u_pupdate_time { get; set; }
        public string u_pupdate_user { get; set; }
        public int u_pstatus { get; set; }
        //主要收入来源信息表
        public string P_iId { get; set; }
        public string P_iIncomeSource { get; set; }
        public DateTime P_iCreateTime { get; set; }
        public string P_iCreateUser { get; set; }

        public DateTime P_iUpdateTime { get; set; }
        public string P_iUpdateUser { get; set; }
        public int P_iStatus { get; set; }
        //学历情况信息表
        public string P_Id { get; set; }
        public string P_Education { get; set; }
        public DateTime P_CreateTime { get; set; }
        public string P_CreateUser { get; set; }

        public DateTime P_UpdateTime { get; set; }
        public string P_UpdateUser { get; set; }
        public int P_Status { get; set; }
        //现工作单位类型信息表
        public string P_cId { get; set; }
        public string P_cComType { get; set; }
        public DateTime P_cCreateTime { get; set; }
        public string P_cCreateUser { get; set; }

        public DateTime P_cUpdateTime { get; set; }
        public string P_cUpdateUser { get; set; }
        public int P_cStatus { get; set; }
        //下辖组织情况信息表
        public string P_sId { get; set; }
        public int P_sExists { get; set; }
        public int P_sCount { get; set; }
        public string P_sInfo { get; set; }
        public DateTime P_sCreateTime { get; set; }
        public string P_sCreateUser { get; set; }

        public DateTime P_sUpdateTime { get; set; }
        public string P_sUpdateUser { get; set; }
        public int P_sStatus { get; set; }
        //护照情况
        public string u_Pid { get; set; }
        public string u_Ptype { get; set; }
        public DateTime u_Pcreate_time { get; set; }
        public string u_Pcreate_user { get; set; }
        public DateTime u_Pupdate_time { get; set; }
        public string u_Pupdate_user { get; set; }
        public int u_Pstatus { get; set; }
        //进入现支部类型
        public string u_gid { get; set; }
        public string u_gtype { get; set; }
        public DateTime u_gcreate_time { get; set; }
        public string u_gcreate_user { get; set; }
        public DateTime u_gupdate_time { get; set; }
        public string u_gupdate_user { get; set; }
        public int u_gstatus { get; set; }
        //生活困难原因
        public string u_bid { get; set; }
        public string u_btype { get; set; }
        public DateTime u_bcreate_time { get; set; }
        public string u_bcreate_user { get; set; }
        public DateTime u_bupdate_time { get; set; }
        public string u_bupdate_user { get; set; }
        public int u_bstatus { get; set; }
        //工作单位信息
        public string u_cid { get; set; }
        public string u_cname { get; set; }
        public int u_cemployee_count { get; set; }
        public string u_crelation_com { get; set; }
        public string u_ccom_type_id { get; set; }
        public string u_cpost_type_id { get; set; }
        public string u_yname { get; set; }
        public int u_yemployee_count { get; set; }
        public string u_yrelation_com { get; set; }
        public string u_ycom_type_id { get; set; }
        public string u_ypost_type_id { get; set; }
        public DateTime u_ccreate_time { get; set; }
        public string u_ccreate_user { get; set; }
        public DateTime u_cupdate_time { get; set; }
        public string u_cupdate_user { get; set; }
        public int u_cstatus { get; set; }
        public string u_ccom_nature { get; set; }
        public int u_cservice_organiz { get; set; }
        //党员奖惩情况
        public string u_rid { get; set; }
        public string u_rtitle { get; set; }
        public string u_rreasont { get; set; }
        public string u_rapproval_authority { get; set; }
        public string u_roffice_level { get; set; }
        public DateTime u_rcreate_time { get; set; }
        public string u_rcreate_user { get; set; }
        public DateTime u_rupdate_time { get; set; }
        public string u_rupdate_user { get; set; }
        public int u_rstatus { get; set; }
        //享受帮扶情况
        public string u_eid { get; set; }
        public DateTime u_estart_time { get; set; }
        public DateTime u_eend_time { get; set; }
        public string u_ehelp_way_id { get; set; }
        public string u_eremark { get; set; }
        public DateTime u_ecreate_time { get; set; }
        public string u_ecreate_user { get; set; }
        public DateTime u_eupdate_time { get; set; }
        public string u_eupdate_user { get; set; }
        public int u_estatus { get; set; }
        //工作岗位类型
        public string u_oid { get; set; }
        public string u_otype { get; set; }
        public DateTime u_ocreate_time { get; set; }
        public string u_ocreate_user { get; set; }
        public DateTime u_oupdate_time { get; set; }
        public string u_oupdate_user { get; set; }
        public int u_ostatus { get; set; }
        //身体健康情况
        public string u_hid { get; set; }
        public string u_htype { get; set; }
        public DateTime u_hcreate_time { get; set; }
        public string u_hcreate_user { get; set; }
        public DateTime u_hupdate_time { get; set; }
        public string u_hupdate_user { get; set; }
        public string u_hdisease_info { get; set; }
        public int u_hstatus { get; set; }
        //帮扶形式类型
        public string u_Hid { get; set; }
        public string u_Htype { get; set; }
        public DateTime u_Hcreate_time { get; set; }
        public string u_Hcreate_user { get; set; }
        public DateTime u_Hupdate_time { get; set; }
        public string u_Hupdate_user { get; set; }
        public int u_Hstatus { get; set; }
        //经济状况
        public string u_Fid { get; set; }
        public string u_Ftype { get; set; }
        public DateTime u_Fcreate_time { get; set; }
        public string u_Fcreate_user { get; set; }
        public DateTime u_Fupdate_time { get; set; }
        public string u_Fupdate_user { get; set; }
        public int u_Fstatus { get; set; }
        //领导班子成员信息
        public string u_Lid { get; set; }
        public string u_Lgroup_id { get; set; }
        public string u_Lname { get; set; }
        public string u_Ljob { get; set; }
        public string u_Lcontact_way { get; set; }
        public string u_Lremark { get; set; }
        public DateTime u_Lcreate_time { get; set; }
        public string u_Lcreate_user { get; set; }
        public DateTime u_Lupdate_time { get; set; }
        public string u_Lupdate_user { get; set; }
        public int u_Lstatus { get; set; }
        //流动党员
        public string u_lid { get; set; }
        public string u_lflow_type { get; set; }
        public string u_llinkman { get; set; }
        public string u_lflow_reason { get; set; }
        public string u_lcontact { get; set; }
        public string u_lid_number { get; set; }
        public string u_lgroup_linkman { get; set; }
        public string u_ldischarge_place { get; set; }
        public string u_lgroup_contact { get; set; }
        public DateTime u_lcreate_time { get; set; }
        public string u_lcreate_user { get; set; }
        public DateTime u_lupdate_time { get; set; }
        public string u_lupdate_user { get; set; }
        public int u_lstatus { get; set; }
        //党组织表
        public int dt_uid { get; set; }
        public string dt_utitle { get; set; }
        public int dt_ugrade { get; set; }
        public int dt_uupgrade_exp { get; set; }
        public decimal dt_uamount { get; set; }
        public int dt_upoint { get; set; }
        public int dt_udiscount { get; set; }
        public int dt_uis_default { get; set; }
        public int dt_uis_upgrade { get; set; }
        public int dt_uis_lock { get; set; }
        public string dt_upid { get; set; }
        public string dt_umanager { get; set; }
        public string dt_uposition { get; set; }
        public string dt_umanager_id { get; set; }
        public int dt_ustatus { get; set; }
        public string dt_ulocation { get; set; }
        public string dt_uorg_code { get; set; }
        public DateTime dt_ucreate_time { get; set; }
        public int dt_usort { get; set; }
        public string dt_ucontact_address { get; set; }
        public string dt_usuperior_org { get; set; }
        public string dt_usub_org_id { get; set; }
        public string dt_uintre { get; set; }
        public string dt_uphone_fax { get; set; }
        public string dt_uterritory_relation { get; set; }
        public DateTime dt_uelected_date { get; set; }
        public DateTime dt_uexpiration_date { get; set; }
        public string dt_ulead_info_id { get; set; }
        public string dt_urewards_punishment_id { get; set; }
        public string dt_ucompany_info_id { get; set; }
        public string dt_uphone { get; set; }
        public string dt_usecretary_name { get; set; }
        public string dt_ucontact_person { get; set; }
        public string dt_ucontact_person_tel { get; set; }
        public int dt_uofficial_male_count { get; set; }

        public int dt_uofficial_female_count { get; set; }
        public int dt_uready_male_count { get; set; }
        public int dt_uready_female_count { get;set; }
        //党员表
        public int dt_Uid { get; set; }
        public string dt_Ugroup_id { get; set; }
        public string dt_Uuser_name { get; set; }
        public string dt_Usalt { get; set; }
        public string dt_Upassword { get; set; }
        public string dt_Umobile { get; set; }
        public string dt_Uemail { get; set; }
        public string dt_Uavatar { get; set; }
        public string dt_Unick_name { get; set; }
        public string dt_Usex { get; set; }
        public DateTime dt_Ubirthday { get; set; }
        public string dt_Utelphone { get; set; }
        public string dt_Uarea { get; set; }
        public string dt_Uaddress { get; set; }
        public string dt_Uqq { get; set; }
        public string dt_Umsn { get; set; }
        public decimal dt_Uamount { get; set; }
        public int dt_Upoint { get; set; }
        public int dt_Uexp { get; set; }
        public int dt_Ustatus { get; set; }
        public DateTime dt_Ureg_time { get; set; }
        public string dt_Ureg_ip { get; set; }
        public string dt_Uclient_id { get; set; }
        public DateTime dt_Ulogin_time { get; set; }
        public int dt_Urole_id { get; set; }
        public string dt_Uid_card { get; set; }
        public string dt_Unation { get; set; }
        public int dt_Umarital_status { get; set; }
        public string dt_Unew_class_id { get; set; }
        public string dt_Uadministration_rank_id { get; set; }
        public string dt_Umilitary_rank_id { get; set; }
        public string dt_Upolice_rank_id { get; set; }
        public int dt_Uchildren_info { get; set; }
        public int dt_Uonly_child_award { get; set; }
        public string dt_Upassport_info_id { get; set; }
        public string dt_Uentitled_group_id { get; set; }
        public string dt_Uincome_source_id { get; set; }
        public string dt_Unative_place { get; set; }
        public string dt_Ulive_place { get; set; }
        public string dt_Ugraduate_school { get; set; }
        public DateTime dt_Ugraduate_time { get; set; }
        public string dt_Ueducation_info_id { get; set; }
        public int dt_Udegree_info { get; set; }
        public DateTime dt_Ujoin_party_time { get; set; }
        public string dt_Uparty_membership { get; set; }
        public string dt_Ufirst_branch { get; set; }
        public string dt_Uparty_job { get; set; }
        public string dt_Unow_organiz { get; set; }
        public string dt_Ugroup_type_id { get; set; }

        public string dt_Ucommunity_info { get; set; }
        public string dt_Ugroup_live { get; set; }

        public string dt_Ureward_punishment_id { get; set; }
        public string dt_Uformer_company_id { get; set; }
        public string dt_Unow_company_id { get; set; }
        public int dt_Uis_badly_off { get; set; }
        public int dt_Uis_organiz_identity { get; set; }
        public string dt_Ufinancial_situation_id { get; set; }
        public string dt_Uhealthy_condition_id { get; set; }
        public string dt_Ubadly_off_reason_id { get; set; }
        public string dt_Ubadly_off_describe { get; set; }
        public string dt_Uenjoy_help_id { get; set; }
        public string dt_Ufloat_commie_id { get; set; }
        public string dt_Uwechat { get; set; }
        public int dt_Ufloat_commie { get; set; }
        public string dt_Ucom_name { get; set; }
        public string dt_Ucom_type_id { get; set; }

    }
}
