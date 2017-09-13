using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace DTcms.Common
{
    public class ExcelHelper
    {
        protected static LogHelper Logger = new LogHelper("ExcelHelper");

        public static void ExportExcel(DataTable data, string fileName)
        {
            try
            {
                if (data != null && data.Rows.Count > 0)
                {
                    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                    HttpContext.Current.Response.Charset = "Utf-8";
                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", Encoding.UTF8));
                    StringBuilder sbHtml = new StringBuilder();
                    sbHtml.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                    sbHtml.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                    sbHtml.AppendLine("<tr style=\"background-color: #FFE88C;font-weight: bold; white-space: nowrap;\">");
                    foreach (DataColumn column in data.Columns)
                    {
                        sbHtml.AppendLine("<td>" + column.ColumnName + "</td>");
                    }
                    sbHtml.AppendLine("</tr>");
                    foreach (DataRow row in data.Rows)
                    {
                        sbHtml.Append("<tr>");
                        foreach (DataColumn column in data.Columns)
                        {
                            sbHtml.Append("<td>").Append(row[column].ToString()).Append("</td>");
                        }
                        sbHtml.AppendLine("</tr>");
                    }
                    sbHtml.AppendLine("</table>");
                    HttpContext.Current.Response.Write(sbHtml.ToString());
                    HttpContext.Current.Response.End();
                }
            }
            catch (Exception ex)
            {
                ExcelHelper.Logger.WriteLog("-----------Excel导出数据异常-----------\r\n" + ex.ToString() + "\r\n");
            }
        }

        public static void ExportSpecialExcel(string strHtml, string fileName)
        {
            try
            {
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                HttpContext.Current.Response.Charset = "Utf-8";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", Encoding.UTF8));
                StringBuilder sbHtml = new StringBuilder();
                sbHtml.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                sbHtml.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                //sbHtml.AppendLine("<tr style=\"background-color: #FFE88C;font-weight: bold; white-space: nowrap;\">");
                //foreach (DataColumn column in data.Columns)
                //{
                //    sbHtml.AppendLine("<td>" + column.ColumnName + "</td>");
                //}
                //sbHtml.AppendLine("</tr>");
                //foreach (DataRow row in data.Rows)
                //{
                //    sbHtml.Append("<tr>");
                //    foreach (DataColumn column in data.Columns)
                //    {
                //        sbHtml.Append("<td>").Append(row[column].ToString()).Append("</td>");
                //    }
                //    sbHtml.AppendLine("</tr>");
                //}

                sbHtml.AppendLine(strHtml);

                sbHtml.AppendLine("</table>");
                HttpContext.Current.Response.Write(sbHtml.ToString());
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                ExcelHelper.Logger.WriteLog("-----------Excel导出数据异常-----------\r\n" + ex.ToString() + "\r\n");
            }
        }

        private static string ConnectionString(string fileName)
        {
            return string.Format(fileName.EndsWith(".xls") ? "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;" : "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES\"", fileName);
        }

        public static DataTable ExcelToDataSet(string sheet, string filename)
        {
            DataTable result;
            try
            {
                OleDbConnection myConn = new OleDbConnection(ExcelHelper.ConnectionString(filename));
                string strCom = " SELECT * FROM [" + sheet + "$]";
                myConn.Open();
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                DataSet ds = new DataSet();
                myCommand.Fill(ds);
                myConn.Close();
                result = ds.Tables[0];
            }
            catch (Exception ex)
            {
                ExcelHelper.Logger.WriteLog("-----------Excel导入数据异常-----------\r\n" + ex.ToString() + "\r\n");
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Author:newbie
        /// 将 Excel 中的数据读取到 DataTable 中
        /// </summary>
        /// <param name="mySheet"> WorkSheet 对象 </param>
        /// <param name="isColumnName"> 第一行数据是否是列名 </param>
        /// <param name="startRow"> 列头的行数-1 </param>
        /// <returns></returns>
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