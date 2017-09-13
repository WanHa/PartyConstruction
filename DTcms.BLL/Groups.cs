using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class Groups
    {
        DAL.Organizations orgazitations = new DAL.Organizations();

        /// <summary>
        /// 保存组织表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int addGroup(Model.user_groups model)
        {
            return orgazitations.addGroup(model);
        }

        /// <summary>
        /// 插入奖惩情况信息表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool addRewardsAndPunishment(Model.P_RewardsAndPunishment model)
        {
            return orgazitations.addRewardsAndPunishment(model);
        }
        /// <summary>
        /// 插入下辖组织信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="exists"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string setSubordinateGroupInfoId(string id, int exists, int count, string info)
        {
            return orgazitations.setSubordinateGroupInfoId(id, exists, count, info);
        }
        /// <summary>
        /// 插入单位信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comname"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public string UnitInfo(string id,string comname,string value,int count,int service)
        {
            return orgazitations.UnitInfo(id,comname,value,count, service);
        }
        /// <summary>
        /// 查询父级id
        /// </summary>
        /// <param name="partyname"></param>
        /// <returns></returns>
        public int? SearchParty(string partyname)
        {
            return orgazitations.SearchParty(partyname);
        }
        /// <summary>
        /// 获取单位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.u_company_type unitinfo(int id)
        {
            return orgazitations.unitinfo(id);
        }
        /// <summary>
        /// 获取奖惩信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.P_RewardsAndPunishment getRewardsAndPunishment(int id)
        {
            return orgazitations.getRewardsAndPunishment(id);
        }
        /// <summary>
        /// 获取组织信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.user_groups getGroups(int id)
        {
            return orgazitations.getGroups(id);
        }
        #region 获取下辖组织情况
        public string getExists(int id)
        {
            return orgazitations.getExists(id);
        }

        public string getCount(int id)
        {
            return orgazitations.getCount(id);
        }

        public string getInfo(int id)
        {
            return orgazitations.getInfo(id);
        }
        #endregion

        /// <summary>
        /// 获取领导班子信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.u_lead_info leadinfo(int id)
        {
            return orgazitations.leadinfo(id);
        }
        /// <summary>
        /// 更改下辖党组织信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="exists"></param>
        /// <param name="count"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool subordinate(string id, int exists, int count, string info)
        {
            return orgazitations.subordinate(id, exists, count, info);
        }
        /// <summary>
        /// 更改单位信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comname"></param>
        /// <param name="sort"></param>
        /// <param name="count"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public bool changeunit(string id, string comname, string sort, int count, int service)
        {
            return orgazitations.changeunit(id, comname, sort, count, service);
        }
        /// <summary>
        /// 更改奖惩信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean changeRewardsAndPunishment(Model.P_RewardsAndPunishment model)
        {
            return orgazitations.changeRewardsAndPunishment(model);
        }
        /// <summary>
        /// 更改领导班子成员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean changeInfo(Model.u_lead_info model)
        {
            return orgazitations.changeInfo(model);
        }

        /// <summary>
        /// 更改党组织表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateUsersGroup(Model.user_groups model)
        {
            return orgazitations.updateUsersGroup(model);
        }
        public bool addUser(int managerid,int mid)
        {
            return orgazitations.addUser(managerid,mid);
        }
    }
}
