<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BarCode_Images.aspx.cs" Inherits="DTcms.Web.admin.MeetingManage.BarCode_Images" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>会议二维码图片</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <%--<link rel="stylesheet" href="http://cache.amap.com/lbs/static/main1119.css" />
    <script src="http://webapi.amap.com/maps?v=1.3&key=d5877335f5f846de8c293264becf2b13"></script>--%>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/webuploader/webuploader.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/uploader.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript">
        function savepic() {
            document.getElementById("<%= download.ClientID %>").click();
        }

    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
    <!--导航栏-->
        <div class="location">
            <a id="back" href="meetingManage_list.aspx" class="back"><i></i><span>返回上一页</span></a>
          <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
          <i class="arrow"></i>
          <span>扫一扫会议签到</span>
        </div>
      <!--/导航栏-->
    <div id="floatHead" class="toolbar-wrap">
        <div class="toolbar">
        <div class="box-wrap">
            <a class="menu-btn"></a>
            <div class="r-list">
            </div>
        </div>
        </div>
        </div>
        <!--/工具栏-->

    <fieldset>
            <section>
			    <div style="margin:10px;text-align:center;vertical-align:middle;">
                    <img id="BarCode_Images" alt="block" style="height:20%;width:20%" src="BarCode_Images/<%=meetingId%>.png"/>
			    </div>
		    </section>
    </fieldset>
         <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap" style="text-align:center">
                <input value="下载二维码" type="button" onclick="savepic()"; class="btn red" style="margin-right:5%;margin-top:5%;"/>
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" style="margin-left:5%;margin-top:5%;" />
                
            </div>
        </div>
        <!--/工具栏-->
    <asp:LinkButton ID="download" runat="server" OnClick="download_Click" CommandArgument='<%=meetingId%>' ForeColor="White"></asp:LinkButton>
    </form>
</body>
</html>
