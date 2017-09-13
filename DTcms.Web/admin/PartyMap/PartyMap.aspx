<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartyMap.aspx.cs" Inherits="DTcms.Web.admin.PartyMap.PartyMap" %>
<%@ Import Namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>党建地图</title>
    <style>
		body { margin: 0; font: 13px/1.5 "Microsoft YaHei", "Helvetica Neue", "Sans-Serif"; min-height: 960px; min-width: 600px; }
		.my-map { margin: 0 auto; width: 600px; height: 600px; }
		.my-map .icon { background: url(http://lbs.amap.com/console/public/show/marker.png) no-repeat; }
		.my-map .icon-flg { height: 32px; width: 29px; }
		.my-map .icon-flg-red { background-position: -65px -5px; }
		.amap-container{height: 100%;}
	</style>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../skin/tinyselect.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://cache.amap.com/lbs/static/main1119.css" />
    <script src="http://webapi.amap.com/maps?v=1.3&key=d5877335f5f846de8c293264becf2b13"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/webuploader/webuploader.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/uploader.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/tinyselect.js"></script>
    <script type="text/javascript">

        $(function () {
            
            //高德地图

            var map = new AMap.Map('container', {
                center: [117.000923, 36.675807],
                zoom: 6
            });
            map.plugin(["AMap.ToolBar"], function () {
                map.addControl(new AMap.ToolBar());
            });

            $.ajax({
                type: "Post",
                url: "PartyMap.aspx/GetPositionList",
                data: "",
                contentType: "application/json; charset=utf-8",

                success: function (data) {
                    var p = strToJson(data.d);
                    //添加点标记
                    var markers = [];
                    for (var i = 0, marker; i < p.length; i++) {
                        marker = new AMap.Marker({
                            icon: "http://lbs.amap.com/console/public/show/marker.png",
                            map: map,
                            position: [p[i].lng, p[i].lat],
                            content: "<div class='icon icon-flg icon-flg-red'></div>"
                        
                        });
                        //设置鼠标划入时显示的文本信息
                        marker.setTitle("人数:" + p[i].num);

                        // 设置label标签
                        marker.setLabel({//label默认蓝框白底左上角显示，样式className为：amap-marker-label
                            offset: new AMap.Pixel(20, 20),//修改label相对于maker的位置
                            content: p[i].title
                        });
                        markers.push(marker);
                    }
                    var newCenter = map.setFitView();
                },
                error: function (err) {
                    alert(err);
                }
            });
        })
        //string转换为json
        function strToJson(str) {
            var json = eval('(' + str + ')');
            return json;
        }
    </script>
    <script type="text/javascript" src="http://webapi.amap.com/demos/js/liteToolbar.js"></script>
</head>

<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="Organization_list.aspx"><span>支部管理</span></a>
            <i class="arrow"></i>
            <span>编辑</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li>
                            <a class="selected" href="javascript:;">党建地图</a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content" style="height: 600px">
            <dl>
                <dt>组织位置</dt>
                <dd>
                    <asp:TextBox ID="position" runat="server" CssClass="input normal"></asp:TextBox>
                    <input id="setFitView" class="button" type="button" value="显示其他支部" />
                </dd>
            </dl>
        </div>

        <div id="container" class="my-map" tabindex="0" style="width: 100%; height: 85%;top:85px">
        </div>

        <!--/内容-->

        <!--工具栏-->
        <div class="page-footer">
            <%--<div class="btn-wrap">
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>--%>
        </div>
        <!--/工具栏-->

    </form>
</body>
</html>
