using System;
using System.Data;
using System.Collections.Generic;
using DTcms.Common;
using DTcms.Model;

namespace DTcms.BLL
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public partial class users
    {
        private readonly Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(); //获得站点配置信息
        private readonly DAL.users dal;
        public users()
        {
            dal = new DAL.users(siteConfig.sysdatabaseprefix);
        }

        #region 基本方法===================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        public bool Exists(string user_name)
        {
            return dal.Exists(user_name);
        }

        /// <summary>
        /// 检查同一IP注册间隔(小时)内是否存在
        /// </summary>
        public bool Exists(string reg_ip, int regctrl)
        {
            return dal.Exists(reg_ip, regctrl);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.users model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.users model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.users GetModel(int id)
        {
            return dal.GetModel(id);
        }

        /// <summary>
        /// 根据用户名密码返回一个实体
        /// </summary>
        /// <param name="user_name">用户名(明文)</param>
        /// <param name="password">密码</param>
        /// <param name="emaillogin">是否允许邮箱做为登录</param>
        /// <param name="mobilelogin">是否允许手机做为登录</param>
        /// <param name="is_encrypt">是否需要加密密码</param>
        /// <returns></returns>
        public Model.users GetModel(string user_name, string password, int emaillogin, int mobilelogin, bool is_encrypt)
        {
            //检查一下是否需要加密
            if (is_encrypt)
            {
                //先取得该用户的随机密钥
                string salt = dal.GetSalt(user_name);
                if (string.IsNullOrEmpty(salt))
                {
                    return null;
                }
                //把明文进行加密重新赋值
                password = DESEncrypt.Encrypt(password, salt);
            }
            return dal.GetModel(user_name, password, emaillogin, mobilelogin);
        }

        /// <summary>
        /// 根据用户名返回一个实体
        /// </summary>
        public Model.users GetModel(string user_name)
        {
            return dal.GetModel(user_name);
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return dal.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }
        #endregion


        #region 扩展方法===================================
        /// <summary>
        /// 检查Email是否存在
        /// </summary>
        public bool ExistsEmail(string email)
        {
            return dal.ExistsEmail(email);
        }

        /// <summary>
        /// 检查手机号码是否存在
        /// </summary>
        public bool ExistsMobile(string mobile)
        {
            return dal.ExistsMobile(mobile);
        }

        /// <summary>
        /// 返回一个随机用户名
        /// </summary>
        public string GetRandomName(int length)
        {
            string temp = Utils.Number(length, true);
            if (Exists(temp))
            {
                return GetRandomName(length);
            }
            return temp;
        }

        /// <summary>
        /// 根据用户名取得Salt
        /// </summary>
        public string GetSalt(string user_name)
        {
            return dal.GetSalt(user_name);
        }

        /// <summary>
        /// 修改一列数据
        /// </summary>
        public int UpdateField(int id, string strValue)
        {
            return dal.UpdateField(id, strValue);
        }

        /// <summary>
        /// 用户升级
        /// </summary>
        public bool Upgrade(int id)
        {
            if (!Exists(id))
            {
                return false;
            }
            Model.users model = GetModel(id);
            Model.user_groups groupModel = new user_groups().GetUpgrade(int.Parse(model.group_id), model.exp);
            if (groupModel == null)
            {
                return false;
            }
            int result = UpdateField(id, "group_id=" + groupModel.id);
            if (result > 0)
            {
                //增加积分
                if (groupModel.point > 0)
                {
                    new BLL.user_point_log().Add(model.id, model.user_name, groupModel.point, "升级获得积分", true);
                }
                //增加金额
                if (groupModel.amount > 0)
                {
                    new BLL.user_amount_log().Add(model.id, model.user_name, groupModel.amount, "升级赠送金额");
                }
            }
            return true;
        }
        #endregion

        #region  保存分表信息===============================
        //添加党员奖惩
        public bool addreward(u_reward_punishment model)
        {
            return dal.addreward(model);
        }

        //添加现工作单位信息
        public bool addcompany(u_company_type model)
        {
            return dal.addcompany(model);
        }
        //添加原工作单位信息
        public bool addOrigCompany(u_company_type model)
        {
            return dal.addOrigCompany(model);
        }
        //添加享受帮扶情况信息
        public bool addEnjoyHelp(u_enjoy_help model)
        {
            return dal.addEnjoyHelp(model);
        }

        //添加流动党员信息
        public bool addFloatCommie(u_float_commie model)
        {
            return dal.addFloatCommie(model);
        }
        #endregion

        #region  修改分表信息==========================
        /// <summary>
        /// 更改奖惩信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updatereward(u_reward_punishment model)
        {
            return dal.updatereward(model);
        }

        /// <summary>
        /// 更改现工作单位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updatecompany(u_company_type model)
        {
            return dal.updatecompany(model);
        }

        /// <summary>
        /// 更改原工作单位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateOrigCompany(u_company_type model)
        {
            return dal.updateOrigCompany(model);
        }

        /// <summary>
        /// 更改享受帮扶信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateEnjoyHelp(u_enjoy_help model)
        {
            return dal.updateEnjoyHelp(model);
        }

        /// <summary>
        /// 修改流动党员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateFloatCommie(u_float_commie model)
        {
            return dal.updateFloatCommie(model);
        }
        #endregion

        #region 获取分表信息（修改显示）================
        /// <summary>
        /// 获取图片表模型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.P_Image GetImageModel(int id)
        {
            return dal.GetImageModel(id);
        }

        //获取党员奖惩信息
        public Model.u_reward_punishment getrewardpunishment(int id)
        {
            return dal.getrewardpunishment(id);
        }

        //获取原单位信息
        public Model.u_company_type getFormerCompany(int id)
        {
            return dal.getFormerCompany(id);
        }

        //获取现单位信息
        public Model.u_company_type getNowCompany(int id)
        {
            return dal.getNowCompany(id);
        }

        //获取享受帮扶形式
        public Model.u_enjoy_help getEnjoyHelp(int id)
        {
            return dal.getEnjoyHelp(id);
        }

        //获取流动党员信息
        public Model.u_float_commie getFloatCommie(int id)
        {
            return dal.getFloatCommie(id);
        }
        #endregion
    }
}