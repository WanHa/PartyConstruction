<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Partypayment_details.aspx.cs" Inherits="DTcms.Web.admin.Partypayment.Partypayment_details" %>
<%@ Import namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<title>收录情况</title>
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
          <a href="Partypayment.aspx" class="back"><i></i><span>返回上一页</span></a>
          <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
          <i class="arrow"></i>
          <span>收录情况</span>
        </div>
      <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
        <div class="toolbar">
        <div class="box-wrap">
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
      <th align="center" width="25%">金额</th>
      <th align="center" width="25%">创建时间</th>
      <th align="center" width="25%">状态</th>
    </tr>
  </HeaderTemplate>
      <ItemTemplate>
          <tr>
              <%-- <td align="center">
        <asp:HiddenField ID="hidId" Value='<%#Eval("P_Id")%>' runat="server" />
      </td>--%>
          <td style="display:none">
           <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" style="vertical-align:middle;"  />
          <asp:HiddenField ID="hidId" Value='<%#Eval("P_Id")%>' runat="server"/>
           </td>
              <td align="center"><%#Eval("P_CreateUser") %></td>
              <td align="center"><%#Eval("P_Money") %></td>
              <td align="center"><%#Eval("P_CreateTime")%></td>
              <td align="center"><%#Eval("PayStatus")%></td>
          </tr>
      </ItemTemplate>
  <FooterTemplate>
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