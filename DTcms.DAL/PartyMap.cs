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
    public partial class PartyMap
    {
        public PartyMap()
        { }
		private const double EARTH_RADIUS = 6378.137;
		private static double Rad(double d)
		{
			return d * Math.PI / 180.0;
		}
		public string MapGetPositionList()
        {
            List<MapPosition> lmp = new List<MapPosition>();
            string sql = @"SELECT
	                            title,POSITION,COUNT(b.id) AS num
                            FROM
	                            dt_user_groups a
                            LEFT JOIN dt_users b ON b.group_id LIKE '%,'+CONVERT(VARCHAR,a.id)+',%'
                            WHERE is_lock = 0
                            GROUP BY title,POSITION";
            DataSet ds = DbHelperSQL.Query(sql);
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MapPosition mp = new MapPosition();
                string a = dt.Rows[i]["position"].ToString();
                if (!string.IsNullOrEmpty(a))
                {
                    string[] pos = a.Split(',');
                    mp.lng = pos[0];
                    mp.lat = pos[1];
                    mp.title = dt.Rows[i]["title"].ToString();
                    mp.num = dt.Rows[i]["num"].ToString();
                    lmp.Add(mp);
                }
            }

            string json = JsonHelper.ObjectToJSON(lmp);
            return json;
        }
		/// <summary>
		/// 检索条件
		/// </summary>
		/// <param name="party"></param>
		/// <returns></returns>
		public List<PartyMassageModel> getPartyMassage(string party, int rows, int page)
		{
         List<PartyMassageModel> result = new List<PartyMassageModel>();
   //         StringBuilder strSql = new StringBuilder();
   //         strSql.Append("select dt_user_groups.id as id ,dt_user_groups.title as name,dt_user_groups.manager as mname");
   //         strSql.Append(" from dt_user_groups where title like '%" + party + "%'");
			//DataSet dt = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strSql.ToString(), "title"));
			//DataSetToModelHelper<PartyMassageModel> model = new DataSetToModelHelper<PartyMassageModel>();
   //         if(dt.Tables[0].Rows.Count!=0)
   //         {
   //             result = model.FillModel(dt);
   //             foreach(var item in result)
   //             {
   //                 StringBuilder sql = new StringBuilder();
   //                 sql.Append("select count(id)as sum from dt_users where group_id like '%" + item.id + "%'");
   //                 DataSet data = DbHelperSQL.Query(sql.ToString());
   //                 DataSetToModelHelper<PartyMassageModel> mo = new DataSetToModelHelper<PartyMassageModel>();
   //                 PartyMassageModel itmes = new PartyMassageModel();
   //                 itmes = mo.FillToModel(data.Tables[0].Rows[0]);
   //                 item.sum = itmes.sum;
   //             }
   //         }
			//else
   //         {
   //             result = null;
   //         }
			return result;
		}
		/// <summary>
		/// 获取党组织详细信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PartyMassageDetailsModel getPartyMassageDetails(string id)
		{
            PartyMassageDetailsModel result = new PartyMassageDetailsModel();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select title as name,manager as manager_name,contact_address as address,intre as info,phone_fax as phone,");
            strSql.Append(" (select count(id) from dt_users where group_id like '%," + id + @",%')as sum from dt_user_groups where id ='" + id + @"'");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            DataSetToModelHelper<PartyMassageDetailsModel> model = new DataSetToModelHelper<PartyMassageDetailsModel>();
            if (ds.Tables[0].Rows.Count != 0)
			{
                result = model.FillToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}			
			return result;
		}
		/// <summary>
		/// 获取职务、姓名列表
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		public List<PartyList> getPartyMember(string groupId,int rows,int page)
		{
            List<PartyList> result = new List<PartyList>();
            StringBuilder strSql = new StringBuilder();
			strSql.Append("select id as userid,user_name as username,role as role from (select dt_users.*,");
            strSql.Append("(select case when dt_user_groups.id is not null then 1 else 0 end from dt_user_groups");
            strSql.Append(" where dt_user_groups.id = '" + groupId + @"' and dt_user_groups.manager_id = dt_users.id) as role from dt_users");
            strSql.Append(" where dt_users.group_id like '%" + groupId + @"%') table1 ");
            DataSet ds = DbHelperSQL.Query(ApiPagingHelper.CreatePagingSql(rows, page, strSql.ToString(), "table1.role desc"));
            DataSetToModelHelper<PartyList> model = new DataSetToModelHelper<PartyList>();
            if (ds.Tables[0].Rows.Count != 0)
			{
                result = model.FillModel(ds);
                foreach(var item in result)
                {
                    StringBuilder sr = new StringBuilder();
                    sr.Append("select P_ImageUrl as ava from P_Image where P_ImageId = '" + item.userid + @"' and P_ImageType='20011'");
                    DataSet data = DbHelperSQL.Query(sr.ToString());
                    DataSetToModelHelper<PartyList> mo = new DataSetToModelHelper<PartyList>();
                    PartyList aa = new PartyList();
                    if(data.Tables[0].Rows.Count!= 0)
                    {
                        aa = mo.FillToModel(data.Tables[0].Rows[0]);
                        item.ava = aa.ava;
                    }
                    else
                    {
                        aa = null;
                    }
                }
			}
			else
			{
                result = null;
			}
            return result;
		}
		/// <summary>
		/// 传入经纬度与距离返回所传经纬度相应距离的位置
		/// </summary>
		/// <param name="lat">纬度</param>
		/// <param name="lng">精度</param>
		/// <returns></returns>
		public List<BranchL> getLocation(double lat, double lng,string party)
		{
			List<BranchL> locationList = new List<BranchL>();
			StringBuilder strSql = new StringBuilder();
			strSql.Append(@"select position from dt_user_groups where not (position is null or position='')");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
			if (ds != null)
			{
				DataTable dt = ds.Tables[0];
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					string location = dt.Rows[i]["position"].ToString();
					if (!string.IsNullOrEmpty(location))
					{
						string[] pos = location.Split(',');
                        double latKnown = double.Parse(pos[0].ToString());//经度
                        double lngKnown = double.Parse(pos[1].ToString());//纬度
						double radLat1 = Rad(lat);
						double radLat2 = Rad(latKnown);
						double a = radLat1 - radLat2;
						double b = Rad(lng) - Rad(lngKnown);
						double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
						result = Math.Round(result * 10000) / 10000;
						if (result < 100)
						{
							if(party == null)
                            {
                                StringBuilder sql = new StringBuilder();
                                sql.Append("select dt_user_groups.id as id ,title as bname,manager as mname,position as location, (select count(*) from dt_users where dt_users.group_id like ('%'+CAST((select id");
                                sql.Append(" from dt_user_groups where [position] = '" + location + @"') as VARCHAR) +'%')) as sum from dt_user_groups ");
                                sql.Append(" WHERE [position] = '" + location + @"'");
                                DataSet data = DbHelperSQL.Query(sql.ToString());
                                DataSetToModelHelper<BranchL> model = new DataSetToModelHelper<BranchL>();
                                if(data.Tables[0].Rows.Count!=0)
                                {
                                    BranchL ss = new BranchL();
                                    ss = model.FillToModel(data.Tables[0].Rows[0]);
                                    ss.latKnown = double.Parse(pos[0].ToString());
                                    ss.lngKnown = double.Parse(pos[1].ToString());
                                    locationList.Add(ss);
                                }
                                else
                                { 
                                    locationList = null;
                                }
                            }
                            if(party != null)
                            {
                                List<BranchL> re = new List<BranchL>();
                                StringBuilder stql = new StringBuilder();
                                stql.Append("select dt_user_groups.id as id ,dt_user_groups.title as bname,dt_user_groups.manager as mname");
                                stql.Append(" from dt_user_groups where title like '%" + party + "%'");
                                DataSet dat = DbHelperSQL.Query(stql.ToString());
                                DataSetToModelHelper<BranchL> model = new DataSetToModelHelper<BranchL>();
                                if (dat.Tables[0].Rows.Count != 0)
                                {
                                    re = model.FillModel(dat);
                                    foreach (var item in re)
                                    {
                                        StringBuilder sql = new StringBuilder();
                                        sql.Append("select count(id)as sum from dt_users where group_id like '%" + item.id + "%'");
                                        DataSet data = DbHelperSQL.Query(sql.ToString());
                                        DataSetToModelHelper<BranchL> mo = new DataSetToModelHelper<BranchL>();
                                        BranchL aa = new BranchL();
                                        aa = mo.FillToModel(data.Tables[0].Rows[0]);
                                        item.sum = aa.sum;
                                    }
                                    return re;
                                }
                                else
                                {
                                    re = null;
                                    return re;
                                }
                            }
                        }
					}
					else
					{
						return null;
					}
				}
			}
			else
			{
				return null;
			}
			return locationList;
		}
    }
	public class MapPosition
    {
        //经度
        public string lng { get; set; }
        //纬度
        public string lat { get; set; }
        //文本
        public string title { get; set; }
        //人数
        public string num { get; set; }
    }
	public class PartyMassageModel
	{
		//党支部id
		public int id { get; set; }
		//党支部名称
		public string name { get; set; }
		//党组织人数
        public string mname { get; set; }
        public int sum { get; set; }
	}
	public class PartyMassageDetailsModel
	{
		//党支部名称
		public string name { get; set; }
		//党支部书记名字
		public string manager_name { get; set; }
		//党组织人数
		public int sum { get; set; }
		//支部电话
		public string phone { get; set; }
		//联系地址
		public string address { get; set; }
		//支部介绍
		public string info { get; set; }
	}
	public class PartyList
	{
        public int role { get; set; }
		public int userid { get; set; }
		public string username { get; set; }
        public string ava { get; set; }
	}
    public class BranchL
    {
        public string location { get; set; }
        public int sum { get; set; }
        public string bname { get; set; }
        public string mname { get; set; }
        public int id { get; set; }
        public double latKnown { get; set; }
        public double lngKnown { get; set; }
    }
}
