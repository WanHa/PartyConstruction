<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_edit.aspx.cs" Inherits="DTcms.Web.admin.users.user_edit" ValidateRequest="false" %>
<%@ Import namespace="DTcms.Common" %>

<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<title>编辑党员</title>
<link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<link href="../../css/qiniu-bootstrap.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
<script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
<script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
<script type="text/javascript" charset="utf-8" src="../../scripts/artdialog/dialog-plus-min.js"></script>
<script type="text/javascript" charset="utf-8" src="../../scripts/webuploader/webuploader.min.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/uploader.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
<script src="../js/qiniu/plupload/moxie.js" type="text/javascript"></script>
<script src="../js/qiniu/plupload/plupload.dev.js" type="text/javascript"></script>
<script src="../js/qiniu/plupload/i18n/zh_CN.js" type="text/javascript"></script>
<script src="../js/qiniu/qiniu/ui.js"></script>
<script src="../js/qiniu/qiniu/qiniu.js" type="text/javascript"></script>
<script src="../js/QiNiuFunction.js"></script>
<script type="text/javascript">
    var qiniu_uptoken = '<%=this.qiniu_uptoken%>', qiniu_domain = '<%=this.qiniu_domain%>';
    $(function () {
        //初始化表单验证
        $("#form1").initValidform();
        InitQiNiuPic();
        ////初始化上传控件
        //$(".upload-img").InitUploader({ sendurl: "../../tools/upload_ajax.ashx", swf: "../../scripts/  /uploader.swf" });
    });
</script>
</head>

<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="user_list.aspx" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="user_list.aspx"><span>党员管理</span></a>
  <i class="arrow"></i>
  <span>编辑党员</span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div id="floatHead" class="content-tab-wrap" >
  <div class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a class="selected" href="javascript:;">账号信息</a></li>
	    <li><a href="javascript:;">党员信息</a></li>
        <li><a href="javascript:;">教育信息</a></li>
        <li><a href="javascript:;">党内信息</a></li>
        <li><a href="javascript:;">奖惩信息</a></li>
        <li><a href="javascript:;">单位信息</a></li>
        <li><a href="javascript:;">生活困难详情</a></li>
        <li><a href="javascript:;">流动党员</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content" >
  <dl>
    <dt>所属支部</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="ddlGroupId" runat="server" datatype="*" errormsg="请选择组别" sucmsg=" "></asp:DropDownList>
      </div>
    </dd>
  </dl>
  <%--<dl>
    <dt>用户状态</dt>
    <dd>
      <div class="rule-multi-radio">
        <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="0" Selected="True">正常</asp:ListItem>
        <asp:ListItem Value="1">待验证</asp:ListItem>
        <asp:ListItem Value="2">待审核</asp:ListItem>
        <asp:ListItem Value="3">禁用</asp:ListItem>
        </asp:RadioButtonList>
      </div>
      <span class="Validform_checktip">*禁用账户无法登录</span>
    </dd>
  </dl>--%>
  <dl>
<%--   ajaxurl="../../tools/admin_ajax.ashx?action=username_validate"--%>
    <dt>用户名</dt>
    <dd><asp:TextBox ID="txtUserName" runat="server" CssClass="input normal"  datatype="*2-100" sucmsg=" "  ></asp:TextBox> <span class="Validform_checktip">*登录的用户名，支持中文</span></dd>
  </dl> 
  <dl>
    <dt>登录密码</dt>
    <dd><asp:TextBox ID="txtPassword" runat="server" CssClass="input normal" TextMode="Password" datatype="*6-20" nullmsg="请设置密码" errormsg="密码范围在6-20位之间" sucmsg=" "></asp:TextBox> <span class="Validform_checktip">*登录的密码，至少6位</span></dd>
  </dl>
  <dl>
    <dt>确认密码</dt>
    <dd><asp:TextBox ID="txtPassword1" runat="server" CssClass="input normal" TextMode="Password" datatype="*" recheck="txtPassword" nullmsg="请再输入一次密码" errormsg="两次输入的密码不一致" sucmsg=" "></asp:TextBox> <span class="Validform_checktip">*再次输入密码</span></dd>
  </dl>
  <dl>
    <dt>邮箱账号</dt>
    <dd><asp:TextBox ID="txtEmail" runat="server" CssClass="input normal" datatype="/^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/" sucmsg=" "></asp:TextBox> <span class="Validform_checktip">*取回密码时用到</span></dd>
  </dl>
  <dl>
    <dt>用户昵称</dt>
    <dd><asp:TextBox ID="txtNickName" runat="server" CssClass="input normal"></asp:TextBox></dd>
  </dl>
  <dl>
    <dt>上传头像</dt>
    <dd>
      <%--<asp:TextBox ID="txtAvatar" runat="server" CssClass="input normal upload-path"></asp:TextBox>
      <div class="upload-box upload-img"></div>--%>
        <div id="qiniu_pic">
            <asp:TextBox ID="txtImgUrl" runat="server" CssClass="input normal upload-path" />
            <a class="btn btn-default btn-lg " id="pickfiles" href="#">

                <span>浏览</span>
            </a>

            <a class="btn btn-default btn-lg " id="up_load" href="#">
                <span>确认上传</span>
            </a>

            <%--<a class="btn btn-default btn-lg " id="stop_load" style="width: 160px" href="#">
                            <span>暂停上传</span>
                        </a>--%>
        </div>

        <div>
            <table id="pic_table" class="table table-striped table-hover text-left" style="margin-top: 40px; width: 450px; display: none">
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
  <dl>
    <dt>用户性别</dt>
    <dd>
      <div class="rule-multi-radio">
        <asp:RadioButtonList ID="rblSex" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="0" Selected="True">男</asp:ListItem>
        <asp:ListItem Value="1">女</asp:ListItem>
        </asp:RadioButtonList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>生日日期</dt>
    <dd>
      <asp:TextBox ID="txtBirthday"  runat="server" CssClass="input rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" " />
    </dd>
  </dl>
  <dl>
    <dt>手机号码</dt>
    <dd><asp:TextBox ID="txtMobile" runat="server" CssClass="input normal" datatype="/^1[3|4|5|7|8]\d{9}$/" placeholder="请输入有效手机号" sucmsg=" "></asp:TextBox></dd>
  </dl>
  <dl>
    <dt>电话号码</dt>
    <dd><asp:TextBox ID="txtTelphone" runat="server" CssClass="input normal"></asp:TextBox></dd>
  </dl>
  <dl>
    <dt>QQ号码</dt>
    <dd><asp:TextBox ID="txtQQ" runat="server" CssClass="input normal"></asp:TextBox></dd>
  </dl>
     <dl>
    <dt>微信号码</dt>
    <dd><asp:TextBox ID="txtwechat" runat="server" CssClass="input normal"></asp:TextBox></dd>
  </dl>
  <dl>
    <dt>MSN</dt>
    <dd><asp:TextBox ID="txtMsn" runat="server" CssClass="input normal"></asp:TextBox></dd>
  </dl>
  <dl>
    <dt>通讯地址</dt>
    <dd><asp:TextBox ID="txtAddress" runat="server" CssClass="input normal" datatype="*2-100" sucmsg=" " placeholder="请填写真实有效地址" ></asp:TextBox></dd>
  </dl>
