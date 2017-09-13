<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="ModelMember_edit.aspx.cs" Inherits="DTcms.Web.admin.PartyConstruction.ModelMember_edit" %>
<%@ Import namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<title>编辑会员</title>
<link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<link href="../skin/tinyselect.css" rel="stylesheet" type="text/css">
<script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
<script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
<script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
<script type="text/javascript" charset="utf-8" src="../../scripts/artdialog/dialog-plus-min.js"></script>
<script type="text/javascript" charset="utf-8" src="../../scripts/webuploader/webuploader.min.js"></script>
<script type="text/javascript" charset="utf-8" src="../../editor/kindeditor-min.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/uploader.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/tinyselect.js"></script>
<script type="text/javascript">
    $(function () {
        //初始化表单验证
        $("#form1").initValidform();
        //初始化上传控件
        //$(".upload-img").InitUploader({ sendurl: "../../tools/upload_ajax.ashx", swf: "../../scripts/webuploader/uploader.swf" });
        //初始化编辑器

        var editor = KindEditor.create('.editor', {
            width: '100%',
            height: '250px',
            resizeType: 1,
            uploadJson: '../../tools/upload_ajax.ashx?action=EditorFile&IsWater=1',
            fileManagerJson: '../../tools/upload_ajax.ashx?action=ManagerFile',
            allowFileManager: true,
            items: [
                'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
                'removeformat', '|', 'lineheight', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                'insertunorderedlist', '|', 'image', 'link', 'preview']
        });

        //var editorMini = KindEditor.create('#des', {
        //    width: '100%',
        //    height: '250px',
        //    resizeType: 1,
        //    uploadJson: '../../tools/upload_ajax.ashx?action=EditorFile&IsWater=1',
        //    items: [
        //        'source', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
        //        'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
        //        'insertunorderedlist', '|', 'image', 'link', 'fullscreen']
        //});
    });

    $(function () {
        $("#txt").ready(function () {
            $.ajax({
                type: "Post",
                url: "ModelMember_edit.aspx/GetNameList",
                data: "{'key':'" + document.getElementById('txt').value + "'}",
                contentType: "application/json; charset=utf-8",

                success: function (data) {
                    var option = "";
                    var json = strToJson(data.d);
                    for (var i = 0; i < json.length; i++) {

                        option += "<option value=" + json[i].val + ">" + json[i].text + "</option>";

                    }
                    $("#txt").append(option);

                    var a = document.getElementById("Hidden").value;
                    option = "<option value= -1  selected = selected>" + a + "</option>";
                    $("#txt").append(option);

                    $("#txt").tinyselect();

                    document.getElementById("txt").value = a;
                },
                error: function (err) {
                    alert(err);
                }
            });
        });
    });
    function strToJson(str) {
        var json = eval('(' + str + ')');
        return json;
    }

    function change(v) {
        document.getElementById("manager").value = v;
    }
</script>
</head>

<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="ModelMember.aspx" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="user_list.aspx"><span>会员管理</span></a>
  <i class="arrow"></i>
  <span>编辑会员</span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div id="floatHead" class="content-tab-wrap">
  <div class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a class="selected" href="javascript:;">基本资料</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content">
  <dl>
    <dt>姓名</dt>
    <dd>
        <div class="r-list">
            <select id="txt" onchange="change(this.options[this.options.selectedIndex].text)">
            </select>

            <asp:HiddenField ID="manager" runat="server" />
            <asp:HiddenField ID="Hidden" runat="server" />
        </div>
    </dd>
  </dl>
  <dl>
    <dt>简介</dt>
    <dd>
        <textarea id="des" class="editor" style="" runat="server"></textarea>
    </dd>
  </dl>
</div>
<!--/内容-->

<!--工具栏-->
<div class="page-footer">
  <div class="btn-wrap">
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" onclick="btnSubmit_Click" />
    <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
  </div>
</div>
<!--/工具栏-->

</form>
</body>
</html>
