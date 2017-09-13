using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// 接口返回数据格式
    /// </summary>
    public class ResponseJsonObject
    {
        /// <summary>
        /// 是否获取到数据
        /// </summary>
        public bool issuccess { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 接口返回数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 接口返回数据数量
        /// </summary>
        public int datacount { get; set; }
    }
}