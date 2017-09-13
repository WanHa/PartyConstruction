using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class PartySatisfaction
    {
        public int satisfaction { get; set; }
    }
    public class PartyUrgent
    {
        public int urgent { get; set; }
    }
    public class PartytrainModel
    {
        public int userid { get; set; }
        public string content { get; set; }
        public string mid { get; set; }
        public int type{ get; set; }
        public int checkstatus { get; set; }
        public string openstate { get; set; }
        public string urgentstatus { get; set; }
        public string verifier { get; set; }
        public List<OptionModel2> options { get; set; }
    }
    public class OptionModel2
    {
        //public string pname { get; set; }
        public string imgurl { get; set; }
    }
    public class member
    {
        private string _createtime;
        public string createtime
        {
            get
            {
                return DateTime.Parse(_createtime == null ? "" : _createtime).ToString("yyyy年MM月dd日");
            }
            set
            {
                _createtime = value;
            }
        }
        public string verifier { get; set; }
        public string content { get; set; }
        public string branch { get; set; }
        public int urgentstatus { get; set; }
        public int checkstatus { get; set; }
        public List<photo> Photolist { get; set; }
        public string createid { get; set; }
        public string imageid { get; set; }
        public string id { get; set; }
        public string tel { get; set; }
        public List<con> Conlist { get; set; }
    }
    public class Trainlist
    {
        private string _createtime;
        public string createtime
        {
            get
            {
                return DateTime.Parse(_createtime == null ? "" : _createtime).ToString("yyyy年MM月dd日");
            }
            set
            {
                _createtime = value;
            }
        }
        public string content { get; set; }
        //public string imageid { get; set; }
        public string createid { get; set; }
        public string id { get;set; }
        public int checkstatus { get; set; }
        public int urgentstatus { get; set; }
        public string tel { get; set; }
        public string branch { get; set; }
        public string name{ get; set; }
        public List<con> Conlist { get;set; }
        public List<photo> Photolist { get; set; }
    }
    public class con
    {
        public string rcontent { get; set; }
        private string _rcreatetime;
        public string rcreatetime
        {
            get
            {
                return DateTime.Parse(_rcreatetime == null ? "" : _rcreatetime).ToString("yyyy-MM-dd");
            }
            set
            {
                _rcreatetime = value;
            }
        }
    }
    public class photo
    {
        public string image { get; set; }
    }
    public class SumCount
    {
        public int sum { get; set; }
    }
}