</div>
<div class="tab-content" style="display:none;">
  <dl>
    <dt>身份证号码</dt>
    <dd><asp:TextBox ID="identity" runat="server" CssClass="input normal" datatype="/^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$/ | /^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{2}$/" sucmsg=" "></asp:TextBox></dd>
  </dl> 
  <dl>
  <dt>民族</dt>
    <dd>
		<asp:TextBox ID="txtnationId" runat="server" CssClass="input normal"></asp:TextBox>
    </dd>
  </dl> 
  <dl>
  <dt>婚姻状况</dt>
    <dd>
		<div class="rule-multi-radio">
			<asp:RadioButtonList ID="maritalStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
				<asp:ListItem Value="0" Selected="True">未婚</asp:ListItem>
				<asp:ListItem Value="1">已婚</asp:ListItem>
				<asp:ListItem Value="2">丧偶</asp:ListItem>
				<asp:ListItem Value="3">离婚</asp:ListItem>
			</asp:RadioButtonList>
       </div>
   </dd>
   </dl>
    <dl>
      <dt>新阶层人员</dt>
      <dd>
          <label><asp:RadioButton ID ="new1" name="new" runat="server" Text="否" GroupName="newClass"/></label>
          <label  style="margin-left:30px"><asp:RadioButton ID ="new2" name="new" runat="server" Text="是" GroupName="newClass"/></label>
          <asp:TextBox ID="new_class" runat="server" CssClass="input normal" datatype="/^[1-7]*$/" placeholder="填写下列所列数字编号" style="width: 130px; height:0px; max-width: 130px; max-height:0px;" sucmsg=" "></asp:TextBox>
        </dd>
        </dl>
    <dl>
        <dd>
          <span>1.民营科技企业技术人员</span><br />
          <span>2.受聘于外资企业管理技术人员</span><br />
          <span>3.个体劳动者</span><br />
          <span>4.城镇个体劳动者（非农户籍）</span><br />
          <span>5.私营企业主</span><br />
          <span>6.中介资质从业人员</span><br />
          <span>7.自由职业者</span><br />
      </dd>
           </dl>
    <dl>
      <dt>行政级别</dt>
      <dd>
          <label style="display:inline-block;width:100px;"><asp:RadioButton ID="rank1" name="rank" runat="server" Text="无" GroupName="administration_rank"/></label>
          <label style="display:inline-block;width:100px;"><asp:RadioButton ID="rank2" name="rank" runat="server" Text="正部级以上" GroupName="administration_rank"/></label>
          <label style="display:inline-block;width:100px;"><asp:RadioButton ID="rank3" name="rank" runat="server" Text="正省部级" GroupName="administration_rank"/></label>
          <label style="display:inline-block;width:100px;"><asp:RadioButton ID="rank4" name="rank" runat="server" Text="副省部级" GroupName="administration_rank"/></label>
          <label style="display:inline-block;width:100px;"><asp:RadioButton ID="rank5" name="rank" runat="server" Text="正厅局级" GroupName="administration_rank"/></label>
          <label style="display:inline-block;width:100px;"><asp:RadioButton ID="rank6" name="rank" runat="server" Text="副厅局级" GroupName="administration_rank"/></label><br />
          <label style="display:inline-block;width:100px;"><asp:RadioButton ID="rank7" name="rank" runat="server" Text="正县处级" GroupName="administration_rank"/></label>
          <label style="display:inline-block;width:100px;"><asp:RadioButton ID="rank8" name="rank" runat="server" Text="副县处级" GroupName="administration_rank"/></label>
          <label style="display:inline-block;width:100px;"><asp:RadioButton ID="rank9" name="rank" runat="server" Text="正乡科级" GroupName="administration_rank"/></label>
          <label style="display:inline-block;width:100px;"><asp:RadioButton ID="rank10" name="rank" runat="server" Text="副乡科级" GroupName="administration_rank"/></label>
          <label style="display:inline-block;width:100px;"><asp:RadioButton ID="rank11" name="rank" runat="server" Text="科员" GroupName="administration_rank"/></label>
          <label style="display:inline-block;width:100px;"><asp:RadioButton ID="rank12" name="rank" runat="server" Text="办事员" GroupName="administration_rank"/></label>
      </dd>
   </dl>
    <dl>
        <dt>军职级别</dt>
        <dd>
               <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military1" name="military" runat="server" Text="无" GroupName="military_rank"/></label>
              <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military2" name="military" runat="server" Text="正军职以上" GroupName="military_rank"/></label>
              <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military3" name="military" runat="server" Text="正军职" GroupName="military_rank"/></label>
              <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military4" name="military" runat="server" Text="副军职" GroupName="military_rank"/></label>
              <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military5" name="military" runat="server" Text="正师职" GroupName="military_rank"/></label>
              <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military6" name="military" runat="server" Text="副师职" GroupName="military_rank"/></label>
              <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military7" name="military" runat="server" Text="正团职" GroupName="military_rank"/></label><br />
               <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military8" name="military" runat="server" Text="副团职" GroupName="military_rank"/></label>
               <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military9" name="military" runat="server" Text="正营职" GroupName="military_rank"/></label>
               <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military10" name="military" runat="server" Text="副营职" GroupName="military_rank"/></label>
               <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military11" name="military" runat="server" Text="正连职" GroupName="military_rank"/></label>
             <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military12" name="military" runat="server" Text="副连职" GroupName="military_rank"/></label>
             <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military13" name="military" runat="server" Text="正排职" GroupName="military_rank"/></label>
             <label style="display:inline-block;width:100px;"><asp:RadioButton ID="military14" name="military" runat="server" Text="副排职" GroupName="military_rank"/></label>
        </dd>
    </dl>
    <dl>
        <dt>警衔级别</dt>
        <dd>
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police1" name="police" runat="server" Text="无" GroupName="police_rank"/></label>
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police2" name="police" runat="server" Text="一级警监以上" GroupName="police_rank"/></label>
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police3" name="police" runat="server" Text="一级警监" GroupName="police_rank"/></label>
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police4" name="police" runat="server" Text="二级警监" GroupName="police_rank"/></label>
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police5" name="police" runat="server" Text="三级警监" GroupName="police_rank"/></label>
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police6" name="police" runat="server" Text="一级警督" GroupName="police_rank"/></label>
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police7" name="police" runat="server" Text="二级警督" GroupName="police_rank"/></label><br />
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police8" name="police" runat="server" Text="三级警督" GroupName="police_rank"/></label>
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police9" name="police" runat="server" Text="一级警司" GroupName="police_rank"/></label>
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police10" name="police" runat="server" Text="二级警司" GroupName="police_rank"/></label>
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police11" name="police" runat="server" Text="三级警司" GroupName="police_rank"/></label>
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police12" name="police" runat="server" Text="一级警员" GroupName="police_rank"/></label>
            <label style="display:inline-block;width:100px;"><asp:RadioButton ID="police13" name="police" runat="server" Text="二级警员" GroupName="police_rank"/></label>
        </dd>
    </dl>
    <dl>
        <dt>本人子女情况</dt>
        <dd>
         <div class="rule-multi-radio">
        <asp:RadioButtonList ID="children_info" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="1" Selected="True">无</asp:ListItem>
        <asp:ListItem Value="2">独生子女</asp:ListItem>
        <asp:ListItem Value="3">两孩及以上</asp:ListItem>
        </asp:RadioButtonList>
      </div>
        </dd>
    </dl>

     <dl>
        <dt>独生子奖励</dt>
        <dd>
         <div class="rule-multi-radio">
        <asp:RadioButtonList ID="only_child_award" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="1" Selected="True">已领取</asp:ListItem>
        <asp:ListItem Value="2">未领取</asp:ListItem>
        </asp:RadioButtonList>
      </div>
        </dd>
    </dl>

    <dl>
        <dt>护照情况</dt>
        <dd>
              <label style="display:inline-block;width:100px;"><asp:RadioButton ID="passport1" name="passport"  runat="server" Text="无" GroupName="passport_info" /></label>
              <label style="display:inline-block;width:100px;"><asp:RadioButton ID="passport2" name="passport"  runat="server" Text="外交护照" GroupName="passport_info" /></label>
              <label style="display:inline-block;width:100px;"><asp:RadioButton ID="passport3" name="passport"  runat="server" Text="公务护照" GroupName="passport_info" /></label>
              <label style="display:inline-block;width:100px;"><asp:RadioButton ID="passport4" name="passport"  runat="server" Text="普通护照" GroupName="passport_info" /></label>
              <label style="display:inline-block;width:100px;"><asp:RadioButton ID="passport5" name="passport"  runat="server" Text="因公普通护照" GroupName="passport_info" /></label>
              <label style="display:inline-block;width:100px;"><asp:RadioButton ID="passport6" name="passport"  runat="server" Text="因私普通护照" GroupName="passport_info" /></label>
        </dd>
    </dl>

    <dl>
    <dt>优抚对象</dt>
    <dd>
        <label style="display:inline-block;width:130px;"><asp:RadioButton ID="Entitled1" name="EntitledGroups"  runat="server" Text="无" GroupName="EntitledGroupsInfo" checked="true"/></label>
        <label style="display:inline-block;width:130px;"><asp:RadioButton ID="Entitled2" name="EntitledGroups"  runat="server" Text="市级以上劳模" GroupName="EntitledGroupsInfo" /></label>
        <label style="display:inline-block;width:130px;"><asp:RadioButton ID="Entitled3" name="EntitledGroups"  runat="server" Text="烈士遗嘱" GroupName="EntitledGroupsInfo" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="Entitled4" name="EntitledGroups"  runat="server" Text="因公牺牲军人遗嘱" GroupName="EntitledGroupsInfo" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="Entitled5" name="EntitledGroups"  runat="server" Text="病故军人遗嘱" GroupName="EntitledGroupsInfo" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="Entitled6" name="EntitledGroups"  runat="server" Text="在乡退伍红军老战士" GroupName="EntitledGroupsInfo" /></label><br />
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="Entitled7" name="EntitledGroups"  runat="server" Text="西路红军老战士" GroupName="EntitledGroupsInfo" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="Entitled8" name="EntitledGroups"  runat="server" Text="红军失散人员" GroupName="EntitledGroupsInfo" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="Entitled9" name="EntitledGroups"  runat="server" Text="残疾军人" GroupName="EntitledGroupsInfo" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="Entitled10" name="EntitledGroups"  runat="server" Text="复员军人" GroupName="EntitledGroupsInfo" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="Entitled11" name="EntitledGroups"  runat="server" Text="带病回乡退伍军人" GroupName="EntitledGroupsInfo" /></label>
    </dd> 
  </dl> 
    <dl>
    <dt>主要收入来源</dt>
     <dd>
	     <label style="display:inline-block;width:100px;"><asp:RadioButton ID="i1001" GroupName="incomeSource" runat="server" Text="工资" name="income" checked="true"/></label>
         <label style="display:inline-block;width:100px;"><asp:RadioButton ID="i1002" GroupName="incomeSource" runat="server" Text="退休金" name="income" /></label> 
         <label style="display:inline-block;width:100px;"><asp:RadioButton ID="i1003" GroupName="incomeSource" runat="server" Text="经商" name="income"/></label> 
         <label style="display:inline-block;width:100px;"><asp:RadioButton ID="i1004" GroupName="incomeSource" runat="server" Text="务工" name="income"/></label>
         <label style="display:inline-block;width:100px;"><asp:RadioButton ID="i1005" GroupName="incomeSource" runat="server" Text="低保" name="income"/></label> 
         <label style="display:inline-block;width:100px;"><asp:RadioButton ID="i1006" GroupName="incomeSource" runat="server" Text="失业救济金" name="income"/></label> 
         <label style="display:inline-block;width:100px;"><asp:RadioButton ID="i1007" GroupName="incomeSource" runat="server" Text="其他" name="income"/></label>
    </dd> 
  </dl> 
      <dl>
    <dt>户籍地址</dt>
    <dd><asp:TextBox ID="nativePlace" runat="server" CssClass="input normal"></asp:TextBox></dd>
  </dl> 
     <dl>
    <dt>居住地址</dt>
    <dd><asp:TextBox ID="livePlace" runat="server" CssClass="input normal"></asp:TextBox></dd>
  </dl> 
    </div>

    <div class="tab-content" style="display:none;">
      <dl>
          <dt>毕业院校</dt>
          <dd>
              		<asp:TextBox ID="shool" runat="server" CssClass="input normal"></asp:TextBox>
          </dd>
      </dl>
          <dl>
          <dt>毕业时间</dt>
          <dd>
              		<asp:TextBox ID="bytime" runat="server" CssClass="input rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" "></asp:TextBox>
          </dd>
      </dl>
  <dl>
   <dt >学历情况</dt>
    <dd >
         <label style="display:inline-block;width:80px;"><asp:RadioButton ID="e1" name="education"  runat="server" Text="博士" GroupName="educationCondition" checked="true"/></label>
         <label style="display:inline-block;width:80px;"><asp:RadioButton ID="e2" name="education" runat="server" Text="硕士" GroupName="educationCondition" /></label> 
         <label style="display:inline-block;width:80px;"><asp:RadioButton ID="e3" name="education" runat="server" Text="党校研究生" GroupName="educationCondition"/></label> 
         <label style="display:inline-block;width:80px;"><asp:RadioButton ID="e4" name="education" runat="server" Text="大学" GroupName="educationCondition"/></label> 
         <label style="display:inline-block;width:80px;"><asp:RadioButton ID="e5" name="education" runat="server" Text="大专" GroupName="educationCondition"/></label>
         <label style="display:inline-block;width:80px;"><asp:RadioButton ID="e6" name="education" runat="server" Text="党校大学" GroupName="educationCondition"/></label> <br />
         <label style="display:inline-block;width:80px;"><asp:RadioButton ID="e7" name="education" runat="server" Text="中职" GroupName="educationCondition"/></label>
         <label style="display:inline-block;width:80px;"><asp:RadioButton ID="e8" name="education" runat="server" Text="高中" GroupName="educationCondition"/></label>
         <label style="display:inline-block;width:80px;"><asp:RadioButton ID="e9" name="education"  runat="server" Text="初中" GroupName="educationCondition"/></label> 
         <label style="display:inline-block;width:80px;"><asp:RadioButton ID="e10" name="education" runat="server" Text="小学" GroupName="educationCondition"/></label>
         <label style="display:inline-block;width:80px;"><asp:RadioButton ID="e11" name="education" runat="server" Text="其他" GroupName="educationCondition"/></label>
    </dd>
  </dl> 
  <dl>
        <dt>学位情况</dt>
        <dd>
         <div class="rule-multi-radio">
        <asp:RadioButtonList ID="degree_info" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="0" Selected="True">名誉博士</asp:ListItem>
        <asp:ListItem Value="1">博士</asp:ListItem>
        <asp:ListItem Value="2">硕士</asp:ListItem>
        <asp:ListItem Value="3">学士</asp:ListItem>
        </asp:RadioButtonList>
        </div>
        </dd>
    </dl>
  </div>

    <div class="tab-content" style="display:none;">
   <dl>
    <dt>入党时间</dt>
    <dd><asp:TextBox ID="joinPartyTime" runat="server" CssClass="input rule-date-input"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" "></asp:TextBox></dd>
  </dl> 
  <dl>
      <dt>入党介绍人</dt>
          <dd>
              <asp:TextBox ID="peoname" runat="server" CssClass="input normal"></asp:TextBox>
          </dd>
  </dl>
     <dl>
    <dt>入党所在支部</dt>
      <dd>
       <asp:TextBox ID="aftergroup" runat="server" CssClass="input normal"></asp:TextBox>
    </dd>
  </dl> 
     <dl>
    <dt>现任党内职务</dt>
      <dd>
       <asp:TextBox ID="nowjob" runat="server" CssClass="input normal"></asp:TextBox>
    </dd>
  </dl> 
       <dl>
    <dt>现所在党组织</dt>
      <dd>
       <asp:TextBox ID="nowgroup" runat="server" CssClass="input normal"></asp:TextBox>
    </dd>
  </dl> 
        <dl>
            <dt>进入现支部类型</dt>
            <dd>
                    <label style="display:inline-block;width:100px;"><asp:RadioButton ID="type1" name="type"  runat="server" Text="新入党" GroupName="group_type" /></label>
                    <label style="display:inline-block;width:100px;"><asp:RadioButton ID="type2" name="type"  runat="server" Text="恢复党籍" GroupName="group_type" /></label>
                    <label style="display:inline-block;width:100px;"><asp:RadioButton ID="type3" name="type"  runat="server" Text="军队转入" GroupName="group_type" /></label>
                    <label style="display:inline-block;width:100px;"><asp:RadioButton ID="type4" name="type"  runat="server" Text="武警转入" GroupName="group_type" /></label>
                    <label style="display:inline-block;width:100px;"><asp:RadioButton ID="type5" name="type"  runat="server" Text="其他系统转入" GroupName="group_type" /></label><br />
                    <label style="display:inline-block;width:100px;"><asp:RadioButton ID="type6" name="type"  runat="server" Text="本省转入" GroupName="group_type" /></label>
                    <label style="display:inline-block;width:100px;"><asp:RadioButton ID="type7" name="type"  runat="server" Text="本市转入" GroupName="group_type" /></label>
                    <label style="display:inline-block;width:100px;"><asp:RadioButton ID="type8" name="type"  runat="server" Text="本县转入" GroupName="group_type" /></label>
                    <label style="display:inline-block;width:100px;"><asp:RadioButton ID="type9" name="type"  runat="server" Text="内部支部间转移" GroupName="group_type" /></label>
            </dd>
        </dl>
         <dl>
        <dt>党员进社区情况</dt>
        <dd>
          <label><asp:RadioButton ID="community1" GroupName="community" runat="server" Text="未参加" name="community_info"/></label><br />
         <label><asp:RadioButton ID="community2" GroupName="community" runat="server" Text="参加" name="community_info" /></label>
