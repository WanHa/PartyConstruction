using DTcms.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using NPOI.HSSF.UserModel;
using DTcms.Model;
using DTcms.BLL;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DTcms.DBUtility;

namespace DTcms.Web.admin.PartyDataBase
{
    public partial class ExcelToChannel : Web.UI.ManagePage
    {
        List<userExcel> data = new List<userExcel>();

        #region 页面加载=================================
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                btnFileSp2.Disabled = true;
                btnFileSp3.Disabled = true;
            }
        }
        #endregion
        #region 验证身份证号码类  
        public class IDCardValidation
        {
            /// <summary>  
                    /// 验证身份证合理性  
                    /// </summary>  
                    /// <param name="Id"></param>  
                    /// <returns></returns>  
            public bool CheckIDCard(string idNumber)
            {
                if (idNumber.Length == 18)
                {
                    bool check = CheckIDCard18(idNumber);
                    return check;
                }
                else if (idNumber.Length == 15)
                {
                    bool check = CheckIDCard15(idNumber);
                    return check;
                }
                else
                {
                    return false;
                }
            }


            /// <summary>  
                    /// 18位身份证号码验证  
                    /// </summary>  
            private bool CheckIDCard18(string idNumber)
            {
                long n = 0;
                if (long.TryParse(idNumber.Remove(17), out n) == false
                || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
                {
                    return false;//数字验证  
                }
                string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
                if (address.IndexOf(idNumber.Remove(2)) == -1)
                {
                    return false;//省份验证  
                }
                string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                DateTime time = new DateTime();
                if (DateTime.TryParse(birth, out time) == false)
                {
                    return false;//生日验证  
                }
                string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                char[] Ai = idNumber.Remove(17).ToCharArray();
                int sum = 0;
                for (int i = 0; i < 17; i++)
                {
                    sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
                }
                int y = -1;
                Math.DivRem(sum, 11, out y);
                if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
                {
                    return false;//校验码验证  
                }
                return true;//符合GB11643-1999标准  
            }


            /// <summary>  
                    /// 16位身份证号码验证  
                    /// </summary>  
            private bool CheckIDCard15(string idNumber)
            {
                long n = 0;
                if (long.TryParse(idNumber, out n) == false || n < Math.Pow(10, 14))
                {
                    return false;//数字验证  
                }
                string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
                if (address.IndexOf(idNumber.Remove(2)) == -1)
                {
                    return false;//省份验证  
                }
                string birth = idNumber.Substring(6, 6).Insert(4, "-").Insert(2, "-");
                DateTime time = new DateTime();
                if (DateTime.TryParse(birth, out time) == false)
                {
                    return false;//生日验证  
                }
                return true;
            }
        }
        # endregion
        #region  导入党员信息
        protected void PpInputFileUploadButton_Click2(object sender, EventArgs e)
        {
            //ExcelToChannel ec = new ExcelToChannel();
            //bool a = ec.PpInputFileUploadButton_Click(object sender, EventArgs e);
            //if(a==true) { }
            try
            {

                // 获取上传的 Excle 文件
                HttpFileCollection UploadFiles = Request.Files;

                // 文件在 Server 上保存的路径
                string ServerSavePath = Server.MapPath("~/upload");

                // 如果文件数为 0 则提示没有选中上传文件
                if (UploadFiles.Count != 0)
                {
                    string FileName2 = string.Empty;

                    // 获取文件路径
                    string FilePath = UploadFiles[1].FileName;

                    #region 获取文件名 IE 8 一下获取到的文件名带有全路径, 其余浏览器只有文件名
                    if (FilePath.IndexOf(@"\") > 0)
                    {
                        string[] arrfiles = FilePath.Split('\\');

                        FileName2 = arrfiles[arrfiles.Length - 1];
                    }
                    else
                    {
                        FileName2 = FilePath;
                    }
                    #endregion

                    UploadFiles[1].SaveAs(Path.Combine(ServerSavePath, FileName2));
                    #region 根据服务器上的路径, 用 NPOI 插件获取 Excel 文件中的内容, 然后倒入到 DB 中.

                    GetUserSpDataFromExcel(Path.Combine(ServerSavePath, FileName2));

                    if (data != null && data.Count > 0)
                    {
                        bool result = AddUser(data);
                    }
                    #endregion
                    btnFileSp3.Disabled = false;
                    JscriptMsg("文件导入成功！", string.Empty);
                    return;
                }
                else
                {
                    JscriptMsg("请选择上传文件！", string.Empty);
                    return;
                }
            }
            catch (Exception ex)
            {
                JscriptMsg("文件上传失败！", string.Empty);
                return;
            }
        }
        # endregion
        #region 导入党组织信息
        protected void PpInputFileUploadButton_Click(object sender, EventArgs e)
        {
           
            try
            {
                // 获取上传的 Excle 文件
                HttpFileCollection UploadFiles = Request.Files;

                // 文件在 Server 上保存的路径
                string ServerSavePath = Server.MapPath("~/upload");

                // 如果文件数为 0 则提示没有选中上传文件
                if (UploadFiles.Count != 0)
                {
                    string FileName = string.Empty;

                    // 获取文件路径
                    string FilePath = UploadFiles[0].FileName;

                    #region 获取文件名 IE 8 一下获取到的文件名带有全路径, 其余浏览器只有文件名
                    if (FilePath.IndexOf(@"\") > 0)
                    {
                        string[] arrfiles = FilePath.Split('\\');

                        FileName = arrfiles[arrfiles.Length - 1];
                    }
                    else
                    {
                        FileName = FilePath;
                    }
                    #endregion

                    UploadFiles[0].SaveAs(Path.Combine(ServerSavePath, FileName));
                    #region 根据服务器上的路径, 用 NPOI 插件获取 Excel 文件中的内容, 然后倒入到 DB 中.

                    GetGroupSpDataFromExcel(Path.Combine(ServerSavePath, FileName));

                    if (data != null && data.Count > 0)
                    {
                        foreach (userExcel bas in data)
                        {
                            bool result = AddGroup(bas);
                        } //调用导入方法
                    }
                    #endregion
                    btnFileSp2.Disabled = false;
                    JscriptMsg("文件导入成功！", string.Empty);
                    return;
                }
                else
                {
                    JscriptMsg("请选择上传文件！", string.Empty);
                    return;
                }
            }
            catch (Exception ex)
            {
                JscriptMsg("文件上传失败！", string.Empty);
                return;
            }
        }
        # endregion
        #region 导入上级组织与书记
        protected void PpInputFileUploadButton_Click3(object sender, EventArgs e)
        {

            try
            {
                // 获取上传的 Excle 文件
                HttpFileCollection UploadFiles = Request.Files;

                // 文件在 Server 上保存的路径
                string ServerSavePath = Server.MapPath("~/upload");

                // 如果文件数为 0 则提示没有选中上传文件
                if (UploadFiles.Count != 0)
                {
                    string FileName = string.Empty;

                    // 获取文件路径
                    string FilePath = UploadFiles[2].FileName;

                    #region 获取文件名 IE 8 一下获取到的文件名带有全路径, 其余浏览器只有文件名
                    if (FilePath.IndexOf(@"\") > 0)
                    {
                        string[] arrfiles = FilePath.Split('\\');

                        FileName = arrfiles[arrfiles.Length - 1];
                    }
                    else
                    {
                        FileName = FilePath;
                    }
                    #endregion

                    UploadFiles[2].SaveAs(Path.Combine(ServerSavePath, FileName));
                    #region 根据服务器上的路径, 用 NPOI 插件获取 Excel 文件中的内容, 然后倒入到 DB 中.

                    GetParentSpDataFromExcel(Path.Combine(ServerSavePath, FileName));

                    if (data != null && data.Count > 0)
                    {
                        bool result = AddManager(data);//调用导入方法
                    }
                    #endregion
                    
                    JscriptMsg("文件导入成功！", string.Empty);
                    return;
                }
                else
                {
                    JscriptMsg("请选择上传文件！", string.Empty);
                    return;
                }
            }
            catch (Exception ex)
            {
                JscriptMsg("文件上传失败！", string.Empty);
                return;
            }
        }
        #endregion
        #region 上级组织与书记
        private void GetParentSpDataFromExcel(string FilePath)
        {
            // 定义 Workbook 对象
            XSSFWorkbook myWorkbook;
            XSSFSheet myWorksheet;

            try
            {
                // 用数据流打开 Excel 文件, 
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8))
                {
                    myWorkbook = new XSSFWorkbook(fs);
                    myWorksheet = myWorkbook.GetSheet("书记与上级党组织") as XSSFSheet;

                    // Excel 文件中的列头【因为 Excel 中的列头有合并,读取的时候不能读到一行,所以需要人为定义】
                    string[] arrHeads = { "组织名称", "上级党组织", "书记" , "手机号" };

                    // 将 Excel 中的数据读取到 DataTable 中
                    DataTable dt = ReadDataFromExcel(myWorksheet, false, arrHeads, 0);

                    data = DetailListModel22(dt);
                }
            }
            catch (Exception ex)
            {
                JscriptMsg("导入表格数据时出现错误，请检查导入的表格数据！", string.Empty);
                //throw ex;
            }
        }
        # endregion
        #region 党组织
        private void GetGroupSpDataFromExcel(string FilePath)
        {
            // 定义 Workbook 对象
            XSSFWorkbook myWorkbook;
            XSSFSheet myWorksheet;

            try
            {
                // 用数据流打开 Excel 文件, 
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8))
                {
                    myWorkbook = new XSSFWorkbook(fs);
                    myWorksheet = myWorkbook.GetSheet("党组织信息采集") as XSSFSheet;

                    // Excel 文件中的列头【因为 Excel 中的列头有合并,读取的时候不能读到一行,所以需要人为定义】
                    string[] arrHeads = { "支部名称","党组织代码","建立党组织时间","党组织类别","党组织简介","党组织属地关系",
                                                      "党组织通讯地址","联系电话及传真","上级党组织名称","领导班子当选日期","领导班子届满日期",
                                                      "是否建立下辖组织","下辖组织数量","下辖组织详情","正式党员男数量","正式党员女数量",
                                                      "预备党员男数量","预备党员女数量","单位名称","单位性质","单位人数",
                                                      "是否建立党组织服务","领导班子成员姓名","成员职务","成员联系方式","备注",
                                                      "组织奖惩名称","奖惩日期","奖惩说明","批准奖惩的组织","定位","组织位置","党组织联络人","联络人电话"};
                    // 将 Excel 中的数据读取到 DataTable 中
                    DataTable dt = ReadDataFromExcel(myWorksheet, false, arrHeads, 0);

                    data = DetailListModel11(dt);
                }
            }
            catch (Exception ex)
            {
                JscriptMsg("导入表格数据时出现错误，请检查导入的表格数据！", string.Empty);
                //throw ex;
            }
        }
        # endregion
        #region 党员信息
        public void GetUserSpDataFromExcel(string FilePath)
        {
            // 定义 Workbook 对象
            XSSFWorkbook myWorkbook;
            XSSFSheet myWorksheet;

            try
            {
                // 用数据流打开 Excel 文件, 
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    myWorkbook = new XSSFWorkbook(fs);
                    myWorksheet = myWorkbook.GetSheet("党员信息采集") as XSSFSheet;

                    // Excel 文件中的列头【因为 Excel 中的列头有合并,读取的时候不能读到一行,所以需要人为定义】
                    string[] arrHeads = {"党员姓名","登录账号","登录密码","性别", "生日","民族","身份证号","手机号","联系电话", "电子邮箱",
                        "QQ","微信", "MSN","注册时间", "注册IP","地区", "联系地址","婚姻状况","新阶层人员类型",
                        "行政级别类型","军职级别类型","警衔级别类型","本人子女情况","独生子女奖励","护照情况类型",
                        "优抚对象","收入来源","籍贯地址","居住地址","毕业院校","毕业时间","学历","学位情况","入党时间",
                        "入党介绍人","入党所在支部","现任党内职务", "现所在党组织","进入现支部类型","党员进社区情况","参加组织生活情况","党员奖惩名称","奖惩原因",
                        "党员奖惩批准机关","党员奖惩批准机关级别","原工作单位名称", "原单位员工总数","原组织关系所在单位", "原工作单位类型","原工作岗位类型",
                        "现工作单位名称", "现单位员工总数","现组织关系所在单位", "现工作单位类型","现工作岗位类型",
                        "建立党员服务组", "是否生活困难", "是否取得组织认定","经济状况类型","身体健康情况类型",
                        "生活困难类型","生活困难具体描述","享受帮扶起始时间","享受帮扶截止时间","享受帮扶形式类型","享受帮扶备注",
                        "流动类型","流动党支部联系","流动原因","联系方式","《活动证》号码","党支部联系人","流出地","党支部联系人或联系方式"};
                    // 将 Excel 中的数据读取到 DataTable 中
                    DataTable dt = ReadDataFromExcel(myWorksheet, false, arrHeads, 0);

                    data = DetailListModel(dt);
                }
            }
            catch (Exception ex)
            {
                JscriptMsg("导入表格数据时出现错误，请检查导入的表格数据！", string.Empty);
                //throw ex;
            }
        }
        # endregion
        #region 详情列表模型
        private List<userExcel> DetailListModel(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }
            List<userExcel> result = new List<userExcel>();
            foreach (DataRow row in dt.Rows)
            {
                userExcel item = DataRowToModel(row);
                if (item != null)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        private List<userExcel> DetailListModel11(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }
            List<userExcel> result = new List<userExcel>();
            foreach (DataRow row in dt.Rows)
            {
                userExcel item = DataRowToModel11(row);
                if (item != null)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        private List<userExcel> DetailListModel22(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }
            List<userExcel> result = new List<userExcel>();
            foreach (DataRow row in dt.Rows)
            {
                userExcel item = DataRowToModel22(row);
                if (item != null)
                {
                    result.Add(item);
                }
            }
            return result;
        }
        # endregion
        #region 转换为实体
        public Model.userExcel DataRowToModel(DataRow row)
        {
            Model.userExcel model = new Model.userExcel();
            if (row != null)
            {
                if (row["党员姓名"] != null && row["党员姓名"].ToString() != "")
                {
                    model.dt_Uuser_name = row["党员姓名"].ToString();
                }
                //else
                //{
                //    model.dt_Uuser_name = row["党员姓名"].ToString();
                //}
                if (row["登录账号"] != null && row["登录账号"].ToString() != "")
                {
                    model.dt_Usalt = row["登录账号"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["登录密码"] != null && row["登录密码"].ToString() != "")
                {
                    model.dt_Upassword = row["登录密码"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["电子邮箱"] != null && row["电子邮箱"].ToString() != "")
                {
                    model.dt_Uemail = row["电子邮箱"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                //if (row["头像"] != null && row["头像"].ToString() != "")
                //{
                //    model.dt_Uavatar = row["头像"].ToString();
                //}
                //else
                //{
                //    return null;
                //}
                //if (row["昵称"] != null && row["昵称"].ToString() != "")
                //{
                //    model.dt_Unick_name = row["昵称"].ToString();
                //}
                //else
                //{
                //    return null;
                //}
                if (row["性别"].ToString() != null && row["性别"].ToString() == "男")
                {
                    model.dt_Usex = "0";
                }
                else if (row["性别"].ToString() != null && row["性别"].ToString() == "女")
                {
                    model.dt_Usex = "1";
                }
                //else
                //{
                //    return null;
                //}

                if (row["生日"] != null && row["生日"].ToString() != "")
                {
                    model.dt_Ubirthday = Convert.ToDateTime(row["生日"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["联系电话"] != null && row["联系电话"].ToString() != "")
                {
                    model.dt_Utelphone = row["联系电话"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["地区"] != null && row["地区"].ToString() != "")
                {
                    model.dt_Uarea = row["地区"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["联系地址"] != null && row["联系地址"].ToString() != "")
                {
                    model.dt_Uaddress = row["联系地址"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["QQ"] != null && row["QQ"].ToString() != "")
                {
                    model.dt_Uqq = row["QQ"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["微信"] != null && row["微信"].ToString() != "")
                {
                    model.dt_Uwechat = row["微信"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["MSN"] != null && row["MSN"].ToString() != "")
                {
                    model.dt_Umsn = row["MSN"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["手机号"] != null && row["手机号"].ToString() != "")
                {
                    Regex reg = new Regex("^[0-9]+$");
                    Match ma = reg.Match(row["手机号"].ToString());
                    if (ma.Success)
                    {
                        model.dt_Umobile = row["手机号"].ToString();
                    }
                    else
                    {
                        JscriptMsg("请检查数据填写是否正确！", string.Empty);
                        return null;
                    }
                    model.dt_Umobile = row["手机号"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                //if (row["得分"] != null && row["得分"].ToString() != "")
                //{
                //    model.dt_Upoint = Convert.ToInt32(row["得分"].ToString());
                //}
                //else
                //{
                //    return null;
                //}
                //if (row["经验"] != null && row["经验"].ToString() != "")
                //{
                //    model.dt_Uexp = Convert.ToInt32(row["经验"].ToString());
                //}
                //else
                //{
                //    return null;
                //}
                //if (row["状态"] != null && row["状态"].ToString() != "")
                //{
                //    model.dt_Ustatus = Convert.ToInt32(row["状态"].ToString());
                //}
                //else
                //{
                //    return null;
                //}
                if (row["注册时间"] != null && row["注册时间"].ToString() != "")
                {
                    model.dt_Ureg_time = Convert.ToDateTime(row["注册时间"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["身份证号"] != null && row["身份证号"].ToString() != "")
                {
                    IDCardValidation card = new IDCardValidation();
                    bool result = card.CheckIDCard(row["身份证号"].ToString());
                    if (result == true)
                    {
                        model.dt_Uid_card = row["身份证号"].ToString();
                    }
                    else
                    {
                        JscriptMsg("请检查身份证号填写是否正确!", string.Empty);
                        return null;
                    }
                    model.dt_Uid_card = row["身份证号"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["注册IP"] != null && row["注册IP"].ToString() != "")
                {
                    model.dt_Ureg_ip = row["注册IP"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                //if (row["最后一次登录时间"] != null && row["最后一次登录时间"].ToString() != "")
                //{
                //    model.dt_Ulogin_time = Convert.ToDateTime(row["最后一次登录时间"].ToString());
                //}
                //else
                //{
                //    return null;
                //}

                if (row["民族"] != null && row["民族"].ToString() != "")
                {
                    model.dt_Unation = row["民族"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["婚姻状况"] != null && row["婚姻状况"].ToString() != "未婚")
                {
                    model.dt_Umarital_status =0;
                }
                else if (row["婚姻状况"] != null && row["婚姻状况"].ToString() != "已婚")
                {
                    model.dt_Umarital_status = 1;
                }
                else if (row["婚姻状况"] != null && row["婚姻状况"].ToString() != "丧偶")
                {
                    model.dt_Umarital_status = 2;
                }
                else if (row["婚姻状况"] != null && row["婚姻状况"].ToString() != "离婚")
                {
                    model.dt_Umarital_status = 3;
                }
                //else
                //{
                //    return null;
                //}
                if (row["新阶层人员类型"] != null && row["新阶层人员类型"].ToString() != "")
                {
                    model.u_type = row["新阶层人员类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["行政级别类型"] != null && row["行政级别类型"].ToString() != "")
                {
                    model.u_atype = row["行政级别类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["军职级别类型"] != null && row["军职级别类型"].ToString() != "")
                {
                    model.u_mtype = row["军职级别类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["警衔级别类型"] != null && row["警衔级别类型"].ToString() != "")
                {
                    model.u_ptype = row["警衔级别类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["本人子女情况"] != null && row["本人子女情况"].ToString() != "无")
                {
                    model.dt_Uchildren_info = 0;
                }
                else if (row["本人子女情况"] != null && row["本人子女情况"].ToString() != "独生子女")
                {
                    model.dt_Uchildren_info = 1;
                }
                else if (row["本人子女情况"] != null && row["本人子女情况"].ToString() != "两个以上")
                {
                    model.dt_Uchildren_info = 2;
                }
                //else
                //{
                //    return null;
                //}
                if (row["独生子女奖励"] != null && row["独生子女奖励"].ToString() != "已领取")
                {
                    model.dt_Uonly_child_award = 0;
                }
                else if (row["独生子女奖励"] != null && row["独生子女奖励"].ToString() != "未领取")
                {
                    model.dt_Uonly_child_award = 1;
                }
                //else
                //{
                //    return null;
                //}
                if (row["护照情况类型"] != null && row["护照情况类型"].ToString() != "")
                {
                    model.u_Ptype = row["护照情况类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["优抚对象"] != null && row["优抚对象"].ToString() != "")
                {
                    model.P_eEntitledGroups = row["优抚对象"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["收入来源"] != null && row["收入来源"].ToString() != "")
                {
                    model.P_iIncomeSource = row["收入来源"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["籍贯地址"] != null && row["籍贯地址"].ToString() != "")
                {
                    model.dt_Unative_place = row["籍贯地址"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["居住地址"] != null && row["居住地址"].ToString() != "")
                {
                    model.dt_Ulive_place = row["居住地址"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["毕业院校"] != null && row["毕业院校"].ToString() != "")
                {
                    model.dt_Ugraduate_school = row["毕业院校"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["毕业时间"] != null && row["毕业时间"].ToString() != "")
                {
                    model.dt_Ugraduate_time = Convert.ToDateTime(row["毕业时间"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["学历"] != null && row["学历"].ToString() != "")
                {
                    model.P_Education = row["学历"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["学位情况"] != null && row["学位情况"].ToString() != "名誉博士")
                {
                    model.dt_Udegree_info = 0;
                }
                else if (row["学位情况"] != null && row["学位情况"].ToString() != "博士")
                {
                    model.dt_Udegree_info = 1;
                }
                else if (row["学位情况"] != null && row["学位情况"].ToString() != "硕士")
                {
                    model.dt_Udegree_info = 2;
                }
                else if (row["学位情况"] != null && row["学位情况"].ToString() != "学士")
                {
                    model.dt_Udegree_info = 3;
                }
                //else
                //{
                //    return null;
                //}
                if (row["入党时间"] != null && row["入党时间"].ToString() != "")
                {
                    model.dt_Ujoin_party_time = Convert.ToDateTime(row["入党时间"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["入党介绍人"] != null && row["入党介绍人"].ToString() != "")
                {
                    model.dt_Uparty_membership = row["入党介绍人"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["入党所在支部"] != null && row["入党所在支部"].ToString() != "")
                {
                    model.dt_Ufirst_branch = row["入党所在支部"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["现任党内职务"] != null && row["现任党内职务"].ToString() != "")
                {
                    model.dt_Uparty_job = row["现任党内职务"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["现所在党组织"] != null && row["现所在党组织"].ToString() != "")
                {
                    model.dt_Unow_organiz = row["现所在党组织"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["进入现支部类型"] != null && row["进入现支部类型"].ToString() != "")
                {
                    model.u_gtype = row["进入现支部类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["党员进社区情况"] != null && row["党员进社区情况"].ToString() != "")
                {
                    model.dt_Ucommunity_info = row["党员进社区情况"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["参加组织生活情况"] != null && row["参加组织生活情况"].ToString() != "")
                {
                    model.dt_Ugroup_live = row["参加组织生活情况"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["党员奖惩名称"] != null && row["党员奖惩名称"].ToString() != "")
                {
                    model.u_rtitle = row["党员奖惩名称"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["奖惩原因"] != null && row["奖惩原因"].ToString() != "")
                {
                    model.u_rreasont = row["奖惩原因"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["党员奖惩批准机关"] != null && row["党员奖惩批准机关"].ToString() != "")
                {
                    model.u_rapproval_authority = row["党员奖惩批准机关"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["党员奖惩批准机关级别"] != null && row["党员奖惩批准机关级别"].ToString() != "")
                {
                    model.u_roffice_level = row["党员奖惩批准机关级别"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["原工作单位名称"] != null && row["原工作单位名称"].ToString() != "")
                {
                    model.u_cname = row["原工作单位名称"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["原单位员工总数"] != null && row["原单位员工总数"].ToString() != "")
                {
                    model.u_cemployee_count = Convert.ToInt32(row["原单位员工总数"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["原组织关系所在单位"] != null && row["原组织关系所在单位"].ToString() != "")
                {
                    model.u_crelation_com = row["原组织关系所在单位"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                //if (row["工作类型"] != null && row["工作类型"].ToString() != "")
                //{
                //    model.u_ccom_type_id = row["工作类型"].ToString();
                //}
                //else
                //{
                //    return null;
                //}
                if (row["原工作岗位类型"] != null && row["原工作岗位类型"].ToString() != "")
                {
                    model.u_cpost_type_id = row["原工作岗位类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["原工作单位类型"] != null && row["原工作单位类型"].ToString() != "")
                {
                    model.u_ccom_type_id = row["原工作单位类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["现工作单位名称"] != null && row["现工作单位名称"].ToString() != "")
                {
                    model.u_yname = row["现工作单位名称"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["现单位员工总数"] != null && row["现单位员工总数"].ToString() != "")
                {
                    model.u_yemployee_count = Convert.ToInt32(row["现单位员工总数"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["现组织关系所在单位"] != null && row["现组织关系所在单位"].ToString() != "")
                {
                    model.u_yrelation_com = row["现组织关系所在单位"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                //if (row["工作类型"] != null && row["工作类型"].ToString() != "")
                //{
                //    model.u_ccom_type_id = row["工作类型"].ToString();
                //}
                //else
                //{
                //    return null;
                //}
                if (row["现工作岗位类型"] != null && row["现工作岗位类型"].ToString() != "")
                {
                    model.u_ypost_type_id = row["现工作岗位类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["现工作单位类型"] != null && row["现工作单位类型"].ToString() != "")
                {
                    model.u_ycom_type_id = row["现工作单位类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["建立党员服务组"] != null && row["建立党员服务组"].ToString() == "是")
                {
                    model.u_cservice_organiz = 0;
                }
                else if (row["建立党员服务组"] != null && row["建立党员服务组"].ToString() == "否")
                {
                    model.u_cservice_organiz = 1;
                }
                //else
                //{
                //    return null;
                //}
                //if (row["金额"] != null && row["金额"].ToString() != "")
                //{
                //    Regex reg = new Regex("^[0-9]+$");
                //    Match ma = reg.Match(row["金额"].ToString());
                //    if (ma.Success)
                //    {
                //        model.dt_Uamount = Convert.ToDecimal(row["金额"].ToString());
                //    }
                //    else
                //    {
                //        JscriptMsg("请检查数据填写是否正确！", string.Empty);
                //        return null;
                //    }
                //    model.dt_Uamount = Convert.ToDecimal(row["金额"].ToString());
                //}
                //else
                //{
                //    return null;
                //}
                if (row["是否生活困难"] != null && row["是否生活困难"].ToString() != "是")
                {
                    model.dt_Uis_badly_off =0;
                }
                else if (row["是否生活困难"] != null && row["是否生活困难"].ToString() != "否")
                {
                    model.dt_Uis_badly_off = 1;
                }
                //else
                //{
                //    return null;
                //}
                if (row["是否取得组织认定"] != null && row["是否取得组织认定"].ToString() != "是")
                {
                    model.dt_Uis_organiz_identity = 0;
                }
                else if (row["是否取得组织认定"] != null && row["是否取得组织认定"].ToString() != "否")
                {
                    model.dt_Uis_organiz_identity = 1;
                }
                //else
                //{
                //    return null;
                //}
                if (row["经济状况类型"] != null && row["经济状况类型"].ToString() != "")
                {
                    model.u_Ftype = row["经济状况类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["身体健康情况类型"] != null && row["身体健康情况类型"].ToString() != "")
                {
                    model.u_htype = row["身体健康情况类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                //if (row["患病情况"] != null && row["患病情况"].ToString() != "")
                //{
                //    model.u_hdisease_info = row["患病情况"].ToString();
                //}
                //else
                //{
                //    return null;
                //}
                if (row["生活困难类型"] != null && row["生活困难类型"].ToString() != "")
                {
                    model.u_btype = row["生活困难类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["生活困难具体描述"] != null && row["生活困难具体描述"].ToString() != "")
                {
                    model.dt_Ubadly_off_describe = row["生活困难具体描述"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                //if (row["享受帮扶起始时间"] != null && row["享受帮扶起始时间"].ToString() != "")
                {
                    model.u_estart_time = Convert.ToDateTime(row["享受帮扶起始时间"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["享受帮扶截止时间"] != null && row["享受帮扶截止时间"].ToString() != "")
                {
                    string a = row["享受帮扶截止时间"].ToString();
                    model.u_eend_time = Convert.ToDateTime(row["享受帮扶截止时间"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["享受帮扶备注"] != null && row["享受帮扶备注"].ToString() != "")
                {
                    model.u_eremark = row["享受帮扶备注"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["享受帮扶形式类型"] != null && row["享受帮扶形式类型"].ToString() != "")
                {
                    model.u_Htype = row["享受帮扶形式类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["流动类型"] != null && row["流动类型"].ToString() != "")
                {
                    model.u_lflow_type = row["流动类型"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["流动党支部联系"] != null && row["流动党支部联系"].ToString() != "")
                {
                    model.u_llinkman = row["流动党支部联系"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["流动原因"] != null && row["流动原因"].ToString() != "")
                {
                    model.u_lflow_reason = row["流动原因"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["联系方式"] != null && row["联系方式"].ToString() != "")
                {
                    model.u_lcontact = row["联系方式"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["《活动证》号码"] != null && row["《活动证》号码"].ToString() != "")
                {
                    model.u_lid_number = row["《活动证》号码"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["党支部联系人"] != null && row["党支部联系人"].ToString() != "")
                {
                    model.u_lgroup_linkman = row["党支部联系人"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["流出地"] != null && row["流出地"].ToString() != "")
                {
                    model.u_ldischarge_place = row["流出地"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["党支部联系人或联系方式"] != null && row["党支部联系人或联系方式"].ToString() != "")
                {
                    model.u_lgroup_contact = row["党支部联系人或联系方式"].ToString();
                }
                //else
                //{
                //    return null;
                //}
            }
            return model;
        }
        public Model.userExcel DataRowToModel11(DataRow row)
        {
            Model.userExcel model = new Model.userExcel();
            if (row != null)
            {
                if (row["支部名称"] != null && row["支部名称"].ToString() != "")
                {
                    model.dt_utitle = row["支部名称"].ToString();
                }
                //else
                //{
                //    model.dt_utitle = "";
                //}
                //if (row["管理员"] != null && row["管理员"].ToString() != "")
                //{
                //    model.dt_umanager = row["管理员"].ToString();
                //}
                //else
                //{
                //    return null;
                //}
                if (row["正式党员男数量"] != null && row["正式党员男数量"].ToString() != "")
                {
                    model.dt_uofficial_male_count = Convert.ToInt32(row["正式党员男数量"].ToString());
                }
                //else
                //{
                //    model.dt_uofficial_male_count = 0;
                //}
                if (row["正式党员女数量"] != null && row["正式党员女数量"].ToString() != "")
                {
                    model.dt_uofficial_female_count = Convert.ToInt32(row["正式党员女数量"].ToString());
                }
                //else
                //{
                //    model.dt_uofficial_female_count = 0;
                //}
                if (row["预备党员男数量"] != null && row["预备党员男数量"].ToString() != "")
                {
                    model.dt_uready_male_count = Convert.ToInt32(row["预备党员男数量"].ToString());
                }
                //else
                //{
                //    model.dt_uready_male_count = 0;
                //}
                if (row["预备党员女数量"] != null && row["预备党员女数量"].ToString() != "")
                {
                    model.dt_uready_female_count = Convert.ToInt32(row["预备党员女数量"].ToString());
                }
                //else
                //{
                //    model.dt_uready_female_count = 0;
                //}
                if (row["定位"] != null && row["定位"].ToString() != "")
                {
                    model.dt_uposition = row["定位"].ToString();
                }
                if (row["党组织代码"] != null && row["党组织代码"].ToString() != "")
                {
                    model.dt_uorg_code = row["党组织代码"].ToString();
                }
                //else
                //{
                //    model.dt_uorg_code = "";
                //}
                if (row["建立党组织时间"] != null && row["建立党组织时间"].ToString() != "")
                {
                    model.dt_ucreate_time = Convert.ToDateTime(row["建立党组织时间"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["党组织类别"] != null && row["党组织类别"].ToString() == "党委")
                {
                    model.dt_usort = 0;
                }
                else if (row["党组织类别"] != null && row["党组织类别"].ToString() == "党总支")
                {
                    model.dt_usort = 1;
                }
                else if (row["党组织类别"] != null && row["党组织类别"].ToString() == "党支部")
                {
                    model.dt_usort = 2;
                }
                else if (row["党组织类别"] != null && row["党组织类别"].ToString() == "联合党支部")
                {
                    model.dt_usort = 3;
                }
                //else
                //{
                //    return null;
                //}
                if (row["党组织通讯地址"] != null && row["党组织通讯地址"].ToString() != "")
                {
                    model.dt_ucontact_address = row["党组织通讯地址"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["组织位置"] != null && row["组织位置"].ToString() != "")
                {
                    model.dt_ulocation = row["组织位置"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["上级党组织名称"] != null && row["上级党组织名称"].ToString() != "")
                {
                    model.dt_usuperior_org = row["上级党组织名称"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["是否建立下辖组织"] != null && row["是否建立下辖组织"].ToString() == "无")
                {
                    model.P_sExists = 0;
                }
                else if (row["是否建立下辖组织"] != null && row["是否建立下辖组织"].ToString() == "有")
                {
                    model.P_sExists = 1;
                }
                //else
                //{
                //    return null;
                //}
                if (row["下辖组织数量"] != null && row["下辖组织数量"].ToString() != "")
                {
                    model.P_sCount = Convert.ToInt32(row["下辖组织数量"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["下辖组织详情"] != null && row["下辖组织详情"].ToString() != "")
                {
                    model.P_sInfo = row["下辖组织详情"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["党组织简介"] != null && row["党组织简介"].ToString() != "")
                {
                    model.dt_uintre = row["党组织简介"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["单位名称"] != null && row["单位名称"].ToString() != "")
                {
                    model.u_cname = row["单位名称"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["单位性质"] != null && row["单位性质"].ToString() == "民办非企业")
                {
                    model.u_ccom_nature = "1001";
                }
                else if (row["单位性质"] != null && row["单位性质"].ToString() == "个体工商户")
                {
                    model.u_ccom_nature = "1002";
                }
                else if (row["单位性质"] != null && row["单位性质"].ToString() == "企业")
                {
                    model.u_ccom_nature = "1003";
                }
                else if (row["单位性质"] != null && row["单位性质"].ToString() == "失业单位")
                {
                    model.u_ccom_nature = "1004";
                }
                else if (row["单位性质"] != null && row["单位性质"].ToString() == "政府机构")
                {
                    model.u_ccom_nature = "1005";
                }
                else if (row["单位性质"] != null && row["单位性质"].ToString() == "其他")
                {
                    model.u_ccom_nature = "1006";
                }
                //else
                //{
                //    return null;
                //}
                if (row["单位人数"] != null && row["单位人数"].ToString() != "")
                {
                    model.u_cemployee_count = Convert.ToInt32(row["单位人数"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["是否建立党组织服务"] != null && row["是否建立党组织服务"].ToString() != "")
                {
                    model.u_cservice_organiz = Convert.ToInt32(row["是否建立党组织服务"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["联系电话及传真"] != null && row["联系电话及传真"].ToString() != "")
                {
                    model.dt_uphone_fax = row["联系电话及传真"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["党组织属地关系"] != null && row["党组织属地关系"].ToString() != "")
                {
                    model.dt_uterritory_relation = row["党组织属地关系"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["领导班子当选日期"] != null && row["领导班子当选日期"].ToString() != "")
                {
                    model.dt_uelected_date = Convert.ToDateTime(row["领导班子当选日期"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["领导班子届满日期"] != null && row["领导班子届满日期"].ToString() != "")
                {
                    model.dt_uexpiration_date = Convert.ToDateTime(row["领导班子届满日期"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                //if (row["所属组织"] != null && row["所属组织"].ToString() != "")
                //{
                //    model.u_Lgroup_id = row["所属组织"].ToString();
                //}
                //else
                //{
                //    return null;
                //}
                if (row["领导班子成员姓名"] != null && row["领导班子成员姓名"].ToString() != "")
                {
                    model.u_Lname = row["领导班子成员姓名"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["成员职务"] != null && row["成员职务"].ToString() != "")
                {
                    model.u_Ljob = row["成员职务"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["成员联系方式"] != null && row["成员联系方式"].ToString() != "")
                {
                    model.u_Lcontact_way = row["成员联系方式"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["备注"] != null && row["备注"].ToString() != "")
                {
                    model.u_Lremark = row["备注"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["组织奖惩名称"] != null && row["组织奖惩名称"].ToString() != "")
                {
                    model.P_rTitle = row["组织奖惩名称"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["奖惩日期"] != null && row["奖惩日期"].ToString() != "")
                {
                    model.P_rDateTime = Convert.ToDateTime(row["奖惩日期"].ToString());
                }
                //else
                //{
                //    return null;
                //}
                if (row["奖惩说明"] != null && row["奖惩说明"].ToString() != "")
                {
                    model.P_rContent = row["奖惩说明"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["批准奖惩的组织"] != null && row["批准奖惩的组织"].ToString() != "")
                {
                    model.P_rRatifyOrganiz = row["批准奖惩的组织"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                //if (row["联系电话"] != null && row["联系电话"].ToString() != "")
                //{
                //    model.dt_uphone = row["联系电话"].ToString();
                //}
                //else
                //{
                //    return null;
                //}
                //if (row["党组织书记姓名"] != null && row["党组织书记姓名"].ToString() != "")
                //{
                //    model.dt_usecretary_name = row["党组织书记姓名"].ToString();
                //}
                //else
                //{
                //    return null;
                //}
                if (row["党组织联络人"] != null && row["党组织联络人"].ToString() != "")
                {
                    model.dt_ucontact_person = row["党组织联络人"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["联络人电话"] != null && row["联络人电话"].ToString() != "")
                {
                    model.dt_ucontact_person_tel = row["联络人电话"].ToString();
                }
                //else
                //{
                //    return null;
                //}

            }
            return model;

        }
        public Model.userExcel DataRowToModel22(DataRow row)
        {
            Model.userExcel model = new Model.userExcel();
            if (row != null)
            {
                if (row["组织名称"] != null && row["组织名称"].ToString() != "")
                {
                    model.dt_utitle = row["组织名称"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["上级党组织"] != null && row["上级党组织"].ToString() != "")
                {
                    model.dt_upid = row["上级党组织"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["书记"] != null && row["书记"].ToString() != "")
                {
                    model.dt_umanager_id = row["书记"].ToString();
                }
                //else
                //{
                //    return null;
                //}
                if (row["手机号"] != null && row["手机号"].ToString() != "")
                {
                    Regex reg = new Regex("^[0-9]+$");
                    Match ma = reg.Match(row["手机号"].ToString());
                    if (ma.Success)
                    {
                        model.dt_Umobile = row["手机号"].ToString();
                    }
                    else
                    {
                        JscriptMsg("请检查数据填写是否正确！", string.Empty);
                        return null;
                    }
                    model.dt_Umobile = row["手机号"].ToString();
                }
                //else
                //{
                //    return null;
                //}
            }
            return model;
        }
        #endregion
        #region 将导入的Excle存入DataTable
        public static DataTable ReadDataFromExcel(ISheet mySheet, bool isColumnName, string[] arrHeads, int startRow)
        {
            DataTable dt = new DataTable();
            DataColumn column;
            DataRow dataRow;
            IRow row;
            ICell cell;
            try
            {
                if (mySheet != null)
                {
                    //代码总页数   
                    int rowCount = mySheet.LastRowNum;
                    if (rowCount > startRow)
                    {
                        //第五行
                        IRow firstRow = mySheet.GetRow(startRow);
                        // 列数
                        int columnCount = firstRow.LastCellNum;

                        //构建datatable的列 
                        if (isColumnName)
                        {
                            for (int i = firstRow.FirstCellNum; i < columnCount; ++i)
                            {
                                cell = firstRow.GetCell(i);
                                if (cell != null)
                                {
                                    if (cell.StringCellValue != null)
                                    {
                                        column = new DataColumn(cell.StringCellValue);
                                        dt.Columns.Add(column);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = firstRow.FirstCellNum; i < columnCount; ++i)
                            {
                                column = new DataColumn(arrHeads[i]);
                                dt.Columns.Add(column);
                            }
                        }


                        //填充行
                        for (int i = startRow + 1; i <= rowCount; ++i)
                        {
                            row = mySheet.GetRow(i);
                            if (row == null) continue;
                            dataRow = dt.NewRow();
                            for (int j = row.FirstCellNum; j < columnCount; ++j)
                            {
                                cell = row.GetCell(j);
                                if (cell == null)
                                {
                                    dataRow[j] = "";
                                }
                                else
                                {
                                    switch (cell.CellType)
                                    {
                                        case CellType.Blank:
                                            dataRow[j] = "";
                                            break;
                                        case CellType.Numeric:

                                            if (HSSFDateUtil.IsCellDateFormatted(cell))
                                            {
                                                dataRow[j] = cell.DateCellValue;
                                            }
                                            else
                                            {
                                                dataRow[j] = cell.NumericCellValue.ToString();
                                            }
                                            break;
                                        case CellType.String:
                                            dataRow[j] = cell.StringCellValue;
                                            break;
                                        case CellType.Formula:
                                            dataRow[j] = cell.NumericCellValue.ToString();
                                            break;

                                    }
                                }
                            }
                            dt.Rows.Add(dataRow);
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region 添加党组织表
        public Boolean AddGroup(userExcel model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.Append("select * from dt_user_groups where title = '" + model.dt_utitle + "'");
                        DataSet ds = DbHelperSQL.Query(sql.ToString());
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            string subid = AddSubordinateGroup(model);
                            //string typeid = AddCompanyType(model);
                            string rewardsid = AddRewardsAndPunishment(model);
                            string companyid = addGcompany(model);
                            string leadid = addLeadInfo(model);
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("insert into dt_user_groups(");
                            strSql.Append("title,grade,upgrade_exp,amount,point,discount,is_default,is_upgrade,is_lock,manager,position,create_time,sort,contact_address,phone,superior_org,sub_org_id,");
                            strSql.Append("intre,status,org_code,secretary_name,contact_person,contact_person_tel,rewards_punishment_id,location,phone_fax,territory_relation,");
                            strSql.Append("elected_date,expiration_date,company_info_id,lead_info_id,official_male_count,official_female_count,ready_male_count,ready_female_count)");
                            strSql.Append(" values(");
                            strSql.Append("@title,@grade,@upgrade_exp,@amount,@point,@discount,@is_default,@is_upgrade,@is_lock,@manager,@position,@create_time,@sort,@contact_address,@phone,@superior_org,@sub_org_id,");
                            strSql.Append("@intre,@status,@org_code,@secretary_name,@contact_person,@contact_person_tel,@rewards_punishment_id,@location,@phone_fax,@territory_relation,");
                            strSql.Append("@elected_date,@expiration_date,@company_info_id,@lead_info_id,@official_male_count,@official_female_count,@ready_male_count,@ready_female_count)");
                            strSql.Append(";select @@IDENTITY");
                            SqlParameter[] parameters = {
                                  new SqlParameter("@title",SqlDbType.NVarChar,100),
                                  new SqlParameter("@grade",SqlDbType.Int,4),
                                  new SqlParameter("@upgrade_exp",SqlDbType.Int,4),
                                  new SqlParameter("@amount",SqlDbType.Decimal,9),
                                  new SqlParameter("@point",SqlDbType.Int,4),
                                  new SqlParameter("@discount",SqlDbType.Int,4),
                                  new SqlParameter("@is_default",SqlDbType.Int,4),
                                  new SqlParameter("@is_upgrade",SqlDbType.Int,4),
                                  new SqlParameter("@is_lock",SqlDbType.Int,4),
                                  new SqlParameter("@manager",SqlDbType.NVarChar,50),
                                  new SqlParameter("@position",SqlDbType.NVarChar,50),
                                  new SqlParameter("@create_time",SqlDbType.DateTime),
                                  new SqlParameter("@sort",SqlDbType.Int,4),
                                  new SqlParameter("@contact_address",SqlDbType.NVarChar,100),
                                  new SqlParameter("@phone",SqlDbType.NVarChar,50),
                                  new SqlParameter("@superior_org",SqlDbType.NVarChar,50),
                                  new SqlParameter("@sub_org_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@intre",SqlDbType.NText,1000),
                                  new SqlParameter("@status",SqlDbType.Int,4),
                                  new SqlParameter("@org_code",SqlDbType.NVarChar,50),
                                  new SqlParameter("@secretary_name",SqlDbType.NVarChar,50),
                                  new SqlParameter("@contact_person",SqlDbType.NVarChar,50),
                                  new SqlParameter("@contact_person_tel",SqlDbType.NVarChar,50),
                                  new SqlParameter("@rewards_punishment_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@location",SqlDbType.NVarChar,50),
                                  new SqlParameter("@phone_fax",SqlDbType.NVarChar,100),
                                  new SqlParameter("@territory_relation",SqlDbType.NVarChar,50),
                                  new SqlParameter("@elected_date",SqlDbType.DateTime),
                                  new SqlParameter("@expiration_date",SqlDbType.DateTime),
                                  new SqlParameter("@company_info_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@lead_info_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@official_male_count",SqlDbType.Int,4),
                                  new SqlParameter("@official_female_count",SqlDbType.Int,4),
                                  new SqlParameter("@ready_male_count",SqlDbType.Int,4),
                                  new SqlParameter("@ready_female_count",SqlDbType.Int,4),
                            };
                            parameters[0].Value = model.dt_utitle;
                            parameters[1].Value = model.dt_ugrade;
                            parameters[2].Value = model.dt_uupgrade_exp;
                            parameters[3].Value = model.dt_uamount;
                            parameters[4].Value = model.dt_upoint;
                            parameters[5].Value = model.dt_udiscount;
                            parameters[6].Value = model.dt_uis_default;
                            parameters[7].Value = model.dt_uis_upgrade;
                            parameters[8].Value = model.dt_uis_lock;
                            parameters[9].Value = model.dt_umanager;
                            parameters[10].Value = model.dt_uposition;
                            parameters[11].Value = model.dt_ucreate_time;
                            parameters[12].Value = model.dt_usort;
                            parameters[13].Value = model.dt_ucontact_address;
                            parameters[14].Value = model.dt_uphone;
                            parameters[15].Value = model.dt_usuperior_org;
                            parameters[16].Value = subid;
                            parameters[17].Value = model.dt_uintre;
                            parameters[18].Value = model.dt_ustatus;
                            parameters[19].Value = model.dt_uorg_code;
                            parameters[20].Value = model.dt_usecretary_name;
                            parameters[21].Value = model.dt_ucontact_person;
                            parameters[22].Value = model.dt_ucontact_person_tel;
                            parameters[23].Value = rewardsid;
                            parameters[24].Value = model.dt_ulocation;
                            parameters[25].Value = model.dt_uphone_fax;
                            parameters[26].Value = model.dt_uterritory_relation;
                            parameters[27].Value = model.dt_uelected_date;
                            parameters[28].Value = model.dt_uexpiration_date;
                            parameters[29].Value = companyid;
                            parameters[30].Value = leadid;
                            parameters[31].Value = model.dt_uofficial_male_count;
                            parameters[32].Value = model.dt_uofficial_female_count;
                            parameters[33].Value = model.dt_uready_male_count;
                            parameters[34].Value = model.dt_uready_female_count;
                            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        }
                        else
                        {
                            JscriptMsg("该组织已存在！", string.Empty);

                        }
                        trans.Commit();


                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion
        #region 添加人员表
        public bool AddUser(List<userExcel> model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (Model.userExcel item in model)
                        {

                            if (item.dt_Uid_card != null)
                            {
                                string m = item.dt_Unow_organiz;
                                string[] arr = m.Split('，');
                                StringBuilder groupid = new StringBuilder();
                                foreach (var b in arr)
                                {
                                    StringBuilder sql = new StringBuilder();
                                    sql.Append("select Convert(varchar,id) as dt_Ugroup_id from dt_user_groups where title ='" + b + "'");
                                    DataSet aa = DbHelperSQL.Query(sql.ToString());
                                    DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                                    if (aa.Tables[0] != null && aa.Tables[0].Rows.Count > 0)
                                    {
                                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                                        groupid.Append(",");
                                        groupid.Append(gid.dt_Ugroup_id);
                                    }
                                }
                                string group_id = groupid + ",";
                                StringBuilder ff = new StringBuilder();
                                ff.Append("select * from dt_users where id_card = '" + item.dt_Uid_card + "'");
                                DataSet ds = DbHelperSQL.Query(ff.ToString());
                                if (ds.Tables[0].Rows.Count == 0)
                                {
                                    string entitledid = selectEntitledGroups(item);
                                    string educationid = selectEducation(item);
                                    //string typeid = selectCompanyType(item);
                                    string nsid = selectNewClass(item);
                                    string arid = selectAdminRank(item);
                                    string mrid = selectMilitaryRank(item);
                                    string prid = selectPoliceRank(item);
                                    string gtid = selectGroupType(item);
                                    string rpid = addRewardPunishment(item);
                                    string cid = addCcompany(item);
                                    string yid = addYcompany(item);
                                    string ffid = selectFinancialFituation(item);
                                    string hcid = selectHealthyCondition(item);
                                    string borid = selectBadlyOffReason(item);
                                    string ehid = addEnjoyHelp(item);
                                    string fcid = addFloatCommie(item);
                                    string siid = selectIncomeSource(item);
                                    string upid = selectPassportInfo(item);
                                    StringBuilder strSql = new StringBuilder();
                                    strSql.Append("insert into dt_users(");
                                    strSql.Append("group_id,user_name,salt,password,mobile,email,avatar,nick_name,sex,birthday,telphone,area,address,qq,wechat,msn,amount,point,");
                                    strSql.Append("exp,status,reg_time,reg_ip,id_card,nation,marital_status,education_info_id,native_place,live_place,join_party_time,party_job,");
                                    strSql.Append("entitled_group_id,income_source_id,role_id,new_class_id,administration_rank_id,military_rank_id,police_rank_id,children_info,");
                                    strSql.Append("only_child_award,graduate_school,graduate_time,degree_info,party_membership,first_branch,now_organiz,");
                                    strSql.Append("group_type_id,community_info,group_live,reward_punishment_id,former_company_id,now_company_id,is_badly_off,is_organiz_identity,financial_situation_id,");
                                    strSql.Append("healthy_condition_id,badly_off_reason_id,badly_off_describe,enjoy_help_id,float_commie_id,passport_info_id)");
                                    strSql.Append(" values(");
                                    strSql.Append("@group_id,@user_name,@salt,@password,@mobile,@email,@avatar,@nick_name,@sex,@birthday,@telphone,@area,@address,@qq,@wechat,@msn,@amount,@point,");
                                    strSql.Append("@exp,@status,@reg_time,@reg_ip,@id_card,@nation,@marital_status,@education_info_id,@native_place,@live_place,@join_party_time,@party_job,");
                                    strSql.Append("@entitled_group_id,@income_source_id,@role_id,@new_class_id,@administration_rank_id,@military_rank_id,@police_rank_id,@children_info,");
                                    strSql.Append("@only_child_award,@graduate_school,@graduate_time,@degree_info,@party_membership,@first_branch,@now_organiz,");
                                    strSql.Append("@group_type_id,@community_info,@group_live,@reward_punishment_id,@former_company_id,@now_company_id,@is_badly_off,@is_organiz_identity,@financial_situation_id,");
                                    strSql.Append("@healthy_condition_id,@badly_off_reason_id,@badly_off_describe,@enjoy_help_id,@float_commie_id,@passport_info_id)");
                                    strSql.Append(";select @@IDENTITY");
                                    SqlParameter[] parameters = {
                                  new SqlParameter("@group_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@user_name",SqlDbType.NVarChar,100),
                                  new SqlParameter("@salt",SqlDbType.NVarChar,20),
                                  new SqlParameter("@password",SqlDbType.NVarChar,100),
                                  new SqlParameter("@mobile",SqlDbType.NVarChar,20),
                                  new SqlParameter("@email",SqlDbType.NVarChar,50),
                                  new SqlParameter("@avatar",SqlDbType.NVarChar,225),
                                  new SqlParameter("@nick_name",SqlDbType.NVarChar,100),
                                  new SqlParameter("@sex",SqlDbType.NVarChar,20),
                                  new SqlParameter("@birthday",SqlDbType.DateTime),
                                  new SqlParameter("@telphone",SqlDbType.NVarChar,50),
                                  new SqlParameter("@area",SqlDbType.NVarChar,225),
                                  new SqlParameter("@address",SqlDbType.NVarChar,225),
                                  new SqlParameter("@qq",SqlDbType.NVarChar,20),
                                  new SqlParameter("@wechat",SqlDbType.NVarChar,50),
                                  new SqlParameter("@msn",SqlDbType.NVarChar,100),
                                  new SqlParameter("@amount",SqlDbType.Decimal,9),
                                  new SqlParameter("@point",SqlDbType.Int,4),
                                  new SqlParameter("@exp",SqlDbType.Int,4),
                                  new SqlParameter("@status",SqlDbType.Int,4),
                                  new SqlParameter("@reg_time",SqlDbType.DateTime),
                                  new SqlParameter("@reg_ip",SqlDbType.NVarChar,20),
                                  new SqlParameter("@id_card",SqlDbType.NVarChar,20),
                                  new SqlParameter("@nation",SqlDbType.NVarChar,50),
                                  new SqlParameter("@marital_status",SqlDbType.Int,4),
                                  new SqlParameter("@education_info_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@native_place",SqlDbType.NVarChar,100),
                                  new SqlParameter("@live_place",SqlDbType.NVarChar,200),
                                  new SqlParameter("@join_party_time",SqlDbType.DateTime),
                                  new SqlParameter("@party_job",SqlDbType.NVarChar,50),
                                  new SqlParameter("@entitled_group_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@income_source_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@role_id",SqlDbType.Int,4),
                                  new SqlParameter("@new_class_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@administration_rank_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@military_rank_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@police_rank_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@children_info",SqlDbType.Int,4),
                                  new SqlParameter("@only_child_award",SqlDbType.Int,4),
                                  new SqlParameter("@graduate_school",SqlDbType.NVarChar,100),
                                  new SqlParameter("@graduate_time",SqlDbType.DateTime),
                                  new SqlParameter("@degree_info",SqlDbType.Int,4),
                                  new SqlParameter("@party_membership",SqlDbType.NVarChar,50),
                                  new SqlParameter("@first_branch",SqlDbType.NVarChar,50),
                                  new SqlParameter("@now_organiz",SqlDbType.NVarChar,50),
                                  new SqlParameter("@group_type_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@community_info",SqlDbType.NVarChar,100),
                                  new SqlParameter("@group_live",SqlDbType.NVarChar,100),
                                  new SqlParameter("@reward_punishment_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@former_company_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@now_company_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@is_badly_off",SqlDbType.Int,4),
                                  new SqlParameter("@is_organiz_identity",SqlDbType.Int,4),
                                  new SqlParameter("@financial_situation_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@healthy_condition_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@badly_off_reason_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@badly_off_describe",SqlDbType.NText),
                                  new SqlParameter("@enjoy_help_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@float_commie_id",SqlDbType.NVarChar,50),
                                  new SqlParameter("@passport_info_id",SqlDbType.NVarChar,50),
                            };
                                    parameters[0].Value = group_id;
                                    parameters[1].Value = item.dt_Uuser_name;
                                    parameters[2].Value = item.dt_Usalt;
                                    parameters[3].Value = item.dt_Upassword;
                                    parameters[4].Value = item.dt_Umobile;
                                    parameters[5].Value = item.dt_Uemail;
                                    parameters[6].Value = item.dt_Uavatar;
                                    parameters[7].Value = item.dt_Unick_name;
                                    parameters[8].Value = item.dt_Usex;
                                    parameters[9].Value = item.dt_Ubirthday;
                                    parameters[10].Value = item.dt_Utelphone;
                                    parameters[11].Value = item.dt_Uarea;
                                    parameters[12].Value = item.dt_Uaddress;
                                    parameters[13].Value = item.dt_Uqq;
                                    parameters[14].Value = item.dt_Uwechat;
                                    parameters[15].Value = item.dt_Umsn;
                                    parameters[16].Value = item.dt_Uamount;
                                    parameters[17].Value = item.dt_Upoint;
                                    parameters[18].Value = item.dt_Uexp;
                                    parameters[19].Value = 1;
                                    parameters[20].Value = item.dt_Ureg_time;
                                    parameters[21].Value = item.dt_Ureg_ip;
                                    parameters[22].Value = item.dt_Uid_card;
                                    parameters[23].Value = item.dt_Unation;
                                    parameters[24].Value = item.dt_Umarital_status;
                                    parameters[25].Value = educationid;
                                    parameters[26].Value = item.dt_Unative_place;
                                    parameters[27].Value = item.dt_Ulive_place;
                                    parameters[28].Value = item.dt_Ujoin_party_time;
                                    parameters[29].Value = item.dt_Uparty_job;
                                    parameters[30].Value = entitledid;
                                    parameters[31].Value = siid;
                                    parameters[32].Value = item.dt_Urole_id;
                                    parameters[33].Value = nsid;
                                    parameters[34].Value = arid;
                                    parameters[35].Value = mrid;
                                    parameters[36].Value = prid;
                                    parameters[37].Value = item.dt_Uchildren_info;
                                    parameters[38].Value = item.dt_Uonly_child_award;
                                    parameters[39].Value = item.dt_Ugraduate_school;
                                    parameters[40].Value = item.dt_Ugraduate_time;
                                    parameters[41].Value = item.dt_Udegree_info;
                                    parameters[42].Value = item.dt_Uparty_membership;
                                    parameters[43].Value = item.dt_Ufirst_branch;
                                    parameters[44].Value = item.dt_Unow_organiz;
                                    parameters[45].Value = gtid;
                                    parameters[46].Value = item.dt_Ucommunity_info;
                                    parameters[47].Value = item.dt_Ugroup_live;
                                    parameters[48].Value = rpid;
                                    parameters[49].Value = yid;
                                    parameters[50].Value = cid;
                                    parameters[51].Value = item.dt_Uis_badly_off;
                                    parameters[52].Value = item.dt_Uis_organiz_identity;
                                    parameters[53].Value = ffid;
                                    parameters[54].Value = hcid;
                                    parameters[55].Value = borid;
                                    parameters[56].Value = item.dt_Ubadly_off_describe;
                                    parameters[57].Value = ehid;
                                    parameters[58].Value = fcid;
                                    parameters[59].Value = upid;
                                    object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                                }

                                else
                                {
                                    JscriptMsg("该用户已存在！", string.Empty);
                                }

                            }

                            else
                            {
                                JscriptMsg("请填好数据，在添加！", string.Empty);
                            }

                        }
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion
        #region 添加上级党组织和书记
        public bool AddManager(List<userExcel> model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {

                        foreach (Model.userExcel item in model)
                        {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("update dt_user_groups set pid=(SELECT id FROM (select * from dt_user_groups) as a WHERE title=@pid),manager=@manager_id,");
                            strSql.Append("manager_id=(SELECT id FROM dt_users WHERE user_name = @manager_id and mobile = @mobile) where id=(");
                            strSql.Append("SELECT id FROM (select * from dt_user_groups) as b WHERE title= @title)");

                            SqlParameter[] parameters = {
                                    new SqlParameter("@pid",SqlDbType.NVarChar,100),
                                    new SqlParameter("@manager",SqlDbType.NVarChar,50),
                                    new SqlParameter("@manager_id",SqlDbType.NVarChar,50),
                                    new SqlParameter("@mobile",SqlDbType.NVarChar,50),
                                    new SqlParameter("@title",SqlDbType.NVarChar,100)
                                };
                            parameters[0].Value = item.dt_upid;
                            parameters[1].Value = item.dt_umanager_id;
                            parameters[2].Value = item.dt_umanager_id;
                            parameters[3].Value = item.dt_Umobile;
                            parameters[4].Value = item.dt_utitle;
                            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                            trans.Commit();
                        }

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion
        #region 添加新阶层人员信息
        public string addNewClass(userExcel model)
        {
            string nsid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_new_class(");
                        strSql.Append("id,type,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@type,@create_time,@create_user,@update_time,@update_user,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@type",SqlDbType.NVarChar,100),
                                new SqlParameter("@status",SqlDbType.Int,4),
                            };
                        parameters[0].Value = nsid;
                        parameters[1].Value = model.u_type;
                        parameters[2].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return nsid;
        }
        #endregion
        #region 添加行政级别信息
        public string addAdminRank(userExcel model)
        {
            string arid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_administration_rank(");
                        strSql.Append("id,type,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@type,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@type",SqlDbType.NVarChar,100),
                                new SqlParameter("@status",SqlDbType.Int,4),
                            };
                        parameters[0].Value = arid;
                        parameters[1].Value = model.u_atype;
                        parameters[2].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return arid;
        }
        #endregion
        #region 添加军职级别信息
        public string addMilitaryRank(userExcel model)
        {
            string mrid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_military_rank(");
                        strSql.Append("id,type,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@type,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@type",SqlDbType.NVarChar,100),
                                new SqlParameter("@status",SqlDbType.Int,4),
                            };
                        parameters[0].Value = mrid;
                        parameters[1].Value = model.u_mtype;
                        parameters[2].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return mrid;
        }
        #endregion
        #region 添加警衔级别信息
        public string addPoliceRank(userExcel model)
        {
            string prid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_police_rank(");
                        strSql.Append("id,type,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@type,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@type",SqlDbType.NVarChar,100),
                                new SqlParameter("@status",SqlDbType.Int,4),
                            };
                        parameters[0].Value = prid;
                        parameters[1].Value = model.u_ptype;
                        parameters[2].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return prid;
        }
        #endregion
        #region 添加护照信息
        public string addPassportInfo(userExcel model)
        {
            string piid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_passport_info(");
                        strSql.Append("id,type,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@type,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@type",SqlDbType.NVarChar,100),
                                new SqlParameter("@status",SqlDbType.Int,4),
                            };
                        parameters[0].Value = piid;
                        parameters[1].Value = model.u_Ptype;
                        parameters[2].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return piid;
        }
        #endregion
        #region 添加进入现支部类型信息
        public string addGroupType(userExcel model)
        {
            string gtid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_group_type(");
                        strSql.Append("id,type,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@type,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@type",SqlDbType.NVarChar,100),
                                new SqlParameter("@status",SqlDbType.Int,4),
                            };
                        parameters[0].Value = gtid;
                        parameters[1].Value = model.u_gtype;
                        parameters[2].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return gtid;
        }
        #endregion
        #region 添加生活困难原因信息
        public string addBadlyOffReason(userExcel model)
        {
            string borid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_badly_off_reason(");
                        strSql.Append("id,type,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@type,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@type",SqlDbType.NVarChar,100),
                                new SqlParameter("@status",SqlDbType.Int,4),
                            };
                        parameters[0].Value = borid;
                        parameters[1].Value = model.u_btype;
                        parameters[2].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return borid;
        }
        #endregion
        #region 添加下辖组织情况
        public string AddSubordinateGroup(userExcel model)
        {
            string subid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into P_SubordinateGroupInfo(");
                        strSql.Append("P_Id,P_Exists,P_Count,P_Info,P_Status)");
                        strSql.Append(" values(");
                        strSql.Append("@P_Id,@P_Exists,@P_Count,@P_Info,@P_Status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Exists",SqlDbType.Int,4),
                                new SqlParameter("@P_Count",SqlDbType.Int,4),
                                new SqlParameter("@P_Info",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Status",SqlDbType.Int,4),
                            };
                        parameters[0].Value = subid;
                        parameters[1].Value = model.P_sExists;
                        parameters[2].Value = model.P_sCount;
                        parameters[3].Value = model.P_sInfo;
                        parameters[4].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return subid;
        }
        #endregion
        #region 添加身体健康情况信息
        public string addHealthyCondition(userExcel model)
        {
            string hcid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_healthy_condition(");
                        strSql.Append("id,type,status,disease_info)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@type,@status,@disease_info)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@type",SqlDbType.NVarChar,100),
                                new SqlParameter("@status",SqlDbType.Int,4),
                                new SqlParameter("@status",SqlDbType.NVarChar,100),
                            };
                        parameters[0].Value = hcid;
                        parameters[1].Value = model.u_htype;
                        parameters[2].Value = 0;
                        parameters[3].Value = model.u_hdisease_info;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return hcid;
        }
        #endregion
        #region 添加帮扶形式类型
        public string addHelpWay(userExcel model)
        {
            string hwid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_help_way(");
                        strSql.Append("id,type,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@type,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@type",SqlDbType.NVarChar,100),
                                new SqlParameter("@status",SqlDbType.Int,4),
                            };
                        parameters[0].Value = hwid;
                        parameters[1].Value = model.u_Htype;
                        parameters[2].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return hwid;
        }
        #endregion
        #region 添加经济状况信息
        public string addFinancialFituation(userExcel model)
        {
            string ffid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_financial_situation(");
                        strSql.Append("id,type,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@type,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@type",SqlDbType.NVarChar,100),
                                new SqlParameter("@status",SqlDbType.Int,4),
                            };
                        parameters[0].Value = ffid;
                        parameters[1].Value = model.u_Ftype;
                        parameters[2].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return ffid;
        }
        #endregion
        #region 添加收入来源
        public string AddIncomeSource(userExcel model)
        {
            string incomeid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_IncomeSourceInfo(");
                        strSql.Append("P_Id,P_IncomeSource,P_Status)");
                        strSql.Append(" values(");
                        strSql.Append("@P_Id,@P_IncomeSource,@P_Status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_IncomeSource",SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status",SqlDbType.Int,4)
                            };
                        parameters[0].Value = incomeid;
                        parameters[1].Value = model.P_iIncomeSource;
                        parameters[2].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return incomeid;
        }
        #endregion
        #region 添加优抚对象
        public string AddEntitledGroups(userExcel model)
        {

            string entitledid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into P_EntitledGroupsInfo(");
                        strSql.Append("P_Id,P_EntitledGroups,P_Status)");
                        strSql.Append(" values(");
                        strSql.Append("@P_Id,@P_EntitledGroups,@P_Status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_EntitledGroups",SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status",SqlDbType.Int,4)
                            };
                        parameters[0].Value = entitledid;
                        parameters[1].Value = model.P_eEntitledGroups;
                        parameters[2].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return entitledid;
        }

        #endregion
        #region 添加学历信息
        public string AddEducation(userExcel model)
        {
            string educationid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into P_EducationInfo(");
                        strSql.Append("P_Id,P_Education,P_Status)");
                        strSql.Append(" values(");
                        strSql.Append("@P_Id,@P_Education,@P_Status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Education",SqlDbType.NVarChar,100),
                                new SqlParameter("@P_Status",SqlDbType.Int,4)
                            };
                        parameters[0].Value = educationid;
                        parameters[1].Value = model.P_Education;
                        parameters[2].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return educationid;
        }
        #endregion
        #region 添加奖惩信息表
        public string AddRewardsAndPunishment(userExcel model)
        {
            string rewardsid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into P_RewardsAndPunishment(");
                        strSql.Append("P_Id,P_Title,P_DateTime,P_Content,P_RatifyOrganiz,P_Status)");
                        strSql.Append(" values(");
                        strSql.Append("@P_Id, @P_Title,@P_DateTime,@P_Content,@P_RatifyOrganiz,@P_Status)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                                new SqlParameter("@P_Id",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Title",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_DateTime",SqlDbType.DateTime),
                                new SqlParameter("@P_Content",SqlDbType.NText,1000),
                                new SqlParameter("@P_RatifyOrganiz",SqlDbType.NVarChar,50),
                                new SqlParameter("@P_Status",SqlDbType.Int,4)
                            };
                        parameters[0].Value = rewardsid;
                        parameters[1].Value = model.P_rTitle;
                        parameters[2].Value = model.P_rDateTime;
                        parameters[3].Value = model.P_rContent;
                        parameters[4].Value = model.P_rRatifyOrganiz;
                        parameters[5].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return rewardsid;
        }
        #endregion
        #region 添加现工作单位信息
        public string addCcompany(userExcel model)
        {
            string cid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string a = selectCompanyType(model);
                        string b = selectPostType(model);
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_company_type(");
                        strSql.Append("id,name,employee_count,relation_com,com_type_id,post_type_id,status,type)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@name,@employee_count,@relation_com,@com_type_id,@post_type_id,@status,@type)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@name",SqlDbType.NVarChar,50),
                                new SqlParameter("@employee_count",SqlDbType.Int,4),
                                new SqlParameter("@relation_com",SqlDbType.NVarChar,100),
                                new SqlParameter("@com_type_id",SqlDbType.NVarChar,50),
                                new SqlParameter("@post_type_id",SqlDbType.NVarChar,50),
                                new SqlParameter("@status",SqlDbType.Int,4),
                                new SqlParameter("@type",SqlDbType.Int,4)
                            };
                        parameters[0].Value = cid;
                        parameters[1].Value = model.u_cname;
                        parameters[2].Value = model.u_cemployee_count;
                        parameters[3].Value = model.u_crelation_com;
                        parameters[4].Value = a;
                        parameters[5].Value = b;
                        parameters[6].Value = 0;
                        parameters[7].Value = 1;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return cid;
        }
        #endregion
        #region 添加原工作单位信息
        public string addYcompany(userExcel model)
        {
            string yid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string a = selectCompanyType(model);
                        string b = selectPostType(model);
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_company_type(");
                        strSql.Append("id,name,employee_count,relation_com,com_type_id,post_type_id,status,type)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@name,@employee_count,@relation_com,@com_type_id,@post_type_id,@status,@type)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@name",SqlDbType.NVarChar,50),
                                new SqlParameter("@employee_count",SqlDbType.Int,4),
                                new SqlParameter("@relation_com",SqlDbType.NVarChar,100),
                                new SqlParameter("@com_type_id",SqlDbType.NVarChar,50),
                                new SqlParameter("@post_type_id",SqlDbType.NVarChar,50),
                                new SqlParameter("@status",SqlDbType.Int,4),
                                new SqlParameter("@type",SqlDbType.Int,4)
                            };
                        parameters[0].Value = yid;
                        parameters[1].Value = model.u_cname;
                        parameters[2].Value = model.u_cemployee_count;
                        parameters[3].Value = model.u_crelation_com;
                        parameters[4].Value = a;
                        parameters[5].Value = b;
                        parameters[6].Value = 0;
                        parameters[7].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return yid;
        }
        #endregion
        #region 添加组织单位信息
        public string addGcompany(userExcel model)
        {
            string yid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_company_type(");
                        strSql.Append("id,name,employee_count,status,com_nature,service_organiz)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@name,@employee_count,@status,@com_nature,@service_organiz)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@name",SqlDbType.NVarChar,50),
                                new SqlParameter("@employee_count",SqlDbType.Int,4),
                                new SqlParameter("@status",SqlDbType.Int,4),
                                new SqlParameter("@com_nature",SqlDbType.NVarChar,50),
                                new SqlParameter("@service_organiz",SqlDbType.NVarChar,50)
                            };
                        parameters[0].Value = yid;
                        parameters[1].Value = model.u_cname;
                        parameters[2].Value = model.u_cemployee_count;
                        parameters[3].Value = 0;
                        parameters[4].Value = model.u_ccom_nature;
                        parameters[5].Value = model.u_cservice_organiz;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return yid;
        }
        #endregion
        #region 添加享受帮扶情况信息
        public string addEnjoyHelp(userExcel model)
        {
            string ehid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string a = selectHelpWay(model);
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_enjoy_help(");
                        strSql.Append("id,start_time,end_time,help_way_id,remark,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@start_time,@end_time,@help_way_id,@remark,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@start_time",SqlDbType.DateTime),
                                new SqlParameter("@end_time",SqlDbType.DateTime),
                                new SqlParameter("@help_way_id",SqlDbType.NVarChar,50),
                                new SqlParameter("@remark",SqlDbType.NText),
                                new SqlParameter("@status",SqlDbType.Int,4),

                            };
                        parameters[0].Value = ehid;
                        parameters[1].Value = model.u_estart_time;
                        parameters[2].Value = model.u_eend_time;
                        parameters[3].Value = a;
                        parameters[4].Value = model.u_eremark;
                        parameters[5].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return ehid;
        }
        #endregion
        #region 添加党员奖惩情况信息
        public string addRewardPunishment(userExcel model)
        {
            string rpid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_reward_punishment(");
                        strSql.Append("id,title,reason,approval_authority,office_level,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@title,@reason,@approval_authority,@office_level,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@title",SqlDbType.NVarChar,50),
                                new SqlParameter("@reason",SqlDbType.NText),
                                new SqlParameter("@approval_authority",SqlDbType.NVarChar,100),
                                new SqlParameter("@office_level",SqlDbType.NVarChar,50),
                                new SqlParameter("@status",SqlDbType.Int,4),

                            };
                        parameters[0].Value = rpid;
                        parameters[1].Value = model.u_rtitle;
                        parameters[2].Value = model.u_rreasont;
                        parameters[3].Value = model.u_rapproval_authority;
                        parameters[4].Value = model.u_roffice_level;
                        parameters[5].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return rpid;
        }
        #endregion
        #region 添加领导班子成员信息
        public string addLeadInfo(userExcel model)
        {
            string liid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_lead_info(");
                        strSql.Append("id,group_id,name,job,contact_way,remark,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@group_id,@name,@job,@contact_way,@remark,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@group_id",SqlDbType.NVarChar,50),
                                new SqlParameter("@name",SqlDbType.NVarChar,50),
                                new SqlParameter("@job",SqlDbType.NVarChar,100),
                                new SqlParameter("@contact_way",SqlDbType.NVarChar,100),
                                new SqlParameter("@remark",SqlDbType.NText),
                                new SqlParameter("@status",SqlDbType.Int,4),
                            };
                        parameters[0].Value = liid;
                        parameters[1].Value = model.u_Lgroup_id;
                        parameters[2].Value = model.u_Lname;
                        parameters[3].Value = model.u_Ljob;
                        parameters[4].Value = model.u_Lcontact_way;
                        parameters[5].Value = model.u_Lremark;
                        parameters[6].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return liid;
        }
        #endregion
        #region 添加流动党员信息
        public string addFloatCommie(userExcel model)
        {
            string fcid = Guid.NewGuid().ToString();
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append(" insert into u_float_commie(");
                        strSql.Append("id,flow_type,linkman,flow_reason,contact,id_number,group_linkman,discharge_place,group_contact,status)");
                        strSql.Append(" values(");
                        strSql.Append("@id,@flow_type,@linkman,@flow_reason,@contact,@id_number,@group_linkman,@discharge_place,@group_contact,@status)");
                        SqlParameter[] parameters = {
                                new SqlParameter("@id",SqlDbType.NVarChar,50),
                                new SqlParameter("@flow_type",SqlDbType.NVarChar,50),
                                new SqlParameter("@linkman",SqlDbType.NVarChar,50),
                                new SqlParameter("@flow_reason",SqlDbType.NText),
                                new SqlParameter("@contact",SqlDbType.NVarChar,100),
                                new SqlParameter("@id_number",SqlDbType.NVarChar,50),
                                new SqlParameter("@group_linkman",SqlDbType.NVarChar,50),
                                new SqlParameter("@discharge_place",SqlDbType.NVarChar,100),
                                new SqlParameter("@group_contact",SqlDbType.NVarChar,100),
                                new SqlParameter("@status",SqlDbType.Int,4),
                            };
                        parameters[0].Value = fcid;
                        parameters[1].Value = model.u_lflow_type;
                        parameters[2].Value = model.u_llinkman;
                        parameters[3].Value = model.u_lflow_reason;
                        parameters[4].Value = model.u_lcontact;
                        parameters[5].Value = model.u_lid_number;
                        parameters[6].Value = model.u_lgroup_linkman;
                        parameters[7].Value = model.u_ldischarge_place;
                        parameters[8].Value = model.u_lgroup_contact;
                        parameters[9].Value = 0;
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), parameters);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        return "0";
                    }
                }
            }
            return fcid;
        }
        #endregion
        #region 查询行政级别id
        public string selectAdminRank(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    
                    try
                    {
                            StringBuilder strSql = new StringBuilder();
                            strSql.Append("select id as u_aid from u_administration_rank where type = '" + model.u_atype + "'");
                            DataSet aa = DbHelperSQL.Query(strSql.ToString());
                            DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                            userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                             id = gid.u_aid;
                        

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
            }
            return id;
        }
        #endregion
        #region 查询军职级别id
        public string selectMilitaryRank(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select id as u_mid from u_military_rank where type = '" + model.u_mtype + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.u_mid;


                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
            }
            return id;
        }
        #endregion
        #region 查询警衔级别id
        public string selectPoliceRank(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select id as u_pid from u_police_rank where type = '" + model.u_ptype + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.u_pid;


                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
            }
            return id;
        }
        #endregion
        #region 查询新阶层人员id
        public string selectNewClass(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select id as u_id from u_new_class where type = '" + model.u_type + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.u_id;


                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
            }
            return id;
        }
        #endregion   
        #region 查询护照情况id
        public string selectPassportInfo(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select id as u_Pid from u_passport_info where type = '" + model.u_Ptype + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.u_Pid;


                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
            }
            return id;
        }
        #endregion
        #region 查询优抚对象id
        public string selectEntitledGroups(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select P_Id as P_eId from P_EntitledGroupsInfo where P_EntitledGroups = '" + model.P_eEntitledGroups + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.P_eId;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
            }
            return id;
        }
        #endregion
        #region 查询进入现支部id
        public string selectGroupType(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select id as u_gid from u_group_type where type = '" + model.u_gtype + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.u_gid;


                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
            }
            return id;
        }
        #endregion
        #region 查询学历情况id
        public string selectEducation(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select P_Id as P_Id from P_EducationInfo where P_Education = '" + model.P_Education + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.P_Id;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
                return id;
            }
        }
        #endregion
        #region 查询收入来源id
        public string selectIncomeSource(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select P_Id as P_iId from P_IncomeSourceInfo where P_IncomeSource = '" + model.P_iIncomeSource + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.P_iId;


                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
                return id;
            }
        }
        #endregion
        #region 查询工作岗位类型id
        public string selectCompanyType(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select id as P_cId from u_post_type where type = '" + model.u_cpost_type_id + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.P_cId;

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
                return id;
            }
        }
        #endregion
        #region 查询工作单位类型id
        public string selectPostType(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select P_Id as u_oid from P_CompanyType where P_ComType = '" + model.u_ccom_type_id + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.u_oid;

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
                return id;
            }
        }
        #endregion
        #region 查询经济状况id
        public string selectFinancialFituation(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select id as u_Fid from u_financial_situation where type = '" + model.u_Ftype + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.u_Fid;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
                return id;
            }
        }
        #endregion
        #region 查询生活困难原因id
        public string selectBadlyOffReason(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select id as u_bid from u_badly_off_reason where type = '" + model.u_btype + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.u_bid;

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
                return id;
            }
        }
        #endregion
        #region 查询帮扶类型id
        public string selectHelpWay(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select id as u_Hid from u_help_way where type = '" + model.u_Htype + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.u_Hid;


                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
            }
            return id;
        }
        #endregion
        #region 查询身体健康id
        public string selectHealthyCondition(userExcel model)
        {
            string id = null;
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("select id as u_hid from u_healthy_condition where type = '" + model.u_htype + "'");
                        DataSet aa = DbHelperSQL.Query(strSql.ToString());
                        DataSetToModelHelper<userExcel> info = new DataSetToModelHelper<userExcel>();
                        userExcel gid = info.FillToModel(aa.Tables[0].Rows[0]);
                        id = gid.u_hid;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return null;
                    }
                }
                return id;
            }
        }
        #endregion
    }
}
