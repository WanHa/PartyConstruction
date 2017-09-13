<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="party_committee_notify.aspx.cs" Inherits="DTcms.Web.admin.article.party_committee_notify" %>

<%@ Import Namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>编辑内容</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
            <%--<script src="../js/jquery-2.0.3.js" type="text/javascript"></script>--%>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/webuploader/webuploader.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../editor/kindeditor-min.js"></script>
    <%--<script type="text/javascript" charset="utf-8" src="../js/uploader.js"></script>--%>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>

    <script src="../js/qiniu/plupload/moxie.js" type="text/javascript"></script>
    <script src="../js/qiniu/plupload/plupload.dev.js" type="text/javascript"></script>
    <script src="../js/qiniu/plupload/i18n/zh_CN.js" type="text/javascript"></script>
    <script src="../js/qiniu/qiniu/ui.js"></script>
    <script src="../js/qiniu/qiniu/qiniu.js" type="text/javascript"></script>
    <%--<script src="../js/QiNiuFunction.js"></script>--%>
    <%--<link href="../../css/bootstrap.css" rel="stylesheet" type="text/css" />--%>
    <link href="../../css/qiniu-bootstrap.css" rel="stylesheet" type="text/css" />
    <%--       <script src="../js/bootstrap-treeview.js"  type="text/javascript"></script>    --%>
