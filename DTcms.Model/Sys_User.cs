using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Model
{
    public partial class Sys_User
    {
        public Sys_User()
        { }
        #region Model
        private string _F_Id;
        private string _F_Account;
        private string _F_RealName;
        private string _F_NickName;
        private string _F_HeadIcon;
        private bool? _F_Gender;
        private DateTime? _F_Birthday;
        private string _F_MobilePhone;
        private string _F_Email;
        private string _F_WeChat;
        private string _F_ManagerId;
        private int? _F_SecurityLevel;
        private string _F_Signature;
        private string _F_OrganizeId;
        private string _F_DepartmentId;
        private string _F_RoleId;
        private string _F_DutyId;
        private bool? _F_IsAdministrator;
        private int? _F_SortCode;
        private bool? _F_DeleteMark;
        private bool? _F_EnabledMark;
        private string _F_Description;
        private DateTime? _F_CreatorTime;
        private string _F_CreatorUserId;
        private DateTime? _F_LastModifyTime;
        private string _F_LastModifyUserId;
        private DateTime? _F_DeleteTime;
        private string _F_DeleteUserId;

        public string F_Id { get => _F_Id; set => _F_Id = value; }
        public string F_Account { get => _F_Account; set => _F_Account = value; }
        public string F_RealName { get => _F_RealName; set => _F_RealName = value; }
        public string F_NickName { get => _F_NickName; set => _F_NickName = value; }
        public string F_HeadIcon { get => _F_HeadIcon; set => _F_HeadIcon = value; }
        public bool? F_Gender { get => _F_Gender; set => _F_Gender = value; }
        public DateTime? F_Birthday { get => _F_Birthday; set => _F_Birthday = value; }
        public string F_MobilePhone { get => _F_MobilePhone; set => _F_MobilePhone = value; }
        public string F_Email { get => _F_Email; set => _F_Email = value; }
        public string F_WeChat { get => _F_WeChat; set => _F_WeChat = value; }
        public string F_ManagerId { get => _F_ManagerId; set => _F_ManagerId = value; }
        public int? F_SecurityLevel { get => _F_SecurityLevel; set => _F_SecurityLevel = value; }
        public string F_Signature { get => _F_Signature; set => _F_Signature = value; }
        public string F_OrganizeId { get => _F_OrganizeId; set => _F_OrganizeId = value; }
        public string F_DepartmentId { get => _F_DepartmentId; set => _F_DepartmentId = value; }
        public string F_RoleId { get => _F_RoleId; set => _F_RoleId = value; }
        public string F_DutyId { get => _F_DutyId; set => _F_DutyId = value; }
        public bool? F_IsAdministrator { get => _F_IsAdministrator; set => _F_IsAdministrator = value; }
        public int? F_SortCode { get => _F_SortCode; set => _F_SortCode = value; }
        public bool? F_DeleteMark { get => _F_DeleteMark; set => _F_DeleteMark = value; }
        public bool? F_EnabledMark { get => _F_EnabledMark; set => _F_EnabledMark = value; }
        public string F_Description { get => _F_Description; set => _F_Description = value; }
        public DateTime? F_CreatorTime { get => _F_CreatorTime; set => _F_CreatorTime = value; }
        public string F_CreatorUserId { get => _F_CreatorUserId; set => _F_CreatorUserId = value; }
        public DateTime? F_LastModifyTime { get => _F_LastModifyTime; set => _F_LastModifyTime = value; }
        public string F_LastModifyUserId { get => _F_LastModifyUserId; set => _F_LastModifyUserId = value; }
        public DateTime? F_DeleteTime { get => _F_DeleteTime; set => _F_DeleteTime = value; }
        public string F_DeleteUserId { get => _F_DeleteUserId; set => _F_DeleteUserId = value; }
        #endregion Model
    }
}
