using DTcms.Common;
using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public class Left_home
    {
        /// <summary>
        /// 根据年龄段获取数量
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public List<Ages> GetAge(string groupid)
        {
            List<Ages> model = new List<Ages>();
            StringBuilder str = new StringBuilder();
            if(groupid =="all")
            {
                str.Append("select '20-30'as age, count(*)as sum from dt_users where year(getdate())-year(birthday)BETWEEN 20 and 30 UNION ALL");
                str.Append(" select '31-40'as age, count(*)as sum from dt_users where year(getdate())-year(birthday)BETWEEN 31 and 40 UNION ALL");
                str.Append(" select '41-50'as age, count(*)as sum from dt_users where year(getdate())-year(birthday)BETWEEN 41 and 50 UNION ALL");
                str.Append(" select '51-60'as age, count(*)as sum from dt_users where year(getdate())-year(birthday)BETWEEN 51 and 60 UNION ALL");
                str.Append(" select '61-80'as age, count(*)as sum from dt_users where year(getdate())-year(birthday)BETWEEN 61 and 80 UNION ALL");
                str.Append(" select '80以上'as age, count(*)as sum from dt_users where year(getdate())-year(birthday)>80");
            }
            else
            {
                str.Append("select '20-30'as age, count(*)as sum from dt_users where group_id like '%," + groupid + ",%'");
                str.Append(" and year(getdate())-year(birthday)BETWEEN 20 and 30 UNION ALL");
                str.Append(" select '31-40'as age, count(*)as sum from dt_users where group_id like '%," + groupid + ",%'");
                str.Append(" and year(getdate())-year(birthday)BETWEEN 31 and 40 UNION ALL");
                str.Append(" select '41-50'as age, count(*)as sum from dt_users where group_id like '%," + groupid + ",%'");
                str.Append(" and year(getdate())-year(birthday)BETWEEN 41 and 50 UNION ALL");
                str.Append(" select '51-60'as age, count(*)as sum from dt_users where group_id like '%," + groupid + ",%'");
                str.Append(" and year(getdate())-year(birthday)BETWEEN 51 and 60 UNION ALL");
                str.Append(" select '61-80'as age, count(*)as sum from dt_users where group_id like '%," + groupid + ",%'");
                str.Append(" and year(getdate())-year(birthday)BETWEEN 61 and 80 UNION ALL");
                str.Append(" select '80以上'as age, count(*)as sum from dt_users where group_id like '%," + groupid + ",%'");
                str.Append(" and year(getdate())-year(birthday)>80");
            }
            DataSet dt = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<Ages> result = new DataSetToModelHelper<Ages>();
            if (dt.Tables[0].Rows.Count != 0)
            {
                model = result.FillModel(dt);
            }
            else
            {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// 获取性别
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public Gender GetSex(string groupid)
        {
            StringBuilder strsql = new StringBuilder();
            Gender models = new Gender();
            if (groupid == "all")
            {
                strsql.Append("select sum(case when sex='0' then 1 else 0 end )as man,sum(case when sex='1' then 1 else 0 end )as woman");
                strsql.Append(" from dt_users");
            }
            else
            {
                strsql.Append("select sum(case when sex='0' then 1 else 0 end )as man,sum(case when sex='1' then 1 else 0 end )as woman");
                strsql.Append(" from dt_users where group_id like '%," + groupid + ",%'");
            }
            DataSet dt = DbHelperSQL.Query(strsql.ToString());
                DataSetToModelHelper<Gender> result = new DataSetToModelHelper<Gender>();
                if (dt.Tables[0].Rows.Count != 0)
                {
                    models = result.FillToModel(dt.Tables[0].Rows[0]);
                }
                else
                {
                    models = null;
                }
            return models;
        }
        /// <summary>
        /// 获取学习时长
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public List<Learntime> GetGrouptime(string groupid)
        {
            List<Learntime> list = new List<Learntime>();
            List<Learntime> user = new List<Learntime>();
            Learntime model = new Learntime();
            StringBuilder str = new StringBuilder();
            if (groupid =="all")
            {
                str.Append("select id as userid from dt_users");
            }
            else
            {
                str.Append("select id as userid from dt_users where group_id like '%," + groupid + ",%'");
            }
            DataSet ds = DbHelperSQL.Query(str.ToString());
            DataSetToModelHelper<Learntime> result = new DataSetToModelHelper<Learntime>();
            if (ds.Tables[0].Rows.Count>0)
            {
                user = result.FillModel(ds);
                StringBuilder sql = new StringBuilder();
                if(user != null)
                {
                    sql.Append(" and ( ");
                    for (int i = 0; i < user.Count; i++)
                    {
                        if (i == user.Count - 1)
                        {
                            sql.Append(" P_UserId = " + user[i].userid + @" ");
                        }
                        if (i <= user.Count - 2)
                        {
                            sql.Append(" P_UserId = " + user[i].userid + @"  or  ");
                        }
                    }
                    sql.Append(" )");
                }
                for (int j = 1; j <= 12; j++)
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("select '" + j + @"'as month,sum(P_MaxPlaybackTime) as time from P_VideoRecord");
                    strsql.Append(" where year(P_CreateTime)='"+ DateTime.Now.Year+"' and month(P_CreateTime)=" + j + @" " + sql.ToString() + @"");
                    DataSet dd = DbHelperSQL.Query(strsql.ToString());
                    if(dd!=null)
                    {
                        model = result.FillToModel(dd.Tables[0].Rows[0]);
                        list.Add(model);
                    }
                }
            }
            else
            {
                for (int i = 1; i <= 12; i++)
                {
                    Learntime item = new Learntime() {
                        month = i.ToString(),
                        time = 0,
                        userid = 0
                    };
                    list.Add(item);
                }
            }
            return list;
        }
        /// <summary>
        /// 获取主要经济来源
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public List<Economic> GetMoney(string groupid)
        {
            List<Economic> user = new List<Economic>();
            List<Economic> list = new List<Economic>();
            Economic model = new Economic();
            StringBuilder sql = new StringBuilder();
            if(groupid =="all")
            {
                sql.Append("select id as userid from dt_users");
            }
            else
            {
                sql.Append("select id as userid from dt_users where group_id like '%," + groupid + ",%'");
            }
            DataSet ds = DbHelperSQL.Query(sql.ToString());
            DataSetToModelHelper<Economic> result = new DataSetToModelHelper<Economic>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                user = result.FillModel(ds);

            }
            StringBuilder ssql = new StringBuilder();
            if (user != null && user.Count > 0)
            {
                ssql.Append(" and ( ");
                for (int i = 0; i < user.Count; i++)
                {
                    if (i == user.Count - 1)
                    {
                        ssql.Append(" id = " + user[i].userid + @" ");
                    }
                    if (i <= user.Count - 2)
                    {
                        ssql.Append(" id = " + user[i].userid + @"  or  ");
                    }
                }
                ssql.Append(" )");
                StringBuilder strsql = new StringBuilder();
                strsql.Append("select '工资'as econ,count(*)as sum from dt_users where income_source_id ='1001' " + ssql.ToString() + @" ");
                strsql.Append(" UNION ALL ");
                strsql.Append(" select '退休金'as econ,count(*)as sum from dt_users where income_source_id ='1002' " + ssql.ToString() + @"");
                strsql.Append(" UNION ALL ");
                strsql.Append(" select '经商'as econ,count(*)as sum from dt_users where income_source_id ='1003' " + ssql.ToString() + @"");
                strsql.Append(" UNION ALL ");
                strsql.Append(" select '务工'as econ,count(*)as sum from dt_users where income_source_id ='1004' " + ssql.ToString() + @"");
                strsql.Append(" UNION ALL ");
                strsql.Append(" select '低保'as econ,count(*)as sum from dt_users where income_source_id ='1005' " + ssql.ToString() + @"");
                strsql.Append(" UNION ALL ");
                strsql.Append(" select '失业救济金'as econ,count(*)as sum from dt_users where income_source_id ='1006' " + ssql.ToString() + @"");
                strsql.Append(" UNION ALL ");
                strsql.Append(" select '其他'as econ,count(*)as sum from dt_users where income_source_id ='1007' " + ssql.ToString() + @"");
                DataSet dd = DbHelperSQL.Query(strsql.ToString());
                if (dd != null)
                {
                    list = result.FillModel(dd);
                }
            }
            else
            {
                list.Add(new Economic() {
                    econ = "工资",
                    sum = 0
                });
                list.Add(new Economic()
                {
                    econ = "退休金",
                    sum = 0
                });
                list.Add(new Economic()
                {
                    econ = "经商",
                    sum = 0
                });
                list.Add(new Economic()
                {
                    econ = "务工",
                    sum = 0
                });
                list.Add(new Economic()
                {
                    econ = "低保",
                    sum = 0
                });
                list.Add(new Economic()
                {
                    econ = "失业救济金",
                    sum = 0
                });
                list.Add(new Economic()
                {
                    econ = "其他",
                    sum = 0
                });
            }
            return list;
        }
        public class Learntime
        {
            public int userid { get; set; }
            public string month { get; set; }
            private double _time;
            public double time
            {
                get { return Math.Round(_time / 3600,2); }
                set { _time = value; }
            }
        }
        public class Ages
        {
            public string age { get; set; }
            public int sum { get; set; }
        }
        public class Gender
        {
            public int man { get; set; }
            public int woman { get; set; }
        }
        public class Economic
        {
            public int userid { get; set; }
            public string econ { get; set; }
            public int sum { get;set; }
        }
    }
}
