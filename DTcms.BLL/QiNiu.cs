using DTcms.DAL;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class QiNiu
    {
        private QiNiuHelper dal = new QiNiuHelper();

        /// <summary>
        /// 删除七牛云文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Boolean DeleteQiNiuFile(string fileName) {
            return dal.DeleteQiNiuFile(fileName);
        }

        /// <summary>
        /// 获取七牛云配置信息
        /// </summary>
        /// <returns></returns>
        public P_QiNiuInfo GetQiNiuConfigInfo() {
            return dal.GetQiNiuConfigInfo();
        }

        /// <summary>
        /// 七牛云Token
        /// </summary>
        /// <returns></returns>
        public string GetQiNiuToken() {
            return dal.GetQiNiuToken();
        }

        /// <summary>
        /// 更改七牛配置信息
        /// </summary>
        /// <param name="rootUrl"></param>
        /// <param name="score"></param>
        /// <param name="ak"></param>
        /// <param name="sk"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean UpdateQiNiuConfigInfo(string rootUrl, string score, string ak, string sk, int userId) {
            return dal.UpdateQiNiuConfigInfo(rootUrl, score, ak, sk, userId);
        }

    }
}
