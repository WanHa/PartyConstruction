
        //=====================  活动风采   ===================
//$('.center .map_info .active_examples').click(function () {
//    $('.container3').css({
//        "display": "block"
//    });
//    $('.container').css({
//        'pointer-events':'none',
//        "opacity":'0.5',
//        " filter": "url(blur.svg#blur)",
//        "-webkit-filter": "blur(10px)",
//        "-ms-filter": "blur(10px)",
//        "filter": "blur(10px)",
//        "filter": "progid:DXImageTransform.Microsoft.Blur(PixelRadius=10, MakeShadow=false)"
//    });
//    $('body').css({
//        "background": "url(../indexfile/imgs/index_imgs/bg_activity.png) no-repeat center #0b0b23",

//    });
//});
//$.get('../indexfile/api/active.json', function (data) {
//    console.log(data);
//    var html = template('active-tpl', {
//        data: data
//    });
//    $('.container3 .active_list').html("");

//    $('.container3 .active_list').append(html);

//    $('.container3 .active_list .list img').click(function () {
//        var that = this;
//        var index2 = $(".container3 .active_list .list img").index(that);
//        console.log("index2:" + index2);
//        //==========   活动详情  active_details    ==========
//        $('.active_details .active_content .personal_details li:nth-child(1)>span>span').html(data[index2].title);
//        $('.active_details .active_content .personal_details li:nth-child(2)>span>span').html(data[index2].where);
//        $('.active_details .active_content .personal_details li:nth-child(3)>span>span').html(data[index2].time);
//        $('.active_details .active_content .personal_details li:nth-child(4)>span>span').html(data[index2].host);
//        $('.active_details .active_content .personal_details li:nth-child(5)>span>span').html(data[index2].participate_amount + "人");
//        $('.active_details .active_content .active_description p').html(data[index2].active_description);
//        $("#img_a").attr("src", data[index2].img);
//        $('body').css({
//            "background": "url(../indexfile/imgs/index_imgs/模糊底图.png) repeat center #0b0b23"
//        });
//        $('.container3').css({
//            "display": "none"
//        })
//        $('.active_details').css({
//            "display": "block"
//        });
//        $('.active_details .active_content .go_back').click(function () {
//            $('.active_details').css({
//                "display": "none"
//            });
//            $('.container4').css({
//                "display": "none"
//            });
//            $('.container3').css({
//                "display": "block"
//            });
//        });
//        //  =====================滚动条==================
//        $("#scrollbar33").tinyscrollbar({
//            'trackSize': '297'
//        });
//    });

//});
//$('.container3 .active_content .active_close>div').click(function () {
//    $('.container3').css({
//        "display": "none"
//    });
//    $('.container').css({
//        'pointer-events':'auto',
//        'opacity':'1',
//        " filter": "url(blur.svg#blur)",
//        "-webkit-filter": "blur(0)",
//        "-ms-filter": "blur(0)",
//        "filter": "blur(0)",
//        "filter": "progid:DXImageTransform.Microsoft.Blur(PixelRadius=10, MakeShadow=false)"
//    });
//    $('body').css({
//        "background":"url(../indexfile/imgs/index_imgs/bg1.png) repeat #0b0b23",
//    });
//});

//function ClickGroupActivity(groupId) {
//    alert(groupId);
//    $('.container3').css({
//        "display": "block"
//    });
//    $('.container').css({
//        'pointer-events':'none',
//        "opacity":'0.5',
//        " filter": "url(blur.svg#blur)",
//        "-webkit-filter": "blur(10px)",
//        "-ms-filter": "blur(10px)",
//        "filter": "blur(10px)",
//        "filter": "progid:DXImageTransform.Microsoft.Blur(PixelRadius=10, MakeShadow=false)"
//    });
//    $('body').css({
//        "background": "url(../indexfile/imgs/index_imgs/bg_activity.png) no-repeat center #0b0b23",

//    });

//    $.ajax({
//        type: "GET",
//        url: webApiDomain + "homepagedata/get/map/group-activity",
//        data: { "groupid": groupId},
//        dataType: "JSON",
//        success: function (result) {

//            var html = template('active-tpl', {
//                data: result.data
//            });
//            $('.container3 .active_list').html("");

//            $('.container3 .active_list').append(html);

//            $('.container3 .active_list .list img').click(function () {
//                var that = this;
//                var index2 = $(".container3 .active_list .list img").index(that);
//                console.log("index2:" + index2);
//                //==========   活动详情  active_details    ==========
//                $('.active_details .active_content .personal_details li:nth-child(1)>span>span').html(data[index2].activity_name);
//                $('.active_details .active_content .personal_details li:nth-child(2)>span>span').html(data[index2].activity_address);
//                $('.active_details .active_content .personal_details li:nth-child(3)>span>span').html(data[index2].start_time + "-" + data[index2].end_time);
//                $('.active_details .active_content .personal_details li:nth-child(4)>span>span').html(data[index2].organizer);
//                $('.active_details .active_content .personal_details li:nth-child(5)>span>span').html(data[index2].personnel_count + "人");
//                $('.active_details .active_content .active_description p').html(data[index2].activity_detail);
//                $("#img_a").attr("src", data[index2].cover_pic);
//                $('body').css({
//                    "background": "url(../indexfile/imgs/index_imgs/模糊底图.png) repeat center #0b0b23"
//                });
//                $('.container3').css({
//                    "display": "none"
//                })
//                $('.active_details').css({
//                    "display": "block"
//                });
//                $('.active_details .active_content .go_back').click(function () {
//                    $('.active_details').css({
//                        "display": "none"
//                    });
//                    $('.container4').css({
//                        "display": "none"
//                    });
//                    $('.container3').css({
//                        "display": "block"
//                    });
//                });
//                //  =====================滚动条==================
//                $("#scrollbar33").tinyscrollbar({
//                    'trackSize': '297'
//                });
//            });
//        },
//        error: function (er) {

//        }
//    });
//}

