using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThoughtWorks.QRCode.Codec;

namespace DTcms.Web.admin.MeetingManage
{
    public partial class BarCode_Images : System.Web.UI.Page
    {
        public string meetingId = string.Empty;
        public static string downId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.meetingId = DTRequest.GetQueryString("meetingId");
           
            //生成二维码图片  
            Create_CodeImages(meetingId);
            downId = meetingId;
        }

        #region 下载图片
        public void download_Click(object sender, EventArgs e)
        {
            string templetFilePath = HttpContext.Current.Server.MapPath("BarCode_Images/"+ downId +".png");

            Bitmap img;

            try
            {
                using (FileStream file = new FileStream(templetFilePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    img = new Bitmap(file);
                }

                // 定义二进制流
                using (MemoryStream myMemoryStream = new MemoryStream())
                {
                    // 将对象写入二进制流
                    img.Save(myMemoryStream, img.RawFormat);

                    byte[] byteImage = new Byte[myMemoryStream.Length];
                    byteImage = myMemoryStream.ToArray();

                    // 下载文件
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename=会议签到" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png"));
                    Response.BinaryWrite(byteImage);
                    Response.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                img = null;
                Response.End();
            }
            
        }
        #endregion

        #region 二维码生成  

        //程序路径  
        readonly string currentPath = HttpContext.Current.Server.MapPath("BarCode_Images");// Server.MapPath() + @"BarCode_Images";

        /// <summary>  
        /// 生成二维码图片  
        /// </summary>  
        private void Create_CodeImages(string meetingId)
        {
            try
            {
                if (!string.IsNullOrEmpty(meetingId))
                {
                    //清空目录  
                    DeleteDir(currentPath);
                    //生成图片  
                    Bitmap image = Create_ImgCode(meetingId, 100);
                    //保存图片  
                    SaveImg(currentPath, image, meetingId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>  
        /// 保存图片  
        /// </summary>  
        /// <param name="strPath">保存路径</param>  
        /// <param name="img">图片</param>  
        public void SaveImg(string strPath, Bitmap img, string meettingId)
        {
            //保存图片到目录  
            if (Directory.Exists(strPath))
            {
                //文件名称  
                string guid = meettingId + ".png";
                img.Save(strPath + "/" + guid, System.Drawing.Imaging.ImageFormat.Png);
            }
            else
            {
                //当前目录不存在，则创建  
                Directory.CreateDirectory(strPath);
            }
        }
        /// <summary>  
        /// 生成二维码图片  
        /// </summary>  
        /// <param name="codeNumber">要生成二维码的字符串</param>       
        /// <param name="size">大小尺寸</param>  
        /// <returns>二维码图片</returns>  
        public Bitmap Create_ImgCode(string codeNumber, int size)
        {
            //创建二维码生成类  
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //设置编码模式  
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //设置编码测量度  
            qrCodeEncoder.QRCodeScale = size;
            //设置编码版本  
            qrCodeEncoder.QRCodeVersion = 0;
            //设置编码错误纠正  
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //生成二维码图片  
            System.Drawing.Bitmap image = qrCodeEncoder.Encode(codeNumber);
            return image;
        }
        /// <summary>  
        /// /打开指定目录  
        /// </summary>  
        /// <param name="path"></param>  
        public void Open_File(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", path);
        }
        /// <summary>  
        /// 删除目录下所有文件  
        /// </summary>  
        /// <param name="aimPath">路径</param>  
        public void DeleteDir(string aimPath)
        {
            try
            {
                //目录是否存在  
                if (Directory.Exists(aimPath))
                {
                    // 检查目标目录是否以目录分割字符结束如果不是则添加之  
                    if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                        aimPath += Path.DirectorySeparatorChar;
                    // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组  
                    // 如果你指向Delete目标文件下面的文件而不包含目录请使用下面的方法  
                    string[] fileList = Directory.GetFiles(aimPath);
                    //string[] fileList = Directory.GetFileSystemEntries(aimPath);  
                    // 遍历所有的文件和目录  
                    foreach (string file in fileList)
                    {
                        // 先当作目录处理如果存在这个目录就递归Delete该目录下面的文件  
                        if (Directory.Exists(file))
                        {
                            DeleteDir(aimPath + Path.GetFileName(file));
                        }
                        // 否则直接Delete文件  
                        else
                        {
                            File.Delete(aimPath + Path.GetFileName(file));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}