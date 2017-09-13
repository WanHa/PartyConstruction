function InitGroup() {
    $.ajax({
        type: "GET",
        url: webApiDomain + "homepagedata/get/org/location",
        dataType: "JSON",
        success: function (result) {
            //var arr = [];
            //for (var i = 0; i < 20; i++) {
            //    var option = i % 2;
            //    arr.push(result.data[option]);
            //}

            var html = template('map-tp1', {
                data: result.data
            });
            $('.flag').html("");
            $('.flag').append(html);
        },
        error: function (er) {
            parent.jsprint("获取数据出现异常.", "");
        }
    });
}

// ------------地图用户信息-----------
function ClickUser(userId) {
    $.ajax({
        type: "GET",
        url: webApiDomain + "homepagedata/get/map/user-info",
        data: { "userid": userId },
        dataType: "JSON",
        success: function (result) {
            //var index = $(".center .map_info .flagInfo_partyName span p").index(this);
            //console.log(index);
            //console.log(partyName_arr[index]);
            var data = result.data;
            $('.container4 .active_content .personal_details li:nth-child(1) span>span').html(data.user_name);
            $('.container4 .active_content .personal_details li:nth-child(2) span>span').html(data.group_name);
            $('.container4 .active_content .personal_details li:nth-child(3) span>span').html(data.secretary);
            $('.container4 .active_content .personal_details li:nth-child(4) span>span').html(data.superior_organization);

            var html = template('ex_active-tpl', {
                data: data.activitys
            });
            $('.container4 .active_list').html("");
            $('.container4 .active_list').append(html);
            $('.container4').css({
                "display": "block"
            });
            $('.container').css({
                'pointer-events': 'none',
                "opacity": '0.4',
                " filter": "url(blur.svg#blur)",
                "-webkit-filter": "blur(10px)",
                "-ms-filter": "blur(10px)",
                "filter": "blur(10px)",
                "filter": "progid:DXImageTransform.Microsoft.Blur(PixelRadius=10, MakeShadow=false)"
            });
            $('body').css({
                "background": "url(../indexfile/imgs/index_imgs/模糊底图.png) repeat center #0b0b23"
            });

            //  =====================滚动条==================
            $("#scrollbar3").tinyscrollbar({
                'trackSize': '297'
            });

            $('.container4 .active_list .list img').click(function () {
                var that = this;
                var index2 = $(".container4 .active_list .list img").index(that);
                console.log("index2:" + index2);
                var activitys = data.activitys;
                //==========   活动详情  active_details    ==========
                $('.active_details .active_content .personal_details li:nth-child(1)>span>span').html(activitys[index2].activity_name);
                $('.active_details .active_content .personal_details li:nth-child(2)>span>span').html(activitys[index2].activity_address);
                $('.active_details .active_content .personal_details li:nth-child(3)>span>span').html(activitys[index2].start_time + "-" + activitys[index2].end_time);
                $('.active_details .active_content .personal_details li:nth-child(4)>span>span').html(activitys[index2].organizer);
                $('.active_details .active_content .personal_details li:nth-child(5)>span>span').html(activitys[index2].personnel_count + "人");
                $('.active_details .active_content .active_description p').html(activitys[index2].activity_detail);
                $("#img_a").attr("src", activitys[index2].cover_pic);
                $('body').css({
                    "background": "url(../indexfile/imgs/index_imgs/模糊底图.png) repeat center #0b0b23"
                });

                $('.container4').css({
                    "display": "none"
                })
                $('.active_details').css({
                    "display": "block"
                });
                $('.active_details .active_content .go_back').click(function () {
                    $('.active_details').css({
                        "display": "none"
                    });
                    $('.container3').css({
                        "display": "none"
                    });
                    $('.container4').css({
                        "display": "block"
                    });
                });

                //  =====================滚动条==================
                $("#scrollbar33").tinyscrollbar({
                    'trackSize': '297'
                });

            })

        },
        error: function (er) {
            parent.jsprint("获取数据出现异常.", "");
        }
    });
}

// -------------点击组织---------
function ClickFlag(id) {
    getcount(id);
    getmeetingcount(id);
    getpartypayinfo(id);
    getappealinfo(id);
    GetSex(id);
    GetAge(id);
    GetLearntime(id);
    GetEconomic(id);
    LoadingOrganizationInfo(id);
}

