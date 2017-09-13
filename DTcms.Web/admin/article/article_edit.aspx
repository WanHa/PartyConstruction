<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="article_edit.aspx.cs" Inherits="DTcms.Web.admin.article.article_edit" ValidateRequest="false" %>

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
    <script type="text/javascript">
        var videoCount = 0;

        $(function () {
            InitPage();
            InitQiNiuPic();
            InitQiNiuVedio();
            //初始化表单验证
            $("#form1").initValidform();

            //计算用户组价格
            $("#field_control_sell_price").change(function () {
                var sprice = parseFloat($(this).val());
                if (sprice > 0) {
                    $(".groupprice").each(function () {
                        var num = parseFloat($(this).attr("discount")) * sprice / 100;
                        $(this).val(ForDight(num, 2));
                    });
                }
            });

            //初始化编辑器
            var editor = KindEditor.create('.editor', {
                width: '100%',
                height: '350px',
                resizeType: 1,
                uploadJson: '../../tools/upload_ajax.ashx?action=EditorFile&IsWater=1',
                fileManagerJson: '../../tools/upload_ajax.ashx?action=ManagerFile',
                allowFileManager: true,
                items: [
                    'fontname', 'fontsize', '|','forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
                    'removeformat', '|','lineheight', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'link', 'preview']
            });
            var editorMini = KindEditor.create('.editor-mini', {
                width: '100%',
                height: '250px',
                resizeType: 1,
                uploadJson: '../../tools/upload_ajax.ashx?action=EditorFile&IsWater=1',
                items: [
                    'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'emoticons', 'image', 'link']
            });

      <%--  //初始化上传控件
        $(".upload-img").InitUploader({ filesize: "<%=siteConfig.imgsize %>", sendurl: "../../tools/upload_ajax.ashx", swf: "../../scripts/webuploader/uploader.swf", filetypes: "<%=siteConfig.fileextension %>" });
        $(".upload-video").InitUploader({ filesize: "<%=siteConfig.videosize %>", sendurl: "../../tools/upload_ajax.ashx", swf: "../../scripts/webuploader/uploader.swf", filetypes: "<%=siteConfig.videoextension %>" });
        $(".upload-album").InitUploader({ btntext: "批量上传", multiple: true, water: true, thumbnail: true, filesize: "<%=siteConfig.imgsize %>", sendurl: "../../tools/upload_ajax.ashx", swf: "../../scripts/webuploader/uploader.swf" });--%>

        //设置封面图片的样式
        $(".photo-list ul li .img-box img").each(function () {
            if ($(this).attr("src") == $("#hidFocusPhoto").val()) {
                $(this).parent().addClass("selected");
            }
        });

        //创建上传附件
        $(".attach-btn").click(function () {
            showAttachDialog();
        });
        });

        //初始化附件窗口
        function showAttachDialog(obj) {
            var objNum = arguments.length;
            var attachDialog = top.dialog({
                id: 'attachDialogId',
                title: "上传附件",
                url: 'dialog/dialog_attach.aspx',
                width: 500,
                height: 180,
                onclose: function () {
                    var liHtml = this.returnValue; //获取返回值
                    if (liHtml.length > 0) {
                        $("#showAttachList").children("ul").append(liHtml);
                    }
                }
            }).showModal();
            //如果是修改状态，将对象传进去
            if (objNum == 1) {
                attachDialog.data = obj;
            }
        }
        //删除附件节点
        function delAttachNode(obj) {
            $(obj).parent().remove();
        }

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

        function InitQiNiuVedio() {

            var uploader = Qiniu.uploader({
                runtimes: 'html5,flash,html4',      // 上传模式，依次退化
                browse_button: 'vedio_file',         // 上传选择的点选按钮，必需
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
                container: 'qiniu_vedio',             // 上传区域DOM ID，默认是browser_button的父元素
                multi_selection: false,               // 一次只能选择一个文件
                filters: {
                    max_file_size: '1024mb',
                    prevent_duplicates: true,
                    // Specify what files to browse for
                    mime_types: [
                        { title: "Video files", extensions: "avi,rm,rmvb,mkv,mp4" },
                        //{ title: "Image files", extensions: "jpg,gif,png" }, // 限定jpg,gif,png后缀上传
                    ]
                },
                //max_file_size: '1024mb',             // 最大文件体积限制
                flash_swf_url: 'Content/js/qiniu/plupload/Moxie.swf',  //引入flash，相对路径
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
                        plupload.each(files, function (file) {
                            var progress = new FileProgress(file, 'vedioUploadProgress');
                            progress.setStatus("等待...");
                        });
                        //if (uploader.files.length > 1) {
                        //    uploader.removeFile(uploader.files[0]);
                        //    $("#vedioUploadProgress").empty();
                        //}

                        //if (uploader.files.length == 1) {
                        //    plupload.each(files, function (file) {
                        //        var progress = new FileProgress(file, 'vedioUploadProgress');
                        //        progress.setStatus("等待...");
                        //    });
                        //}
                    },
                    'BeforeUpload': function (up, file) {
                        // 每个文件上传前，处理相关的事情
                        var progress = new FileProgress(file, 'vedioUploadProgress');
                        var chunk_size = plupload.parseSize(this.getOption('chunk_size'));
                        if (up.runtime === 'html5' && chunk_size) {
                            progress.setChunkProgess(chunk_size);
                        }
                    },
                    'UploadProgress': function (up, file) {
                        // 每个文件上传时，处理相关的事情
                        var progress = new FileProgress(file, 'vedioUploadProgress');
                        var chunk_size = plupload.parseSize(this.getOption('chunk_size'));

                        progress.setProgress(file.percent + "%", file.speed, chunk_size);
                    },
                    'UploadComplete': function () {
                        $("#progress_table tbody").html("");
                        $("#progress_table").hide();
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

                        var video_length = 0;
                        // 获取视频时长
                        $.ajax({
                            url: up.getOption('domain') + file.name + "?avinfo",
                            type: "GET",
                            async: false,
                            dataType: "json",
                            success: function (result) {
                                video_length = parseInt(result.format.duration);
                            }
                        });

                        var progress = new FileProgress(file, 'vedioUploadProgress');
                        progress.setComplete(up, info);

                        var url = up.getOption('domain') + file.name;

                        var tableStr = "<tr id='" + videoCount + "'><td>第" + (parseInt(videoCount) + 1)
                            + "课:</td><td><input name='video_name' readonly='true' style='border: 0px;' value='" + file.name
                            + "'></input></td><td>视频地址:<input name='video_url' readonly='true' style='width: 300px;border: 0px;' value='"
                            + url + "'></input></td>" + "<td>视频时长(秒)：<input name='video_length' readonly='true' style='width:50px;border:0px;' value='"
                            + video_length
                            + "'></input></td>"
                            + "<td><input type='button' onclick='DeteleVideo(" + videoCount + ")' value='删除'/></td></tr>";
                        $("#data_table").append(tableStr);
                        videoCount++;
                    },
                    'Error': function (up, err, errTip) {
                        //上传出错时，处理相关的事情
                        $('table').show();
                        var progress = new FileProgress(err.file, 'vedioUploadProgress');
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

            $('#vedio_up_load').on('click', function () {
                uploader.start();
            });
            //$('#vedio_up_load').on('click', function () {
            //    uploader.stop();
            //});
        }


        function DeteleVideo(deleteId) {
            var name = new Array();
            var url = new Array();
            var videoLength = new Array();
            $("input[name='video_name']").each(function (i) {
                if (deleteId != i) {
                    name.push($(this).val());
                } else {
                    // 删除七牛云的文件
                    $.ajax({
                        type: "POST",
                        url: "article_edit.aspx/DeleteQiNiuFileFun",
                        data: "{'filename':'" + $(this).val() + "'}",
                        contentType: "application/json; charset=utf-8",
                    });
                }
            });

            $("input[name='video_url']").each(function (i) {
                if (deleteId != i) {
                    url.push($(this).val());
                }
            });

            $("input[name='video_length']").each(function (i) {
                if (deleteId != i) {
                    videoLength.push($(this).val());
                }
            });

            $("#data_table tbody").empty();

            for (var i = 0; i < name.length; i++) {
                var str = "<tr><td>第" + (i + 1) + "课</td><td><input name='video_name' readonly='true' style='border: 0px;' value='" +
                    name[i] + "' /></td><td>视频地址：<input input name='video_url' readonly='true' style='width: 300px;border: 0px;' value='" +
                    url[i] + "' /></td>"
                    + "<td>视频时长(秒)：<input name='video_length' readonly='true' style='width:50px;border:0px;' value='"
                    + videoLength[i]
                    + "'></input></td>" +
                    "<td><input type='button' onclick='DeteleVideo(" + i + ")'  value='删除' /></td></tr>";
                $("#data_table").append(str);
            }
            videoCount--;
        }

        function InitPage() {
            videoCount = '<%=this.video_count%>';
            if (videoCount != 0) {
                $("#data_table").append($("#video_str").val());
            }
        }
    </script>

</head>

<body class="mainbody">
    <form id="form1" runat="server">
        <input id="video_str" runat="server" style="display:none"/>
        <!--导航栏-->
        <div class="location">
            <a href="article_list.aspx?channel_id=<%=this.channel_id %>" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="article_list.aspx?channel_id=<%=this.channel_id %>"><span>内容管理</span></a>
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
                        <li id="field_tab_item" runat="server" visible="false"><a href="javascript:;">扩展选项</a></li>
                        <li><a href="javascript:;">详细描述</a></li>
                        <%--<li><a href="javascript:;">SEO选项</a></li>--%>
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
            <%--<dl>
                <dt>显示状态</dt>
                <dd>
                    <div class="rule-multi-radio">
                        <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="0" Selected="True">正常</asp:ListItem>
                            <asp:ListItem Value="1">待审核</asp:ListItem>
                            <asp:ListItem Value="2">不显示</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </dd>
            </dl>--%>
            <dl style="display:none;">
                <dt>推荐类型</dt>
                <dd>
                    <div class="rule-multi-checkbox">
                        <asp:CheckBoxList ID="cblItem" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="1">允许评论</asp:ListItem>
                            <asp:ListItem Value="1">置顶</asp:ListItem>
                            <asp:ListItem Value="1">推荐</asp:ListItem>
                            <asp:ListItem Value="1">热门</asp:ListItem>
                            <asp:ListItem Value="1">幻灯片</asp:ListItem>
                        </asp:CheckBoxList>
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
            <dl id="div_sub_title" runat="server" visible="false">
                <dt>
                    <asp:Label ID="div_sub_title_title" runat="server" Text="副标题" /></dt>
                <dd>
                    <asp:TextBox ID="field_control_sub_title" runat="server" CssClass="input normal" datatype="*0-255" sucmsg=" " />
                    <asp:Label ID="div_sub_title_tip" runat="server" CssClass="Validform_checktip" />
                </dd>
            </dl>
            <dl>
                <dt>封面图片</dt>
                <dd>
                    
                    <%--<div class="upload-box upload-img"></div>--%>
                    <div id="qiniu_pic">
                        <asp:TextBox ID="txtImgUrl" runat="server" CssClass="input normal upload-path" />
                        <a class="btn btn-default btn-lg " id="pickfiles"  href="#">
                           
                            <span>浏览</span>
                        </a>

                        <a class="btn btn-default btn-lg " id="up_load"  href="#">
                            <span>确认上传</span>
                        </a>

                        <%--<a class="btn btn-default btn-lg " id="stop_load" style="width: 160px" href="#">
                            <span>暂停上传</span>
                        </a>--%>
                    </div>

                    <div>
                        <table id="pic_table" class="table table-striped table-hover text-left" style="margin-top: 40px;width:450px; display: none">
                            <%--<thead>
                                <tr>
                                    <th class="col-md-4">Filename</th>
                                    <th class="col-md-2">Size</th>
                                    <th class="col-md-6">Detail</th>
                                </tr>
                            </thead>--%>
                            <tbody id="fsUploadProgress"></tbody>
                        </table>
                            
                    </div>
                </dd>
            </dl>
            <dl id="div_goods_no" runat="server" visible="false">
                <dt>
                    <asp:Label ID="div_goods_no_title" runat="server" Text="商品货号" /></dt>
                <dd>
                    <asp:TextBox ID="field_control_goods_no" runat="server" CssClass="input normal" datatype="*0-100" sucmsg=" " />
                    <asp:Label ID="div_goods_no_tip" runat="server" CssClass="Validform_checktip" />
                </dd>
            </dl>
            <dl id="div_stock_quantity" runat="server" visible="false">
                <dt>
                    <asp:Label ID="div_stock_quantity_title" runat="server" Text="库存数量" /></dt>
                <dd>
                    <asp:TextBox ID="field_control_stock_quantity" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
                    <asp:Label ID="div_stock_quantity_tip" runat="server" CssClass="Validform_checktip" />
                </dd>
            </dl>
            <dl id="div_market_price" runat="server" visible="false">
                <dt>
                    <asp:Label ID="div_market_price_title" runat="server" Text="市场价格" /></dt>
                <dd>
                    <asp:TextBox ID="field_control_market_price" runat="server" CssClass="input small" datatype="/^(([1-9]{1}\d*)|([0]{1}))(\.(\d){1,2})?$/" sucmsg=" ">0</asp:TextBox>
                    元
      <asp:Label ID="div_market_price_tip" runat="server" CssClass="Validform_checktip" />
                </dd>
            </dl>
            <dl id="div_sell_price" runat="server" visible="false">
                <dt>
                    <asp:Label ID="div_sell_price_title" runat="server" Text="销售价格" /></dt>
                <dd>
                    <asp:TextBox ID="field_control_sell_price" runat="server" CssClass="input small" datatype="/^(([1-9]{1}\d*)|([0]{1}))(\.(\d){1,2})?$/" sucmsg=" ">0</asp:TextBox>
                    元
      <asp:Label ID="div_sell_price_tip" runat="server" CssClass="Validform_checktip" />
                </dd>
            </dl>
            <asp:Repeater ID="rptPrice" runat="server">
                <HeaderTemplate>
                    <dl>
                        <dt>党员价格</dt>
                        <dd>
                            <div class="table-container">
                                <table border="0" cellspacing="0" cellpadding="0" class="border-table">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <th width="20%"><%#Eval("title")%></th>
                        <td width="80%">
                            <asp:HiddenField ID="hidePriceId" runat="server" />
                            <asp:HiddenField ID="hideGroupId" Value='<%#Eval("id") %>' runat="server" />
                            <asp:TextBox ID="txtGroupPrice" runat="server" discount='<%#Eval("discount") %>' CssClass="td-input groupprice" MaxLength="10" Style="width: 60px;" datatype="/^(([1-9]{1}\d*)|([0]{1}))(\.(\d){1,2})?$/" sucmsg=" ">0</asp:TextBox>
                            <span class="Validform_checktip">*享受<%#Eval("discount")%>折优惠</span>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
      </div>
    </dd>
  </dl>
                </FooterTemplate>
            </asp:Repeater>
            <dl id="div_point" runat="server" visible="false">
                <dt>
                    <asp:Label ID="div_point_title" runat="server" Text="积分" /></dt>
                <dd>
                    <asp:TextBox ID="field_control_point" runat="server" CssClass="input small" datatype="/^-?\d+$/" sucmsg=" ">0</asp:TextBox>
                    <asp:Label ID="div_point_tip" runat="server" CssClass="Validform_checktip" />
                </dd>
            </dl>
            <dl>
                <dt>排序数字</dt>
                <dd>
                    <asp:TextBox ID="txtSortId" runat="server" CssClass="input small" datatype="n" sucmsg=" ">99</asp:TextBox>
                    <span class="Validform_checktip">*数字，越小越向前</span>
                </dd>
            </dl>
            <%--<dl>
                <dt>浏览次数</dt>
                <dd>
                    <asp:TextBox ID="txtClick" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
                    <span class="Validform_checktip">点击浏览该信息自动+1</span>
                </dd>
            </dl>--%>
            <dl>
                <dt>发布时间</dt>
                <dd>
                    <asp:TextBox ID="txtAddTime" runat="server" CssClass="input rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}\s{1}(\d{1,2}:){2}\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" " />
                    <span class="Validform_checktip">不选择默认当前发布时间</span>
                </dd>
            </dl>
            <dl id="div_albums_container" runat="server" visible="false">
                <dt>图片相册</dt>
                <dd>
                    <div class="upload-box upload-album"></div>
                    <input type="hidden" name="hidFocusPhoto" id="hidFocusPhoto" runat="server" class="focus-photo" />
                    <div class="photo-list">
                        <ul>
                            <asp:Repeater ID="rptAlbumList" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <input type="hidden" name="hid_photo_name" value="<%#Eval("id")%>|<%#Eval("original_path")%>|<%#Eval("thumb_path")%>" />
                                        <input type="hidden" name="hid_photo_remark" value="<%#Eval("remark")%>" />
                                        <div class="img-box" onclick="setFocusImg(this);">
                                            <img src="<%#Eval("thumb_path")%>" bigsrc="<%#Eval("original_path")%>" />
                                            <span class="remark"><i><%#Eval("remark").ToString() == "" ? "暂无描述..." : Eval("remark").ToString()%></i></span>
                                        </div>
                                        <a href="javascript:;" onclick="setRemark(this);">描述</a>
                                        <a href="javascript:;" onclick="delImg(this);">删除</a>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </dd>
            </dl>
            <dl id="div_attach_container" runat="server" visible="false">
                <dt>上传附件</dt>
                <dd>
                    <a class="icon-btn add attach-btn"><span>添加附件</span></a>
                    <div id="showAttachList" class="attach-list">
                        <ul>
                            <asp:Repeater ID="rptAttachList" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <input name="hid_attach_id" type="hidden" value="<%#Eval("id")%>" />
                                        <input name="hid_attach_filename" type="hidden" value="<%#Eval("file_name")%>" />
                                        <input name="hid_attach_filepath" type="hidden" value="<%#Eval("file_path")%>" />
                                        <input name="hid_attach_filesize" type="hidden" value="<%#Eval("file_size")%>" />
                                        <i class="icon"></i>
                                        <a href="javascript:;" onclick="delAttachNode(this);" class="del" title="删除附件"></a>
                                        <a href="javascript:;" onclick="showAttachDialog(this);" class="edit" title="更新附件"></a>
                                        <div class="title"><%#Eval("file_name")%></div>
                                        <div class="info">类型：<span class="ext"><%#Eval("file_ext")%></span> 大小：<span class="size"><%#Convert.ToInt32(Eval("file_size")) > 1024 ? Convert.ToDouble((Convert.ToDouble(Eval("file_size")) / 1024f)).ToString("0.0") + "MB" : Eval("file_size") + "KB"%></span> 下载：<span class="down"><%#Eval("down_num")%></span>次</div>
                                        <div class="btns">下载积分：<input type="text" name="txt_attach_point" onkeydown="return checkNumber(event);" value="<%#Eval("point")%>" /></div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </dd>
            </dl>
        </div>

        <div id="field_tab_content" runat="server" visible="false" class="tab-content" style="display: none">
        </div>

        <div class="tab-content" style="display: none">
           <%-- <dl>
                <dt>调用别名</dt>
                <dd>
                    <asp:TextBox ID="txtCallIndex" runat="server" CssClass="input normal" datatype="/^\s*$|^[a-zA-Z0-9\-\_]{2,50}$/" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">*别名访问，非必填，不可重复</span>
                </dd>
            </dl>--%>
           <%-- <dl>
                <dt>URL链接</dt>
                <dd>
                    <asp:TextBox ID="txtLinkUrl" runat="server" MaxLength="255" CssClass="input normal" />
                    <span class="Validform_checktip">填写后直接跳转到该网址</span>
                </dd>
            </dl>--%>
            <dl id="div_source" runat="server" visible="false">
                <dt>
                    <asp:Label ID="div_source_title" runat="server" Text="信息来源" /></dt>
                <dd>
                    <asp:TextBox ID="field_control_source" runat="server" CssClass="input normal"></asp:TextBox>
                    <asp:Label ID="div_source_tip" runat="server" CssClass="Validform_checktip" />
                </dd>
            </dl>
            <dl id="div_author" runat="server" visible="false">
                <dt>
                    <asp:Label ID="div_author_title" runat="server" Text="文章作者" /></dt>
                <dd>
                    <asp:TextBox ID="field_control_author" runat="server" CssClass="input normal" datatype="s0-50" sucmsg=" "></asp:TextBox>
                    <asp:Label ID="div_author_tip" runat="server" CssClass="Validform_checktip" />
                </dd>
            </dl>
            <%--<dl>
                <dt>内容摘要</dt>
                <dd>
                    <asp:TextBox ID="txtZhaiyao" runat="server" CssClass="input" TextMode="MultiLine" datatype="*0-255" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">不填写则自动截取内容前255字符</span>
                </dd>
            </dl>--%>
            <dl>
                <dt>内容描述</dt>
                <dd>
                    <textarea id="txtContent" class="editor" style="visibility: hidden;" runat="server"></textarea>
                </dd>
            </dl>
        </div>

        <div class="tab-content" style="display: none">
            <dl>
                <dt>SEO标题</dt>
                <dd>
                    <asp:TextBox ID="txtSeoTitle" runat="server" MaxLength="255" CssClass="input normal" datatype="*0-100" sucmsg=" " />
                    <span class="Validform_checktip">255个字符以内</span>
                </dd>
            </dl>
            <dl>
                <dt>SEO关健字</dt>
                <dd>
                    <asp:TextBox ID="txtSeoKeywords" runat="server" CssClass="input" TextMode="MultiLine" datatype="*0-255" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">以“,”逗号区分开，255个字符以内</span>
                </dd>
            </dl>
            <dl>
                <dt>SEO描述</dt>
                <dd>
                    <asp:TextBox ID="txtSeoDescription" runat="server" CssClass="input" TextMode="MultiLine" datatype="*0-255" sucmsg=" "></asp:TextBox>
                    <span class="Validform_checktip">255个字符以内</span>
                </dd>
            </dl>
        </div>
        <!--/内容-->

        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" OnClick="btnSubmit_Click" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        <!--/工具栏-->

    </form>
</body>
</html>