<%--		 <asp:TextBox ID="TextBox2" runat="server" CssClass="input normal" style="width: 100px; height:0px; max-width: 100px; max-height:0px;"></asp:TextBox>--%>
        </dd>           
             <dd>
                 <textarea id="txtContent" class="editor" runat="server" style="width: 300px;height:70px;" placeholder="请填写详细信息"></textarea>
             </dd>


    </dl>
          <dl>
        <dt>参加组织生活情况</dt>
        <dd>
          <label><asp:RadioButton ID="live1" GroupName="group_live" runat="server" Text="未曾参加" name="live"/></label><br />
         <label><asp:RadioButton ID="live2" GroupName="group_live" runat="server" Text="参加" name="live" /></label>
       <%--  <asp:TextBox ID="textlive" runat="server" CssClass="input normal" style="width: 100px; height:0px; max-width: 100px; max-height:0px;"></asp:TextBox><br />--%>
        </dd>
                    <dd>
                 <textarea id="txtlive" class="editor" runat="server" style="width: 300px;height:70px;" placeholder="请填写详细信息"></textarea>
             </dd>

    </dl>
    </div>

    <div class="tab-content" style="display:none;">
        <dl>
            <dt>奖惩名称</dt>
                <dd><asp:TextBox ID="title" runat="server" CssClass="input normal"></asp:TextBox></dd>
        </dl>
          <dl>
            <dt>奖惩原因</dt>
                <dd><asp:TextBox ID="txtreason" runat="server" CssClass="input readonly"  TextMode="MultiLine" Style="width:300px; height: 200px; max-width: 300px; max-height: 200px;"></asp:TextBox></dd>
        </dl>
        <dl>
            <dt>奖惩批准机关</dt>
            <dd><asp:TextBox ID="txtauthority" runat="server" CssClass="input normal"></asp:TextBox></dd>
        </dl>
          <dl>
            <dt>批准机关级别</dt>
              <dd><asp:TextBox ID="txtlevel" runat="server" CssClass="input normal"></asp:TextBox></dd>          
        </dl>
        </div>

    <div class="tab-content" style="display:none;">
      <dl>
    <dt>现工作单位名称</dt>
    <dd><asp:TextBox ID="nowCompanyName" runat="server" CssClass="input normal"></asp:TextBox></dd>
  </dl> 
    <dl>
    <dt>现工作单位人数</dt>
    <dd><asp:TextBox ID="nowpeoplecount" runat="server" CssClass="input normal" datatype="/^[0-9]*$/" sucmsg=""></asp:TextBox></dd>
  </dl> 
        <dl>
            <dt>组织关系所在单位</dt>
            <dd>
         <label  style="display:inline-block;width:190px;"><asp:RadioButton ID="com1" GroupName="nowrelation_com" runat="server" Text="现工作单位" name="com"/></label>
         <label  style="display:inline-block;width:190px;"><asp:RadioButton ID="com2" GroupName="nowrelation_com" runat="server" Text="其他" name="com" /></label>
            </dd>
        </dl>
    <dl>
    <dt>现工作单位类型</dt>
    <dd>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1010" GroupName="nowcompanyType" runat="server" Text="政府" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1011" GroupName="nowcompanyType" runat="server" Text="行政机关" /></label>
	     <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1012" GroupName="nowcompanyType" runat="server" Text="事业单位" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1013" GroupName="nowcompanyType" runat="server" Text="内资企业" /></label><br />
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1014" GroupName="nowcompanyType" runat="server" Text="国有企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1015" GroupName="nowcompanyType" runat="server" Text="集体企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1016" GroupName="nowcompanyType" runat="server" Text="股份合作企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1017" GroupName="nowcompanyType" runat="server" Text="联营企业" /></label><br />
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1018" GroupName="nowcompanyType" runat="server" Text="有限责任公司" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1019" GroupName="nowcompanyType" runat="server" Text="股份有限公司" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1020" GroupName="nowcompanyType" runat="server" Text="私营企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1021" GroupName="nowcompanyType" runat="server" Text="港,澳,台商投资企业" /></label> <br />
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1022" GroupName="nowcompanyType" runat="server" Text="合资经营企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1023" GroupName="nowcompanyType" runat="server" Text="合作经营企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1024" GroupName="nowcompanyType" runat="server" Text="港,澳,台商独资经营企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1025" GroupName="nowcompanyType" runat="server" Text="港,澳,台商投资股份有限公司" /></label> <br />
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1026" GroupName="nowcompanyType" runat="server" Text="外商投资企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1027" GroupName="nowcompanyType" runat="server" Text="中外合资经营企业 " /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1028" GroupName="nowcompanyType" runat="server" Text="中外合作经营企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1029" GroupName="nowcompanyType" runat="server" Text="外商投资股份有限公司" /></label><br />
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1030" GroupName="nowcompanyType" runat="server" Text="民办非企业" /></label>
        <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1031" GroupName="nowcompanyType" runat="server" Text="个体工商户" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c1032" GroupName="nowcompanyType" runat="server" Text="其他" /></label>
    </dd> 
  </dl> 

    <dl>
    <dt>现工作岗位类型</dt>
    <dd>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2010" GroupName="nowpostType" runat="server" Text="公务员" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2011" GroupName="nowpostType" runat="server" Text="参照公务员法管理的机关人员" /></label>
	     <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2012" GroupName="nowpostType" runat="server" Text="公有企业管理人员" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2013" GroupName="nowpostType" runat="server" Text="公有企业技术人员" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2014" GroupName="nowpostType" runat="server" Text="公有事业工作人员" /></label>  
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2015" GroupName="nowpostType" runat="server" Text="非公企业管理" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2016" GroupName="nowpostType" runat="server" Text="非公企业技术" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2017" GroupName="nowpostType" runat="server" Text="民办非管理" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2018" GroupName="nowpostType" runat="server" Text="民办非技术" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2019" GroupName="nowpostType" runat="server" Text="公有经济工勤" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2020" GroupName="nowpostType" runat="server" Text="非公经济工勤" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2021" GroupName="nowpostType" runat="server" Text="城镇户口个体" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2022" GroupName="nowpostType" runat="server" Text="乡镇户" /></label> 
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2023" GroupName="nowpostType" runat="server" Text="个体" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2024" GroupName="nowpostType" runat="server" Text="农业生产" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2025" GroupName="nowpostType" runat="server" Text="牧业生产" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2026" GroupName="nowpostType" runat="server" Text="渔业生产" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2027" GroupName="nowpostType" runat="server" Text="林业生产 " /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2028" GroupName="nowpostType" runat="server" Text="军人" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2029" GroupName="nowpostType" runat="server" Text="武警" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2030" GroupName="nowpostType" runat="server" Text="学生" /></label>
        <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2031" GroupName="nowpostType" runat="server" Text="居委会社区干部" /></label>
        <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2032" GroupName="nowpostType" runat="server" Text="自由职业" /></label>
        <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2033" GroupName="nowpostType" runat="server" Text="未就业毕业生" /></label>
        <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2034" GroupName="nowpostType" runat="server" Text="未就业转业军人" /></label> 
        <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2035" GroupName="nowpostType" runat="server" Text="下岗职工" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c2036" GroupName="nowpostType" runat="server" Text="其他" /></label>
    </dd> 
  </dl> 

     <dl>
    <dt>原工作单位名称</dt>
    <dd><asp:TextBox ID="originalComName" runat="server" CssClass="input normal"></asp:TextBox></dd>
  </dl> 
    <dl>
    <dt>原工作单位人数</dt>
    <dd><asp:TextBox ID="originalpeocount" runat="server" CssClass="input normal"  datatype="/^[0-9]*$/" sucmsg=""></asp:TextBox></dd>
  </dl> 
        <dl>
            <dt>组织关系所在单位</dt>
            <dd>
         <label  style="display:inline-block;width:190px;"><asp:RadioButton ID="originalcom1" GroupName="originalrelation_com" runat="server" Text="现工作单位" name="originalcom"/></label>
         <label  style="display:inline-block;width:190px;"><asp:RadioButton ID="originalcom2" GroupName="originalrelation_com" runat="server" Text="其他" name="originalcom" /></label>
            </dd>
        </dl>
    <dl>
    <dt>原工作单位类型</dt>  
    <dd>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3010" GroupName="originalcompanyType" runat="server" Text="政府" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3011" GroupName="originalcompanyType" runat="server" Text="行政机关" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3012" GroupName="originalcompanyType" runat="server" Text="事业单位" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3013" GroupName="originalcompanyType" runat="server" Text="内资企业" /></label><br />
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3014" GroupName="originalcompanyType" runat="server" Text="国有企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3015" GroupName="originalcompanyType" runat="server" Text="集体企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3016" GroupName="originalcompanyType" runat="server" Text="股份合作企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3017" GroupName="originalcompanyType" runat="server" Text="联营企业" /></label><br />
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3018" GroupName="originalcompanyType" runat="server" Text="有限责任公司" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3019" GroupName="originalcompanyType" runat="server" Text="股份有限公司" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3020" GroupName="originalcompanyType" runat="server" Text="私营企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3021" GroupName="originalcompanyType" runat="server" Text="港,澳,台商投资企业" /></label> <br />
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3022" GroupName="originalcompanyType" runat="server" Text="合资经营企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3023" GroupName="originalcompanyType" runat="server" Text="合作经营企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3024" GroupName="originalcompanyType" runat="server" Text="港,澳,台商独资经营企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3025" GroupName="originalcompanyType" runat="server" Text="港,澳,台商投资股份有限公司" /></label> <br />
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3026" GroupName="originalcompanyType" runat="server" Text="外商投资企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3027" GroupName="originalcompanyType" runat="server" Text="中外合资经营企业 " /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3028" GroupName="originalcompanyType" runat="server" Text="中外合作经营企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3029" GroupName="originalcompanyType" runat="server" Text="外商投资股份有限公司" /></label><br />
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3030" GroupName="originalcompanyType" runat="server" Text="民办非企业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3031" GroupName="originalcompanyType" runat="server" Text="个体工商户" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3032" GroupName="originalcompanyType" runat="server" Text="其他" /></label>
    </dd> 
  </dl> 

    <dl>
    <dt>原工作岗位类型</dt>    
    <dd>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3041" GroupName="originalpostType" runat="server" Text="公务员" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3042" GroupName="originalpostType" runat="server" Text="参照公务员法管理的机关人员" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3043" GroupName="originalpostType" runat="server" Text="公有企业管理人员" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3044" GroupName="originalpostType" runat="server" Text="公有企业技术人员" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3045" GroupName="originalpostType" runat="server" Text="公有事业工作人员" /></label>  
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3046" GroupName="originalpostType" runat="server" Text="非公企业管理" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3047" GroupName="originalpostType" runat="server" Text="非公企业技术" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3048" GroupName="originalpostType" runat="server" Text="民办非管理" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3049" GroupName="originalpostType" runat="server" Text="民办非技术" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3050" GroupName="originalpostType" runat="server" Text="公有经济工勤" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3051" GroupName="originalpostType" runat="server" Text="非公经济工勤" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3052" GroupName="originalpostType" runat="server" Text="城镇户口个体" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3053" GroupName="originalpostType" runat="server" Text="乡镇户" /></label> 
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3054" GroupName="originalpostType" runat="server" Text="个体" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3055" GroupName="originalpostType" runat="server" Text="农业生产" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3056" GroupName="originalpostType" runat="server" Text="牧业生产" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3057" GroupName="originalpostType" runat="server" Text="渔业生产" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3058" GroupName="originalpostType" runat="server" Text="林业生产 " /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3059" GroupName="originalpostType" runat="server" Text="军人" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3060" GroupName="originalpostType" runat="server" Text="武警" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3061" GroupName="originalpostType" runat="server" Text="学生" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3062" GroupName="originalpostType" runat="server" Text="居委会社区干部" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3063" GroupName="originalpostType" runat="server" Text="自由职业" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3064" GroupName="originalpostType" runat="server" Text="未就业毕业生" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3065" GroupName="originalpostType" runat="server" Text="未就业转业军人" /></label> 
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3066" GroupName="originalpostType" runat="server" Text="下岗职工" /></label>
         <label style="display:inline-block;width:190px;"><asp:RadioButton ID="c3067" GroupName="originalpostType" runat="server" Text="其他" /></label>
    </dd> 
  </dl> 
