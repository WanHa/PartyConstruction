<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Partypayment_Edit.aspx.cs" Inherits="DTcms.Web.admin.Partypayment.Partypayment_Edit" %>

<%@ Import Namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>编辑党费缴纳</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript">
        $(function () {
            //初始化表单验证
            $("#form1").initValidform();

        });

    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
     
        <!--导航栏-->
        <div class="location">
            <a href="Partypayment.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="Partypayment.aspx"><span>党费缴纳</span></a>
            <i class="arrow"></i>
            <span>编辑党费缴纳</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">基本信息</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content">
            
            <dl>
                <dt>标题</dt>
                <dd>
                    <asp:TextBox ID="TextBox1" runat="server" CssClass="input normal" Visible="False" Text="excelpath"></asp:TextBox>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="input normal" datatype="*2-100" sucmsg=" "></asp:TextBox>
                </dd>
            </dl>
            <dl runat="server" id="text">
                <dt>文件</dt>
                <dd>

                    <input type="file" name="SpUploadFile" id="SpUploadFile" style="display: none;" accept=".xlsx" runat="server"/>
                    
                    <asp:Button ID="PpInputFileUploadButton" runat="server" style="display: none;" Text="上传" onclick="PpInputFileUploadButton_Click" />                  
                        <asp:TextBox ID="excelName" runat="server" CssClass="input normal" readonly="true" Text="请上传以姓名,金额,手机号为列名的.xlsx类型excel文件"></asp:TextBox>
                        <button id="btnFileSp" type="button" style="margin-left: 5px; margin-top: -3px;" class="btn btn-default btn-sm">
                        <span class="glyphicon glyphicon-open" aria-hidden="true" style="padding-right: 3px;"></span>文件选择</button>

                        <button id="btnImportSp" type="button" style="margin-left: 5px; margin-top: -3px;" class="btn btn-primary">
                        <span class="glyphicon glyphicon-open" aria-hidden="true" style="padding-right: 3px;"></span>导入</button>
                </dd>
            </dl>
            <dl>
                <dt>状态</dt>
                <dd>
                    <div class="rule-multi-radio">
                        <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
<%--                            <asp:ListItem Value="" Selected="True">请选择状态</asp:ListItem>--%>
                            <asp:ListItem Value="0" Selected="True" >启用中</asp:ListItem>
                            <asp:ListItem Value="1">已关闭</asp:ListItem>     
                        </asp:RadioButtonList>
                    </div>
                </dd>
            </dl>
        </div>

        <!--/内容-->
        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" OnClick="btnSubmit_Click" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        <!--/工具栏-->
    </form>
</body>
</html>
<script type="text/javascript">
    /**
 ** 数据导入处理 -- 信息导入
 **/
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
</script>
