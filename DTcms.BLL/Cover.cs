using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.BLL
{
    public class Cover
    {
        private PInfo pin =new PInfo();
        /// <summary>
        /// 封面接口
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Update(CoverModel model)
        {
            return pin.Update(model);
        }
    }
}
