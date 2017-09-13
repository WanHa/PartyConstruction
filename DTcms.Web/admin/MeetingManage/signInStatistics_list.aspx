<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signInStatistics_list.aspx.cs" Inherits="DTcms.Web.admin.MeetingManage.signInStatistics_list" %>
<%@ Import namespace="DTcms.Common" %>
<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<title>签到详情</title>
<link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
<script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script>
        //function txtva(a) {
        //    var  datajson = { ' id': a };
        //    var jsondata = JSON.parse(datajson);
        //    $.ajax({
        //        url: "signInStatistics_list.aspx/UpdateType",
        //        data: jsondata ,
        //        contentType: "application/json",
        //        type: "POST",
        //        success: function (data) {
        //            if (data) {
        //                //刷新页面
        //                location.reload();
        //            }
        //        },
        //        error: function (er) {
        //            alert(er.responseText);
        //        }
        //    })
        //}
        var  webApiDomain = '<%=webApiDomain%>';

        function txtva(a) {
            var ajaxurl = webApiDomain + "v3/partygroupforum/sign/in";
            //var ajaxurl = "http://localhost:54066/v3/partygroupforum/sign/in";
            var ajaxData = {
                "id":a
            }
            $.ajax({
               url: ajaxurl,
               data: ajaxData,
               dataType: "json",
               type: "post",
               async: false,
               success: function (data) {
                   location.reload();
                   if (!data.issuccess)
                   {
                       dialogFunction('手动签到失败,请重试.');
                   }
                },
               error: function (a, e, c) {
                   dialogFunction('手动签到失败,请重试.');
                }
            })
        }

        function dialogFunction(msg) {
            var dialogMsg = top.dialog({
                title: '提示',
                content: msg,
                okValue: '确定',
            }).showModal();

            dialogMsg.addEventListener('close', function () {
                window.location.href = "MeetingManage/meetingManage_list.aspx";
            });
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
          <span>签到详情</span>
        </div>
      <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
        <div class="toolbar">
        <div class="box-wrap">
            <a class="menu-btn"></a>
            <div class="r-list">
            <asp:TextBox ID="txtKeywords" runat="server" CssClass="keyword" />
            <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn-search" onclick="btnSearch_Click">查询</asp:LinkButton>
            </div>
        </div>
        </div>
        </div>
        <!--/工具栏-->

<!--列表-->
<div class="table-container">
  <asp:Repeater ID="rptList" runat="server">
  <HeaderTemplate>
  <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
    <tr>
      <th align="center" width="25%">姓名</th>
      <th align="center" width="25%">组织</th>
        <th align="center" width="25%">签到时间</th>
        <th align="center" width="25%">签到状态</th>
    </tr>
  </HeaderTemplate>
  <ItemTemplate>
    <tr>
      <td style="display:none">
       <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" style="vertical-align:middle;"  />
        <asp:HiddenField ID="hidId" Value='<%#Eval("P_Id")%>' runat="server"/>
      </td>
      <td align="center"><%#Eval("user_name")%></td>
      <td align="center"><%#Eval("title")%></td>
      <td align="center"><%#Eval("P_CreateTime")%></td>      
      <td align="center"><%#Eval("P_SigninInfo").ToString().Trim() == "0" ? "<input type='button' value='手动签到' onclick=txtva('"+Eval("P_Id")+"')>" : "已签到" %></td>
    </tr>
  </ItemTemplate>
  <FooterTemplate>
 <%--   <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"9\">暂无记录</td></tr>" : ""%>--%>
  </table>
  </FooterTemplate>
  </asp:Repeater>
</div>
<!--/列表-->

<!--内容底部-->
<div class="line20"></div>
<div class="pagelist">
  <div class="l-btns">
    <span>显示</span><asp:TextBox ID="txtPageNum" runat="server" CssClass="pagenum" onkeydown="return checkNumber(event);"
                OnTextChanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox><span>条/页</span>
  </div>
  <div id="PageContent" runat="server" class="default"></div>
</div>
<!--/内容底部-->


    </form>
</body>
</html>
