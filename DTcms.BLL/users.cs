using System;
using System.Data;
using System.Collections.Generic;
using DTcms.Common;
using DTcms.Model;

namespace DTcms.BLL
{
    /// <summary>
    /// �û���Ϣ
    /// </summary>
    public partial class users
    {
        private readonly Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(); //���վ��������Ϣ
        private readonly DAL.users dal;
        public users()
        {
            dal = new DAL.users(siteConfig.sysdatabaseprefix);
        }

        #region ��������===================================
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// ����û����Ƿ����
        /// </summary>
        public bool Exists(string user_name)
        {
            return dal.Exists(user_name);
        }

        /// <summary>
        /// ���ͬһIPע����(Сʱ)���Ƿ����
        /// </summary>
        public bool Exists(string reg_ip, int regctrl)
        {
            return dal.Exists(reg_ip, regctrl);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(Model.users model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public bool Update(Model.users model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.users GetModel(int id)
        {
            return dal.GetModel(id);
        }

        /// <summary>
        /// �����û������뷵��һ��ʵ��
        /// </summary>
        /// <param name="user_name">�û���(����)</param>
        /// <param name="password">����</param>
        /// <param name="emaillogin">�Ƿ�����������Ϊ��¼</param>
        /// <param name="mobilelogin">�Ƿ������ֻ���Ϊ��¼</param>
        /// <param name="is_encrypt">�Ƿ���Ҫ��������</param>
        /// <returns></returns>
        public Model.users GetModel(string user_name, string password, int emaillogin, int mobilelogin, bool is_encrypt)
        {
            //���һ���Ƿ���Ҫ����
            if (is_encrypt)
            {
                //��ȡ�ø��û��������Կ
                string salt = dal.GetSalt(user_name);
                if (string.IsNullOrEmpty(salt))
                {
                    return null;
                }
                //�����Ľ��м������¸�ֵ
                password = DESEncrypt.Encrypt(password, salt);
            }
            return dal.GetModel(user_name, password, emaillogin, mobilelogin);
        }

        /// <summary>
        /// �����û�������һ��ʵ��
        /// </summary>
        public Model.users GetModel(string user_name)
        {
            return dal.GetModel(user_name);
        }

        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// ��ò�ѯ��ҳ����
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return dal.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }
        #endregion


        #region ��չ����===================================
        /// <summary>
        /// ���Email�Ƿ����
        /// </summary>
        public bool ExistsEmail(string email)
        {
            return dal.ExistsEmail(email);
        }

        /// <summary>
        /// ����ֻ������Ƿ����
        /// </summary>
        public bool ExistsMobile(string mobile)
        {
            return dal.ExistsMobile(mobile);
        }

        /// <summary>
        /// ����һ������û���
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
        /// �����û���ȡ��Salt
        /// </summary>
        public string GetSalt(string user_name)
        {
            return dal.GetSalt(user_name);
        }

        /// <summary>
        /// �޸�һ������
        /// </summary>
        public int UpdateField(int id, string strValue)
        {
            return dal.UpdateField(id, strValue);
        }

        /// <summary>
        /// �û�����
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
                //���ӻ���
                if (groupModel.point > 0)
                {
                    new BLL.user_point_log().Add(model.id, model.user_name, groupModel.point, "������û���", true);
                }
                //���ӽ��
                if (groupModel.amount > 0)
                {
                    new BLL.user_amount_log().Add(model.id, model.user_name, groupModel.amount, "�������ͽ��");
                }
            }
            return true;
        }
        #endregion

        #region  ����ֱ���Ϣ===============================
        //��ӵ�Ա����
        public bool addreward(u_reward_punishment model)
        {
            return dal.addreward(model);
        }

        //����ֹ�����λ��Ϣ
        public bool addcompany(u_company_type model)
        {
            return dal.addcompany(model);
        }
        //���ԭ������λ��Ϣ
        public bool addOrigCompany(u_company_type model)
        {
            return dal.addOrigCompany(model);
        }
        //������ܰ�������Ϣ
        public bool addEnjoyHelp(u_enjoy_help model)
        {
            return dal.addEnjoyHelp(model);
        }

        //���������Ա��Ϣ
        public bool addFloatCommie(u_float_commie model)
        {
            return dal.addFloatCommie(model);
        }
        #endregion

        #region  �޸ķֱ���Ϣ==========================
        /// <summary>
        /// ���Ľ�����Ϣ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updatereward(u_reward_punishment model)
        {
            return dal.updatereward(model);
        }

        /// <summary>
        /// �����ֹ�����λ��Ϣ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updatecompany(u_company_type model)
        {
            return dal.updatecompany(model);
        }

        /// <summary>
        /// ����ԭ������λ��Ϣ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateOrigCompany(u_company_type model)
        {
            return dal.updateOrigCompany(model);
        }

        /// <summary>
        /// �������ܰ����Ϣ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateEnjoyHelp(u_enjoy_help model)
        {
            return dal.updateEnjoyHelp(model);
        }

        /// <summary>
        /// �޸�������Ա��Ϣ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool updateFloatCommie(u_float_commie model)
        {
            return dal.updateFloatCommie(model);
        }
        #endregion

        #region ��ȡ�ֱ���Ϣ���޸���ʾ��================
        /// <summary>
        /// ��ȡͼƬ��ģ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.P_Image GetImageModel(int id)
        {
            return dal.GetImageModel(id);
        }

        //��ȡ��Ա������Ϣ
        public Model.u_reward_punishment getrewardpunishment(int id)
        {
            return dal.getrewardpunishment(id);
        }

        //��ȡԭ��λ��Ϣ
        public Model.u_company_type getFormerCompany(int id)
        {
            return dal.getFormerCompany(id);
        }

        //��ȡ�ֵ�λ��Ϣ
        public Model.u_company_type getNowCompany(int id)
        {
            return dal.getNowCompany(id);
        }

        //��ȡ���ܰ����ʽ
        public Model.u_enjoy_help getEnjoyHelp(int id)
        {
            return dal.getEnjoyHelp(id);
        }

        //��ȡ������Ա��Ϣ
        public Model.u_float_commie getFloatCommie(int id)
        {
            return dal.getFloatCommie(id);
        }
        #endregion
    }
}