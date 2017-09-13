<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestQuestion_edit.aspx.cs" Inherits="DTcms.Web.admin.LearnTest.TestQuestion_edit" %>
<%@ Import namespace="DTcms.Common" %>
<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<title>编辑试题</title>
<link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
<script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
<script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
<script type="text/javascript">
        var answerNumber = new Array("A.", "B.", "C.", "D.", "E.", "F.", "G.", "H.", "I.", "G.", "K.");
        var answerCount = 0;
    $(function () {
        //初始化表单验证
        $("#form1").initValidform();
        InitAnswerTable();
    });

    function InitAnswerTable() {
        answerCount = '<%=this.answerCount%>';
        var id = '<%=this.id%>';
        if (id != "") {
            $("#answer_table").append($("#answerHtmlSt").val());
        }
    }

    function add_answer() {
        var tr = "<tr id='tr" + answerCount + "'>"
            + "<td style='padding-left: 5px; padding-top: 5px;'>"
            //+ "<input name='isSelected"
            //+ "' type='checkbox'  value='1' onclick='this.value=(this.value==1)?2:1' style='width: 20px; height: 20px;'>"
            //+ "</input>"
            + "<input id='isSelected" + answerCount
            + "' type='checkbox' onclick='SelectedFun(" + answerCount + ")' style='width: 20px; height: 20px;'>"
            + "</input>"
            + "<input id='isanswer" + answerCount + "' name='isanswer' type='text' value='0' style='display: none'></input>"
            + "</td>"
            + "<td style='padding-left: 5px; padding-top: 5px; font-size: 18px'>" +
            answerNumber[answerCount]
            + "</td>"
            + "<td style='padding-left: 5px; padding-top: 5px;' class='formValue'>" + "<input name='answer"
            + "' type='text' style='width: 500px; height: 30px;'>" + "</input>" + "</td>"
            + "<td style='padding-left: 5px; padding-top: 5px;'>" + "<input style='height: 30px;' type='button' onclick='DeleteTableRow("
            + answerCount + ")' value='删除'></input>" + "</td>"
            + "</tr>";

        $("#answer_table").append(tr);
        answerCount++;
    }
    function DeleteTableRow(deleteId) {

        var isanswer = new Array();
        var answer = new Array();
        $("input[name='isanswer']").each(function (i) {
            if (deleteId != i) {
                isanswer.push($(this).val());
            }
        });

        $("input[name='answer']").each(function (i) {
            if (deleteId != i) {
                answer.push($(this).val());
            }
        });
        $('#answer_table tbody').empty();
        for (var i = 0; i < isanswer.length; i++) {
            var tr = "<tr id='tr" + i + "'>"
                + "<td style='padding-left: 5px; padding-top: 5px;'>" +
                "<input id='isSelected" + i + "' type='checkbox' onclick='SelectedFun(" + i + ")' style='width: 20px; height: 20px;'>"
                + "</input>"
                + "<input id='isanswer" + i + "' name='isanswer' type='text' value='"
                + isanswer[i]
                +"' style='display: none'></input>"
                + "</td>"
                + "<td style='padding-left: 5px; padding-top: 5px; font-size: 18px''>" +
                answerNumber[i]
                + "</td>"
                + "<td style='padding-left: 5px; padding-top: 5px;' class='formValue'>" + "<input name='answer" + "' type='text' value='"
                + answer[i] + "' style='width: 500px; height: 30px;'>" + "</input>" + "</td>"
                + "<td style='padding-left: 5px; padding-top: 5px;'>" + "<input style='height: 30px;' type='button' onclick='DeleteTableRow("
                + i + ")' value='删除'></input>" + "</td>"
                + "</tr>";
            $("#answer_table").append(tr);
            if (isanswer[i] == 1) {
                $("#isSelected" + i).attr("checked", "checked");
            }
        }

        answerCount--;
    }

    function SelectedFun(id) {
        var text = $("#isanswer" + id).val() == "0" ? "1" : "0";
        $("#isanswer" + id).val(text);
    }

    function submit() {
        var isanswer = new Array();
        var answer = new Array();
        var rightAnserCount = 0;
        $("input[name='isanswer']").each(function (i) {
            isanswer.push($(this).val());
            if ($(this).val() == 1) {
                rightAnserCount++;
            }
        });

        $("input[name='answer']").each(function (i) {
                answer.push($(this).val());
        });
        if (answer.length == 0) {
            parent.jsprint("请录入试题答案！", "");
            return;
        }

        if (rightAnserCount != 1) {
            parent.jsprint("请选择一个正确答案", "");
            return;
        }

        document.getElementById("<%= btnSubmit.ClientID %>").click();
    }

</script>
</head>
  

<body class="mainbody">
    <input id="answerHtmlSt" type="text" runat="server" style=" display:none" />
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
          <a id="back" href="TestQuestion_list.aspx?id=<%=this.parentid%>" class="back"><i></i><span>返回列表页</span></a>
          <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
          <i class="arrow"></i>
          <a href="TestQuestion_list.aspx?id=<%=this.parentid%>"><span>试题</span></a>
          <i class="arrow"></i>
          <span>编辑试题</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

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

<div class="tab-content">
    <dl>
        <dt>试题题干</dt>
        <dd>
            <asp:TextBox ID="txtTitle" runat="server" CssClass="input normal"></asp:TextBox>
        </dd>
    </dl>
    <%--试题类型先隐藏--%>
    <dl style="display:none">
        <dt style="display:none">试题类型</dt>
        <dd style="display:none">
            <asp:TextBox ID="TextType" runat="server" CssClass="input normal" hidden="hidden"></asp:TextBox>
        </dd>
    </dl>


    <dl>
        <dt>设置分数</dt>
        <dd>
          <asp:TextBox ID="TextScroe" runat="server"  CssClass="input normal" datatype="/^-?\d+$/" placeholder="具体数字" sucmsg=" "></asp:TextBox>
            <%--<asp:TextBox ID="TextScroe" runat="server" CssClass="input normal" onkeyup="if(!/^\d+$/.test(this.value)){alert('只能输入数字.');;this.value='';}"></asp:TextBox>--%>
        </dd>
    </dl>


            <dl>
                <dt>设置选项</dt>
                <dt><input type="button" id="btn1" value="添加选项" onclick="add_answer()" ></dt>
                <dt>(选中为正确答案)</dt>
            </dl>
                <table id="answer_table" style="margin-left: 80px;"></table>
        </div>



<!--/内容-->
        <!--工具栏-->
        <div class="page-footer">
          <div class="btn-wrap">
            <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" onclick="btnSubmit_Click" style="display:none;"/>
            <%--<input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />--%>
          </div>
        </div>
        <!--/工具栏-->
    </form>
    <div class="page-footer">
            <div class="btn-wrap">
                <input value="提交保存" type="button" onclick="submit()" class="btn red"/>
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
</body>
</html>

