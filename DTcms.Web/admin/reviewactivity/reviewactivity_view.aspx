<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reviewactivity_view.aspx.cs" Inherits="DTcms.Web.admin.reviewactivity.reviewactivity_view" %>

<%@ Import Namespace="DTcms.Common" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>评选活动图表统计</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
   <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/webuploader/webuploader.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/uploader.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/tinyselect.js"></script>
    <script src="../js/echarts.js"></script>
    <script>
        $(function () {
            var id = $("#Hidden").val();
            $.ajax({
                type: "POST",
                url: "reviewactivity_view.aspx/GetReviewactity",
                data: "{'id':'"+id+"'}",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    var av = strToJson(data.d);
                    openwin(av);
                    
                },
                error: function (er) {
                    alert(er.responseText);
                }
            });
            //$.ajax({
            //    url: "reviewactivity_view.aspx/GetReviewactity",
            //    type: "POST",
            //    data: "{'id':'" + $("#Hidden").val() + "'}",
            //    contentType: "application/json; charset=utf-8",
            //    success: function (data) {
            //        alert("sas");
            //    },
            //    error: function (er) {
            //        alert("错误");
            //    }
                
            //});
        });
        //string转换为json
        function strToJson(str) {
            var json = eval('(' + str + ')');
            return json;
        }
        function openwin(data) {
            var datakey = new Array();
            var datavalue = new Array();
            for (var i = 0; i < data.length; i++) {
                datakey.push(data[i].username);
                datavalue.push(data[i].cont);
               
            }
            var myChart = echarts.init(document.getElementById('main'));
            var option = {
                title: {
                    text: '投票活动图表统计'
                },
                tooltip: {},
                legend: {
                    data: ['票数']
                },
                xAxis: {
                    data: datakey
                },
                yAxis: {},
                series: [{
                    name: '票数',
                    type: 'bar',
                    data: datavalue
                }]
            };
            myChart.setOption(option);
        }
</script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <div class="location">
          <a href="reviewactivity_list.aspx" class="back"><i></i><span>返回列表页</span></a>
          <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
          <i class="arrow"></i>
        </div>
        <div>
            <asp:HiddenField ID="Hidden" runat="server" ></asp:HiddenField>             
        </div>
        <div id="main" style="width: 1000px;height:500px;top:100px;margin:auto;">

        </div>
    </form>
</body>
</html>
