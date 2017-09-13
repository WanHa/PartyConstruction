using DTcms.DAL;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class UserLogin
    {
        private UserLogic dal = new UserLogic();

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public UserLoginModel Register(string account, string password, string card, int groupId, string clientId) {
            return dal.Register(account, password, card, groupId, clientId);
        }

        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public Tuple<int, UserLoginModel> Login(string account, string password, string clientId) {
            return dal.Login(account, password, clientId);
        }

        /// <summary>
        /// 获取党组织列表
        /// </summary>
        /// <returns></returns>
        public List<PartyOrganizationModel> GetPartyOrganizationList(string mobile) {
            return dal.GetPartyOrganizationList(mobile);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="account">账号（电话号）</param>
        /// <returns></returns>
        public Boolean Logout(string account, string clientid) {
            return dal.Logout(account, clientid);
        }

        /// <summary>
        /// 获取账号状态
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public int GetAccountStatus(string account) {
            return dal.GetAccountStatus(account);
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TokenResult getToken(string userId)
        {
            return dal.getToken(userId);
        }
    }
}
