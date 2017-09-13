using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using DTcms.Model.WebApiModel;
using Qiniu.IO.Model;
using Qiniu.RS;
using Qiniu.RS.Model;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class QiNiuHelper
    {
        /// <summary>
        /// 获取七牛文件大小
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public long GetQiNiuFileSize(string fileName)
        {

            P_QiNiuInfo qiNiuInfo = GetQiNiuConfigInfo();
            Mac mac = new Mac(qiNiuInfo.P_AK, qiNiuInfo.P_SK);

            BucketManager bm = new BucketManager(mac);

            StatResult result = bm.Stat(qiNiuInfo.P_Scope, fileName);

            if (result.Code == 200)
            {
                return result.Result.Fsize;
            }
            return 0;

        }

        /// <summary>
        /// 删除多条七牛视频
        /// </summary>
        /// <param name="ds"></param>
        public void DeleteVideos(DataSet ds)
        {

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    DeleteQiNiuFile(item["P_VideoName"].ToString());
                }
            }
        }

        /// <summary>
        /// 删除多条七牛图片
        /// </summary>
        /// <param name="ds"></param>
        public void DeletePics(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    DeleteQiNiuFile(item["P_PictureName"].ToString());
                }
            }
        }

        /// <summary>
        /// 删除七牛云文件
        /// </summary>
        /// <param name="key">文件名称</param>
        /// <returns></returns>
        public bool DeleteQiNiuFile(string key)
        {
            try
            {
                P_QiNiuInfo qiNiuInfo = GetQiNiuConfigInfo();
                Mac mac = new Mac(qiNiuInfo.P_AK, qiNiuInfo.P_SK);

                BucketManager bm = new BucketManager(mac);

                var result = bm.Delete(qiNiuInfo.P_Scope, key);

                //根据状态码判断删除是否成功
                switch (result.RefCode)
                {
                    case 200:
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取七牛云视频时长
        /// </summary>
        /// <param name="VideoUri">视频URL</param>
        /// <returns></returns>
        public double GetVideoLength(string videoUri) {
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri(SyncRepairOrderUrl);

            // Add an Accept header for JSON format.
            // 为JSON格式添加一个Accept报头
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("*/*"));

            var response = client.GetAsync(new Uri(videoUri + "?avinfo")).Result;

            if (response.IsSuccessStatusCode)
            {
                String result = response.Content.ReadAsStringAsync().Result;
                //QiNiuVideoLengthModel videoLength = JsonConvert.DeserializeObject<QiNiuVideoLengthModel>(result);
                QiNiuVideoInfoModel videoInfo = JsonHelper.JSONToObject<QiNiuVideoInfoModel>(result);
                double length = Math.Floor(videoInfo.format.duration);

                return length;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取七牛云配置信息
        /// </summary>
        /// <returns></returns>
        public P_QiNiuInfo GetQiNiuConfigInfo() {

            string sql = String.Format(@"select * from P_QiNiuInfo");

            DataSet ds = DbHelperSQL.Query(sql);

            DataSetToModelHelper<P_QiNiuInfo> helper = new DataSetToModelHelper<P_QiNiuInfo>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) {
                return helper.FillToModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// 根据文件名称获取文件七牛URL
        /// </summary>
        /// <param name="picName"></param>
        /// <returns></returns>
        public string GetQiNiuFileUrl(string fileName) {

            P_QiNiuInfo info = GetQiNiuConfigInfo();

            if (info != null && !String.IsNullOrEmpty(info.P_RootUrl)) {
                return info.P_RootUrl + fileName;
            }

            return null;
        }

        /// <summary>
        /// 获取七牛视频截图图片
        /// </summary>
        /// <param name="videoUrl"></param>
        /// <returns></returns>
        public string GetQiNiuVideoPicUrl(string videoUrl) {
            return videoUrl + "?vframe/jpg/offset/0";
        }

        /// <summary>
        /// 获取七牛云token
        /// </summary>
        /// <returns></returns>
        public string GetQiNiuToken() {

            P_QiNiuInfo info = GetQiNiuConfigInfo();

            if (info != null) {

            Mac mac = new Mac(info.P_AK, info.P_SK);
            Auth auth = new Auth(mac);
            PutPolicy putPolicy = new PutPolicy();

            putPolicy.Scope = info.P_Scope;
            putPolicy.SetExpires(3600);
            putPolicy.InsertOnly = 0;
            return auth.CreateUploadToken(putPolicy.ToJsonString());
            }
            return "";
        }

        /// <summary>
        /// 修改七牛配置信息
        /// </summary>
        /// <param name="rootUrl"></param>
        /// <param name="score"></param>
        /// <param name="ak"></param>
        /// <param name="sk"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean UpdateQiNiuConfigInfo(string rootUrl, string score, string ak, string sk, int userId) {

            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (Exist())
                        {
                            StringBuilder updateSql = new StringBuilder();
                            updateSql.Append("update P_QiNiuInfo set ");
                            updateSql.Append(" P_AK = @P_AK,");
                            updateSql.Append(" P_SK = @P_SK,");
                            updateSql.Append(" P_Scope = @P_Scope,");
                            updateSql.Append(" P_RootUrl = @P_RootUrl,");
                            updateSql.Append(" P_UpdateTime = @P_UpdateTime,");
                            updateSql.Append(" P_UpdateUser = @P_UpdateUser");
                            SqlParameter[] parameter1 = {
                                new SqlParameter("@P_AK",SqlDbType.NVarChar),
                                new SqlParameter("@P_SK",SqlDbType.NVarChar),
                                new SqlParameter("@P_Scope",SqlDbType.NVarChar),
                                new SqlParameter("@P_RootUrl",SqlDbType.NVarChar),
                                new SqlParameter("@P_UpdateTime",SqlDbType.DateTime),
                                new SqlParameter("@P_UpdateUser",SqlDbType.NVarChar),
                            };
                            parameter1[0].Value = ak;
                            parameter1[1].Value = sk;
                            parameter1[2].Value = score;
                            parameter1[3].Value = rootUrl;
                            parameter1[4].Value = DateTime.Now;
                            parameter1[5].Value = userId.ToString();

                            DbHelperSQL.GetSingle(conn, trans, updateSql.ToString(), parameter1); //带事务
                        }
                        else
                        {
                            StringBuilder insertSql = new StringBuilder();
                            insertSql.Append("insert into P_QiNiuInfo (");
                            insertSql.Append("P_Id,P_AK,P_SK,P_RootUrl,P_Scope,P_CreateUser,P_CreateTime,P_Status)");
                            insertSql.Append("VALUES (@P_Id,@P_AK,@P_SK,@P_RootUrl,@P_Scope,@P_CreateUser,@P_CreateTime,@P_Status)");

                            SqlParameter[] parameter = {
                                new SqlParameter("@P_Id",SqlDbType.NVarChar),
                                new SqlParameter("@P_AK",SqlDbType.NVarChar),
                                new SqlParameter("@P_SK",SqlDbType.NVarChar),
                                new SqlParameter("@P_RootUrl",SqlDbType.NVarChar),
                                new SqlParameter("@P_Scope",SqlDbType.NVarChar),
                                new SqlParameter("@P_CreateUser",SqlDbType.NVarChar),
                                new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                                new SqlParameter("@P_Status",SqlDbType.Int)
                            };
                            parameter[0].Value = Guid.NewGuid().ToString();
                            parameter[1].Value = ak;
                            parameter[2].Value = sk;
                            parameter[3].Value = rootUrl;
                            parameter[4].Value = score;
                            parameter[5].Value = userId;
                            parameter[6].Value = DateTime.Now;
                            parameter[7].Value = 0;
                            object obj = DbHelperSQL.GetSingle(conn, trans, insertSql.ToString(), parameter); //带事务
                            
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                   
                }
            }

            return true;
        }

        private Boolean Exist() {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_QiNiuInfo");
            return DbHelperSQL.Exists(strSql.ToString());
        }

    }
}
