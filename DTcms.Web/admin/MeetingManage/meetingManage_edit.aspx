<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="meetingManage_edit.aspx.cs" Inherits="DTcms.Web.admin.MeetingManage.meetingManage_edit" %>

<%@ Import Namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>发起会议</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <%--<link rel="stylesheet" type="text/css" href="../js/dist/tinyselect.css" />
    <link rel="stylesheet" type="text/css" href="../skin/mettingcss/example.css" />--%>

    <script type="text/javascript" src="../js/meetingjs/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <%--<script type="text/javascript" src="../js/dist/tinyselect.js"></script>--%>

    <%--<script src="http://cdn.bootcss.com/jquery/3.1.0/jquery.min.js"></script>--%>
    <%--<script src="http://cdn.bootcss.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>--%> 
    <script src="http://cdn.bootcss.com/bootstrap-tagsinput/0.8.0/bootstrap-tagsinput.min.js"></script>
    <script src="http://cdn.bootcss.com/typeahead.js/0.11.1/typeahead.bundle.js"></script>
    <link href="http://cdn.bootcss.com/bootstrap/3.3.7/css/bootstrap.css" rel="stylesheet">
    <%--<link href="http://cdn.bootcss.com/bootstrap-tagsinput/0.8.0/bootstrap-tagsinput.css" rel="stylesheet">--%>
    <link rel="stylesheet" type="text/css" href="../../css/bootstrap-tagsinput.css" />

    <script type="text/javascript">
        $(function () {
            //初始化表单验证
            $("#form1").initValidform();
        });

        var action = '<%=action%>', id = '<%=id%>', userId = '<%=userId%>', webApiDomain = '<%=webApiDomain%>';
        init();
        function init() {


            if (action == 'Edit'){
                $.ajax({
                    url: webApiDomain + "mac/mee/web/detail",
                    type: "GET",
                    data: {"id": id},
                    dataType:"json",
                    success: function (result) {
                        var data = result.data;
                        if (result.issuccess) {
                            $("#txtTitle").val(data.title);
                            $("#txtStartTime").val(data.starttime);
                            $("#txtEndTime").val(data.endtime);
                            $("#txtSite").val(data.site);
                            $("#txtNumber").val(data.peoplecount);
                            $("#txtContent").val(data.content);
                            var cities = new Bloodhound({
                                datumTokenizer: Bloodhound.tokenizers.obj.whitespace('text'),
                                queryTokenizer: Bloodhound.tokenizers.whitespace,
                                prefetch: 'assets/cities.json'
                            });
                            cities.initialize();

                            var elt = $('#select_people');
                            elt.tagsinput({
                                itemValue: 'value',
                                itemText: 'text',
                                typeaheadjs: {
                                    name: 'cities',
                                    displayKey: 'text',
                                    source: cities.ttAdapter()
                                },

                            });
                            for (var i = 0, length = data.people.length; i < length; i++) {
                                //alert(data.people[i].userid + data.people[i].username);
                                elt.tagsinput('add', { "value": data.people[i].userid, "text": data.people[i].username });
                            }
                            for (var i = 0, length = data.people.length; i < length; i++) {
                                //alert(data.people[i].userid + data.people[i].username);
                                elt.tagsinput('add', { "value": data.people[i].userid, "text": data.people[i].username });
                            }
                        }
                    },
                    error: function (a, e, c) {
                        alert(e + c.message + a);
                        dialogFunction('获取会议数据失败,请重试.');
                    }
                });
            }
        }

        //string转换为json
        function strToJson(str) {
            var json = eval('(' + str + ')');
            return json;
        }
        function SelectPersonnel() {
            var winDialog = top.dialog({
                title: '人员选择器',
                url: 'MeetingManage/SelParticipant.aspx',
                width: 650,
                height: 400,
                data: window //传入当前窗口
            }).showModal();

            winDialog.addEventListener('close', function () {

                var data = winDialog.returnValue;

                var cities = new Bloodhound({
                    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('text'),
                    queryTokenizer: Bloodhound.tokenizers.whitespace,
                    prefetch: 'assets/cities.json'
                });
                cities.initialize();

                var elt = $('#select_people');
                elt.tagsinput({
                    itemValue: 'value',
                    itemText: 'text',
                    typeaheadjs: {
                        name: 'cities',
                        displayKey: 'text',
                        source: cities.ttAdapter()
                    },
                   
                });

                for (var i = 0; i < data.length; i++) {
                    elt.tagsinput('add', { "value": data[i].id, "text": data[i].text });
                }
            });
        }

        function submit() {
            //if (document.getElementById("txtStartTime").value == "") {
            //    alert("开始时间不能为空");
            //    document.getElementById("txtStartTime").focus();
            //    return false;
            //}
            //if (document.getElementById("txtEndTime").value == "") {
            //    alert("结束时间不能为空");
            //    document.getElementById("txtEndTime").focus();
            //    return false;
            //}
            //if (document.getElementById("txtTitle").value == "") {
            //    alert("会议主题不能为空");
            //    document.getElementById("txtTitle").focus();
            //    return false;
            //}
            if (action == 'Edit') {
                edit();
                return false;
            } 

            add();
        }

        function add() {
            var title = $("#txtTitle").val(),
                startTime = $("#txtStartTime").val(),
                endTime = $("#txtEndTime").val(),
                site = $("#txtSite").val(),
                number = $("#txtNumber").val(),
                content = $("#txtContent").val();

            if (title == "") {
                parent.jsprint("请填写会议主题", "");
                return;
            }
            if (startTime == "" ) {
                parent.jsprint("请填写开始时间", "");
                return;
            }
            if (endTime == ""){
                parent.jsprint("请填写结束时间", "");
                return;
            }
            var ajaxData = {
                "title": title,
                "statrtime": startTime,
                "endtime": endTime,
                "site": site,
                "count": number,
                "content": content,
                "people": $("#select_people").val(),
                "userid": userId
            };

            $.ajax({
                url: webApiDomain+"mac/mee/web/add",
                type: "POST",
                data: ajaxData,
                dataType: "json",
                success: function (result) {
                    //alert(result);
                    if (result.issuccess) {
                        //dialogFunction('新建会议成功');
                        parent.jsprint("新建会议成功", "");
                        window.location.href = "meetingManage_list.aspx";
                    } else {
                        //dialogFunction('新建会议失败');
                        parent.jsprint("新建会议失败", "");
                    }
                    
                },
                error: function (a, e, c) {
                    //dialogFunction('新建会议失败');
                    parent.jsprint("新建会议失败", "");
                }
            });
        }

        function edit() {
            var title = $("#txtTitle").val(),
                startTime = $("#txtStartTime").val(),
                endTime = $("#txtEndTime").val(),
                site = $("#txtSite").val(),
                number = $("#txtNumber").val(),
                content = $("#txtContent").val();
            if (title == "") {
                parent.jsprint("请填写会议主题", "");
                return;
            }
            if (startTime == "") {
                parent.jsprint("请填写开始时间", "");
                return;
            }
            if (endTime == "") {
                parent.jsprint("请填写结束时间", "");
                return;
            }
            var ajaxData = {
                "id": id,
                "title": title,
                "statrtime": startTime,
                "endtime": endTime,
                "site": site,
                "content": content,
                "count": number,
                "people": $("#select_people").val(),
                "userid": userId
            };
            $.ajax({
                url: webApiDomain + "mac/mee/web/edit",
                type: "POST",
                data: ajaxData,
                dataType: "json",
                success: function (result) {
                    //alert(result);
                    if (result.issuccess) {
                        dialogFunction('修改会议成功');
                    } else {
                        dialogFunction('修改会议失败');
                    }

                },
                error: function (a, e, c) {
                    dialogFunction('修改会议失败');
                }
            });
        }

        function dialogFunction(msg) {
            var dialogMsg = top.dialog({
                title: '提示',
                content: msg,
                okValue: '确定',
            }).showModal();

            dialogMsg.addEventListener('close', function () {
                window.location.href = "MeetingManage/meetingManage_list.aspx"; 
            });
        }

    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a id="back" href="meetingManage_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="meetingManage_list.aspx"><span>发起会议</span></a>
            <i class="arrow"></i>
            <span>会议详情</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">会议详情</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content">
            <dl>

                <dt>会议主题</dt>
                <dd>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="input normal" datatype="*2-100" sucmsg=" "></asp:TextBox></dd>
            </dl>

            <dl>
                <dt>会议时间</dt>
                <dd>
                    <asp:TextBox ID="txtStartTime" runat="server" CssClass="input rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}\s{1}(\d{1,2}:){2}\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" "></asp:TextBox>----
              <asp:TextBox ID="txtEndTime" runat="server" CssClass="input rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}\s{1}(\d{1,2}:){2}\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" "></asp:TextBox></dd>
            </dl>

            <dl>
                <dt>会议地点</dt>
                <dd>
                    <asp:TextBox ID="txtSite" runat="server" CssClass="input normal" datatype="*2-100" sucmsg=" "></asp:TextBox></dd>
            </dl>

            <dl>
                <dt>与会人数</dt>
                <dd>
                    <asp:TextBox ID="txtNumber" runat="server" CssClass="input normal" datatype="/^[0-9]*$/" sucmsg=""></asp:TextBox>
                </dd>
            </dl>

            <dl>
                <dt>与会人员</dt>
                <dd>
                    <div class=" btn-wrap">
                        <%--<asp:HiddenField ID="data" runat="server"></asp:HiddenField>--%>
                        <div class="bs-docs-example" style="width: 300px; float:left;">
                            <input id="select_people" class="input normal" type="text" />
                        </div>
                        <input type="button" value="人员选择" style="margin-left: 5px;float:left;" class="btn btn-default" onclick="SelectPersonnel()" />
                    </div>
                </dd>
            </dl>

            <dl>
                <dt>议题/内容</dt>
                <dd>
                    <%--<asp:TextBox ID="TextContent" runat="server" CssClass="input normal" datatype="*2-100" sucmsg=" "></asp:TextBox>--%>
                    <textarea id="txtContent" class="editor" runat="server" style="width: 300px;height:80px;"></textarea>
                </dd>
            </dl>
        </div>

    </form>
            <!--/内容-->
            <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <input value="提交保存" type="button" onclick="submit()" class="btn red"/>
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        <!--/工具栏-->
</body>
</html>