<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModelMember.aspx.cs" Inherits="DTcms.Web.admin.PartyConstruction.ModelMember" %>

<%@ Import Namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>党员概览</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../skin/tinyselect.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/tinyselect.js"></script>
    <script type="text/javascript"> 
        //$(function () {
        //    $("#txt").ready(function () {
        //        $.ajax({
        //            type: "Post",
        //            url: "ModelMember.aspx/GetNameList",    
        //            data: "{'key':'" + document.getElementById('txt').value + "'}",
        //            contentType: "application/json; charset=utf-8",
                   
        //            success: function (data) {
        //                var option = "";
        //                var json = strToJson(data.d);
        //                for (var i = 0; i < json.length; i++) {

        //                    option += "<option value=" + json[i].val + ">" + json[i].text + "</option>";
                           
        //                }
        //                $("#txt").append(option);

        //                $("#txt").tinyselect();

        //            },
        //            error: function (err) {
        //                alert(err);
        //            }
        //        });
        //    });
        //}); 
        //function strToJson(str) {
        //    var json = eval('(' + str + ')');
        //    return json;
        //} 

        /* This parser won´t respect "---" selection */
        function dataParserA(data, selected) {
            retval = [{ val: "-1", text: "---" }];

            data.forEach(function (v) {
                if (selected == "-1" && v.val == 3)
                    v.selected = true;
                retval.push(v);
            });

            return retval;
        }

        function change(v) {
            document.getElementById("HiddenField").value = v;
        }  
    </script>
</head>

<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>频道管理</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <ul class="icon-list">
                            <li><a class="add" href="ModelMember_edit.aspx?action=<%=DTEnums.ActionEnum.Add %>"><i></i><span>新增</span></a></li>
                            <%--<li><asp:LinkButton ID="btnSave" runat="server" CssClass="save" onclick="btnSave_Click"><i></i><span>保存</span></asp:LinkButton></li>--%>
                            <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
                            <li>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete','请确认删除，是否继续？');" OnClick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
                        </ul>
                    </div>
                    <div class="r-list">
                        <asp:TextBox ID="txtKeywords" runat="server" CssClass="keyword" />
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn-search" OnClick="btnSearch_Click">查询</asp:LinkButton>
                    </div>
                    
                    <%--<div class="r-list">
                        <select id="txt" onchange="change(this.options[this.options.selectedIndex].text)">
                            <option value="-1">---</option>
                        </select>

                        <asp:HiddenField ID="HiddenField" runat="server" />

                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn-search" OnClick="btnSearch_Click">添加</asp:LinkButton>
                    </div>--%>
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
                            <th width="10%">选择</th>
                            <th align="left" width="20%">姓名</th>
                            <th align="left" width="60%">简介</th>
                            <th width="10%">操作</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" Style="vertical-align: middle;" />
                            <asp:HiddenField ID="hidId" Value='<%#Eval("P_Id")%>' runat="server" />
                        </td>
                        <td><%#Eval("user_name")%></td>
                        <td><%#Eval("P_Description")%></td>
                        <td align="center"><a href="ModelMember_edit.aspx?action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("P_Id")%>">修改</a></td>
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