</div>
        <div class="tab-content" style="display:none;">
            <dl>
                <dt>目前是否生活困难</dt>
                <dd>
                 <div class="rule-multi-radio">
                 <asp:RadioButtonList ID="badly_of" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                 <asp:ListItem Value="0" Selected="True">是</asp:ListItem>
                 <asp:ListItem Value="1">否</asp:ListItem>
                 </asp:RadioButtonList>
                 </div>
                </dd>
            </dl>

              <dl>
                <dt>是否取得组织认定</dt>
                <dd>
                 <div class="rule-multi-radio">
                 <asp:RadioButtonList ID="is_identity" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                 <asp:ListItem Value="0" Selected="True">是</asp:ListItem>
                 <asp:ListItem Value="1">否</asp:ListItem>
                 </asp:RadioButtonList>
                 </div>
                </dd>
            </dl>

            <dl>
                <dt>经济状况</dt>
               <dd>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="financial1" GroupName="financial_situation" runat="server" Text="低收入" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="financial2" GroupName="financial_situation" runat="server" Text="无社保" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="financial3" GroupName="financial_situation" runat="server" Text="低退休工资" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="financial4" GroupName="financial_situation" runat="server" Text="其他" /></label>
               </dd>
            </dl>

            <dl>
                <dt>身体健康情况</dt>
                <dd>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="healthy1" GroupName="healthy_condition" runat="server" Text="健康" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="healthy2" GroupName="healthy_condition" runat="server" Text="一般" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="healthy3" GroupName="healthy_condition" runat="server" Text="慢性病" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="healthy4" GroupName="healthy_condition" runat="server" Text="重大疾病" /></label><br />
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="healthy5" GroupName="healthy_condition" runat="server" Text="言语残疾" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="healthy6" GroupName="healthy_condition" runat="server" Text="肢体残疾" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="healthy7" GroupName="healthy_condition" runat="server" Text="精神残疾" /></label>
         <label style="display:inline-block;width:130px;"><asp:RadioButton ID="healthy8" GroupName="healthy_condition" runat="server" Text="其他残疾" /></label>
                </dd>
            </dl>
            <dl>
                <dt>生活困难原因</dt>
                <dd>
         <label style="display:inline-block;width:180px;"><asp:RadioButton ID="reason1" GroupName="badly_off_reason" runat="server" Text="本人重病" /></label>
         <label style="display:inline-block;width:180px;"><asp:RadioButton ID="reason2" GroupName="badly_off_reason" runat="server" Text="家庭成员重病" /></label>
         <label style="display:inline-block;width:180px;"><asp:RadioButton ID="reason3" GroupName="badly_off_reason" runat="server" Text="失业" /></label>
         <label style="display:inline-block;width:180px;"><asp:RadioButton ID="reason4" GroupName="badly_off_reason" runat="server" Text="劳动技能不强" /></label><br />
         <label style="display:inline-block;width:180px;"><asp:RadioButton ID="reason5" GroupName="badly_off_reason" runat="server" Text="丧失劳动能力" /></label>
         <label style="display:inline-block;width:180px;"><asp:RadioButton ID="reason6" GroupName="badly_off_reason" runat="server" Text="本人或家庭成员发生重大事故" /></label>
         <label style="display:inline-block;width:180px;"><asp:RadioButton ID="reason7" GroupName="badly_off_reason" runat="server" Text="遭受自然灾害" /></label>
         <label style="display:inline-block;width:180px;"><asp:RadioButton ID="reason8" GroupName="badly_off_reason" runat="server" Text="其他" /></label>
                </dd>
                <dt>具体情况描述</dt>
                <dd>
                    <asp:TextBox ID="info5" runat="server" CssClass="input readonly" TextMode="MultiLine" Style="width:300px; height: 200px; max-width: 300px; max-height: 200px;"></asp:TextBox>
                </dd>
            </dl>

            <dl>
                <dt>享受帮扶情况</dt>
                <dd>
                    <label><asp:RadioButton ID="help1" GroupName="enjoy_help" runat="server" Text="未享受帮扶" /></label>
                    <label style="margin-left:50px"><asp:RadioButton ID="help2" GroupName="enjoy_help" runat="server" Text="享受帮扶，请填写：" /></label>
                </dd>
                <dt>起始时间</dt>
                <dd>
                    <asp:TextBox ID="TextBox3" runat="server" CssClass="input rule-date-input"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" "></asp:TextBox>
                </dd>
                <dt>截止时间</dt>
                <dd>
                    <asp:TextBox ID="TextBox4" runat="server" CssClass="input rule-date-input"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" "></asp:TextBox>
                </dd>
                <dt>帮扶形式</dt>
                <dd>
                    <asp:TextBox ID="TextBox5" runat="server" CssClass="input normal"></asp:TextBox>                   
                </dd>
                <dt>选择以下帮扶形式</dt>
                <dd>
                    <span>1.低保 </span>&nbsp;<span> 2.医保</span>&nbsp;<span>  3.社保</span>&nbsp;<span>  4.节日慰问</span>&nbsp;<span>  5.专项慰问</span>&nbsp;<span>  6.生活困难党员专项帮扶资金</span>&nbsp; <span> 7.捐赠</span>&nbsp;<span>  8.结对帮扶</span>&nbsp;<span>  9.就业培训 </span>&nbsp;<span> 10.其他</span>
                </dd>
                <dt>备注</dt>
                <dd>
                    <asp:TextBox ID="TextBox6" runat="server" CssClass="input normal"></asp:TextBox>
                </dd>
            </dl>
