<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Organization_edit.aspx.cs" Inherits="DTcms.Web.admin.Organization.Organization_edit" %>

<%@ Import Namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <style>
        body {
            margin: 0;
            font: 13px/1.5 "Microsoft YaHei", "Helvetica Neue", "Sans-Serif";
            min-height: 960px;
            min-width: 600px;
        }

        .my-map {
            margin: 0 auto;
            width: 600px;
            height: 600px;
        }

            .my-map .icon {
                background: url(http://lbs.amap.com/console/public/show/marker.png) no-repeat;
            }

            .my-map .icon-flg {
                height: 32px;
                width: 29px;
            }

            .my-map .icon-flg-red {
                background-position: -65px -5px;
            }

        .amap-container {
            height: 100%;
        }
    </style>
    <title>编辑支部管理</title>
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
            //初始化表单验证
            $("#form1").initValidform();

            //下拉框--选择党员
            $("#txt").ready(function () {
                $.ajax({
                    type: "Post",
                    url: "Organization_edit.aspx/GetNameList",
                    data: "{'key':'" + document.getElementById('txt').value + "'}",
                    contentType: "application/json; charset=utf-8",

                    success: function (data) {
                        var option = "";
                        var json = strToJson(data.d);
                        for (var i = 0; i < json.length; i++) {

                            option += "<option value=" + json[i].val + ">" + json[i].text + "</option>";

                        }
                        $("#txt").append(option);
                        var b = document.getElementById("Hidden1").value;
                        var a = document.getElementById("Hidden").value;
                        option = "<option value= '" + b + "'  selected = selected>" + a + "</option>";
                        $("#txt").append(option);

                        $("#txt").tinyselect();

                        document.getElementById("txt").text = a;
                        document.getElementById("txt").value = b;
                    },
                    error: function (err) {
                        alert(err);
                    }
                });
            });



            //高德地图
            var map = new AMap.Map('container', {
                resizeEnable: true,
                zoom: 13,
            });
            AMap.plugin('AMap.Geocoder', function () {
                var geocoder = new AMap.Geocoder({
                    city: "010"//城市，默认：“全国”
                });
                var marker = new AMap.Marker({
                    map: map,
                    bubble: true
                })
                var input = document.getElementById('position');
                var message = document.getElementById('message');
                var longAndLat = document.getElementById('longAndLat');
                map.on('click', function (e) {
                    marker.setPosition(e.lnglat);
                    geocoder.getAddress(e.lnglat, function (status, result) {
                        if (status == 'complete') {
                            input.value = result.regeocode.formattedAddress
                            longAndLat.value = e.lnglat
                            message.innerHTML = ''
                        } else {
                            message.innerHTML = '无法获取地址'
                        }
                    })
                })

                input.onchange = function (e) {
                    var address = input.value;
                    geocoder.getLocation(address, function (status, result) {
                        if (status == 'complete' && result.geocodes.length) {
                            marker.setPosition(result.geocodes[0].location);
                            map.setCenter(marker.getPosition())
                            message.innerHTML = ''
                        } else {
                            message.innerHTML = '无法获取位置'
                        }
                    })
                }

            });

            //显示其他支部
            $("#setFitView").click(function () {
                $.ajax({
                    type: "Post",
                    url: "Organization_edit.aspx/GetPositionList",
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
            });
        })
        //string转换为json
        function strToJson(str) {
            var json = eval('(' + str + ')');
            return json;
        }
        //下拉框change事件
        function change(v) {
            document.getElementById("manager").value = v;
            document.getElementById("manager1").value = document.getElementById("txt").value;
        }

        function pnamechange(a) {
            document.getElementById("hipname").value = a;
        }

    </script>
    <script type="text/javascript" src="http://webapi.amap.com/demos/js/liteToolbar.js"></script>
</head>

<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="Organization_list.aspx" class="back"><i></i><span>返回列表页</span></a>
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
                            <a class="selected" href="javascript:;">基本资料</a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content" style="height: 2400px">
            <dl>
                <dt>父级</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="pname" runat="server"></asp:DropDownList>
                        <asp:HiddenField ID="hipname" runat="server" />
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>党支部名称</dt>
                <dd>
                    <asp:TextBox ID="name" runat="server" CssClass="input normal" RequiredFieldValidator=""></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="name" ErrorMessage="请输入党支部名称"></asp:RequiredFieldValidator>
                </dd>
            </dl>
            <dl>
                <dt>党委书记</dt>
                <dd>
                    <div class="r-list">
                        <select id="txt" onchange="change(this.options[this.options.selectedIndex].text)" runat="server">
                        </select>

                        <asp:HiddenField ID="manager" runat="server" />
                        <asp:HiddenField ID="Hidden" runat="server" />
                        <asp:HiddenField ID="Hidden1" runat="server" />
                        <asp:HiddenField ID="manager1" runat="server" />
                        <asp:HiddenField ID="longAndLat" runat="server" />
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>党组织代码</dt>
                <dd>
                    <asp:TextBox ID="partyCode" runat="server" CssClass="input normal"></asp:TextBox></dd>
            </dl>
            <dl>
                <dt>建立党组织日期</dt>
                <dd>
                    <asp:TextBox ID="createOrganizTime" runat="server" CssClass="input rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" "></asp:TextBox></dd>
            </dl>
            <dl>
                <dt>党组织类别</dt>
                <dd>
                    <div class="rule-multi-radio">
                        <asp:RadioButtonList ID="partyGroupSort" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="0" Selected="True">党委</asp:ListItem>
                            <asp:ListItem Value="1">党总支</asp:ListItem>
                            <asp:ListItem Value="2">党支部</asp:ListItem>
                            <asp:ListItem Value="3">联合党支部</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>党组织属地关系</dt>
                <dd>
                    <asp:TextBox ID="TextBox1" runat="server" CssClass="input normal"></asp:TextBox></dd>
            </dl>
            <dl>
                <dt>党组织通讯地址</dt>
                <dd>
                    <asp:TextBox ID="communicationSite" runat="server" CssClass="input normal"></asp:TextBox></dd>
            </dl>
            <dl>
                <dt>联系电话或传真</dt>
                <dd>
                    <asp:TextBox ID="phone" runat="server" CssClass="input normal" datatype="/^\d+$/" sucmsg=" "></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>领导班子当选日期</dt>
                <dd>
                    <asp:TextBox ID="dateTime1" runat="server" CssClass="input rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" "></asp:TextBox></dd>
            </dl>
            <dl>
                <dt>领导班子届满日期</dt>
                <dd>
                    <asp:TextBox ID="dateTime2" runat="server" CssClass="input rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" "></asp:TextBox></dd>
            </dl>
            <dl>
                <%--                <dt>领导班子书记姓名</dt>
                <dd>
                    <asp:TextBox ID="secretary" runat="server" CssClass="input normal"></asp:TextBox></dd>
            </dl>
            <dl>
                <dt>领导班子副书记姓名</dt>
                <dd>
                    <asp:TextBox ID="secretary1" runat="server" CssClass="input normal"></asp:TextBox></dd>
            </dl>
            <dl>
                <dt>领导班子其他成员</dt>
                <dd>
                    <asp:TextBox ID="secretary2" runat="server" CssClass="input normal"></asp:TextBox></dd>
            </dl>--%>
                <%--     <dl>
    <dt>党组织联络人姓名</dt>
    <dd><asp:TextBox ID="contactPerson" runat="server" CssClass="input normal"></asp:TextBox></dd>
  </dl>
     <dl>
    <dt>党组织联络人电话</dt>
    <dd><asp:TextBox ID="contactPersonTel" runat="server" CssClass="input normal" onkeyup="if(!/^\d+$/.test(this.value)){alert('只能输入数字.');;this.value='';}"></asp:TextBox></dd>
  </dl> --%>
                    <dt>上级党组织名称</dt>
                    <dd>
                        <asp:TextBox ID="superiorGroup" runat="server" CssClass="input normal"></asp:TextBox></dd>
                </dl>
                <dl>
                    <dt>下辖党组织情况</dt>
                    <dd>
                        <label>
                            <asp:RadioButton ID="s1001" GroupName="sub" runat="server" Text="无" />
                        </label>
                        &nbsp&nbsp
                  <label>
                      <asp:RadioButton ID="s1002" GroupName="sub" runat="server" Text="有" /> &nbsp 数量
                       <asp:TextBox ID="txtnumber" runat="server" CssClass="input normal" Style="width: 50px; max-width: 50px;" onkeyup="if(!/^\d+$/.test(this.value)){alert('只能输入数字.');;this.value='';}"></asp:TextBox>个，分别为：<br />
<%--                       <asp:TextBox ID="txtcontent" runat="server" CssClass="input normal" Style="width: 100px; max-width: 100px;"></asp:TextBox>--%>
                        <textarea id="textcontent" class="editor" runat="server" style="width: 300px;height:200px;"></textarea>
                  </label>
                    </dd>
                </dl>
                <dl>
                    <dt>正式党员数量</dt>
                    <dd>
                        <label>
                            男:
           <asp:TextBox ID="TextBox2" runat="server" CssClass="input normal" Style="width: 50px; max-width: 50px;" datatype="/^\d+$/" sucmsg=" "></asp:TextBox>人，女:          
           <asp:TextBox ID="TextBox3" runat="server" CssClass="input normal" Style="width: 100px; max-width: 50px;" datatype="/^\d+$/" sucmsg=" "></asp:TextBox>人</label>
                    </dd>
                </dl>
                <dl>
                    <dt>预备党员数量</dt>
                    <dd>
                        <label>
                            男:
           <asp:TextBox ID="TextBox4" runat="server" CssClass="input normal" Style="width: 50px; max-width: 50px;" datatype="/^\d+$/" sucmsg=" "></asp:TextBox>人，女:
           <asp:TextBox ID="TextBox5" runat="server" CssClass="input normal" Style="width: 100px; max-width: 50px;" datatype="/^\d+$/" sucmsg=" "></asp:TextBox>人</label>
                    </dd>
                </dl>
                <dl>
                    <dt>党组织简介</dt>
                    <dd>
                        <asp:TextBox ID="txtorganizIntro" runat="server" TextMode="MultiLine" Style="width: 300px; height: 200px; max-width: 500px; max-height: 800px;"></asp:TextBox></dd>
                </dl>
                <h5>单位信息</h5>
                <dl>
                    <dt>单位名称</dt>
                    <dd>
                        <asp:TextBox ID="companyName" runat="server" CssClass="input normal"></asp:TextBox></dd>
                </dl>
                <dl>
                    <dt>单位性质</dt>
                    <dd>
                        <label>
                            <asp:RadioButton ID="RadioButton1" GroupName="subordinateGroup" runat="server" Text="民办非企业" />
                        </label>
                        &nbsp&nbsp
       
                    <label>
                        <asp:RadioButton ID="RadioButton2" GroupName="subordinateGroup" runat="server" Text="个体工商户" />
                    </label>
                        &nbsp&nbsp
       
                    <label>
                        <asp:RadioButton ID="RadioButton3" GroupName="subordinateGroup" runat="server" Text="企业" />
                    </label>
                        &nbsp&nbsp
       
                    <label>
                        <asp:RadioButton ID="RadioButton4" GroupName="subordinateGroup" runat="server" Text="事业单位" />
                    </label>
                        &nbsp&nbsp
       
                    <label>
                        <asp:RadioButton ID="RadioButton5" GroupName="subordinateGroup" runat="server" Text="政府机构" />
                    </label>
                        &nbsp&nbsp
       
                    <label>
                        <asp:RadioButton ID="RadioButton6" GroupName="subordinateGroup" runat="server" Text="其他" />
                    </label>
                        <dd>
                </dl>
                <dl>
                    <dt>单位人数</dt>
                    <dd>
                        <asp:TextBox ID="peopleCount" runat="server" CssClass="input normal" ></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="请输入有效数字" ControlToValidate="peopleCount" ValidationExpression="^\d+$" ForeColor="Red"></asp:RegularExpressionValidator>
                    </dd>
                </dl>
                <dl>
                    <dt>建立党员服务组织</dt>
                    <dd>
                        <label>
                            <asp:RadioButton ID="t1001" GroupName="teamm" runat="server" Text="无" /></label>
                        &nbsp&nbsp
           <label>
               <asp:RadioButton ID="t1002" GroupName="teamm" runat="server" Text="有" />
               <asp:TextBox ID="team" runat="server" CssClass="input normal" Style="width: 100px; height: 0px; max-width: 100px; max-height: 0px;"></asp:TextBox></label>个
                    </dd>
                </dl>
                <h5>领导班子成员信息</h5>
                <dl>

                    <dt>姓名</dt>
                    <dd>
                        <asp:TextBox ID="Textbox6" runat="server" CssClass="input normal"></asp:TextBox></dd>
                </dl>
                <dl>
                    <dt>职务</dt>
                    <dd>
                        <asp:TextBox ID="Textbox7" runat="server" CssClass="input normal"></asp:TextBox></dd>
                </dl>
                <dl>
                    <dt>联系方式</dt>
                    <dd>
                        <asp:TextBox ID="TextBox" runat="server" CssClass="input normal"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="请输入有效数字" ControlToValidate="TextBox" ValidationExpression="^\d+$" ForeColor="Red"></asp:RegularExpressionValidator>
                    </dd>
                </dl>
                <dl>
                    <dt>备注</dt>
                    <dd>
                        <asp:TextBox ID="Textbox9" runat="server" CssClass="input normal"></asp:TextBox></dd>
                </dl>
                <h5>组织奖惩信息</h5>
                <dl>

                    <dt>奖惩名称</dt>
                    <dd>
                        <asp:TextBox ID="RPtitle" runat="server" CssClass="input normal"></asp:TextBox></dd>
                </dl>
                <dl>
                    <dt>奖惩日期</dt>
                    <dd>
                        <asp:TextBox ID="dateTime" runat="server" CssClass="input rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" "></asp:TextBox></dd>
                </dl>
                <dl>
                    <dt>奖惩说明</dt>
                    <dd>
                        <asp:TextBox ID="content" runat="server" TextMode="MultiLine" Style="width: 300px; height: 200px; max-width: 500px; max-height: 800px;"></asp:TextBox></dd>
                </dl>
                <dl>

                    <dt>批准奖惩的党组织</dt>
                    <dd>
                        <asp:TextBox ID="ratifyOrganiz" runat="server" CssClass="input normal"></asp:TextBox></dd>
                </dl>
                <dl>
                    <dt>组织位置</dt>
                    <dd>
                        <asp:TextBox ID="position" runat="server" CssClass="input normal" placeholder="请点击地图显示组织位置"></asp:TextBox>
                        <input id="setFitView" class="button" type="button" value="显示其他支部" />
                    </dd>
                </dl>
        </div>
        <div id="container" class="my-map" tabindex="0" style="width: 80%; height: 50%; top: 2050px; left: 30px;">
        </div>

        <!--/内容-->

        <!--工具栏-->
        <!--/工具栏-->

        <div class="page-footer">
            <div class="btn-wrap">
                <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" OnClick="btnSubmit_Click" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
    </form>
</body>
</html>
