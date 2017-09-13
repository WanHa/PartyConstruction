<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcelToChannel.aspx.cs" Inherits="DTcms.Web.admin.PartyDataBase.ExcelToChannel" %>
<%@ Import namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<title>导入党员信息</title>
<link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
<script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
 </head>
<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
          <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
          <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
          <i class="arrow"></i>
          <span>导入党员信息</span>
        </div>
      <!--/导航栏-->
		     <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">党员信息导入</a></li>
                    </ul>
                </div>
            </div>
        </div>

       <div class="tab-content">
        <span class="Validform_checktip">*第一步:导入党组织信息</span><br />
           <div>
			<input type="file" name="SpUploadFile" id="SpUploadFile" style="display: none;" accept=".xlsx" runat="server"/>            
			<asp:Button ID="PpInputFileUploadButton" runat="server" style="display: none;" Text="上传" onclick="PpInputFileUploadButton_Click" />             
			<asp:TextBox ID="excelName" runat="server" CssClass="input normal" readonly="true" Text="请选择导入的.xlsx类型excel文件"></asp:TextBox>
			<button id="btnFileSp" type="button" style="margin-left: 5px; margin-top: -3px;" class="btn btn-default btn-sm">
			<span class="glyphicon glyphicon-open" aria-hidden="true" style="padding-right: 3px;"></span>文件选择</button>
			<button id="btnImportSp" type="button" style="margin-left: 5px; margin-top: -3px;" class="btn btn-primary">
			<span class="glyphicon glyphicon-open" aria-hidden="true" style="padding-right: 3px;"></span>导入</button>
	   </div>
           <br />
            <br />
        <span class="Validform_checktip">*第二步:导入党员信息</span><br />
               <div>
			<input type="file" name="SpUploadFile2" id="SpUploadFile2" style="display: none;" accept=".xlsx" runat="server"/>            
			<asp:Button ID="PpInputFileUploadButton2" runat="server" style="display: none;" Text="上传" onclick="PpInputFileUploadButton_Click2" />                  
			<asp:TextBox ID="excelName2" runat="server" CssClass="input normal" readonly="true" Text="请选择导入的.xlsx类型excel文件"></asp:TextBox>
			<button id="btnFileSp2" runat="server" type="button" style="margin-left: 5px; margin-top: -3px;" class="btn btn-default btn-sm">
			<span class="glyphicon glyphicon-open" aria-hidden="true" style="padding-right: 3px;"></span>文件选择</button>
			<button id="btnImportSp2" type="button" style="margin-left: 5px; margin-top: -3px;" class="btn btn-primary">
			<span class="glyphicon glyphicon-open" aria-hidden="true" style="padding-right: 3px;"></span>导入</button>
	   </div>
             <br />
             <br />
        <span class="Validform_checktip">*第三步:导入书记与上级党组织</span><br />
          <div>
			<input type="file" name="SpUploadFile3" id="SpUploadFile3" style="display: none;" accept=".xlsx" runat="server"/>            
			<asp:Button ID="PpInputFileUploadButton3" runat="server" style="display: none;" Text="上传" onclick="PpInputFileUploadButton_Click3" />                  
			<asp:TextBox ID="excelName3" runat="server" CssClass="input normal" readonly="true" Text="请选择导入的.xlsx类型excel文件"></asp:TextBox>
			<button id="btnFileSp3"  runat="server" type="button" style="margin-left: 5px; margin-top: -3px;" class="btn btn-default btn-sm">
			<span class="glyphicon glyphicon-open" aria-hidden="true" style="padding-right: 3px;"></span>文件选择</button>
			<button id="btnImportSp3" type="button" style="margin-left: 5px; margin-top: -3px;" class="btn btn-primary">
			<span class="glyphicon glyphicon-open" aria-hidden="true" style="padding-right: 3px;"></span>导入</button>
	   </div>
             <br />
             <br />
		<%--    <span class="Validform_checktip">*第四步:导入组织奖惩信息</span><br />
          <div>
			<input type="file" name="SpUploadFile4" id="SpUploadFile4" style="display: none;" accept=".xlsx" runat="server"/>            
			<asp:Button ID="PpInputFileUploadButton4" runat="server" style="display: none;" Text="上传" onclick="PpInputFileUploadButton_Click4" />                  
			<asp:TextBox ID="excelName4" runat="server" CssClass="input normal" readonly="true" Text="请选择导入的.xlsx类型excel文件"></asp:TextBox>
			<button id="btnFileSp4" type="button" style="margin-left: 5px; margin-top: -3px;" class="btn btn-default btn-sm">
			<span class="glyphicon glyphicon-open" aria-hidden="true" style="padding-right: 3px;"></span>文件选择</button>
			<button id="btnImportSp4" type="button" style="margin-left: 5px; margin-top: -3px;" class="btn btn-primary">
			<span class="glyphicon glyphicon-open" aria-hidden="true" style="padding-right: 3px;"></span>导入</button>
	   </div>--%>
	 </div>
	</form>
</body>
</html>
<script type="text/javascript">
    /**
     ** 数据导入处理 -- 信息导入
     **/
    //步骤一
    $("#btnFileSp").click(function () {

        document.getElementById("<%= SpUploadFile.ClientID %>").click();

    });
    $("#SpUploadFile").change(function () {
        var fileName = $("#SpUploadFile").val().split("\\").pop();
        $("#excelName").val(fileName);
    });
    $("#btnImportSp").click(function () {
        // 添加文件选择校验
       var btnUpload = document.getElementById("<%= PpInputFileUploadButton.ClientID %>");      
                btnUpload.focus();
                btnUpload.click();
    })

    //步骤二
    $("#btnFileSp2").click(function () {

        document.getElementById("<%= SpUploadFile2.ClientID %>").click();

    });
    $("#SpUploadFile2").change(function () {
        var fileName2 = $("#SpUploadFile2").val().split("\\").pop();
        $("#excelName2").val(fileName2);
    });
    $("#btnImportSp2").click(function () {
        // 添加文件选择校验
        var btnUpload = document.getElementById("<%= PpInputFileUploadButton2.ClientID %>");
        btnUpload.focus();
        btnUpload.click();
    })

    //步骤三
    $("#btnFileSp3").click(function () {

        document.getElementById("<%= SpUploadFile3.ClientID %>").click();

    });
    $("#SpUploadFile3").change(function () {
        var fileName = $("#SpUploadFile3").val().split("\\").pop();
        $("#excelName3").val(fileName);
    });
    $("#btnImportSp3").click(function () {
        // 添加文件选择校验
        var btnUpload = document.getElementById("<%= PpInputFileUploadButton3.ClientID %>");
        btnUpload.focus();
        btnUpload.click();
    })

  <%--  //步骤四
    $("#btnFileSp4").click(function () {

        document.getElementById("<%= SpUploadFile4.ClientID %>").click();

    });
    $("#SpUploadFile4").change(function () {
        var fileName = $("#SpUploadFile4").val().split("\\").pop();
        $("#excelName4").val(fileName);
    });
    $("#btnImportSp4").click(function () {
        // 添加文件选择校验
        var btnUpload = document.getElementById("<%= PpInputFileUploadButton4.ClientID %>");
        btnUpload.focus();
        btnUpload.click();
    })--%>
</script>

