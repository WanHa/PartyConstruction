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
    //$('.left1_canvas_title').html('��Ա����ֲ����');
    //$('.left2_canvas_title').html('��֯ѧϰʱ��');
    //$('.left3_canvas_title').html('��Ҫ������Դ');
    //$('.center2_canvas_title').html('��Ա��Ů�������');
    //$('.center3_canvas_title').html('��ʽ��Ա��Ԥ����Ա����');
    //$('.right1_canvas_title').html('����֯���鿪չ���');
    //$('.right2_canvas_title').html('��Ա���������');
    //$('.right3_canvas_title').html('���ѽ������');
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