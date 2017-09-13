using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Model.WebApiModel
{
    public class Aticle
    {

        public string title { get; set; }

        public string content { get; set; }

        public List<AticleImg> imgurl { get; set; }
        public int userid { get; set; }
    }
    public class AticleImg
    {
        public string imgname { get; set; }
    }
}
