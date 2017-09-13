using DTcms.Common;
using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DTcms.DAL
{
    /// <summary>
    /// 数据访问类：在线考试
    /// </summary>
    public partial class P_QuestionBank
    {
       // private static string RootUrl = ConfigurationManager.AppSettings["QiNiuRootUrl"];
        public P_QuestionBank()
        { }
          /// <summary>
          /// 是否存在该记录
          /// </summary>
         public bool Exists(string id)
         {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from P_QuestionBank");
            strSql.Append(" where P_Id=@id ");
            SqlParameter[] parameters = {
            new SqlParameter("@id", SqlDbType.NVarChar,36)};
            parameters[0].Value = id;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
         }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add(Model.P_QuestionBank model)
         {
              using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
              {
                  conn.Open();
                  using (SqlTransaction trans = conn.BeginTransaction())
                  {
                      try
                      {
                          StringBuilder strSql = new StringBuilder();
                          strSql.Append("insert into P_QuestionBank(");
                          strSql.Append("P_Id,P_QuestionBankName,P_ImageId,P_Description,P_Status,P_CreateTime,P_CreateUser)");
                          strSql.Append("values(");
                          strSql.Append("@P_Id,@P_QuestionBankName,@P_ImageId,@P_Description,@P_Status,@P_CreateTime,@P_CreateUser)");
                          strSql.Append(";select @@IDENTITY");
                          SqlParameter[] parameters ={
                          new SqlParameter("@P_Id",SqlDbType.NVarChar,36),
                          new SqlParameter("@P_QuestionBankName",SqlDbType.NVarChar,100),
                          new SqlParameter("@P_ImageId",SqlDbType.NVarChar,100),
                          new SqlParameter("@P_Description",SqlDbType.NVarChar,300),
                          new SqlParameter("@P_Status",SqlDbType.Int),
                          new SqlParameter("@P_CreateTime",SqlDbType.DateTime,100),
                          new SqlParameter("@P_CreateUser",SqlDbType.NVarChar,50)};
                          parameters[0].Value = model.P_Id;
                          parameters[1].Value = model.P_QuestionBankName;
                          parameters[2].Value = model.P_ImageId;
                          parameters[3].Value = model.P_Description;
                          parameters[4].Value = model.P_Status;
                          parameters[5].Value = model.P_CreateTime;
                          parameters[6].Value = model.P_CreateUser;
                          object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                          //int mainId = Convert.ToInt32(obj);
                        StringBuilder strSq2 = new StringBuilder();
                        strSq2.Append("insert into P_Image (");
                        strSq2.Append("P_Id,P_ImageId,P_ImageUrl,P_CreateTime,P_CreateUser,P_Status,P_PictureName,P_ImageType");
                        strSq2.Append(")");
                        strSq2.Append("values(");
                        strSq2.Append("@P_Id,@P_ImageId,@P_ImageUrl,@P_CreateTime,@P_CreateUser,@P_Status,@P_PictureName,@P_ImageType)");
                        SqlParameter[] parameters1 = {
                                        new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                        new SqlParameter("@P_ImageId", SqlDbType.NVarChar,50),
                                        new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                        new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_Status", SqlDbType.Int,4),
                                        new SqlParameter("@P_PictureName", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_ImageType", SqlDbType.NVarChar,100),
                                        };
                        parameters1[0].Value = Guid.NewGuid().ToString();
                        parameters1[1].Value = model.P_Id;
                        parameters1[2].Value = new QiNiuHelper().GetQiNiuFileUrl(model.P_ImageId);
                        parameters1[3].Value = DateTime.Now;
                        parameters1[4].Value = model.P_CreateUser;
                        parameters1[5].Value = 0;
                        parameters1[6].Value = model.P_ImageId;
                        parameters1[7].Value = (int)ImageTypeEnum.学习测试;
                        DbHelperSQL.GetSingle(conn, trans, strSq2.ToString(), parameters1); //带事务
                        trans.Commit();
                      }
                      catch(Exception e)
                      {
                          trans.Rollback();
                          return "0";
                      }
                  }
              }
              return model.P_Id;
         }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(Model.P_QuestionBank model)
        {
            //Model.P_QuestionBank oldModel = GetModel(model.P_Id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using(SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_QuestionBank set ");
                        strSql.Append("P_QuestionBankName=@P_QuestionBankName,");
                        strSql.Append("P_ImageId=@P_ImageId,");
                        strSql.Append("P_Description=@P_Description,");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser");
                        strSql.Append(" where P_Id=@P_Id");
                        SqlParameter[] parameters ={
                          new SqlParameter("@P_QuestionBankName",SqlDbType.NVarChar,100),
                          new SqlParameter("@P_ImageId",SqlDbType.NVarChar,100),
                          new SqlParameter("@P_Description",SqlDbType.NVarChar,300),
                          new SqlParameter("@P_UpdateTime",SqlDbType.DateTime,100),
                          new SqlParameter("@P_UpdateUser",SqlDbType.NVarChar,50),
                          new SqlParameter("@P_Id",SqlDbType.NVarChar,50)};
                        parameters[0].Value = model.P_QuestionBankName;
                        parameters[1].Value = model.P_ImageId;
                        parameters[2].Value = model.P_Description;
                        parameters[3].Value = model.P_UpdateTime;
                        parameters[4].Value = model.P_UpdateUser;
                        parameters[5].Value = model.P_Id;
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);

                        string picSql = String.Format(@"select top 1 P_PictureName from P_Image 
                            where P_ImageId = '{0}' and P_ImageType = '{1}'", model.P_Id, (int)ImageTypeEnum.学习测试);
                        string picName = Convert.ToString(DbHelperSQL.GetSingle(picSql.ToString()));

                        if (!picName.Equals(model.P_ImageId)) {

                            new QiNiuHelper().DeleteQiNiuFile(picName);
                            //删除图片
                            StringBuilder strSql8 = new StringBuilder();
                            strSql8.Append("delete from P_Image");
                            strSql8.Append(" where P_ImageId=@P_ImageId and P_ImageType = @P_ImageType");
                            SqlParameter[] parameters8 = {
                                new SqlParameter("@P_ImageId", SqlDbType.NVarChar,50),
                                new SqlParameter("@P_ImageType", SqlDbType.NVarChar,50)};

                            parameters8[0].Value = model.P_Id;
                            parameters8[1].Value = (int)ImageTypeEnum.学习测试;
                            DbHelperSQL.ExecuteSql(conn, trans, strSql8.ToString(), parameters8);

                            if (!String.IsNullOrEmpty(model.P_ImageId)) {
                                StringBuilder strSq2 = new StringBuilder();
                                strSq2.Append("insert into P_Image (");
                                strSq2.Append("P_Id,P_ImageId,P_ImageUrl,P_CreateTime,P_CreateUser,P_Status,P_PictureName,P_ImageType");
                                strSq2.Append(")");
                                strSq2.Append("values(");
                                strSq2.Append("@P_Id,@P_ImageId,@P_ImageUrl,@P_CreateTime,@P_CreateUser,@P_Status,@P_PictureName,@P_ImageType)");
                                SqlParameter[] parameters1 = {
                                        new SqlParameter("@P_Id", SqlDbType.NVarChar,50),
                                        new SqlParameter("@P_ImageId", SqlDbType.NVarChar,50),
                                        new SqlParameter("@P_ImageUrl", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_CreateTime", SqlDbType.DateTime,100),
                                        new SqlParameter("@P_CreateUser", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_Status", SqlDbType.Int,4),
                                        new SqlParameter("@P_PictureName", SqlDbType.NVarChar,100),
                                        new SqlParameter("@P_ImageType", SqlDbType.NVarChar,100),
                                        };
                                parameters1[0].Value = Guid.NewGuid().ToString();
                                parameters1[1].Value = model.P_Id;
                                parameters1[2].Value = new QiNiuHelper().GetQiNiuFileUrl(model.P_ImageId);
                                parameters1[3].Value = DateTime.Now;
                                parameters1[4].Value = model.P_CreateUser;
                                parameters1[5].Value = 0;
                                parameters1[6].Value = model.P_ImageId;
                                parameters1[7].Value = (int)ImageTypeEnum.学习测试;
                                DbHelperSQL.GetSingle(conn, trans, strSq2.ToString(), parameters1); //带事务
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

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id, string UserId)
        {

            Model.P_QuestionBank oldModel = GetModel(id); //旧的数据
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update P_QuestionBank set ");
                        strSql.Append("P_Status=@P_Status,");
                        strSql.Append("P_UpdateTime=@P_UpdateTime,");
                        strSql.Append("P_UpdateUser=@P_UpdateUser");
                        strSql.Append(" where P_Id=@P_Id ");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Status", SqlDbType.Int,4),
                                new SqlParameter("@P_UpdateTime", SqlDbType.DateTime,100),
                                new SqlParameter("@P_UpdateUser", SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Id", SqlDbType.NVarChar,100)};
                        parameters[0].Value = 1;
                        parameters[1].Value = DateTime.Now;
                        parameters[2].Value = UserId;
                        parameters[3].Value = id;

                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
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
        /// 得到对象实体
        /// </summary>
        /// <param name="id"></param>
        public Model.P_QuestionBank GetModel(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  P_Id,P_QuestionBankName,P_ImageId,P_Description,P_Status,P_CreateTime,P_CreateUser,P_UpdateTime,P_UpdateUser ");
            strSql.Append("from P_QuestionBank ");
            strSql.Append("where P_Id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = id;

            Model.P_QuestionBank model = new Model.P_QuestionBank();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM P_QuestionBank  ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where P_Status = 0 and " + strWhere);
            }
            else
            {
                strSql.Append(" where P_Status = 0 ");
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public Model.P_QuestionBank DataRowToModel(DataRow row)
        {
            Model.P_QuestionBank model = new Model.P_QuestionBank();
            if (row != null)
            {
                if (row["P_Id"] != null && row["P_Id"].ToString() != "")
                {
                    model.P_Id = row["P_Id"].ToString();
                }
                if (row["P_QuestionBankName"]!=null && row["P_QuestionBankName"].ToString() != "")
                {
                    model.P_QuestionBankName = row["P_QuestionBankName"].ToString();
                }
                if (row["P_ImageId"] != null && row["P_ImageId"].ToString() != "")
                {
                    model.P_ImageId = row["P_ImageId"].ToString();
                }
                if (row["P_Description"] != null)
                {
                    model.P_Description = row["P_Description"].ToString();
                }
                if (row["P_CreateTime"] != null)
                {
                    model.P_CreateTime = Convert.ToDateTime(row["P_CreateTime"].ToString());
                }
                if (row["P_CreateUser"] != null)
                {
                    model.P_CreateUser = row["P_CreateUser"].ToString();
                }
                if (row["P_UpdateTime"] != null && row["P_UpdateTime"].ToString()!="")
                {
                    model.P_UpdateTime = Convert.ToDateTime(row["P_UpdateTime"].ToString());
                }
                if (row["P_UpdateUser"] != null && row["P_UpdateUser"].ToString() != "")
                {
                    model.P_UpdateUser = row["P_UpdateUser"].ToString();
                }
                if (row["P_Status"] != null && row["P_Status"].ToString() != "")
                {
                    model.P_Status = Convert.ToInt32(row["P_Status"]);
                }
            }
            return model;
        }
    }
}