// -----------加载地图组织信息-----------
function LoadingOrganizationInfo(id) {
    //  后添加===================   
    $(".common_flag").css({
        'background': 'url("../indexfile/imgs/index_imgs/flag.png") no-repeat center',
    });

    $("#flag" + id).css({
        'width': '26px',
        'height': '28px',
        'background': 'url("../indexfile/imgs/index_imgs/Shape Copy 11.png") no-repeat center'
    });
   

    $.ajax({
        type: "GET",
        url: webApiDomain + "homepagedata/get/map/group-info",
        data: { "groupid": id },
        dataType: "JSON",
        success: function (res) {
            $('.center .map_info .flagInfo_partyName').css({
                display: 'block',
                "z-index": 100
            });

            var data = res.data;
            $('.center .map_info .flag_info .party_name').html(data.group_name);
            $('.center .map_info .flag_info .party_addr').html('地址：' + data.contact_address);
            $('.center .map_info .flag_info .party_clerk').html('书记：' + data.secretary);
            $('.center .map_info .flag_info .party_amount').html('党员：' + data.personnel_count + '名')
            $('.center .map_info .flag_info p').css({
                //"height": '18px',
                //"line-height": '18px',
                "margin-left": '32px'
            });
            var sumHeight = $('p.party_name').height() + $('p.party_addr').height() +
                $('p.party_clerk').height() + $('div.party_amount_box').height() + $('div.active_examples').height();
            var p_h = $("div.flag_info").height();
            $('.center .map_info .flag_info p:eq(0)').css({
                "padding-top": (p_h - sumHeight) / 2
            });
            $('.center .map_info .flag_info p:eq(1)').css({
                "width": '179px',
                "overflow": 'hidden'
            });
            $('.center .map_info .building1').css({
                "display": 'block',
                "top": '62px',
                "left": '193px'
            });
            $('.center .map_info .building2').show();
            $('.center .map_info .building2').css({
                "display": 'block',
                "top": '154px',
                "left": '110px'
            });
            //===================       党员名字       =======================
            var partyName_arr = data.group_users;
            console.log("党员信息：" + partyName_arr);
            var html = template('partyName-tpl', {
                data: partyName_arr
            });
            $('.center .map_info .flagInfo_partyName span').html("");
            $('.center .map_info .flagInfo_partyName span').append(html);
            $("#scrollbar_partyName").tinyscrollbar({
                'trackSize': '100'
            });
            $('.center .map_info .active_examples').click(function () {
                $('.container3').css({
                    "display": "block"
                });
                $('.container').css({
                    'pointer-events': 'none',
                    "opacity": '0.5',
                    " filter": "url(blur.svg#blur)",
                    "-webkit-filter": "blur(10px)",
                    "-ms-filter": "blur(10px)",
                    "filter": "blur(10px)",
                    "filter": "progid:DXImageTransform.Microsoft.Blur(PixelRadius=10, MakeShadow=false)"
                });
                $('body').css({
                    "background": "url(../indexfile/imgs/index_imgs/bg_activity.png) no-repeat center #0b0b23",

                });

                $.ajax({
                    type: "GET",
                    url: webApiDomain + "homepagedata/get/map/group-activity",
                    data: { "groupid": data.group_id },
                    dataType: "JSON",
                    success: function (result) {
                        var activity_item = result.data;
                        
                        var html = template('active-tpl', {
                            data: activity_item
                        });
                        $('.container3 .active_list').html("");

                        $('.container3 .active_list').append(html);

                        $('.container3 .active_list .list img').click(function () {
                            var that = this;
                            var index2 = $(".container3 .active_list .list img").index(that);
                            console.log("index2:" + index2);
                            //==========   活动详情  active_details    ==========
                            $('.active_details .active_content .personal_details li:nth-child(1)>span>span').html(activity_item[index2].activity_name);
                            $('.active_details .active_content .personal_details li:nth-child(2)>span>span').html(activity_item[index2].activity_address);
                            $('.active_details .active_content .personal_details li:nth-child(3)>span>span').html(activity_item[index2].start_time + "-" + activity_item[index2].end_time);
                            $('.active_details .active_content .personal_details li:nth-child(4)>span>span').html(activity_item[index2].organizer);
                            $('.active_details .active_content .personal_details li:nth-child(5)>span>span').html(activity_item[index2].personnel_count + "人");
                            $('.active_details .active_content .active_description p').html(activity_item[index2].activity_detail);
                            $("#img_a").attr("src", activity_item[index2].cover_pic);
                            $('body').css({
                                "background": "url(../indexfile/imgs/index_imgs/模糊底图.png) repeat center #0b0b23"
                            });
                            $('.container3').css({
                                "display": "none"
                            })
                            $('.active_details').css({
                                "display": "block"
                            });
                            $('.active_details .active_content .go_back').click(function () {
                                $('.active_details').css({
                                    "display": "none"
                                });
                                $('.container4').css({
                                    "display": "none"
                                });
                                $('.container3').css({
                                    "display": "block"
                                });
                                $('body').css({
                                    "background": "url(../indexfile/imgs/index_imgs/bg_activity.png) no-repeat center #0b0b23",
                                });
                            });
                            //  =====================滚动条==================
                            $("#scrollbar33").tinyscrollbar({
                                'trackSize': '297'
                            });
                        });
                        $("#scrollbar1").tinyscrollbar({
                            'trackSize': '100'
                        });
                    },
                    error: function (er) {

                    }
                });
                
            });
        },
        error: function (err) {
            parent.jsprint("获取数据出现异常.", "");
        }
    });
}

$('.container3 .active_content .active_close>div').click(function () {
    $('.container3').css({
        "display": "none"
    });
    $('.container').css({
        'pointer-events': 'auto',
        'opacity': '1',
        " filter": "url(blur.svg#blur)",
        "-webkit-filter": "blur(0)",
        "-ms-filter": "blur(0)",
        "filter": "blur(0)",
        "filter": "progid:DXImageTransform.Microsoft.Blur(PixelRadius=10, MakeShadow=false)"
    });
    $('body').css({
        "background": "url(../indexfile/imgs/index_imgs/bg1.png) repeat #0b0b23",
    });
});

