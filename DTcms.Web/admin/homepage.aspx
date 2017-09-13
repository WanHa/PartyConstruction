<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="homepage.aspx.cs" Inherits="DTcms.Web.admin.homepage" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <meta charset="utf-8">
    <link rel="stylesheet" type="text/css" href="../indexfile/css/common.css">
    <link rel="stylesheet" type="text/css" href="../indexfile/css/index.css">
    <link rel="stylesheet" type="text/css" href="../indexfile/css/tinyscrollbar.css">
    <script type="text/javascript">
        function Login() {
            document.getElementById("<%= btnSubmit.ClientID %>").click();
        }
    </script>
      <style>
    
   input::-webkit-input-placeholder {
       /* placeholder颜色  */
        color: #ffffff;
     }
     input {
        border: 1px solid white;
     }
   </style>
</head>

<body>
    <div class="container w">
        <div class="left l">
            <div class="left1 little_bg">
                <div class="ctBox">
                    <div class="canvas_title"></div>
                    <p class="left1_canvas_title"></p>
                </div>
                <%--<canvas id="left1_canvas" width="314" height="220" data-zr-dom-id="zr_0" style="position: absolute;
                    -webkit-user-select: none;
                    -webkit-tap-highlight-color: rgba(0, 0, 0, 0); 
                    padding: 0px;
                    border-width: 0px;
                    margin:0px;
                    font-family: '锐字逼格青春体简2.0'">
                </canvas>--%>
                <div id="left1_canvas" style="width:314px;height:220px; position:absolute;"></div>
            </div>
            <div class="left2 little_bg">
                <div class="ctBox">
                    <div class="canvas_title"></div>
                    <p class="left2_canvas_title"></p>
                </div>
                <%--<canvas id="left2_canvas" width="314" height="220" data-zr-dom-id="zr_0" style="position: absolute;
                     -webkit-user-select: none;
                     -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
                     padding: 0px;
                     margin: 0px;
                     width:314px;
                     border-width: 0px;">
                </canvas>--%>
                <div id="left2_canvas" style="width: 314px; height: 220px; position:absolute"></div>
            </div>
            <div class="left3 little_bg">
                <div class="ctBox">
                    <div class="canvas_title"></div>
                    <p class="left3_canvas_title"></p>
                </div>
                <%--<canvas id="left3_canvas" width="314" height="220" data-zr-dom-id="zr_0" style="position: relative;
                            /*left: 26px;*/
                            /*top: 16px;*/
                            width: 314px;
                            height: 220px;
                            -webkit-user-select: none;
                            -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
                            padding: 0px;
                            margin: 0px;
                            border-width: 0px;">
                </canvas>--%>
                <div id="left3_canvas" style="width: 314px; height: 220px; position:absolute"></div>
            </div>
        </div>
        <div class="center w l">
            <div class="title w">
                <!--                <p>智慧党建大数据管理平台</p>
