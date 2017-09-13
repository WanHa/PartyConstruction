/**
 * Created by lixue on 2017/6/1 0001.
 */
//============================  left1  option1==============
// 基于准备好的dom，初始化echarts实例
var left1 = echarts.init(document.getElementById('left1_canvas'));
option1 = {
    color: ['#9b1924', '#f39800', '#1f83cc', '#23b576', '#3c6bdf', '#19428a'],
    textStyle: {
            fontFamily:'锐字逼格青春体简2.0'
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
                    url: "../indexfile/api/canvas.json",
                    data: {},
                    dataType: "json", //返回数据形式为json
                    success: function (result) {
                        if (result[0]) {
                            for (var i = 0; i < result[0].length; i++) {
                                arr.push(result[0][i]);
                            }
                            //console.log("arr=>"+arr);
                        }
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
//============================  left2   option2==============
var left2 = echarts.init(document.getElementById('left2_canvas'));
option2 = {
    //color: ['#9b1924','#f39800','#2495e8','#23b576'],
    textStyle: {
        fontFamily:'锐字逼格青春体简2.0',
        color: "#8F9EA6"
    },
    color: ['#3398DB'],
    //   坐标轴指示器
    tooltip: {
        trigger: 'axis',
        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
        }
    },
    grid: {
        show:false,
        left: '3%',
        bottom: '8%',
        containLabel: true
    },

    xAxis: [
        {
            type: 'category',
            color:'#8F9EA6',
            data: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12','  /月'],
            axisTick: {
                alignWithLabel: true,
                interval: 0//interval 是指间隔多少个类别画栅格，为 0 时则每个数据都画，为 1 时间隔 1 个画，以此类推。
            },
            //  坐标轴轴线相关设置
             axisLine:{
                 show: false,
                 lineStyle:{
                      type:'dashed',
                      color:'#dedede',
                     // width: 0.5
                 }
             },
            axisLabel: {
                //rotate: 12, //刻度旋转45度角
                show:true,           
                interval: 0,
                textStyle: {
                    color: "#8F9EA6",
                    fontSize: 12
                }
            },
            // 坐标轴刻度相关设置
            axisTick:{
                lineStyle:{
                    opacity:'0',
                }
            }
        }
    ],
    yAxis: {
        type: 'value',
        data: ['0', '100', '200', '300', '400', '500'],
        //axisTick: {length:2},//设置x轴之间的距离
        axisLabel: {
            //rotate: 12, //刻度旋转45度角
                show:true,           
            interval: 0,
            textStyle: {
                color: "#8F9EA6",
                fontSize: 12,
                fontFamily:'锐字逼格青春体简2.0'
            }
        },
        // 纵坐标透明度设为0，则不显示
        axisLine:{
            lineStyle:{
                opacity:'0'
            }
        },
        // 坐标轴刻度相关设置
        axisTick:{
            lineStyle:{
                opacity:'0',
            }
        },
        // y轴上的横向分割线
        splitLine:{ 
            show:true,
            lineStyle:{
                type:'dashed',
                color:'#8F9EA6',
                width: 0.5
            }
        }
    },
    series: [
        {
            name: '直接访问',
            type: 'bar',
            //设置柱的宽度，要是数据太少，柱子太宽不美观~
            barWidth: '50%',
            itemStyle: {
                normal: {
                    color: function (params) {
                        var colorList = [
                            '#f39800', '#9b1924', '#23b576', '#f39800', '#9b1924', '#23b576', '#f39800', '#9b1924', '#23b576', '#f39800', '#9b1924', '#23b576'
                        ];
                        return colorList[params.dataIndex]
                    }
                }
            },
            data: (function () {
                var arr = [];
                $.ajax({
                    type: "get",
                    async: false, //同步执行
                    url: "../indexfile/api/canvas.json",
                    data: {},
                    dataType: "json", //返回数据形式为json
                    success: function (result) {
                        //console.log(result[5]+'result[5]')
                        if (result[5]) {
                            for (var i = 0; i < result[5].length; i++) {
                                arr.push(result[5][i]);
                            }
                            //console.log("arr=>"+arr);
                        }
                    },
                    error: function (errorMsg) {
                        alert("获取图表请求数据失败!");
                        myChart.hideLoading();
                    }
                });
                return arr;
            })()
        }
    ]
};
//============================  left3  option3==============
var left3 = echarts.init(document.getElementById('left3_canvas'),'chalk');
option3 = {
        textStyle: {
            color: "#8F9EA6",
            fontSize: '12',
            fontFamily:'锐字逼格青春体简2.0'
        },
    radar: [
        {
            indicator: [
                {text: '养老金', max: 100},
                {text: '经商', max: 100},
                {text: '务工', max: 100},
                {text: '低保', max: 100},
                {text: '失业救济金', max: 100},
                {text: '投资', max: 100},
                {text: '其他', max: 100},
                {text: '退休金', max: 100},
                {text: '工资', max: 100}
            ],
            center: ['50%', '60%'],
            radius: 60,
            // y轴上的横向分割线
            splitLine:{ 
                show:true,
                lineStyle:{
                    type:'dashed',
                    color:'#eaeaea',
                    width:0.5
                }
            },
            nameGap:5 , //指示器名称和指示器轴的距离。
                //  坐标轴在 grid 区域中的分隔区域
            splitArea:{
                areaStyle:{
                    opacity:0
                }
            }

        }
    ],
    series: [
        {
            type: 'radar',
/*            tooltip: {
                trigger: 'item'
            },
*/            itemStyle: {normal: {areaStyle: {type: 'default'}}},
            data: [
                {
                    value: [60, 73, 85, 60, 73, 85, 60, 73, 40]
                }
            ]
        }
    ]
};
//===========================   center2  option4  ================
var center2 = echarts.init(document.getElementById('center2_canvas'));
//男女扇形的颜色
var colorList = [
    '#d32f3d', '#2495e8'
];
var originalData = [{
    value: 15,
    name: '女'
}, {
    value: 70,
    name: '男'
}];
echarts.util.each(originalData, function (item, index) {
    item.itemStyle = {
        normal: {
            color: colorList[index]
        }
    };
});
option4 = {
/*    textStyle: {
        fontFamily:'锐字逼格青春体简2.0'
    },*/
        axisLabel: {
            show:true,           
            textStyle: {
                fontSize: '12',
                fontFamily:'锐字逼格青春体简2.0'
            }
        },



    series: [{
        hoverAnimation: false, //设置饼图默认的展开样式
        center: ['50%', '60%'],
        radius: [15, 50],
        name: 'pie',
        type: 'pie',
        selectedMode: 'single',
        //selectedOffset: 18, //选中是扇区偏移量
        clockwise: true,
        startAngle: 90,
        // label: {
        //    normal: {
        //        textStyle: {
        //            //fontSize: 14,
        //            // color: '#fff'
        //        }
        //    }
        // },
        // markPoint: {
        //     symbol: 'circle',
        //     data: [{
        //         name: 'MAN'
        //     }]
        // },
        labelLine: {
            normal: {
                lineStyle: {
                    //color: '#999',

                }
            }
        },
        data: originalData
    }]
};
//=======================   center3  option5   ====================
var center3 = echarts.init(document.getElementById('center3_canvas'));
//男女扇形的颜色
var colorList2 = [
    '#f39800', '#9b1924'
];
// 总和
//var total = {
//    name: '总单数',
//    value: '145'
//}
var originalData2 = [{
    value: 15,
    name: '预备党员'
}, {
    value: 100,
    name: '正式党员'
}];
echarts.util.each(originalData2, function (item, index) {
    item.itemStyle = {
        normal: {
            color: colorList2[index]
        }
    };
});
option5 = {
    textStyle: {
        fontFamily:'锐字逼格青春体简2.0'
    },
    series: [{
        hoverAnimation: false, //设置饼图默认的展开样式
        center: ['50%', '60%'],
        radius: [15, 50],
        name: 'pie',
        type: 'pie',
        selectedMode: 'single',
        //selectedOffset: 16, //选中是扇区偏移量
        clockwise: true,
        startAngle: 90,
        data: originalData2
    }]
};
//===============================  right1 option6  ============
var right1 = echarts.init(document.getElementById('right1_canvas'));
option6 = {
    //color: ['#9b1924','#f39800','#2495e8','#23b576'],
    color: ['#3398DB'],
    tooltip: {
        trigger: 'axis',
        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
        }
    },
    //   坐标轴线的文字  可设置图表的大小
    grid: {
        show:false,
        left: '3%',
        bottom: '8%',
        containLabel: true
    },
    xAxis: [
        {
            type: 'category',
            color:'#8F9EA6',
            data: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12','  /月'],
            axisTick: {
                alignWithLabel: true,
                interval: 0//interval 是指间隔多少个类别画栅格，为 0 时则每个数据都画，为 1 时间隔 1 个画，以此类推。
            },
            //  坐标轴轴线相关设置
             axisLine:{
                 show: false,
                 lineStyle:{
                      type:'dashed',
                      color:'#dedede',
                     // width: 0.5
                 }
             },
            axisLabel: {
                //rotate: 12, //刻度旋转45度角
                interval: 0,
                textStyle: {
                    color: "#8F9EA6",
                    fontSize: 12,
                    fontFamily:'锐字逼格青春体简2.0'
                }
            },
            // 坐标轴刻度相关设置   把默认的灰线去掉
            axisTick:{
                lineStyle:{
                    opacity:'0',
                }
            }
        }
    ],
    yAxis: {
        type: 'value',
        data: ['0', '100', '200', '300', '400', '500'],
        //axisTick: {length:2},//设置x轴之间的距离
        axisLabel: {
            //rotate: 12, //刻度旋转45度角
            interval: 0,
            textStyle: {
                color: "#8F9EA6",
                fontSize: 12,
                fontFamily:'锐字逼格青春体简2.0'
            }
        },
        // 纵坐标透明度设为0，则不显示
        axisLine:{
            lineStyle:{
                opacity:'0'
            }
        },
        // 坐标轴刻度相关设置
        axisTick:{
            lineStyle:{
                opacity:'0',
            }
        },
        // y轴上的横向分割线
        splitLine:{ 
            show:true,
            lineStyle:{
                type:'dashed',
                color:'#dedede',
                width: 0.5
            }
        }
    },


    series: [
        {
            name: '直接访问',
            type: 'bar',
            //设置柱的宽度，要是数据太少，柱子太宽不美观~
            barWidth: '50%',
            //data: [10, 52, 200, 334],
            itemStyle: {
                normal: {
                    color: function (params) {
                        var colorList = [
                            '#f39800', '#9b1924', '#23b576', '#f39800', '#9b1924', '#23b576', '#f39800', '#9b1924', '#23b576', '#f39800', '#9b1924', '#23b576'
                        ];
                        return colorList[params.dataIndex]
                    },
                    //以下为是否显示，显示位置和显示格式的设置了
                    label: {
                        //show: true,
                        position: 'top',
//                             formatter: '{c}'
                        formatter: '{b}\n{c}'
                    }
                }
            },
            data: (function () {
                var arr = [];
                $.ajax({
                    type: "get",
                    async: false, //同步执行
                    url: "../indexfile/api/canvas.json",
                    data: {},
                    dataType: "json", //返回数据形式为json
                    success: function (result) {
                        //console.log(result[1]+'result[1]')
                        if (result[1]) {
                            for (var i = 0; i < result[1].length; i++) {
                                arr.push(result[1][i]);
                            }
                            //console.log("arr=>"+arr);
                        }

                    },
                    error: function (errorMsg) {
                        alert("获取图表请求数据失败!");
                        myChart.hideLoading();
                    }
                });
                return arr;
            })()
        }
    ]
};

// 指定图表的配置项和数据
//option6 = {
//    color: ['#9b1924', '#f39800', '#1f83cc', '#23b576', '#3c6bdf', '#19428a'],
//
//    tooltip: {
//        trigger: 'item',
//        formatter: "{a} <br/>{b} : {c} ({d}%)"
//    },
//    legend: {
//        x: 'center',
//        y: 'bottom'
//        //data:['rose1','rose2','rose3','rose4','rose5','rose6']
//    },
//    toolbox: {
//        show: true,
//        feature: {
//            mark: {show: true},
//            dataView: {show: true, readOnly: false},
//            magicType: {
//                show: true,
//                type: ['pie', 'funnel']
//            },
//            restore: {show: true},
//            saveAsImage: {show: true}
//        },
//    },
//    calculable: true,
//    series: [
//        {
//            name: '面积模式',
//            type: 'pie',
//            radius: [20, 70],
//            center: ['50%', '60%'],
//            roseType: 'area',
//            //data: []
//            data: (function(){
//                var arr=[];
//                $.ajax({
//                    type : "post",
//                    async : false, //同步执行
//                    url : "api/canvas.json",
//                    data : {},
//                    dataType : "json", //返回数据形式为json
//                    success : function(result) {
//                        if (result[1]) {
//                            for(var i=0;i<result[1].length;i++){
//                                arr.push(result[1][i]);
//                            }
//                            //console.log("arr=>"+arr);
//                        }
//                    },
//                    error : function(errorMsg) {
//                        alert("获取图表请求数据失败!");
//                        myChart.hideLoading();
//                    }
//                });
//                return arr;
//            })()
//
//        }
//    ]
//};
//=======================  right2  option7  ==========
var right2 = echarts.init(document.getElementById('right2_canvas'),'dark');
option7 = {
    textStyle: {
        fontFamily:'锐字逼格青春体简2.0'
    },
    //  是否出现纵向比较的线
    tooltip: {
        trigger: 'axis'
    },
    color: [
        '#23B576', '#F39800', '#9B1924'
    ],
    //   图例组件
    legend: {
        data: [
            {
                name: '提交诉求数',
                textStyle: {
                    color: '#23B576',
                    fontSize:'10px'
                }
            },
            {
                name: '已处理',
                textStyle: {
                    color: '#F39800',
                    fontSize:'10px'
                }
            },
            {
                name: '未处理',
                textStyle: {
                    color: '#9B1924',
                    fontSize:'10px'
                }
            }
        ],
        x: '30',
        y: '50'
    },
    //   坐标轴线的文字  可设置图表的大小
    grid: {
        show:false,
        top:'40%',
        left: '3%',
        bottom: '8%',
        containLabel: true
    },
    xAxis: [
        {
/*            show:false,
*/          type: 'category',
            color:'#8F9EA6',
            data: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12','  /月'],
            axisTick: {
                alignWithLabel: true,
                interval: 0//interval 是指间隔多少个类别画栅格，为 0 时则每个数据都画，为 1 时间隔 1 个画，以此类推。
            },
            //  坐标轴轴线相关设置
            axisLine:{
                lineStyle:{
                    type:'dashed',
                    color:'#dedede',
                    width:0.5
                }
            },
            splitLine:{
                show:false
            },
            axisLabel: {
                //rotate: 12, //刻度旋转45度角
                interval: 0,
                textStyle: {
                    color: "#8F9EA6",
                    fontSize: 12,
                    fontFamily:'锐字逼格青春体简2.0'
                }
            },            // 坐标轴刻度相关设置
            axisTick:{
                lineStyle:{
                    opacity:'0',
                }
            }
        }
    ],
    yAxis: {
        type: 'value',
        data: ['0', '100', '200', '300', '400', '500'],
        //axisTick: {length:2},//设置x轴之间的距离
        axisLabel: {
            //rotate: 12, //刻度旋转45度角
            interval: 0,
            textStyle: {
                color: "#8F9EA6",
                fontSize: 12,
                fontFamily:'锐字逼格青春体简2.0'
            }
        },  
              // 纵坐标透明度设为0，则不显示
        axisLine:{
            lineStyle:{
                opacity:'0'
            }
        },
        //   y轴上的横向分割线
        splitLine:{ 
            show:true,
            lineStyle:{
                type:'dashed',
                color:'#dedede',
                width: 0.5
            }
        },
         // 坐标轴刻度相关设置   把默认的灰线去掉
        axisTick:{
            lineStyle:{
                opacity:'0',
            }
        }

    },
    splitArea : {show : true},
    series: [
        {
            name: '提交诉求数',
            type: 'line',
            stack: '总量',
            data: [13, 30, 8, 15, 24, 28, 25, 36, 19, 14, 26, 10]
        },
        {
            name: '已处理',
            type: 'line',
            stack: '总量',
            data: [27, 30, 8, 15, 24, 28, 25, 16, 29, 10, 36, 18]
        },
        {
            name: '未处理',
            type: 'line',
            stack: '总量',
            data: [13, 40, 18, 25, 24, 28, 25, 16, 29, 10, 36, 18]
        }
    ]
};
//============================  right3  option3   ==============
var right3 = echarts.init(document.getElementById('right3_canvas'),'chalk');
option8 = {
    color: ['#9b1924', '#f39800'],
    textStyle: {
        fontFamily:'锐字逼格青春体简2.0'
    },
/*    tooltip: {
        trigger: 'axis'
    },
*/    //   图例组件
    legend: {
        right:'20',
        y:'30',
        width:'20',
        data:['已提交','未提交'],
        textStyle: {
            color:  ['#9b1924', '#f39800'],//文本颜色
            fontSize: 10,
            fontWeight: 100
        }
    },
    radar: [
        {
            indicator: (function () {
                var res = [];
                for (var i = 1; i <= 12; i++) {
/*                    res.push({text: i + '月', max: 100});
*/         res.push({text: i , max: 100}); 
       }
                return res;
            })(),
            center: ['50%', '60%'],
            radius: 60,
            //  坐标轴在 grid 区域中的分隔线
            splitLine:{
                lineStyle:{
                    type:'dashed',
                    width:0.5,
                    color:'#eaeaea'
                }
            },
            //  坐标轴在 grid 区域中的分隔区域
            splitArea:{
                areaStyle:{
                    opacity:0
                }
            },
            textStyle: {
                fontSize: '12',
                fontFamily:'锐字逼格青春体简2.0'
            },
            nameGap:5   //指示器名称和指示器轴的距离
        }
    ],
    series: [
        {
            type: 'radar',
            //radarIndex: 2,
            itemStyle: {normal: {areaStyle: {type: 'default'}}},
            data: [
                {
                    name: '已提交',
                    value: [32.6, 35.9, 39.0, 26.4, 28.7, 70.7, 75.6, 82.2, 48.7, 18.8, 66.0, 42.3
                        //(function () {
                        //    var arr = [];
                        //    $.ajax({
                        //        type: "post",
                        //        async: false, //同步执行
                        //        url: "api/canvas.json",
                        //        data: {},
                        //        dataType: "json", //返回数据形式为json
                        //        success: function (result) {
                        //            //console.log(result[5]+'result[5]')
                        //            if (result[3][1]) {
                        //                for (var i = 0; i < result[3][1].length; i++) {
                        //                    arr.push(result[3][1][i]);
                        //                    return arr;
                        //
                        //                }
                        //            }
                        //        },
                        //        error: function (errorMsg) {
                        //            alert("获取图表请求数据失败!");
                        //            myChart.hideLoading();
                        //        }
                        //    });
                        //    return arr;
                        //    console.log("submit_arr=>"+arr);
                        //})()
                    ]
                },
                {
                    name: '未提交',
                    value: [12.0, 4.9, 7.0, 23.2, 25.6, 76.7, 35.6, 62.2, 32.6, 20.0, 6.4, 33.3
                        //(function () {
                        //    var arr = [];
                        //    $.ajax({
                        //        type: "post",
                        //        async: false, //同步执行
                        //        url: "api/canvas.json",
                        //        data: {},
                        //        dataType: "json", //返回数据形式为json
                        //        success: function (result) {
                        //            //console.log(result[5]+'result[5]')
                        //            if (result[3][1]) {
                        //                for (var i = 0; i < result[3][1].length; i++) {
                        //                    arr.push(result[3][1][i]);
                        //                    return arr;
                        //                }
                        //                //console.log("arr=>"+arr);
                        //            }
                        //
                        //        },
                        //        error: function (errorMsg) {
                        //            alert("获取图表请求数据失败!");
                        //            myChart.hideLoading();
                        //        }
                        //    });
                        //    return arr;
                        //})()
                    ]
                }
            ]
        }
    ]
};

// 使用刚指定的配置项和数据显示图表。
left1.setOption(option1);
left2.setOption(option2);
left3.setOption(option3);
center2.setOption(option4, true);
center3.setOption(option5, true);
right1.setOption(option6);
right2.setOption(option7);
right3.setOption(option8);
