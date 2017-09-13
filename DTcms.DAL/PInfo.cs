using DTcms.Common;
using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class PInfo
    {
        private QiNiuHelper qiniu = new QiNiuHelper();
        /// <summary>
        /// 封面接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Update(CoverModel model)
        {
            string pic = qiniu.GetQiNiuFileUrl(model.pname);
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("select P_ImageUrl from P_Image where P_CreateUser ='" + model.userid + @"' and P_ImageType =20013");
                        DataSet da = DbHelperSQL.Query(sql.ToString());
                        DataSetToModelHelper<CoverModel> helper = new DataSetToModelHelper<CoverModel>();
                        if(da.Tables[0].Rows.Count == 0)//此用户没有封面，封面表、图片表分别插入数据
                        {
                            StringBuilder strsql = new StringBuilder();
                            strsql.Append("insert into P_Cover(");
                            strsql.Append("P_Id,P_UserId,create_time,create_user,status)");
                            strsql.Append("values(");
                            strsql.Append("@P_Id,@P_UserId,@create_time,@create_user,@status)");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_UserId", SqlDbType.NVarChar,50),
                                new SqlParameter("@create_time", SqlDbType.DateTime),
                                new SqlParameter("@create_user", SqlDbType.NVarChar,50),
                                new SqlParameter("@status", SqlDbType.Int),
                            };
                            string partentid = Guid.NewGuid().ToString();
                            parameters[0].Value = partentid;
                            parameters[1].Value = model.userid;
                            parameters[2].Value = DateTime.Now;
                            parameters[3].Value = model.userid;
                            parameters[4].Value = 0;
                            object obj = DbHelperSQL.GetSingle(conn, trans, strsql.ToString(), parameters); //带事务    

                            StringBuilder tsql = new StringBuilder();
                            tsql.Append("insert into P_Image(");
                            tsql.Append("P_Id,P_ImageId,P_ImageUrl,P_CreateTime,P_CreateUser,P_PictureName,P_ImageType,P_Status)");
                            tsql.Append("values(");
                            tsql.Append("@P_Id,@P_ImageId,@P_ImageUrl,@P_CreateTime,@P_CreateUser,@P_PictureName,@P_ImageType,@P_Status)");
                            SqlParameter[] paras = {
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,36),
                                new SqlParameter("@P_ImageId", SqlDbType.NVarChar,36),
                                new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar,200),
                                new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_PictureName", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_ImageType", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Status", SqlDbType.Int),
                            };
                            paras[0].Value = Guid.NewGuid().ToString();
                            paras[1].Value = partentid;
                            paras[2].Value = pic;
                            paras[3].Value = DateTime.Now;
                            paras[4].Value = model.userid;
                            paras[5].Value = model.pname;
                            paras[6].Value = 20013;
                            paras[7].Value = 0;
                            object obj2 = DbHelperSQL.GetSingle(conn, trans, tsql.ToString(), paras); //带事务
                            trans.Commit();
                        }
                        if(da.Tables[0].Rows.Count != 0)//修改封面图片url
                        {
                            StringBuilder sr = new StringBuilder();
                            //string pic = qiniu.GetQiNiuFileUrl(model.pname);
                            sr.Append("Update P_Image set P_ImageUrl =@P_ImageUrl,P_PictureName=@P_PictureName");
                            sr.Append(" where P_CreateUser = @P_CreateUser and P_ImageType =@P_ImageType");
                            SqlParameter[] parameters = {
                                new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar,200),
                                new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_ImageType",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_PictureName",SqlDbType.NVarChar,100)
                            };
                            parameters[0].Value = pic;
                            parameters[1].Value = model.userid;
                            parameters[2].Value = 20013;
                            parameters[3].Value = model.pname;
                            object obj = DbHelperSQL.GetSingle(conn, trans, sr.ToString(), parameters); //带事务      
                            trans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return "";
                    }
                }
            }
            return pic;
        }
    }
    public class CoverModel
    {
        public string userid { get; set; }
        public string pname { get; set; }
    }
}