-->
            </div>
            <div class="center1 w">
                <div class="party_num_info">
                    <div>
                        <p>党员总数</p>
                        <div class="num num1">
                            <p></p>
                        </div>
                    </div>
                    <div>
                        <p>党组织总数</p>
                        <div class="num num2">
                            <p></p>
                        </div>
                    </div>
                    <div>
                        <p>党员服务组织总数</p>
                        <div class="num num3">
                            <p></p>
                        </div>
                    </div>
                </div>
                <div class="map_info">
                    <div class="flag">
                        <%--<div class="little_flag1 common_flag"></div>
                        <div class="little_flag2 common_flag"></div>
                        <div class="little_flag3 common_flag"></div>
                        <div class="little_flag4 common_flag"></div>
                        <div class="little_flag5 common_flag"></div>
                        <div class="little_flag6 common_flag"></div>
                        <div class="little_flag7 common_flag"></div>
                        <div class="little_flag8 common_flag"></div>
                        <div class="little_flag9 common_flag"></div>--%>
                    </div>
                    <div class="flagInfo_partyName">
                        <div class="flag_info party_info">
                            <p class="party_name" style="width: 150px; word-wrap: break-word; white-space: normal;"></p>
                            <p class="party_addr" style="word-wrap: break-word; white-space: normal;">地址:</p>
                            <p class="party_clerk">书记:</p>
                            <div style="position: relative;" class="party_amount_box">
                                <p class="party_amount" style="display: inline-block">党员:</p>
                                <i class="more one1 one"></i>
                                <i class="more one2 one"></i>
                            </div>
                            <div style="position: relative;" class="active_examples">
                                <p>活动风采</p>
                                <i class="more two"></i>
                            </div>
                        </div>
                        <div id="scrollbar_partyName">
                            <div class="scrollbar">
                                <div class="track">
                                    <div class="thumb">
                                        <div class="end"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="viewport">
                                <div class="overview">
                                    <span></span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="building1"></div>
                    <div class="building2"></div>
                </div>
            </div>
            <div class="center2 little_bg2 l">
                <div class="ctBox">
                    <div class="canvas_title"></div>
                    <p class="center2_canvas_title"></p>
                </div>
                <%--<canvas id="center2_canvas" width="340" height="186" data-zr-dom-id="zr_0" style="position: absolute;
                    /*left: 0px; top: 0px;*/
                    /*width: 796px; */
                    /*height: 150px; */
                    -webkit-user-select: none;
                    -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
                    padding: 0px;
                    margin: 0px;
                    border-width: 0px;">
                </canvas>--%>
                <div id="center2_canvas" style="width: 284px; height: 190px; position:absolute"></div>
            </div>
            <div class="center3 little_bg2 r">
                <div class="ctBox">
                    <div class="canvas_title"></div>
                    <p class="center3_canvas_title"></p>
                </div>
                <%--<canvas id="center3_canvas" width="340" height="186" data-zr-dom-id="zr_0" style="position: absolute;
                    /*left: 0px; top: 0px;*/
                    /*width: 796px; */
                    /*height: 150px; */
                    -webkit-user-select: none;
                    -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
                    padding: 0px;
                    margin: 0px;
                    border-width: 0px;">
                </canvas>--%>
                <div id="center3_canvas" style="width: 284px; height: 190px; position:absolute"></div>
            </div>
        </div>
        <div class="right l">
            <div class="login w">
                <p>《 登 录 》</p>
            </div>
            <div class="right1 little_bg">
                <div class="ctBox">
                    <div class="canvas_title"></div>
                    <p class="right1_canvas_title"></p>
                </div>
                <%--<canvas id="right1_canvas" width="314" height="220" data-zr-dom-id="zr_0" style="position: absolute;
                    -webkit-user-select: none;
                    -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
                    padding: 0px;
                    margin: 0px; border-width: 0px;">
                </canvas>--%>
                <div id="right1_canvas" style="width: 314px; height: 220px; position:absolute"></div>
            </div>
            <div class="right2 little_bg">
                <div class="ctBox">
                    <div class="canvas_title"></div>
                    <p class="right2_canvas_title"></p>
                </div>
                <%--<canvas id="right2_canvas" width="314" height="220" data-zr-dom-id="zr_0" style="position: absolute;
                    /*left: 0px;*/
                    /*top: 0px;*/
                    -webkit-user-select: none;
                    -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
                    padding: 0px;
                    margin: 0px;
                    border-width: 0px;">
                </canvas>--%>
                <div id="right2_canvas" style="width: 314px; height: 220px; position:absolute"></div>
            </div>
            <div class="right3 little_bg">
                <div class="ctBox">
                    <div class="canvas_title"></div>
                    <p class="right3_canvas_title"></p>
                </div>
                <%--<canvas id="right3_canvas" width="314" height="220" data-zr-dom-id="zr_0" style="position: absolute;
                            /*left: 26px;*/
                            /*top: 16px;*/
                            width: 314px;
                            height: 220px;
                            text-align: center;
                            -webkit-user-select: none;
                            -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
                            padding: 0px;
                            margin: 0px;
                            border-width: 0px;">
                </canvas>--%>
                <div id="right3_canvas" style="width: 314px; height: 220px; position:absolute"></div>
            </div>
        </div>
    </div>
    <!--===========================   container2  登录 =========================-->
    <div class="container2" style="position: absolute">
        <div class="logo"></div>
        <div class="word">
            <p>智慧党建</p>
        </div>
        <form runat="server">
            <div class="div"><asp:Button ID="btnSubmit" runat="server" onclick="btnSubmit_Click" style="display:none;"/></div>
            <div class="input_box"><span class="span1 l"></span><input id="login_name" type="text" placeholder="请输入用户名" runat="server"></div>
            <div class="input_box" style="margin-top:18px;"><span class="span2 l"></span><input id="password" type="password" placeholder="请输入密码" runat="server"></div>
        </form>
            <button onclick="Login()">登 录</button>
        <div class="close"></div>
    </div>
    <!--==========================  container3 活动风采 ==============================-->
    <div class="container3">
        <div class="top_nav"></div>
        <div class="active_content">
            <div class="active_title">活动风采</div>
            <div id="scrollbar1">
                <div class="scrollbar">
                    <div class="track" style="height: 390px;">
                        <div class="thumb">
                            <div class="end"></div>
                        </div>
                    </div>
                </div>
                <div class="viewport">
                    <div class="overview">
                        <div class="active_list"></div>
                    </div>
                </div>
            </div>
            <div class="active_close">
                <div></div>
            </div>
        </div>
    </div>
    <!--==========================  container4  个人详情  ==============================-->
    <div class="container4">
        <div class="top_nav"></div>
        <div class="active_content">
            <!--<div class="active_title">活动风采</div>-->
            <div class="go_back">
                <p class="r">返回</p>
            </div>
            <div id="scrollbar3">
                <div class="scrollbar">
                    <div class="track" style="height:390px;">
                        <div class="thumb">
                            <div class="end"></div>
                        </div>
                    </div>
                </div>
                <div class="viewport">
                    <div class="overview">
                        <ul class="personal_details">
                            <li>
                                <i></i>
                                <span>姓名: <span></span> </span>
                            </li>
                            <li>
                                <i></i>
                                <span>所属支部: <span></span> </span>
                            </li>
                            <li>
                                <i></i>
                                <span>支部书记: <span></span> </span>
                            </li>
                            <li>
                                <i></i>
                                <span>上级党组织: <span></span> </span>
                            </li>
                            <li>
                                <i></i>
                                <span>过往活动: <span></span> </span>
                            </li>
                        </ul>
                        <div class="active_list"></div>
                        <!--<div class="active_close">-->
                        <!--<div></div>-->
                        <!--</div>-->
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--===================    active_details  同container3布局类似   ============-->
    <div class="active_details">
        <div class="top_nav"></div>
        <div class="active_content">
            <!--<div class="active_title">活动风采</div>-->
            <div class="go_back">
                <p class="r">返回</p>
            </div>
            <div id="scrollbar33">
                <div class="scrollbar">
                    <div class="track" style="height:285px;">
                        <div class="thumb">
                            <div class="end"></div>
                        </div>
                    </div>
                </div>
                <div class="viewport">
                    <div class="overview">
                        <ul class="personal_details">
                            <li>
                                <i></i>
                                <span>活动名称: <span></span> </span>
                            </li>
                            <li>
                                <i></i>
                                <span>活动地点: <span></span> </span>
                            </li>
                            <li>
                                <i></i>
                                <span>活动时间: <span></span> </span>
                            </li>
                            <li>
                                <i></i>
                                <span>主办单位: <span></span> </span>
                            </li>
                            <li>
                                <i></i>
                                <span>参与人数: <span></span>
                                <%--<div class="span"></div>--%>
                                </span>
                            </li>
                        </ul>
                        <div class="active_description">
                            <p>
                            </p>
                        </div>
                        <div class="img">
                            <img src="../indexfile/imgs/active/timg.jpg" alt="" id="img_a">
                        </div>
                    </div>
                </div>
            </div>



        </div>
    </div>


    <!--        ============================  html模板  ===============================      -->
    <script type="text/html" id="active-tpl">
        {{each data as value i}}
        <div id="exlist1{{i}}" class="l" style="width: 190px; height: 260px; margin-right: 10px; margin-left: 10px;">
            <div class="l list" id="list{{i}}">
                <!--//跳转时会用到的  id  -->
                <a href="#">
                    <img src="{{value.cover_pic}}" />
                    <p>{{value.activity_name}}</p>
                </a>
            </div>
        </div>
        {{/each}}
    </script>
    <script type="text/html" id="ex_active-tpl">
        {{each data as value i}}
        <div  id="exlist{{i}}" class="l" style="width:190px;height:240px;margin-right: 40px;">
        <div class="l list">
            <!--//跳转时会用到的  id  -->
            <a href="#">
                <img src="{{value.cover_pic}}" />
                <p>{{value.activity_name}}</p>
            </a>
        </div>
            </div>
        {{/each}}
    </script>
    <script type="text/html" id="partyName-tpl">
        {{each data as value i}}
        <p onclick="ClickUser({{value.user_id}})">{{value.user_name}}</p>
        {{/each}}
    </script>
    <script type="text/html" id="map-tp1">
        {{each data as value i}}
        <div id="flag{{value.id}}" class="little_flag{{i+1}} common_flag" onclick="ClickFlag({{value.id}})"></div>
        {{/each}}
    </script>
    <script src="../indexfile/vendors/template.js"></script>
    <script src="../indexfile/vendors/jquery-3.1.1.min.js"></script>
    <script src="../indexfile/vendors/echarts.min.js"></script>
    <script src="../indexfile/vendors/dark.js"></script>
    <script src="../indexfile/vendors/chalk.js"></script>
    <script src="../indexfile/vendors/jquery.tinyscrollbar.js"></script>
    <!--<script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js"></script>-->
    <script src="../indexfile/js/index.js"></script>
    <%--<script src="../indexfile/js/active_examples.js"></script>--%>
    <script src="../indexfile/js/login.js"></script>
    <script src="../indexfile/js/canvas_title.js" charset="utf-8"></script>
    <script src="../indexfile/js/canvas_left.js"></script>
    <script src="../indexfile/js/party_info.js"></script>
    <script src="../indexfile/js/canvas_right.js"></script>
    <script type="text/javascript">
        $('.left1_canvas_title').html('党员年龄分布情况');
        $('.left2_canvas_title').html('组织学习时长');
        $('.left3_canvas_title').html('主要经济来源');
        $('.center2_canvas_title').html('党员男女比例情况');
        $('.center3_canvas_title').html('正式党员与预备党员比例');
        $('.right1_canvas_title').html('党组织会议开展情况');
        $('.right2_canvas_title').html('党员诉求情况表');
        $('.right3_canvas_title').html('党费缴纳情况');
        var webApiDomain = '<%=webApiDomain%>';
        InitGroup();
        InitMapTitle();
        getmeetingcount("all");
        getpartypayinfo("all");
        getappealinfo("all");
        GetLearntime("all");
        GetEconomic("all");
        getcount("all");
        GetSex("all");
        GetAge("all");
    </script>
</body>
</html>

