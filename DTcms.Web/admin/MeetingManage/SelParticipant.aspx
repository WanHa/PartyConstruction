<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelParticipant.aspx.cs" Inherits="DTcms.Web.admin.MeetingManage.SelParticipant" %>

<%@ Import Namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>与会人员</title>
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

        var api = top.dialog.get(window); //获取窗体对象
        $(function () {
            document.getElementById("txta").value = "";
            //设置按钮及事件
            api.button([{
                value: '确定',
                callback: function () {
                    var data = new Array();
                    var repeaterId = '<%=rptList.ClientID%>';
                    var count = <%=rptList.Items.Count%>;
                    var item = null;

                    for (var i = 0; i < count; i++) {
                        if (document.getElementById("rptList_chkId_" + i).checked) {
                            item = {
                                "id": document.getElementById("rptList_hidId_" + i).value,
                                "text": document.getElementById("rptList_hiduser_" + i).value
                            };
                            if (item != null) {
                                data.push(item);
                            }
                        }
                    }
                    api.close(data);
                },
                autofocus: true
            }, {
                value: '取消',
                callback: function () { }
            }]);

                //下拉框--选择组织
                $("#txt").ready(function () {
                    $.ajax({
                        type: "Post",
                        url: "SelParticipant.aspx/GetOrganizeList",
                        data: "{'key':'" + document.getElementById('txt').value + "'}",
                        contentType: "application/json; charset=utf-8",

                        success: function (data) {
                            var option = "";
                            var json = strToJson(data.d);
                            for (var i = 0; i < json.length; i++) {

                                option += "<option value='" + json[i].id + "'>" + json[i].text + "</option>";

                            }
                            $("#txt").append(option);

                            //$("#txt").tinyselect();
                        },
                        error: function (err) {
                            //alert("aa");
                        }
                    });
                });
            
        });

        //string转换为json
        function strToJson(str) {
            var json = eval('(' + str + ')');
            return json;
        }
        function getTxtId(v) {

            document.getElementById("txta").value = v;
            var s = $("#txta").val();
        }


    </script>
</head>

<body class="mainbody" style="width: 600px;">


    <form id="form1" runat="server">
        <asp:HiddenField ID="txta" Value="" runat="server" />
        <!--/导航栏-->
        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <ul class="icon-list">
                            <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
                        </ul>
                    </div>
                    <div class="r-list">
                        <select id="txt" onchange="getTxtId(this.options[this.options.selectedIndex].text)" class="select" style="height: 32px;float:left" >
                            <option value="">--请选择支部--</option>
                        </select>


                        <asp:TextBox ID="txtKeywords" runat="server" CssClass="keyword"/>
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn-search" OnClick="lbtnSearch_Click">搜索</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <!--/工具栏-->
        <!--列表-->
        <div class="table-container" style="height:350px;overflow-y:scroll;overflow-x:hidden">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table style="width:90%;" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th width="50%">选择</th>
                            <th align="left" width="50%">人员</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" Style="vertical-align: middle;" />
                            <asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" />
                            <asp:HiddenField ID="hiduser" Value='<%#Eval("user_name")%>' runat="server" />
                        </td>
                        <td><%#Eval("user_name")%></td>
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
         <%--   <div class="l-btns">
                <span>显示</span><asp:TextBox ID="txtPageNum" runat="server" CssClass="pagenum" onkeydown="return checkNumber(event);"
                    OnTextChanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox><span>条/页</span>
            </div>--%>
               <div id="PageContent" runat="server" class="default"></div>
        </div>
        <!--/内容底部-->
    </form>
</body>
</html>



