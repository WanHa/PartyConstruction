$(function () {
    //$.ajax({
    //    type: "get",
    //    //async: false,
    //    url: "../indexfile/api/canvas_title.json",
    //    data: {},
    //    dataType: "json",
    //    success: function (res) {
    //        console.log(res);
    //        $('.left1_canvas_title').html(res.title1);
    //        $('.left2_canvas_title').html(res.title2);
    //        $('.left3_canvas_title').html(res.title3);
    //        $('.center2_canvas_title').html(res.title4);
    //        $('.center3_canvas_title').html(res.title5);
    //        $('.right1_canvas_title').html(res.title6);
    //        $('.right2_canvas_title').html(res.title7);
    //        $('.right3_canvas_title').html(res.title8)
    //    },
    //    error: function (err) {
    //        alert("err");
    //    }

    //})
    //$('.left1_canvas_title').html('党员年龄分布情况');
    //$('.left2_canvas_title').html('组织学习时长');
    //$('.left3_canvas_title').html('主要经济来源');
    //$('.center2_canvas_title').html('党员男女比例情况');
    //$('.center3_canvas_title').html('正式党员与预备党员比例');
    //$('.right1_canvas_title').html('党组织会议开展情况');
    //$('.right2_canvas_title').html('党员诉求情况表');
    //$('.right3_canvas_title').html('党费缴纳情况');
})

function InitMapTitle() {
    $.ajax({
        type: "GET",
        url: webApiDomain + "homepagedata/get/map/title",
        dataType: "JSON",
        success: function (result) {
            $('.party_num_info .num1 p').html(result.data.party_member_count);
            $('.party_num_info .num2 p').html(result.data.party_organization_count);
            $('.party_num_info .num3 p').html(result.data.service_organization_count);
            //$('.party_num_info .num1 p').html(420);
            //$('.party_num_info .num2 p').html(20);
            //$('.party_num_info .num3 p').html(20);
        },
        error: function (er) {

        }
    });
}