/**
 * Created by lixue on 2017/6/2 0002.
 */
$(function () {//预加载处理
  
//============================ container4  返回 个人详情 ====================

    $('.container4 .active_content .go_back p').click(function () {
        $('.container3').css({
            "display": "none"
        });
        $('.container4').css({
            "display": "none"
        });
        $(".container").css({
            'pointer-events':'auto'
        });//   做点击穿透问题处理;
        $('.container').css({
            "opacity": '1',
            " filter": "url(blur.svg#blur)",
            "-webkit-filter": "blur(0)",
            "-ms-filter": "blur(0)",
            "filter": "blur(0)",
            "filter": "progid:DXImageTransform.Microsoft.Blur(PixelRadius=10, MakeShadow=false)"
        });
        $('body').css({
            "background":"url(../indexfile/imgs/index_imgs/bg1.png) repeat #0b0b23"
        });

    });
    //     党员名字弹出&隐藏
    $('.flagInfo_partyName .party_info i.one1').unbind('click').click(function(){
        $('.center .flagInfo_partyName #scrollbar_partyName').css({
           "display":"none"
       });
       $('.flagInfo_partyName .party_info i.one1').css({
            "display":"none"
       });
        $('.flagInfo_partyName .party_info i.one2').css({
            "display":"block"
       }); 
    })

    $('.flagInfo_partyName .party_info i.one2').unbind('click').click(function(){
        $(' .center .flagInfo_partyName #scrollbar_partyName').css({
           "display":"block"
       });
       $('.flagInfo_partyName .party_info i.one2').css({
            "display":"none"
       });
        $('.flagInfo_partyName .party_info i.one1').css({
            "display":"block"
       });
    })

})
