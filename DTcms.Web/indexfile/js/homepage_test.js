function test(abc) {
    alert("----------------------------------" + abc);
    var left1 = echarts.init(document.getElementById('left1_canvas'));
    option1 = {
        color: ['#9b1924', '#f39800', '#1f83cc', '#23b576', '#3c6bdf', '#19428a'],
        textStyle: {
            fontFamily: '锐字逼格青春体简2.0'
        },
        series: [
            {
                type: 'pie',
                radius: [20, 70],
                center: ['50%', '60%'],
                roseType: 'area',
                /*            itemStyle: {     //itemStyle有正常显示：normal，有鼠标hover的高亮显示：emphasis
                                normal :{//normal显示阴影,与shadow有关的都是阴影的设置
                                    shadowBlur:1000,//阴影大小
                                    shadowOffsetX:0,//阴影水平方向上的偏移
                                    shadowOffsetY:0//阴影垂直方向上的偏移
                                    
                                }, 
                                emphasis:{//normal显示阴影,与shadow有关的都是阴影的设置
                                    shadowBlur:50,//阴影大小
                                    shadowOffsetX:0,//阴影水平方向上的偏移
                                    shadowColor:'rgba(0,0,0,0.5)'//阴影颜色
                                }
                            },*/
                data: (function () {
                    var arr = [];
                    $.ajax({
                        type: "GET",
                        async: false, //同步执行
                        url: "http://localhost:54066/homepagedata/get/getpartymembercount/list",
                        data: { "groupid": "3102" },
                        dataType: "json", //返回数据形式为json
                        success: function (result) {
                            alert(result.data);
                            //if (result[0]) {
                            //    for (var i = 0; i < result[0].length; i++) {
                            //        arr.push(result[0][i]);
                            //    }
                            //    //console.log("arr=>"+arr);
                            //}
                        },
                        error: function (errorMsg) {
                            alert("获取图表请求数据失败!111");
                            myChart.hideLoading();
                        }
                    });
                    return arr;
                })()

            }
        ]
    };
    left1.setOption(option1);
}