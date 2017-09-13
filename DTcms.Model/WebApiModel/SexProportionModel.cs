using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
     public class SexProportionModel
    {
        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int num { get; set; }
    }
    public class MonthlyMeetingNum
    {
        /// <summary>
        /// 月份
        /// </summary>
        public Int16 month { get; set; }

        /// <summary>
        /// 会议数量
        /// </summary>
        public int meetingNum { get; set; }
    }

    public class IncomeSourceModel
    {
        /// <summary>
        /// 收入种类
        /// </summary>
        public string IncomeTypeId { get; set; }

        /// <summary>
        /// 各类收入人数
        /// </summary>
        public int IncomeNum { get; set; }
    }

    public class PartyMemberKind
    {
        /// <summary>
        /// 正式党员
        /// </summary>
        public int off_count { get; set; }
        /// <summary>
        /// 预备数量
        /// </summary>
        public int ready_count { get; set; }
    }

    public class MeetingNumber
    {
        public int userid { get; set; }
        /// <summary>
        /// 参与会议次数
        /// </summary>
        public int count { get; set; } 
    }

    public class GetInfo
    {
        public string userid { get; set; }

        public string type { get; set; }


        public int count { get; set; }
    }
}
