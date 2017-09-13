using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using System.Text;

namespace WebApi.Controllers
{

    public class ApiControllerBase : ApiController
    {
        private const string JSONMEDIATYPE = "application/json";

        /// <summary>
        /// 出现异常返回方法
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected HttpResponseMessage RenderErrorMessage(Exception ex)
        {
            HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.InternalServerError);
            ResponseJsonObject obj = new ResponseJsonObject();
            obj.issuccess = false;
            obj.message = ex.Message;
            obj.data = ex;
            string jsonText = JsonConvert.SerializeObject(obj);
            HttpContent content = new StringContent(jsonText, Encoding.UTF8, JSONMEDIATYPE);
            message.Content = content;
            return message;
        }

        /// <summary>
        /// 获取数据成功返回方法
        /// </summary>
        /// <param name="status"></param>
        /// <param name="msgText"></param>
        /// <param name="body"></param>
        /// <param name="dataCount"></param>
        /// <returns></returns>
        protected HttpResponseMessage RenderMessage(bool status, string msgText, object body, int dataCount)
        {
            HttpResponseMessage message = default(HttpResponseMessage);
            ResponseJsonObject jsonObj = new ResponseJsonObject()
            {
                issuccess = status,
                message = msgText,
                data = body,
                datacount = dataCount
            };
            message = Request.CreateResponse(HttpStatusCode.OK);
            string jsonText = JsonConvert.SerializeObject(jsonObj);
            HttpContent content = new StringContent(jsonText, Encoding.UTF8, JSONMEDIATYPE);
            message.Content = content;
            return message;
        }

        protected HttpResponseMessage RenderMessage(bool status, string msgText)
        {
            HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                ResponseJsonObject obj = new ResponseJsonObject()
                {
                    issuccess = status,
                    message = msgText,
                    datacount = 0
                };
                string jsonText = JsonConvert.SerializeObject(obj);
                HttpContent content = new StringContent(jsonText, Encoding.UTF8, JSONMEDIATYPE);
                //HttpContent content = JsonHelper.StringContentFrom(obj); // 2016-7-15 lgx 修改
                message.Content = content;
            }
            catch (Exception)
            {
                throw;
            }
            return message;
        }

        /// <summary>
        /// 获取列表数据，无数据的HttpResponseMessage返回方法
        /// </summary>
        /// <returns></returns>
        protected HttpResponseMessage RenderListFalseMessage() {
            HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                ResponseJsonObject obj = new ResponseJsonObject()
                {
                    issuccess = false,
                    message = "未能获取到列表数据",
                    datacount = 0
                };
                string jsonText = JsonConvert.SerializeObject(obj);
                HttpContent content = new StringContent(jsonText, Encoding.UTF8, JSONMEDIATYPE);
                //HttpContent content = JsonHelper.StringContentFrom(obj); // 2016-7-15 lgx 修改
                message.Content = content;
            }
            catch (Exception)
            {
                throw;
            }
            return message;
        }

        /// <summary>
        /// 获取列表数据，有数据的HttpResponseMessage返回方法
        /// </summary>
        /// <param name="body">列表数据</param>
        /// <param name="dataCount">数据数量</param>
        /// <returns></returns>
        protected HttpResponseMessage RenderListTrueMessage(object body, int dataCount) {

            HttpResponseMessage message = default(HttpResponseMessage);
            ResponseJsonObject jsonObj = new ResponseJsonObject()
            {
                issuccess = true,
                message = "获取列表数据成功",
                data = body,
                datacount = dataCount
            };
            message = Request.CreateResponse(HttpStatusCode.OK);
            string jsonText = JsonConvert.SerializeObject(jsonObj);
            HttpContent content = new StringContent(jsonText, Encoding.UTF8, JSONMEDIATYPE);
            message.Content = content;
            return message;
        }

        /// <summary>
        /// 获取详情数据,无数据时返回HttpResponseMessage方法
        /// </summary>
        /// <returns></returns>
        protected HttpResponseMessage RenderEntityFalseMessage() {
            HttpResponseMessage message = Request.CreateResponse(HttpStatusCode.OK);
            try
            {
                ResponseJsonObject obj = new ResponseJsonObject()
                {
                    issuccess = false,
                    message = "未能获取到数据",
                    datacount = 0
                };
                string jsonText = JsonConvert.SerializeObject(obj);
                HttpContent content = new StringContent(jsonText, Encoding.UTF8, JSONMEDIATYPE);
                //HttpContent content = JsonHelper.StringContentFrom(obj); // 2016-7-15 lgx 修改
                message.Content = content;
            }
            catch (Exception)
            {
                throw;
            }
            return message;
        }


    }
}
