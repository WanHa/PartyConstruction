<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="qiniu.aspx.cs" Inherits="DTcms.Web.admin.qiniupage.qiniu" %>

<%@ Import Namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>配置项</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <style>
        #center {
            width: 500px;
            margin-top: 100px;
            margin-left: 100px;
            height: 500px;
            font-size: 13px;
            color: gray;
        }

        .while {
            color: white;
        }
    </style>

</head>

<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>配置项</span>
        </div>
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



        <%--           <div class="page-footer">
          <div class="btn-wrap">
            <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" onclick="btnSubmit_Click" />
            <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
          </div>
        </div>--%>


        <!--列表-->
        <div id="center">
            URL:&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<asp:TextBox ID="url" CssClass="input normal" runat="server" Style="vertical-align: middle;" /><br>
            <br>
            <br>
            存储名称：&nbsp&nbsp&nbsp&nbsp<asp:TextBox ID="saveName" CssClass="input normal" runat="server" Style="vertical-align: middle;" /><br>
            <br>
            <br>
            AK码:&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<asp:TextBox ID="ak" CssClass="input normal" runat="server" Style="vertical-align: middle;" /><br>
            <br>
            <br>
            SK码:&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<asp:TextBox ID="sk" CssClass="input normal" runat="server" Style="vertical-align: middle;" /><br>
            <br>
            <br>
            <asp:LinkButton ID="btnSave" runat="server" CssClass="btn" OnClick="btnSave_Click"><span class="while">提交保存</span></asp:LinkButton>
            <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
        </div>
    </form>
</body>
<!--列表-->
