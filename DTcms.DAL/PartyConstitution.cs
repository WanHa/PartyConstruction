using DTcms.Common;
using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTcms.Model.WebApiModel.PartyConstitutionModel;

namespace DTcms.DAL
{
    public class PartyConstitution
    {
        /// <summary>
        /// 获取党规党章下的类别
        /// </summary>
        /// <param name="conid"></param>
        /// <returns></returns>
       public List<Constitution> GetPartyConstitution()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select dt_article_category.id as subclassid,P_Image.P_ImageUrl as imageurl from dt_article_category
                        LEFT JOIN P_Image on P_Image.P_ImageId = convert(nvarchar,dt_article_category.id) and P_Image.P_ImageType = @P_ImageType
                        where dt_article_category.channel_id = 16");

            SqlParameter[] par = {
                new SqlParameter("@P_ImageType", SqlDbType.Int),
            };
            par[0].Value = (int)ImageTypeEnum.栏目;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), par);
            DataSetToModelHelper<Constitution> helper = new DataSetToModelHelper<Constitution>();
            List<Constitution> model = helper.FillModel(ds);
            return model;
        }
    }
}
