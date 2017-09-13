using DTcms.Common;
using DTcms.DBUtility;
using DTcms.Model;
using Qiniu.RS;
using Qiniu.RS.Model;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.DAL
{
    public partial class PartyCloud
    {
        //public static string AccessKey = ConfigurationManager.AppSettings["QiNiuAk"];
        //public static string SecretKey = ConfigurationManager.AppSettings["QiNiuSk"];
        //public static string RootUrl = ConfigurationManager.AppSettings["QiNiuRootUrl"];
        //public static string bucket = ConfigurationManager.AppSettings["QiNiuScope"];
        
        public PartyCloud()
        { }

        /// <summary>
        /// 获取所有bucket
        /// </summary>
        public string buckets()
        {
            P_QiNiuInfo info = new QiNiuHelper().GetQiNiuConfigInfo();
            Mac mac = new Mac(info.P_AK, info.P_SK);
            BucketManager bm = new BucketManager(mac);

            var result = bm.Buckets().Text;

            return result;
        }

        #region 获取空间所有文件（分页/模糊查询）
        /// <summary>
        /// 获取空间文件列表          
        /// </summary>
        public List<Item> listFiles(int pageSize, int page, Tuple<string,string> strWhere ,out int totalCount)
        {
            List<Item> model = new List<Item>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select P_Id as mid ,P_Type as type from P_PartyCloud");
            //根据选择筛选条件查询
            if(strWhere.Item1 == ""&& strWhere.Item2 == "")
            {
                strSql.Append(" where P_Status = 0");
            }
            else if(Convert.ToInt32(strWhere.Item2) == 3)
            {
                strSql.Append(" where P_Type = 0 and P_Status = 0");
            }
            else if(Convert.ToInt32(strWhere.Item2) == 1)
            {
                strSql.Append(" where P_Type = 1 and P_Status = 0");
            }
            else if (Convert.ToInt32(strWhere.Item2) == 2)
            {
                strSql.Append(" where P_Type = 2 and P_Status = 0");
            }
            totalCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            DataSet ds = DbHelperSQL.Query(PagingHelper.CreatePagingSql(totalCount, pageSize, page, strSql.ToString(), "P_Id"));
            DataSetToModelHelper<Item> helper = new DataSetToModelHelper<Item>();
            if(ds.Tables[0].Rows.Count!=0)
            {
                model = helper.FillModel(ds);
            }
            else
            {
                model = null;
                return model;
            }
            foreach (var item in model)
            {
                switch (item.type)
                {
                    case 0:
                        StringBuilder str = new StringBuilder();
                        str.Append("select P_ImageUrl as img_url,P_PictureName as id,P_PictureName as title from P_Image where P_ImageId = '" + item.mid + @"'");
                        DataSet dts = DbHelperSQL.Query(str.ToString());
                        DataSetToModelHelper<Item> mo = new DataSetToModelHelper<Item>();
                        Item aa = new Item();
                        if (dts.Tables[0].Rows.Count != 0)
                        {
                            aa = mo.FillToModel(dts.Tables[0].Rows[0]);
                            item.img_url = aa.img_url;
                            item.title = aa.title;
                            item.id = aa.id;
                        }
                        else
                        {
                            aa = null;
                        }
                        break;
                    case 1:
                        StringBuilder sql = new StringBuilder();
                        sql.Append("select P_VideoPic as img_url,P_VideoName as title,P_VideoName as id from P_Video where P_ParentId = '" + item.mid + @"'");
                        DataSet dt = DbHelperSQL.Query(sql.ToString());
                        DataSetToModelHelper<Item> mod = new DataSetToModelHelper<Item>();
                        Item ab = new Item();
                        if (dt.Tables[0].Rows.Count != 0)
                        {
                            ab = mod.FillToModel(dt.Tables[0].Rows[0]);
                            item.img_url = ab.img_url;
                            item.title = ab.title;
                            item.id = ab.id;
                        }
                        else
                        {
                            ab = null;
                        }
                        break;
                    case 2:
                        StringBuilder ssql = new StringBuilder();
                        ssql.Append("select P_DocUrl as img_url,P_Title as title,P_Title as id from P_Document where P_RelationId = '" + item.mid + @"'");
                        DataSet dats = DbHelperSQL.Query(ssql.ToString());
                        DataSetToModelHelper<Item> mode = new DataSetToModelHelper<Item>();
                        Item abc = new Item();
                        if (dats.Tables[0].Rows.Count != 0)
                        {
                            abc = mode.FillToModel(dats.Tables[0].Rows[0]);
                            item.img_url = abc.img_url;
                            item.title = abc.title;
                            item.id = abc.id;
                        }
                        else
                        {
                            abc = null;
                        }
                        break;
                }
            }
            return model;

            //Root root = new Root();
            //P_QiNiuInfo info = new QiNiuHelper().GetQiNiuConfigInfo();
            //Mac mac = new Mac(info.P_AK, info.P_SK);
            ////Mac mac = new Mac(AccessKey, SecretKey);

            //string marker = ""; // 首次请求时marker必须为空
            //string prefix = null; // 按文件名前缀保留搜索结果
            //string delimiter = null; // 目录分割字符(比如"/")
            //int limit = 100; // 单次列举数量限制(最大值为1000)

            //BucketManager bm = new BucketManager(mac);

            //List<FileDesc> items = new List<FileDesc>();
            //List<string> commonPrefixes = new List<string>();

            //do
            //{
            //    //获取云端数据
            //    var result = bm.ListFiles(info.P_Scope, prefix, marker, limit, delimiter);
            //    //获取json数据
            //    var json = result.Text;
            //    root = JsonHelper.JSONToObject<Root>(json); //转换为实体

            //    //Console.WriteLine(result);
            //    marker = result.Result.Marker;

            //} while (!string.IsNullOrEmpty(marker));

            //List<ItemsItem> list = root.items;
            //totalCount = list.Count;
            ////分页、模糊查询
            //var newlist = (
            //          from item in list
            //          where item.key.Contains(strWhere.Item1) && item.mimeType.Contains(strWhere.Item2)
            //          orderby item.putTime descending
            //          select item
            //          ).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //List<Item> li = new List<Item>();
            ////页面所需数据处理
            //foreach (var a in newlist)
            //{
            //    Item i = new Item();
            //    i.id = a.key;
            //    i.title = a.key.Split('.')[0];
            //    i.type = a.mimeType;
            //    //如果是视频获取第一帧作为封面图
            //    if (a.mimeType.Contains("video"))
            //    {
            //        i.img_url = info.P_RootUrl + a.key + "?vframe/png/offset/0";
            //    }
            //    else
            //    {
            //        i.img_url = info.P_RootUrl + a.key;
            //    }

            //    li.Add(i);
            //}

            //return li;
        }
        #endregion
        #region 删除指定文件
        public bool delete(string key)
        {
            try
            {
                string[] strArr = key.Split(',');
                P_QiNiuInfo info = new QiNiuHelper().GetQiNiuConfigInfo();
                Mac mac = new Mac(info.P_AK, info.P_SK);
                //Mac mac = new Mac(AccessKey, SecretKey);

                BucketManager bm = new BucketManager(mac);

                var result = bm.Delete(info.P_Scope, key);

                //根据状态码判断删除是否成功
                switch (result.RefCode)
                {
                    case 200:
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
            }
        }
        #endregion

        #region 文件查看/下载记录
        public bool VisitRecord(string userid, string fileid)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "insert into P_VisitRecord values('" + Guid.NewGuid().ToString("N") +"','"+ fileid +"','"+ userid +"','"+ time +"')";
            int result = DbHelperSQL.ExecuteSql(sql);
            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DownloadRecord(string userid, string fileid)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "insert into P_DownloadRecord values('" + Guid.NewGuid().ToString("N") + "','" + fileid + "','" + time + "','" + userid + "')";
            int result = DbHelperSQL.ExecuteSql(sql);
            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
    public class Pcloud
    {

    }
    #region 七牛云返回文件类
    public class ItemsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hash { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int fsize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mimeType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string putTime { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ItemsItem> items { get; set; }
    }
    #endregion

    /// <summary>
    /// 返给页面的类
    /// </summary>
    public class Item
    {
        /// <summary>
        /// 文件路径url
        /// </summary>
        public string img_url { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 类型（0图片，1视频，2文档）
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 子项id
        /// </summary>
        public string mid { get; set; }
        //public string video { get; set; }
        //public string photo { get; set; }
        //public string document { get; set; }
    }
}
