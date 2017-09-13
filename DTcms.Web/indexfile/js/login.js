    // ===========================   弹出登录注册  ===================
$(function () {//预加载处理
    $('.login').on('click', function () {
        $('.container').css({
            'pointer-events':'none',
            "opacity":'0.4',
            " filter": "url(blur.svg#blur)",
            "-webkit-filter": "blur(10px)",
            "-ms-filter": "blur(10px)",
            "filter": "blur(10px)",
            "filter": "progid:DXImageTransform.Microsoft.Blur(PixelRadius=10, MakeShadow=false)"
        });

        $('.container2').css({
            display: 'block'
        });
        $('body').css({
            "background":"url(../indexfile/imgs/index_imgs/模糊底图.png) repeat center #0b0b23"
        })
    });
    //$('.container2 button').on('click', function () {
    //    console.log('跳转到另一页');
    //    location.href = '../indexfile/html/function.html';
    //    $(".container").css({
    //        'pointer-events':'auto'
    //    });

    //});
    $('.container2 .close').on('click', function () {
        $('.container2').css({
            display: 'none'
        });
        $('.container').css({
            "opacity": '1',
            'pointer-events':'auto',
            " filter": "url(blur.svg#blur)",
            "-webkit-filter": "blur(0)",
            "-ms-filter": "blur(0)",
            "filter": "blur(0)",
        });
        $('body').css({
            "background":"url(../indexfile/imgs/index_imgs/bg1.png) repeat center #0b0b23"
        })
    })})
