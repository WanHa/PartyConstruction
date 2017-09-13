using DTcms.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTcms.Model.WebApiModel;
using DTcms.Model.WebApiModel.FromBody;

namespace DTcms.BLL
{
    public class RefushCover
    {
        public Boolean RufushCover(RefushCoverModel cm)
        {
            return new RefushGroupCover().RefushCover(cm.type,cm.groupId, cm.imageName);
        }
    }
}
