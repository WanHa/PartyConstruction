<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExperienceExchange_edit.aspx.cs" Inherits="DTcms.Web.admin.onlinelearning.ExperienceExchange_edit" %>

<%@ Import Namespace="DTcms.Common" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>心得交流</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript">

    </script>


    </head>
<body class="mainbody">
        <!--导航栏-->
        <div class="location">
            <a id="back" href="ExperienceExchange_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="ExperienceExchange_list.aspx" class="back"><span>交流内容</span></a>
            <i class="arrow"></i>
            <span>详情</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">内容详情</a></li>
                    </ul>
                </div>
            </div>
        </div>

    <div class="tab-content">
        <div>
            <dl>
                <dd>
                    <p runat="server" id="p1" style="font-weight: 900; font-size: large; margin-left: 355px"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
                </dd>
            </dl>
        </div>

        <div>
            <dl>
                <dd style="float: left;">
                    <p runat="server" id="p2" style="float: left; margin-left: 100px;"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
                </dd>
            </dl>
        </div>
        <div>
            <dl>
                <dd style="float: left;">
                    <p runat="server" id="p4" style="float: left; margin-left: 100px;"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
                    <p runat="server" id="p3" style="float: left; margin-left: 30px;"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
                </dd>
            </dl>
        </div>
        <%--图片--%>
        <div id="field_tab_content" runat="server" style="border: 50px"></div>
        <div>
            <dl>
                <dd>
                    <%--内容--%>
                    <%--  <asp:TextBox ID="txtcontent" runat="server" CssClass="input readonly" datatype="*2-100" sucmsg=" " disabled="disabled" TextMode="MultiLine" Style="width: 820px; height: 500px; max-width: 820px; max-height: 500px;"></asp:TextBox></dd>--%>
                    <%--    <textarea id="Textcontent" style="display:none" runat="server" rows="100" cols="50"></textarea>--%>
                    <textarea id="Textcontent" runat="server" align="center" rows="100" cols="110" disabled="disabled" style="background: transparent; border-style: none;" onfocus="window.activeobj=this;this.clock=setInterval(function(){activeobj.style.height=activeobj.scrollHeight+'px';},200);" onblur="clearInterval(this.clock);"></textarea>
                </dd>
            </dl>
        </div>

    </div>
    <form id="form1" runat="server">
        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <asp:Button ID="btnSubmit" runat="server" Text="通过" CssClass="btn" OnClick="btnPass_Click" />
                <asp:Button ID="editSubmit" runat="server" Text="拒绝" CssClass="btn" OnClick="btnRefuse_Click" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        <!--/工具栏-->
    </form>-
</body>
</html>
