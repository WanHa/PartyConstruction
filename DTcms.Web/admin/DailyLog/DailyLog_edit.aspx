<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailyLog_edit.aspx.cs" Inherits="DTcms.Web.admin.DailyLog.DailyLog_edit" %>

<%@ Import Namespace="DTcms.Common" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>工作日志</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
</head>
<body class="mainbody">
    <!--导航栏-->
    <div class="location">
        <a href="DailyLog_list.aspx" class="back"><i></i><span>返回列表页</span></a>
        <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
        <i class="arrow"></i>
        <a href="DailyLog_list.aspx"><span>工作日志</span></a>
        <i class="arrow"></i>
        <span>日志详情</span>
    </div>
    <div class="line10"></div>
    <!--/导航栏-->
    <form runat="server">
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">日志详情</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content">
               <div>
                <dl>
                    <dd >
                       <p runat="server" id="title" style="font-weight:900; font-size:large;"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
                    </dd>
                </dl>
               </div>
            <div>
                <dl>
                    <dd style="float: left;">
                        <p runat="server" id="type" style="float: left;"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
                        <p runat="server" id="time" style="float: left; margin-left: 25px;"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
                    </dd>
                </dl>
            </div>
            <div>
                <dl>
                    <dd style="float: left;">
                        <p style="float: left;">发起人：</p>
                        <p runat="server" id="username" style="float: left; margin-left: 1px;"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
                        <p style="float: left; margin-left: 25px;">接收人：</p>
                        <p runat="server" id="senduser" style="float: left; margin-left: 1px;"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
                       
                    </dd>
                </dl>
            </div>
            <%--图片--%>
            <div id="field_tab_content" runat="server" style="border: 0px"></div>

            <div>
                <dl>
                    <dd>
                        <%--内容--%>
                        <textarea id="site" runat="server" align="center" rows="10" cols="87" disabled="disabled" style="background: transparent; border-style: none;" onfocus="window.activeobj=this;this.clock=setInterval(function(){activeobj.style.height=activeobj.scrollHeight+'px';},200);" onblur="clearInterval(this.clock);"></textarea>
                    </dd>
                </dl>
            </div>
            <div id="txt2" runat ="server" style="float:left;">
                <dl>
                    <dt><strong>回复结果：</strong></dt>
                    <dd>
                        <p id="par" runat="server"><u style="border-bottom: 1px; padding-bottom: 3px;"></u></p>
                    </dd>
                </dl>
            </div>
            <div id="txt3" runat="server">
                <dl>
                    <dt style="float:left;"><strong>回复内容：</strong></dt>
                    <dd>
                        <p runat="server"><u style="border-bottom: 1px solid #555555; padding-bottom: 3px;"></u></p>
                        <asp:TextBox ID="txtcontent" runat="server" CssClass="input normal" datatype="*2-100" sucmsg=" " Style="width: 500px;"></asp:TextBox>
                    </dd>
                </dl>
            </div>
            <div class="page-footer" id="txt4" runat="server" style="float:left;">
                <div class="btn-wrap">
                    <asp:Button ID="btnSubmit" runat="server" Text="回复" CssClass="btn" OnClick="btnSubmit_Click" />
                    <input name="btnReturn" type="button" value="返回" class="btn yellow" onclick="javascript: history.back(-1);" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
