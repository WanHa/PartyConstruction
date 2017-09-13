using donet.io.rong.methods;
using donet.io.rong.models;
using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class UploadPicture
    {
        /// <summary>
        /// 上传头像接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AddImage(UploadPictureModel model)
        {
            QiNiuHelper name = new QiNiuHelper();
            string imageurl = name.GetQiNiuFileUrl(model.imagename);
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using(SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //融云
                        string RongAppKey = ConfigHelper.GetAppSettings("RongAppKey");
                        string RongAppSecret = ConfigHelper.GetAppSettings("RongAppSecret");
                        User user = new User(RongAppKey, RongAppSecret);

                        if (ispm(model.userid)==false)
                        {
                           
                            StringBuilder sql = new StringBuilder();
                            sql.Append("insert into P_Image(");
                            sql.Append("P_Id,P_ImageId,P_ImageUrl,P_CreateTime,P_CreateUser,P_Status,P_PictureName,P_ImageType)");
                            sql.Append(" values(");
                            sql.Append("@P_Id,@P_ImageId,@P_ImageUrl,@P_CreateTime,@P_CreateUser,@P_Status,@P_PictureName,@P_ImageType)");
                            sql.Append(";select @@IDENTITY");
                            SqlParameter[] parameters = {
                        new SqlParameter("@P_Id",SqlDbType.NVarChar,36),
                        new SqlParameter("@P_ImageId", SqlDbType.NVarChar, 36),
                        new SqlParameter("@P_ImageUrl",SqlDbType.NVarChar,200),
                        new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                        new SqlParameter("@P_CreateUser",SqlDbType.NVarChar,50),
                        new SqlParameter("@P_Status",SqlDbType.Int,4),
                        new SqlParameter("@P_PictureName", SqlDbType.NVarChar, 100),
                        new SqlParameter("@P_ImageType",SqlDbType.Int,4)
                        };
                            string imgid = Guid.NewGuid().ToString();
                            parameters[0].Value = imgid;
                            parameters[1].Value = model.userid;
                            parameters[2].Value = imageurl;
                            parameters[3].Value = DateTime.Now;
                            parameters[4].Value = model.userid;
                            parameters[5].Value = 0;
                            parameters[6].Value = model.imagename;
                            parameters[7].Value = (int)ImageTypeEnum.头像;
                            object obj = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameters);
                            
                            //刷新融云头像
                            CodeSuccessReslut csr = user.refresh(model.userid.ToString(),null, imageurl);
                        }
                        else
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("update  P_Image set ");
                            strSql.Append("P_ImageUrl=@P_ImageUrl,P_UpdateTime=@P_UpdateTime,P_UpdateUser=@P_UpdateUser,P_PictureName=@P_PictureName ");
                            strSql.Append(" where ");
                            strSql.Append(" P_ImageId=@P_ImageId and P_ImageType=@P_ImageType ");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_UpdateUser", SqlDbType.Int,100),
                                new SqlParameter("@P_ImageId", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ImageType", SqlDbType.Int,100),
                                new SqlParameter("@P_PictureName", SqlDbType.NVarChar,100)
                        };
                            parameters[0].Value = imageurl;
                            parameters[1].Value = DateTime.Now;
                            parameters[2].Value = model.userid;
                            parameters[3].Value = model.userid;
                            parameters[4].Value = (int)ImageTypeEnum.头像;
                            parameters[5].Value = model.imagename;
                            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters); //带事务

                            //刷新融云头像
                            CodeSuccessReslut csr = user.refresh(model.userid.ToString(), null, imageurl);
                        }                      
                        trans.Commit();
                    }
                    catch(Exception e)
                    {
                        trans.Rollback();
                        return "";
                    }
                }
            }
            return imageurl;
        }
        public bool ispm(int userid)
        {
            StringBuilder strsql = new StringBuilder();
            bool ispm = false;
            string sql = "select * from P_Image where P_ImageId ='" + userid + "' and P_ImageType='"+(int)ImageTypeEnum.头像+@"'";
            strsql.Append(sql);
            DataTable da = DbHelperSQL.Query(strsql.ToString()).Tables[0];
            if (da.Rows.Count > 0)
            {
                ispm = true;
            }
            return ispm;

        }
    }
}
