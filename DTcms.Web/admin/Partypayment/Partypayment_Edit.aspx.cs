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

namespace DTcms.Web.admin.Partypayment
{
    public partial class Partypayment_Edit : Web.UI.ManagePage
    {
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private string id = string.Empty;
        private List<P_PartyPayMentPeople> data = new List<P_PartyPayMentPeople>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");

            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryString("id");
                text.Visible = false;
                if (string.IsNullOrEmpty(id))
                {
                    JscriptMsg("传输参数不正确！", "back");
                    return;
                }
                if (!new BLL.P_PartyPayMent().Exists(this.id))
                {
                    JscriptMsg("记录不存在或已删除！", "back");
                    return;
                }
            }

            if (!Page.IsPostBack)
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.View.ToString()); //检查权限

                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }

            }
        }
        #region 赋值操作=================================
        private void ShowInfo(string _id)
        {
            BLL.P_PartyPayMent bll = new BLL.P_PartyPayMent();
            Model.P_PartyPayMent model = bll.GetModel(_id);
            //string username = model.P_CreateUser;
            txtTitle.Text = model.P_Title;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            Model.manager user = GetAdminInfo();

            Model.P_PartyPayMent model = new Model.P_PartyPayMent();
            BLL.P_PartyPayMent bll = new BLL.P_PartyPayMent();

            model.P_Id = Guid.NewGuid().ToString();
            model.P_Title = txtTitle.Text;
            model.P_PayMentState = Convert.ToInt32(rblStatus.Text);
           
            model.P_CreateTime = DateTime.Now;
            model.P_CreateUser = user.role_id.ToString();
            model.P_UpdateTime = DateTime.Now;
            model.P_UpdateUser = user.role_id.ToString();
            model.P_Status = 0;

            if (this.TextBox1.Text != "excelpath") {

            GetSpDataFromExcel(this.TextBox1.Text);
            }

            if (data != null && data.Count > 0)
            {
                string result = bll.Add(model, data);
                if (!string.IsNullOrEmpty(result))
                {
                    AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加缴纳党费成功" + model.P_Id); //记录日志
                    return true;
                }
                else
                {
                    AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加缴纳党费失败" + model.P_Title); //记录日志
                    return false;
                }
            }
            else
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "导入Excel的文件中无数据无法添加" + model.P_Title); //记录日志
                return true;
            }

        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(string _id)
        {
            Model.manager user = GetAdminInfo();

            BLL.P_PartyPayMent bll = new BLL.P_PartyPayMent();
            Model.P_PartyPayMent model = bll.GetModel(_id);
            string old_name = model.P_Title;
            model.P_Title = txtTitle.Text;
            model.P_PayMentState = Convert.ToInt32(rblStatus.Text);
            model.P_UpdateTime = DateTime.Now;
            model.P_CreateUser = user.role_id.ToString();
            model.P_Status = 0;
            if (bll.Update(model))
            {
                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改缴纳党费成功" + model.P_Title + "原数据" + old_name); //记录日志
                return true;
            }
            else
            {
                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改缴纳党费失败，只能存在一条启用数据" + model.P_Title + "原数据" + old_name); //记录日志
                return false;
            }
            //else
            //{
            //    AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "导入Excel的文件中无数据无法添加" + model.P_Title); //记录日志
            //    return true;
            //}
            //if (!bll.Update(model,data))
            //{
            //    AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改缴纳党费失败" + model.P_Title + "原数据" + old_name); //记录日志
            //    return false;
            //}
            //AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改缴纳党费成功" + model.P_Title + "原数据" + old_name); //记录日志
            //return true;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("修改失败，只能存在一条启用中的数据！", "");
                    return;
                }
                JscriptMsg("修改数据成功！", "Partypayment.aspx", "parent.loadMenuTree");
            }
            else //添加
            {
                ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("文件数据或状态有误！", "");
                    return;
                }
                else
                {
                    JscriptMsg("添加数据成功！", "Partypayment.aspx", "parent.loadMenuTree");
                }               
            }
        }

        //上传Excel
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
                    this.TextBox1.Text = Path.Combine(ServerSavePath, FileName);
                    #region 根据服务器上的路径, 用 NPOI 插件获取 Excel 文件中的内容, 然后倒入到 DB 中.

                    //GetSpDataFromExcel(Path.Combine(ServerSavePath, FileName));

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

        //private void GetSpDataFromExcel(string FilePath)
        public void GetSpDataFromExcel(string FilePath)
        {
            // 定义 Workbook 对象
            XSSFWorkbook myWorkbook;
            XSSFSheet myWorksheet;

            try
            {
                // 用数据流打开 Excel 文件, 
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8))
                //using (FileStream fs = File.Open(FilePath, FileMode.Open,FileAccess.Read,FileShare.Read))

                {
                    myWorkbook = new XSSFWorkbook(fs);
                    myWorksheet = myWorkbook.GetSheet("Sheet1") as XSSFSheet;

                    // Excel 文件中的列头【因为 Excel 中的列头有合并,读取的时候不能读到一行,所以需要人为定义】
                    string[] arrHeads = { "姓名", "金额", "手机号" };

                    // 将 Excel 中的数据读取到 DataTable 中
                    DataTable dt = ReadDataFromExcel(myWorksheet, false, arrHeads, 0);

                    data = DetailListModel(dt);

                    PartyPayMentPeople bll = new PartyPayMentPeople();
                }
            }
            catch (Exception ex)
            {
                JscriptMsg("导入表格数据时出现错误，请检查导入的表格数据！", string.Empty);
                //throw ex;
            }
        }
        public List<P_PartyPayMentPeople> DetailListModel(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<P_PartyPayMentPeople> result = new List<P_PartyPayMentPeople>();
            foreach (DataRow row in table.Rows)
            {
                P_PartyPayMentPeople item = DataRowToModel(row);
                if (item != null)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        #region 扩展方法================================
        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public Model.P_PartyPayMentPeople DataRowToModel(DataRow row)
        {
            Model.P_PartyPayMentPeople model = new Model.P_PartyPayMentPeople();
            if (row != null)
            {
                if (row[0] != null && row[0].ToString() != "")
                {                    
                    model.P_CreateUser = row[0].ToString();                  
                }
                else {
                    return null;
                }
                if (row[2] != null && row[2].ToString() != "") {
                    //TODO 需要添加是否是数字校验
                    Regex reg = new Regex("^[0-9]+$");
                    Match ma = reg.Match(row[2].ToString());
                    if (ma.Success)
                    {
                        model.P_Tel =row[2].ToString(); 
                        }
                    else
                    {
                        JscriptMsg("导入表格数据出现错误，请检查数据填写是否正确！", string.Empty);
                        return null;
                    }
                    //model.P_Money = row["金额"].ToString();
                }
                else
                {
                    return null;
                }
                if (row[1] != null && row[1].ToString() != "")
                {
                    //TODO 需要添加是否是数字校验
                    Regex reg = new Regex("^[0-9]+(.[0-9]{1,3})?$");
                    Match ma = reg.Match(row[1].ToString());
                    if (ma.Success)
                    {
                        model.P_Money = decimal.Parse(row[1].ToString());
                    }
                    else
                    {
                        JscriptMsg("导入表格数据出现错误，请检查数据填写是否正确", string.Empty);
                        return null;
                    }                    
                }
                else
                {
                    return null;
                }
                model.P_ID = Guid.NewGuid().ToString();
                model.P_Status = 0;
                //model.P_CreateUser = GetAdminInfo().id.ToString();
                model.P_CreateTime = DateTime.Now;
            }
            return model;
        }

        #endregion

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
                    //dt = new DataTable();
                    // 当前 Sheet 页数据的总行数  
                    int rowCount = mySheet.LastRowNum;

                    if (rowCount > startRow)
                    {
                        // 第五行  
                        IRow firstRow = mySheet.GetRow(startRow);
                        // 列数
                        int columnCount = firstRow.LastCellNum;

                        //构建datatable的列  
                        if (isColumnName)
                        {
                            //startRow = 1;//如果第一行是列名，则从第二行开始读取  
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

                            //if (String.IsNullOrEmpty(row.GetCell(0).StringCellValue)) continue;

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
                                    //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)  
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

                                            //short format = cell.CellStyle.DataFormat;
                                            ////对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理  
                                            //if (format == 14 || format == 31 || format == 57 || format == 58)
                                            //    dataRow[j] = cell.DateCellValue;
                                            //else
                                            //    dataRow[j] = cell.NumericCellValue.ToString();
                                            break;
                                        case CellType.String:
                                            dataRow[j] = cell.StringCellValue;
                                            break;
                                        case CellType.Formula:
                                            dataRow[j] = cell.NumericCellValue.ToString();

                                            //switch(cell.CachedFormulaResultType)
                                            //{
                                            //    case :
                                            //        dataRow[j] = "";
                                            //        break;

                                            //}

                                            //dataRow[j] = cell.StringCellValue;
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
        
    }
}