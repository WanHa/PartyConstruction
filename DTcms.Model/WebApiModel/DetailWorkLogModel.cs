using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class ImageModel
    {
        public string imgurl { get; set; }
    }
    public class DetailWorkLogModel
    {
        public List<ImageModel> image { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        private string _time;
        public string time
        {
            get
            {
                return DateTime.Parse(_time == null ? "" : _time).ToString("yyyy年MM月dd日");
            }
            set
            {
                _time = value;
            }
        }

    }
}
