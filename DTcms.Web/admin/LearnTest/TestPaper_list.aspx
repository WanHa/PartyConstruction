﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPaper_list.aspx.cs" Inherits="DTcms.Web.admin.LearnTest.TestPaper_list" %>
<%@ Import namespace="DTcms.Common" %>
<!DOCTYPE html>

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<title>试卷</title>
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
          <a id="back" href="questionbank_list.aspx" class="back"><i></i><span>返回上一页</span></a>
          <a href="../center.aspx" class="home"><i></i><span>题库</span></a>
          <i class="arrow"></i>
          <span>试卷</span>
        </div>
      <!--/导航栏-->
         <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
        <div class="toolbar">
        <div class="box-wrap">
            <a class="menu-btn"></a>
            <div class="l-list">
            <ul class="icon-list">
                <li><a class="add" href="TestPaper_edit.aspx?action=<%=DTEnums.ActionEnum.Add %>&parentid=<%=this.id %>"><i></i><span>新增</span></a></li>
              
                <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
                <li><asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete','数据删除后将无法恢复，是否继续？');" onclick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
            </ul>
                 </div>
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
      <th width="8%">选择</th>
      <th align="left" width="30%">试卷名称</th>
      <th align="left" width="18%">创建时间</th>
      <th width="10%">操作</th>
      <th  width="10%">查看</th>
    </tr>
  </HeaderTemplate>
       <ItemTemplate>
    <tr>
      <td align="center">
        <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" style="vertical-align:middle;" />
        <asp:HiddenField ID="hidId" Value='<%#Eval("P_Id")%>' runat="server" />
      </td>
      <td><%#Eval("P_TestPaperName")%></td>
      <td><%#Eval("P_CreateTime")%></td>
    
      <td align="center"><a href="TestPaper_edit.aspx?action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("P_Id")%>&parentid=<%=this.id %>">修改</a></td>
        <td align="center"><a href="TestQuestion_list.aspx?action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("P_Id") %>&bankid=<%=this.id %>">查看试题</a></td>
    </tr>
  </ItemTemplate>
  <FooterTemplate>
    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"9\">暂无记录</td></tr>" : ""%>
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
