using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using DTcms.Model.WebApiModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class Notify
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean AddNotify(Model.ztree model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append("insert into  dt_article(");
                        str.Append("channel_id,category_id,title,content,add_time,user_id) ");
                        str.Append("values(");
                        str.Append("@channel_id,@category_id,@title,@content,@add_time,@user_id) ");
                        str.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                             new SqlParameter("@channel_id",SqlDbType.Int,4),
                             new SqlParameter("@category_id", SqlDbType.Int,4),
                             new SqlParameter("@title", SqlDbType.NVarChar,100),
                             new SqlParameter("@content", SqlDbType.NText),
                             new SqlParameter("@add_time",SqlDbType.DateTime),
                             new SqlParameter("@user_id",SqlDbType.Int,4)
                        };
                        parameters[0].Value = 20;
                        parameters[1].Value = model.category_id;
                        parameters[2].Value = model.title;
                        parameters[3].Value = model.content;
                        parameters[4].Value = DateTime.Now;
                        parameters[5].Value = model.createuser;
                        object obj = DbHelperSQL.GetSingle(conn, trans, str.ToString(), parameters);

                        QiNiuHelper name = new QiNiuHelper();
                        string imagename = name.GetQiNiuFileUrl(model.img_url);
                        StringBuilder image = new StringBuilder();
                        image.Append("insert into P_Image(");
                        image.Append("P_Id,P_ImageId,P_ImageUrl,P_CreateTime,P_CreateUser,P_Status,P_ImageType,P_PictureName)");
                        image.Append(" values(");
                        image.Append("@P_Id,@P_ImageId,@P_ImageUrl,@P_CreateTime,@P_CreateUser,@P_Status,@P_ImageType,@P_PictureName)");
                        image.Append(";select @@IDENTITY");
                        SqlParameter[] parameters2 = {
                            new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_ImageId", SqlDbType.NVarChar,50),
                            new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar,200),
                            new SqlParameter("@P_CreateTime", SqlDbType.DateTime),
                            new SqlParameter("@P_CreateUser",SqlDbType.NVarChar,50),
                            new SqlParameter("@P_Status", SqlDbType.Int,4),
                            new SqlParameter("@P_ImageType",SqlDbType.NVarChar,100),
                            new SqlParameter("@P_PictureName",SqlDbType.NVarChar,100)
                            };
                        string imgid = Guid.NewGuid().ToString();
                        parameters2[0].Value = imgid;
                        parameters2[1].Value = obj;
                        parameters2[2].Value = imagename;
                        parameters2[3].Value = DateTime.Now;
                        parameters2[4].Value = model.createuser;
                        parameters2[5].Value = 0;
                        parameters2[6].Value = (int)ImageTypeEnum.党委通知;
                        parameters2[7].Value = model.img_url;
                        object obj2 = DbHelperSQL.GetSingle(conn, trans, image.ToString(), parameters2); //带事务

                        List<string> userids = new List<string>();
                        if(model.userid != null && model.userid.Count > 0)
                        {
                            string arry = string.Empty;
                            for (int i = 0; i < model.userid.Count; i++)
                            {
                                if (i == model.userid.Count - 1)
                                {
                                    arry += model.userid[i];
                                }
                                else
                                {
                                    arry += model.userid[i] + ",";
                                }
                            }

                            StringBuilder ss = new StringBuilder();
                            ss.Append("select id from dt_users where id in(" + arry + ")");
                            DataSet ds = DbHelperSQL.Query(ss.ToString());
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                StringBuilder sql = new StringBuilder();
                                sql.Append("insert into P_PartyCommitteeNotify(");
                                sql.Append("P_Id,P_Relation,P_GroupId,P_ToUser,P_CreateTime,P_CreateUser,P_Status) ");
                                sql.Append("values(");
                                sql.Append("@P_Id,@P_Relation,@P_GroupId,@P_ToUser,@P_CreateTime,@P_CreateUser,@P_Status)");
                                sql.Append(";select @@IDENTITY");
                                SqlParameter[] parameter3 = {
                                   new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                                   new SqlParameter("@P_Relation",SqlDbType.NVarChar,50),
                                   new SqlParameter("@P_GroupId",SqlDbType.NVarChar,50),
                                   new SqlParameter("@P_ToUser",SqlDbType.NVarChar,50),
                                   new SqlParameter("@P_CreateTime",SqlDbType.DateTime),
                                   new SqlParameter("@P_CreateUser",SqlDbType.NVarChar,50),
                                   new SqlParameter("@P_Status",SqlDbType.NVarChar,50)
                                };
                                string notid = Guid.NewGuid().ToString();
                                parameter3[0].Value = notid;
                                parameter3[1].Value = obj;
                                parameter3[2].Value = model.groupid;
                                parameter3[3].Value = item[0];
                                parameter3[4].Value = DateTime.Now;
                                parameter3[5].Value = model.createuser;
                                parameter3[6].Value = 0;
                                object obj3 = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameter3); //带事务
                            }                       
                        }                 
                        trans.Commit();
                        Task task = new Task(() =>
                        {
                            PushMessageHelper.PushMessages(obj.ToString(), "您收到一条党委通知.", model.userid, 0, (int)PushTypeEnum.党委通知);
                        });
                        task.Start();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.ztree GetNotify(int id)
        {
            ztree model = new ztree();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 category_id,title,P_Image.P_PictureName as img_url,content");
            strSql.Append(" from dt_article");
            strSql.Append(" left join P_Image on P_ImageId = convert(VARCHAR,dt_article.id) and P_ImageType = 20017 ");
            strSql.Append(" where id='"+id+"'");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            DataSetToModelHelper<ztree> helper = new DataSetToModelHelper<ztree>();
            model = helper.FillToModel(ds.Tables[0].Rows[0]);

            StringBuilder str = new StringBuilder();
            str.Append("select P_ToUser as userid from P_PartyCommitteeNotify where P_Relation = '"+id+"'");
            DataSet dd = DbHelperSQL.Query(str.ToString());

            //model.userid = helper.FillModel(dd);
            List<string> users = new List<string>();
            if (dd.Tables[0] != null && dd.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow dr in dd.Tables[0].Rows)
                {
                    users.Add(dr["userid"].ToString());
                }
            }
            model.userid = users;
            return model;
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Boolean EditNotify(Model.ztree model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update dt_article set ");
                        strSql.Append("channel_id=@channel_id,");
                        strSql.Append("category_id=@category_id,");
                        strSql.Append("title=@title,");
                        strSql.Append("content=@content ");
                        strSql.Append(" where id=@id");
                        SqlParameter[] parameters = {
                               new SqlParameter("@channel_id",SqlDbType.Int,4),
                               new SqlParameter("@category_id", SqlDbType.Int,4),
                               new SqlParameter("@title", SqlDbType.NVarChar,100),
                               new SqlParameter("@content", SqlDbType.NText),
                               new SqlParameter("@id", SqlDbType.Int,4),
                        };
                        parameters[0].Value = 20;
                        parameters[1].Value = model.category_id;
                        parameters[2].Value = model.title;
                        parameters[3].Value = model.content;
                        parameters[4].Value = model.id;
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                        QiNiuHelper name = new QiNiuHelper();
                        string imagename = name.GetQiNiuFileUrl(model.img_url);
                        StringBuilder image = new StringBuilder();
                        image.Append("update P_Image set ");
                        image.Append("P_ImageUrl=@P_ImageUrl, ");
                        image.Append("P_PictureName=@P_PictureName ");
                        image.Append(" where P_ImageId = @id");
                        SqlParameter[] parameters2 = {
                            new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar,200),
                            new SqlParameter("@P_PictureName",SqlDbType.NVarChar,100),
                            new SqlParameter("@id", SqlDbType.NVarChar,50)
                            };
                        parameters2[0].Value = imagename;
                        parameters2[1].Value = model.img_url;
                        parameters2[2].Value = model.id;
                        object obj2 = DbHelperSQL.GetSingle(conn, trans, image.ToString(), parameters2); //带事务

                        StringBuilder del = new StringBuilder();
                        del.Append("delete from P_PartyCommitteeNotify where P_Relation = @id");
                        SqlParameter[] parameter = {
                            new SqlParameter("@id",SqlDbType.NVarChar,50)
                        };
                        parameter[0].Value = model.id;
                        DbHelperSQL.ExecuteSql(conn, trans, del.ToString(), parameter);

                        if (model.userid != null && model.userid.Count > 0)
                        {
                            string arry = string.Empty;
                            for (int i = 0; i < model.userid.Count; i++)
                            {
                                if (i == model.userid.Count - 1)
                                {
                                    arry += model.userid[i];
                                }
                                else
                                {
                                    arry += model.userid[i] + ",";
                                }
                            }
                            StringBuilder ss = new StringBuilder();
                            ss.Append("select id from dt_users where id in(" + arry + ")");
                            DataSet ds = DbHelperSQL.Query(ss.ToString());
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                StringBuilder sql = new StringBuilder();
                                sql.Append("insert into P_PartyCommitteeNotify(");
                                sql.Append("P_Id,P_Relation,P_GroupId,P_ToUser,P_Status) ");
                                sql.Append("values(");
                                sql.Append("@P_Id,@P_Relation,@P_GroupId,@P_ToUser,@P_Status)");
                                sql.Append(";select @@IDENTITY");
                                SqlParameter[] parameter3 = {
                                   new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                                   new SqlParameter("@P_Relation",SqlDbType.NVarChar,50),
                                   new SqlParameter("@P_GroupId",SqlDbType.NVarChar,50),
                                   new SqlParameter("@P_ToUser",SqlDbType.NVarChar,50),
                                   new SqlParameter("@P_Status",SqlDbType.NVarChar,50)
                                };
                                string notid = Guid.NewGuid().ToString();
                                parameter3[0].Value = notid;
                                parameter3[1].Value = model.id;
                                parameter3[2].Value = model.groupid;
                                parameter3[3].Value = item[0];
                                parameter3[4].Value = 0;
                                object obj3 = DbHelperSQL.GetSingle(conn, trans, sql.ToString(), parameter3); //带事务
                            }
                        }
                        trans.Commit();
                    }
                    catch(Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        public List<ZTreeModel> GetGroup()
        {
            List<ZTreeModel> model = new List<ZTreeModel>();
            StringBuilder str = new StringBuilder();
            str.Append("select id as id,pid as pid,title as name from dt_user_groups");
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<ZTreeModel> info = new DataSetToModelHelper<ZTreeModel>();
            model = info.FillModel(ds);
           
            return model;
        }
    }
}
