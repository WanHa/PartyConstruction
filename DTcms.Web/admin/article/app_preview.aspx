<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="app_preview.aspx.cs" Inherits="DTcms.Web.admin.article.app_preview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title></title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript">
        $("#preview").width(window.location.width);
        //jQuery(function ($) {
        //    try {
        //        　　top.location.hostname;
        //        　　if (top.location.hostname != window.location.hostname) {
        //            　　　　top.location.href = window.location.href;
        //        　　}
        //    }
        //    catch (e) {
        //        　　top.location.href = window.location.href;
        //    }
        //});
    </script>
    <style type="text/css">
        .top-div{ padding: 20px 15px; border-top:none; box-sizing:border-box; overflow: hidden; }
    </style>
</head>
<body>
    <div id="preview" runat="server" class="top-div">
    </div>
    
</body>
</html>
