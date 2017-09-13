using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTcms.Common
{
    public enum LoginReturnEnum
    {
        登录成功 = 10000,
        账号不存在 = 10001,
        密码不正确 = 10002,
        登录失败 =10003,
        账号正在审核中 = 10004
    }
}