<%--    <link href="../js/ztree/css/demo.css" rel="stylesheet" type="text/css" />--%>
    <%--ztree文件引用--%>
    <link href="../js/ztree/css/metroStyle.css" rel="stylesheet" type="text/css" />
    <link href="../js/ztree/css/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="../js/ztree/js/jquery.ztree.all.js" type="text/javascript"></script>
    <script src="../js/ztree/js/ztree_fun.js" type="text/javascript"></script>
    <script type="text/javascript">
        
        var editor;
        var action = '<%=action%>', id = '<%=id%>', userId = '<%=userId%>', webApiDomain = '<%=webApiDomain%>';
        $(function () {
            InitQiNiuPic();
            InitTree();

            //初始化表单验证
            $("#form1").initValidform();

            //初始化编辑器
            editor = KindEditor.create('.editor', {
                width: '100%',
                height: '350px',
                resizeType: 1,
                uploadJson: '../../tools/upload_ajax.ashx?action=EditorFile&IsWater=1',
                fileManagerJson: '../../tools/upload_ajax.ashx?action=ManagerFile',
                allowFileManager: true,
                items: [
                    'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'lineheight', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'link', 'preview'],
                afterBlur: function () {

                    this.sync();
                }
            });


            function InitQiNiuPic() {

                var uploader = Qiniu.uploader({
                    runtimes: 'html5,flash,html4',      // 上传模式，依次退化
                    browse_button: 'pickfiles',         // 上传选择的点选按钮，必需
                    // 在初始化时，uptoken，uptoken_url，uptoken_func三个参数中必须有一个被设置
                    // 切如果提供了多个，其优先级为uptoken > uptoken_url > uptoken_func
                    // 其中uptoken是直接提供上传凭证，uptoken_url是提供了获取上传凭证的地址，如果需要定制获取uptoken的过程则可以设置uptoken_func
                    uptoken: '<%=this.qiniu_uptoken%>',
                    //uptoken_url: $('#uptoken').val(), // Ajax请求uptoken的Url，强烈建议设置（服务端提供）如果写接口链接，会出现跨域的情况
                    // uptoken_func: function(file){    // 在需要获取uptoken时，该方法会被调用
                    //    // do something
                    //    return uptoken;
                    // },
                    get_new_uptoken: false,             // 设置上传文件的时候是否每次都重新获取新的uptoken
                    // downtoken_url: '/downtoken',
                    // Ajax请求downToken的Url，私有空间时使用，JS-SDK将向该地址POST文件的key和domain，服务端返回的JSON必须包含url字段，url值为该文件的下载地址
                    // unique_names: true,              // 默认false，key为文件名。若开启该选项，JS-SDK会为每个文件自动生成key（文件名）
                    // save_key: true,                  // 默认false。若在服务端生成uptoken的上传策略中指定了sava_key，则开启，SDK在前端将不对key进行任何处理
                    domain: '<%=this.qiniu_domain%>',     // bucket域名，下载资源时用到，必需
                    container: 'qiniu_pic',             // 上传区域DOM ID，默认是browser_button的父元素
                    multi_selection: false,               // 一次只能选择一个文件
                    filters: {
                        max_file_size: '500mb',
                        prevent_duplicates: true,
                        // Specify what files to browse for
                        mime_types: [
                            { title: "Image files", extensions: "jpg,gif,png" }, // 限定jpg,gif,png后缀上传
                        ]
                    },
                    max_file_size: '500mb',             // 最大文件体积限制
                    flash_swf_url: 'admin/js/qiniu/plupload/Moxie.swf',  //引入flash，相对路径
                    max_retries: 3,                     // 上传失败最大重试次数
                    dragdrop: true,                     // 开启可拖曳上传
                    drop_element: 'container',          // 拖曳上传区域元素的ID，拖曳文件或文件夹后可触发上传
                    chunk_size: '0',                  // 分块上传时，每块的体积
                    auto_start: false,                   // 选择文件后自动上传，若关闭需要自己绑定事件触发上传
                    //x_vars : {
                    //    查看自定义变量
                    //    'time' : function(up,file) {
                    //        var time = (new Date()).getTime();
                    // do something with 'time'
                    //        return time;
                    //    },
                    //    'size' : function(up,file) {
                    //        var size = file.size;
                    // do something with 'size'
                    //        return size;
                    //    }
                    //},
                    init: {
                        'FilesAdded': function (up, files) {
                            // 文件添加进队列后，处理相关的事情
                            $('table').show();
                            $('#success').hide();

                            if (uploader.files.length > 1) {
                                uploader.removeFile(uploader.files[0]);
                                $("#fsUploadProgress").empty();
                            }

                            if (uploader.files.length == 1) {
                                plupload.each(files, function (file) {
                                    var progress = new FileProgress(file, 'fsUploadProgress');
                                    progress.setStatus("等待...");
                                });
                            }
                        },
                        'BeforeUpload': function (up, file) {
                            // 每个文件上传前，处理相关的事情
                            var progress = new FileProgress(file, 'fsUploadProgress');
                            var chunk_size = plupload.parseSize(this.getOption('chunk_size'));
                            if (up.runtime === 'html5' && chunk_size) {
                                progress.setChunkProgess(chunk_size);
                            }
                        },
                        'UploadProgress': function (up, file) {
                            // 每个文件上传时，处理相关的事情
                            var progress = new FileProgress(file, 'fsUploadProgress');
                            var chunk_size = plupload.parseSize(this.getOption('chunk_size'));

                            progress.setProgress(file.percent + "%", file.speed, chunk_size);
                        },
                        'UploadComplete': function () {
                            //队列文件处理完毕后，处理相关的事情
                            $('#success').show();
                        },
                        'FileUploaded': function (up, file, info) {
                            // 每个文件上传成功后，处理相关的事情
                            // 其中info是文件上传成功后，服务端返回的json，形式如：
                            // {
                            //    "hash": "Fh8xVqod2MQ1mocfI4S4KpRL6D98",
                            //    "key": "gogopher.jpg"
                            //  }
                            // 查看简单反馈
                            // var domain = up.getOption('domain');
                            // var res = parseJSON(info);
                            // var sourceLink = domain +"/"+ res.key; 获取上传成功后的文件的Url
                            var progress = new FileProgress(file, 'fsUploadProgress');
                            progress.setComplete(up, info);
                            //$("#txtImgUrl").val(up.getOption('domain') + file.name);
                            $("#txtImgUrl").val(file.name);
                            $("#pic_table tbody").html("");
                            $("#pic_table").hide();
                        },
                        'Error': function (up, err, errTip) {
                            //上传出错时，处理相关的事情
                            $('table').show();
                            var progress = new FileProgress(err.file, 'fsUploadProgress');
                            progress.setError();
                            progress.setStatus(errTip);
                        },
                        'Key': function (up, file) {
                            // 若想在前端对每个文件的key进行个性化处理，可以配置该函数
                            // 该配置必须要在unique_names: false，save_key: false时才生效

                            var key = file.name;
                            // do something with key here
                            return key
                        }
                    }
                });

                // domain为七牛空间对应的域名，选择某个空间后，可通过 空间设置->基本设置->域名设置 查看获取

                // uploader为一个plupload对象，继承了所有plupload的方法

                $('#up_load').on('click', function () {
                    uploader.start();
                });
                //$('#stop_load').on('click', function () {
                //    uploader.stop();
                //});
            }
        });

        
        init();
        function init() {
         if (action == 'Edit') {
                $.ajax({
                    url: webApiDomain + "partycomm/notify/web/detail",
                    type: "GET",
                    data: { "id": id },
                    dataType: "json",
                    success: function (result) {
                        var data = result.data;
                        if (result.issuccess) {
                            $("#ddlCategoryId").val(data.category_id);
                            $("#txtTitle").val(data.title);
                            $("#txtImgUrl").val(data.img_url);
                            editor.html(data.content);
                            check_users = data.userid;
                        }
                    },
                    error: function (a, e, c) {
                        alert(e + c.message + a);
                        dialogFunction('获取党委通知失败,请重试.');
                    }
                });
            }
        }

        var arr =[];
        function submit() {
            if (action == 'Edit') {
                Edit();
                return false;
            }
            var treeObj = $.fn.zTree.getZTreeObj("grouptree");
             var   nodes = treeObj.getCheckedNodes(true);
            for (var i = 0; i < nodes.length; i++) {
                //alert(nodes[i].id); //获取选中节点的值
              //result.push(nodes[i].id);
             }
            for (var i = 0; i < check_users.length; i++) {
               //arr.push(check_users[i]);
            }
            Add();
        }

        function Add() {
            var category_id = $("#ddlCategoryId").val(),
                title = $("#txtTitle").val(),
                img_url = $("#txtImgUrl").val(),
                content = $("#txtContent").val(),
                //P_GroupId = $("#grouptree").va(),
                createuser = userId,
                userid = check_users;


            var ajaxData = {
                "category_id": category_id,
                "title": title,
                "img_url": img_url,
                "content": content,
                //"P_GroupId": result,
                "createuser": createuser,
                "userid": userid
            };

            $.ajax({
                url: webApiDomain + "partycomm/notify/web/add",
                type: "POST",
                data: ajaxData,
                dataType: "json",
                success: function (result) {

                    if (result.issuccess) {
                        parent.jsprint("新建党委通知成功", "");
                        window.location.href = "party_committee_notify_list.aspx?channel_id=20";
                    } else {
                        parent.jsprint("新建党委通知失败", "");
                    }
                },
                error: function (a, e, c) {
                    parent.jsprint("新建党委通知失败", "");
                }
            });
        }

        function Edit() {
            var category_id = $("#ddlCategoryId").val(),
                title = $("#txtTitle").val(),
                img_url = $("#txtImgUrl").val(),
                content = $("#txtContent").val(),
                userid = check_users;
            var ajaxData = {
                "id": id,
                "category_id": category_id,
                "title": title,
                "img_url": img_url,
                "content": content,
                "userid": userid
            };
            $.ajax({
                url: webApiDomain + "partycomm/notify/web/edit",
                type: "POST",
                data: ajaxData,
                dataType: "JSON",
                success: function (result) {
                    if (result.issuccess) {
                        parent.jsprint("修改党委通知成功", "");
                        window.location.href = "party_committee_notify_list.aspx?channel_id=20";
                    } else {
                        parent.jsprint("修改党委通知失败", "");
                    }
                },
                error: function (a, e, c) {
                    parent.jsprint("修改党委通知失败", "");
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
                window.location.href = "article/party_committee_notify_list.aspx?channel_id=20";
            });
        }
    </script>

</head>

<body class="mainbody">
    <form id="form1" runat="server">
        <input id="video_str" runat="server" style="display:none"/>
        <!--导航栏-->
        <div class="location">
            <a href="party_committee_notify_list.aspx?channel_id=<%=this.channel_id %>" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="party_committee_notify_list.aspx?channel_id=<%=this.channel_id %>"><span>内容管理</span></a>
            <i class="arrow"></i>
            <span>编辑内容</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">基本信息</a></li>
                        <li><a href="javascript:;">支部详细</a></li>
                    </ul>                   
                </div>
            </div>
        </div>

        <div class="tab-content">
            <dl>
                <dt>所属类别</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlCategoryId" runat="server" datatype="*" sucmsg=" "></asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>内容标题</dt>
                <dd>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="input normal" datatype="*2-100" sucmsg=" " />
                    <span class="Validform_checktip">*标题最多100个字符</span>
                </dd>
            </dl>
  
            <dl>
             <dt>封面图片</dt>
                <dd>                    
                    <div id="qiniu_pic">
                        <asp:TextBox ID="txtImgUrl" runat="server" CssClass="input normal upload-path" />
                        <a class="btn btn-default btn-lg " id="pickfiles"  href="#">                           
                            <span>浏览</span>
                        </a>
                        <a class="btn btn-default btn-lg " id="up_load"  href="#">
                            <span>确认上传</span>
                        </a>
                    </div>
                    
                    <div>
                        <table id="pic_table" class="table table-striped table-hover text-left" style="margin-top: 40px;width:450px; display: none">
                            <tbody id="fsUploadProgress"></tbody>
                        </table>                          
                    </div>
                </dd>
            </dl>

              <dl>
                <dt>内容描述</dt>
                <dd>
                    <textarea id="txtContent" class="editor" style="visibility: hidden;" runat="server"></textarea>
                </dd>
            </dl>
        </div>

      <%--  <div class="tab-content" style="display:none;"> 
            <div style="float:left;width:300px;">
                   <h4 style="padding-left:10%;">组织结构</h4>
                    <ul id="grouptree" class="ztree" style="float:left"></ul>   
            </div>

                <div style="float:left;width:500px;margin-left: 10px;">     
                    <h4 style="padding-left:10%;">组员列表</h4>
                   <ul id="usertree" class="user_ztree" style="float:left"></ul>   
                </div>

        </div>--%>
           <div class="tab-content" style="display: none;">
            <div style="float: left; width: 350px;height: 500px;background:#eee">
              <%--  <h1 style="padding-left: 10%;">组织结构</h1>--%>
                <ul id="grouptree" class="ztree" style="float: left"></ul>
            </div>

            <div style="float: left; width: 550px; margin-left:20px; padding-top: 10px;">
                <div style="width: 550px;border-bottom: 3px solid #ff0000; padding-bottom: 5px; margin-left: 5px;">
                    <span style="font-size: 30px;">党员：</span>
                </div>
                <div style="width: 550px;border-bottom: 1px dotted #EEE;">
                    <input id="group_name" type="text" style="width: 545px;height:50px;font-size: 20px;margin-left: 5px;border:none;text-align:center" readonly="readonly"/>
                </div>
                <ul id="usertree" class="user_ztree" style="float: left"></ul>
            </div>
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
