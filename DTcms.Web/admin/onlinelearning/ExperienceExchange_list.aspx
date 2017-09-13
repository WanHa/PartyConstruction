<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExperienceExchange_list.aspx.cs" Inherits="DTcms.Web.admin.onlinelearning.ExperienceExchange_list" %>
<%@ Import namespace="DTcms.Common" %>
<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<title>学习交流</title>
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
          <span>在线学习</span>
        </div>
      <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
        <div class="toolbar">
        <div class="box-wrap">
            <a class="menu-btn"></a>
            <div class="l-list">
            <ul class="icon-list">            
                <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
                <li><asp:LinkButton ID="btnDelete" runat="server" CssClass="del" onclick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
<%--				<li><asp:LinkButton ID="btnPass" runat="server" CssClass="lock" onclick="btnPass_Click"><i></i><span>通过</span></asp:LinkButton></li>
				<li><asp:LinkButton ID="btnRefuse" runat="server" CssClass="lock" onclick="btnRefuse_Click"><i></i><span>拒绝</span></asp:LinkButton></li>	--%>						
            </ul>
           
            </div>
            <div class="r-list">
           <asp:textbox id="txtkeywords" runat="server" cssclass="keyword" />
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
      <th align="center" width="10%">选择</th>
      <th align="center" width="20%">标题</th>
      <th align="center" width="15%">创建人</th>
      <th align="center" width="25%">创建时间</th>
      <th align="center" width="15%">审批状态</th>
      <th align="center width="15%">操作</th>
    </tr>
  </HeaderTemplate>
  <ItemTemplate>
    <tr>
      <td align="center">
        <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" style="vertical-align:middle;" />
        <asp:HiddenField ID="hidId" Value='<%#Eval("P_Id")%>' runat="server" />
        </td>
        <td align="center"><%#Eval("P_title")%></td>
        <td align="center"><%#Eval("Name")%></td>
        <td align="center"><%#Eval("P_CreateTime")%></td> 
		<td align="center"><%#Eval("P_AuditState").ToString() == "0" ?"未审批" : Eval("P_AuditState").ToString() == "1" ?"通过" : "拒绝"%></td> 		
         <td align="center"><a href="ExperienceExchange_edit.aspx?action=<%#DTEnums.ActionEnum.View %>&id=<%#Eval("P_Id")%>">查看</a></td>
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
