<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDemand_reply.aspx.cs" Inherits="DTcms.Web.admin.UserDemand.UserDemand_reply" %>

<%@ Import Namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>回复内容</title>
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
            <a href="UserDemand_Main.aspx" class="back"><i></i><span>返回上一页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>内容</span>
        </div>
        <!--/导航栏-->
        <div>
            <div id="Head" class="content-tab-wrap">
                <div class="content-tab">
                    <div class="content-tab-ul-wrap">
                        <ul>
                            <li><a class="selected" href="javascript:;">内容</a></li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="tab-content">
                <div>
                    <dl>
                        <dt><strong>诉求内容：</strong></dt>
                        <dd>
                            <p runat="server" id="p1"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
<%--                            <asp:TextBox ID="TextBox1" runat="server" CssClass="input readonly" datatype="*2-100" sucmsg=" " disabled="disabled" Style="width: 500px;"></asp:TextBox>--%>
                        </dd>
                    </dl>
                </div>
                <div id="image" runat="server"  style="border:100px"></div>
                <br />
                <div id="txt" runat="server">
                    <dl>
                        <dt><strong>首次处理：</strong></dt>
                        <dd>
                            <p runat="server" id="p2"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
<%--                            <asp:TextBox ID="TextBox2" runat="server" CssClass="input readonly" datatype="*2-100" sucmsg=" " disabled="disabled" Style="width: 500px;"></asp:TextBox>--%>
                        </dd>
                    </dl>
                </div>
                <br />
                <div>
                    <dl>
                        <dt><strong>处理结果：</strong></dt>
                        <dd>
                            <p id="par" runat="server" style="color:red"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
                            </dd>
                    </dl>
                </div>
                <br />
                <div id="txt1" runat="server">
                    <dl>
                        <dt><strong>不满意原因：</strong></dt>
                        <dd>
                            <p runat="server" id="p3"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
<%--                            <asp:TextBox ID="TextBox3" runat="server" CssClass="input readonly" datatype="*2-100" sucmsg=" " disabled="disabled" Style="width: 500px;"></asp:TextBox>--%>
                        </dd>
                    </dl>
                </div>
<%--                <br />--%>
                <div id="txt2" runat="server">
                    <dl>
                        <dt><strong>再次处理：</strong></dt>
                        <dd>
                            <p runat="server" id="p4"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
<%--                            <asp:TextBox ID="TextBox4" runat="server" CssClass="input readonly" datatype="*2-100" sucmsg=" " disabled="disabled" Style="width: 500px;"></asp:TextBox>--%>
                        </dd>
                    </dl>
                </div>
                <br />
                <div id="txt3" runat="server">
                    <dl>
                        <dt><strong>回复内容：</strong></dt>
                        <dd>
                            <p runat="server"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
                            <asp:TextBox ID="txtcontent" runat="server" CssClass="input normal" datatype="*2-100" sucmsg=" " Style="width: 500px;"></asp:TextBox>
                        </dd>
                    </dl>
                </div >
                <div class="page-footer" id="txt4" runat="server" >
                    <div class="btn-wrap">
                        <asp:Button ID="btnSubmit" runat="server" Text="回复" CssClass="btn" OnClick="btnSubmit_Click"/>
                        <input name="btnReturn" type="button" value="返回" class="btn yellow" onclick="javascript: history.back(-1);" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

