<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeChat.aspx.cs" Inherits="DTcms.Web.admin.WeChatPage.WeChat" %>

<%@ Import Namespace="DTcms.Common" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>微信</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <style>
        #center {
            margin-top: 100px;
            margin-left: 100px;
            height: 500px;
            font-size: 13px;
            color: gray;
        }

        .while {
            color: white;
        }
    </style>
    <script type="text/javascript">
        var webApiDomain = '<%=this.webApiDomain%>';
        $(function () {
            initWeChat();
        })
        function initWeChat() {
            $.ajax({
                type: "GET",
                url: webApiDomain + "v2/wechat/init/page",
                dataType: "json",
                success: function (result) {
                    var data = result.data;
                    if (result.issuccess) {
                        $("#aLiAppId").val(data.ALiAppId);
                        $("#aLiPayPublidKey").val(data.ALiPayPublicKey);
                        $("#aLiPayPrivateKey").val(data.ALiPayPrivateKey);
                        $("#weiXinAppId").val(data.WeiXinAppId);
                        $("#weiXinPublicKey").val(data.WeiXinPublicKey);
                        $("#weiXinPrivateKey").val(data.WeiXinPrivateKey);
                    }
                },
                error: function (a, e, c) {
                    dialogFunction('网络出现异常，请联系管理员！');
                }
            })
        }

        ///页面修改保存
        function dialogFunction(msg) {
            var dialogMsg = top.dialog({
                title: '提示',
                content: msg,
                okValue: '确定',
            }).showModal();

            dialogMsg.addEventListener('close', function () {
                //window.location.href = "WeChatPage/WeChat.aspx";
            });
        }


        function SaveWeChat() {
            var initdata = {
                "ALiAppId": $("#aLiAppId").val(),
                "ALiPayPublicKey": $("#aLiPayPublidKey").val(),
                "ALiPayPrivateKey": $("#aLiPayPrivateKey").val(),
                "WeiXinAppId": $("#weiXinAppId").val(),
                "WeiXinPublicKey": $("#weiXinPublicKey").val(),
                "WeiXinPrivateKey": $("#weiXinPrivateKey").val()    
            };
        
                //alert(initdata.A);
                //alert(initdata.B);
                //alert(initdata.C);
                //alert(initdata.D);
            
            $.ajax({
                type: "POST",
                url: webApiDomain + "v2/wechat/Save/page",
                data: initdata,
                dataType: "json",
                success: function (result) {
                    if (result.issuccess) {
                        dialogFunction('保存成功');
                    } else {
                        dialogFunction('保存失败');
                    }
                },
                error: function (a, e, c) {
                    dialogFunction('保存失败');
                 
                }
            })
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>微信</span>
        </div>
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">基本信息</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <!--列表-->
        <div id="center">
            支付宝AppId:&nbsp<asp:TextBox ID="aLiAppId" style="width: 500px;vertical-align: middle;" CssClass="input" runat="server" /><br/><br/>
            支付宝公钥:&nbsp&nbsp&nbsp&nbsp<asp:TextBox ID="aLiPayPublidKey" CssClass="input" TextMode="multiline" runat="server" Style="vertical-align: middle;width: 500px;" /><br/><br/>
            支付宝私钥:&nbsp&nbsp&nbsp&nbsp<asp:TextBox ID="aLiPayPrivateKey" CssClass="input" TextMode="multiline" runat="server" Style="vertical-align: middle;width: 500px;" /><br/><br/>
            微信AppID:&nbsp&nbsp&nbsp&nbsp<asp:TextBox ID="weiXinAppId" CssClass="input" runat="server" Style="vertical-align: middle;width: 500px;" /><br/><br/>
            微信公钥:&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<asp:TextBox ID="weiXinPublicKey" CssClass="input" TextMode="multiline" runat="server" Style="vertical-align: middle;width: 500px;" /><br/><br/>
            微信秘钥:&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<asp:TextBox ID="weiXinPrivateKey" CssClass="input"  TextMode="multiline" runat="server" Style="vertical-align: middle;width: 500px;" /><br/><br/>
             <input name="btnReturn" type="button" value="提交保存" class="btn red" onclick="javascript: SaveWeChat();" />
            <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
        </div>
    </form>
</body>
</html>