</div>

    <div class="tab-content" style="display:none;">
        <dl>
            <dt>流动类型</dt>
           <dd> <asp:TextBox ID="TextBox7" runat="server" CssClass="input normal"></asp:TextBox></dd>
        </dl>
          <dl>
            <dt>流动党支部联系人</dt>
            <dd><asp:TextBox ID="TextBox8" runat="server" CssClass="input normal"></asp:TextBox></dd>
        </dl>
         <dl>
            <dt>流动原因</dt>
           <dd> <asp:TextBox ID="TextBox9" runat="server" CssClass="input normal"></asp:TextBox></dd>
        </dl>
         <dl>
            <dt>联系方式</dt>
            <dd><asp:TextBox ID="TextBox10" runat="server" CssClass="input normal"></asp:TextBox></dd>
        </dl>
         <dl>
            <dt>《活动证》号码</dt>
            <dd><asp:TextBox ID="TextBox11" runat="server" CssClass="input normal"></asp:TextBox></dd>
        </dl>
         <dl>
            <dt>党支部联系人</dt>
          <dd>  <asp:TextBox ID="TextBox12" runat="server" CssClass="input normal"></asp:TextBox></dd>
        </dl>
         <dl>
            <dt>流出(入)地(单位)</dt>
            <dd><asp:TextBox ID="TextBox13" runat="server" CssClass="input normal"></asp:TextBox></dd>
        </dl>
         <dl>
            <dt>支部联系人或联系方式</dt>
            <dd><asp:TextBox ID="TextBox14" runat="server" CssClass="input normal"></asp:TextBox></dd>
        </dl>
    </div>
<!--/内容-->

<!--工具栏-->
<div class="page-footer">
  <div class="btn-wrap">
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" onclick="btnSubmit_Click" />
    <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript:history.back(-1);" />
  </div>
</div>
<!--/工具栏-->

</form>
</body>
</html>
