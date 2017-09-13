using DTcms.BLL;
using DTcms.Common;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Controllers;

namespace Co.Tton.PartyConstruction.WebApi.Controllers
{
    [RoutePrefix("v1/user")]
    public class UserLoginController : ApiControllerBase
    {
        /// <summary>
        /// 用户注册接口
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        [Route("registered"), AcceptVerbs("POST")]
        public HttpResponseMessage RegisteredUser([FromBody]UserRegisteredModel fromBody) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                UserLogin bll = new DTcms.BLL.UserLogin();
                UserLoginModel result = bll.Register(fromBody.account, fromBody.password, fromBody.card, fromBody.groupid, fromBody.clientid);
                if (result != null) {
                    message = RenderMessage(true, "账号注册成功", result, 1);
                }
                else {
                    message = RenderMessage(false, "您输入的信息无法注册账号.");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        [Route("login"), AcceptVerbs("POST")]
        public HttpResponseMessage UserLogin([FromBody] UserLoginFormBody fromBody) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                UserLogin bll = new DTcms.BLL.UserLogin();
                Tuple<int, UserLoginModel> result = bll.Login(fromBody.account, fromBody.password, fromBody.clientid);
                switch (result.Item1)
                {
                    case (int)LoginReturnEnum.登录成功:
                        message = RenderMessage(true, "登录成功", result.Item2, 1);
                        break;
                    case (int)LoginReturnEnum.密码不正确:
                    case (int)LoginReturnEnum.账号不存在:
                        message = RenderMessage(false, "账号或密码不正确");
                        break;
                    case (int)LoginReturnEnum.账号正在审核中:
                        message = RenderMessage(false, "账号正在审核中,无法登录.");
                        break;
                    default:
                        message = RenderMessage(false, "登录失败");
                        break;
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        /// <summary>
        /// 获取党组织列表
        /// </summary>
        /// <returns></returns>
        [Route("organization/list"), AcceptVerbs("GET")]
        public HttpResponseMessage GetPartyOrganizationList([FromUri]string account) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                UserLogin bll = new DTcms.BLL.UserLogin();
                List<PartyOrganizationModel> result = bll.GetPartyOrganizationList(account);
                if (result != null && result.Count > 0)
                {
                    message = RenderListTrueMessage(result, result.Count);
                }
                else
                {
                    message = RenderListFalseMessage();
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }
            return message;
        }

        /// <summary>
        /// 登出(销毁用户登录的个推clientid)
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        [Route("logout"), AcceptVerbs("GET")]
        public HttpResponseMessage Logout([FromUri]string account, [FromUri]string clientid)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                Boolean result = new UserLogin().Logout(account, clientid);
                if (result) {
                    message = RenderMessage(true, "退出成功", "退出成功", 1);
                }
                else {
                    message = RenderMessage(false, "退出失败");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        /// <summary>
        /// 获取账号状态
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [Route("account/status"),AcceptVerbs("GET")]
        public HttpResponseMessage GetAccountStatus([FromUri]string account) {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                int status = new UserLogin().GetAccountStatus(account);

                if (status > -1) {
                    Dictionary<string, int> data = new Dictionary<string, int>();
                    data.Add("status",status);
                    message = RenderMessage(true,"获取成功", data, 1);
                }
                else {
                    message = RenderMessage(false, "获取失败");
                }

            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="fromBody"></param>
        /// <returns></returns>
        [Route("token"), AcceptVerbs("GET")]
        public HttpResponseMessage GetUserToken([FromUri] string userId)
        {

            HttpResponseMessage message = new HttpResponseMessage();

            try
            {
                TokenResult result = new UserLogin().getToken(userId);
                if (result != null)
                {
                    message = RenderMessage(true, "获取名字，头像，Token成功", result, 1);
                }
                else
                {
                    message = RenderMessage(false, "获取名字，头像，Token失败.");
                }
            }
            catch (Exception ex)
            {
                message = RenderErrorMessage(ex);
            }

            return message;
        }

        
    }
}
