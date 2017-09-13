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
    public class RefushGroupCover
    {
        public Boolean RefushCover(int type,string groupId,string imageName)
        {
            Boolean issuccess = false;
            string imageUrl = new QiNiuHelper().GetQiNiuFileUrl(imageName);
            if (type==0)
            {
                using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            StringBuilder querySql = new StringBuilder();
                            querySql.Append(" select * from P_Image " +
                                                            " where P_Image.P_Status=0" +
                                                            " and P_Image.P_ImageId=@P_ImageId" +
                                                            " and P_Image.P_ImageType=" + (int)ImageTypeEnum.支部封面);
                            SqlParameter[] queryPar = {
                            new SqlParameter("@P_ImageId",SqlDbType.NVarChar,36)
                        };
                            queryPar[0].Value = groupId;

                            DataSet ds = DbHelperSQL.Query(querySql.ToString(), queryPar);

                            if (ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
                            {
                                StringBuilder sql = new StringBuilder();
                                string id = Guid.NewGuid().ToString();
                                sql.Append("INSERT P_Image (P_Id, P_ImageId, P_ImageUrl, P_PictureName, P_CreateTime, P_Status, P_ImageType)" +
                                                                   " VALUES (@P_Id, @P_ImageId, @P_ImageUrl, @P_PictureName, @P_CreateTime, @P_Status,@P_ImageType)");
                                SqlParameter[] parameters = {
                            new SqlParameter("@P_Id",SqlDbType.NVarChar,36),
                            new SqlParameter("@P_ImageId",SqlDbType.NVarChar,36),
                            new SqlParameter("@P_ImageUrl",SqlDbType.NVarChar,200),
                            new SqlParameter("@P_PictureName",SqlDbType.NVarChar,100),
                            new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                            new SqlParameter("@P_Status",SqlDbType.Int),
                            new SqlParameter("@P_ImageType",SqlDbType.Int)
                            };

                                parameters[0].Value = id;
                                parameters[1].Value = groupId;
                                parameters[2].Value = imageUrl;
                                parameters[3].Value = imageName;
                                parameters[4].Value = DateTime.Now;
                                parameters[5].Value = 0;
                                parameters[6].Value = (int)ImageTypeEnum.支部封面;
                                DbHelperSQL.ExecuteSql(conn, trans, sql.ToString(), parameters);
                                issuccess = true;
                                trans.Commit();
                            }
                            else
                            {
                                StringBuilder sql = new StringBuilder();
                                sql.Append("update P_Image " +
                                                    " set P_Image.P_ImageUrl = @P_ImageUrl ," +
                                                    " P_Image.P_PictureName = @P_PictureName" +
                                                    " where P_Image.P_ImageId =@P_ImageId " +
                                                    " and P_Image.P_ImageType=" + (int)ImageTypeEnum.支部封面 +
                                                    " and P_Image.P_Status=0");
                                SqlParameter[] parameter = {
                            new SqlParameter("@P_ImageUrl",SqlDbType.NVarChar,200),
                            new SqlParameter("@P_PictureName",SqlDbType.NVarChar,100),
                            new SqlParameter("@P_ImageId",SqlDbType.NVarChar,36)
                            };
                                parameter[0].Value = imageUrl;
                                parameter[1].Value = imageName;
                                parameter[2].Value = groupId;
                                DbHelperSQL.ExecuteSql(conn, trans, sql.ToString(), parameter);
                                issuccess = true;
                                trans.Commit();
                            }

                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            else if (type ==1)//论坛
            {
                using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("select P_ImageUrl from P_Image where P_ImageId='" + groupId + "'");
                            str.Append(" and P_Status=0 and P_ImageType='20014'");
                            DataSet ds = DbHelperSQL.Query(str.ToString());
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                StringBuilder sql = new StringBuilder();
                                sql.Append("update P_Image " +
                                                " set P_Image.P_ImageUrl = @P_ImageUrl ," +
                                                " P_Image.P_PictureName = @P_PictureName" +
                                                " where P_Image.P_ImageId =@P_ImageId " +
                                                " and P_Image.P_ImageType=" + (int)ImageTypeEnum.论坛封面 +
                                                " and P_Image.P_Status=0");
                                SqlParameter[] parameter = {
                                    new SqlParameter("@P_ImageUrl",SqlDbType.NVarChar,200),
                                    new SqlParameter("@P_PictureName",SqlDbType.NVarChar,100),
                                    new SqlParameter("@P_ImageId",SqlDbType.NVarChar,36)
                                };
                                parameter[0].Value = imageUrl;
                                parameter[1].Value = imageName;
                                parameter[2].Value = groupId;
                                DbHelperSQL.ExecuteSql(conn, trans, sql.ToString(), parameter);
                                issuccess = true;
                                trans.Commit();
                            }
                            else if (ds.Tables[0].Rows.Count == 0)
                            {
                                StringBuilder sql = new StringBuilder();
                                string id = Guid.NewGuid().ToString();
                                sql.Append("INSERT P_Image (P_Id, P_ImageId, P_ImageUrl, P_PictureName, P_CreateTime, P_Status, P_ImageType)" +
                                                                   " VALUES (@P_Id, @P_ImageId, @P_ImageUrl, @P_PictureName, @P_CreateTime, @P_Status,@P_ImageType)");
                                SqlParameter[] parameters = {
                                    new SqlParameter("@P_Id",SqlDbType.NVarChar,36),
                                    new SqlParameter("@P_ImageId",SqlDbType.NVarChar,36),
                                    new SqlParameter("@P_ImageUrl",SqlDbType.NVarChar,200),
                                    new SqlParameter("@P_PictureName",SqlDbType.NVarChar,100),
                                    new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                                    new SqlParameter("@P_Status",SqlDbType.Int),
                                    new SqlParameter("@P_ImageType",SqlDbType.Int)
                                };

                                parameters[0].Value = id;
                                parameters[1].Value = groupId;
                                parameters[2].Value = imageUrl;
                                parameters[3].Value = imageName;
                                parameters[4].Value = DateTime.Now;
                                parameters[5].Value = 0;
                                parameters[6].Value = (int)ImageTypeEnum.论坛封面;
                                DbHelperSQL.ExecuteSql(conn, trans, sql.ToString(), parameters);
                                issuccess = true;
                                trans.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            return issuccess;
        }
    }
}
