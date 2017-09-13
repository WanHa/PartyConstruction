//============================  center1  option4  ==============
function GetSex(groupid) {
    
    $.ajax({
        url: webApiDomain + "home/page/sex/list",
        type: "GET",
        data: { "groupid": groupid },
        dataType: "json",
        success: function (result) {
            var center2 = echarts.init(document.getElementById('center2_canvas'));
            
            var arr = [];
            var man = { name: "男", value: result.data.man };
            var woman = { name: "女", value: result.data.woman };
            arr.push(man);
            arr.push(woman);
            var option4 = {
                tooltip: {
                    show: true,
                },
                textStyle: {
                    fontFamily: '锐字逼格青春体简2.0'
                },
                series: [{
                    hoverAnimation: false, //设置饼图默认的展开样式
                    center: ['50%', '60%'],
                    radius: [15, 50],
                    name: '党员男女比列',
                    type: 'pie',
                    selectedMode: 'single',
                    clockwise: true,
                    startAngle: 90,
                    labelLine: {
                        normal: {
                            lineStyle: {
                            }
                        }
                    },
                    data: arr
                }]
            };
            //男女扇形的颜色
            var colorList = [
                '#d32f3d', '#2495e8'
            ];

            echarts.util.each(arr, function (item, index) {
                item.itemStyle = {
                    normal: {
                        color: colorList[index]
                    }
                };
            });
            center2.setOption(option4);
        },
        error: function (a, e, c) {
            parent.jsprint("获取男女比例数据出现异常","");
            myChart.hideLoading();
        }
    });
}
//============================  left1  option1==============
function GetAge(groupid)
{
    $.ajax({
        type: "GET",
        url: webApiDomain + "home/page/age",
        data: { "groupid": groupid },
        dataType: "JSON",
        success: function (result) {
            // 基于准备好的dom，初始化echarts实例
            var left1 = echarts.init(document.getElementById('left1_canvas'));
            
            var arr = [];
            for (var i = 0; i < result.data.length; i++) {
                arr.push({ name: result.data[i].age, value: result.data[i].sum });
            }
            
            option1 = {
                tooltip: {
                    show: true,
                },
                textStyle: {
                    fontFamily: '锐字逼格青春体简2.0'
                },
                series: [
                    {
                        type: 'pie',
                        radius: [20, 70],
                        center: ['50%', '60%'],
                        roseType: 'area',
                        data: arr
                    }]
            };
            var colorList1 = ['#9b1924', '#f39800', '#1f83cc', '#23b576', '#3c6bdf', '#19428a'];
            echarts.util.each(arr, function (item, index) {
                item.itemStyle = {
                    normal: {
                        color: colorList1[index]
                    }
                };
            });
            left1.setOption(option1);
        },
        error: function (er) { }
    });
}
//============================  left2  option2==============
function GetLearntime(groupid)
{
    $.ajax({
        type: "GET",
        url: webApiDomain + "home/page/learn/time",
        data: { "groupid": groupid },
        dataType: "JSON",
        success: function (result) {
            var left2 = echarts.init(document.getElementById('left2_canvas'));
            var arr = [];
            for (var i = 0; i < result.data.length; i++) {
                arr.push(result.data[i].time);
            }
            option2 = {
                textStyle: {
                    fontFamily: '锐字逼格青春体简2.0',
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
                    show: false,
                    left: '3%',
                    bottom: '8%',
                    containLabel: true
                },

                xAxis: [
                    {
                        type: 'category',
                        color: '#8F9EA6',
                        data: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '  /月'],
                        axisTick: {
                            alignWithLabel: true,
                            interval: 0//interval 是指间隔多少个类别画栅格，为 0 时则每个数据都画，为 1 时间隔 1 个画，以此类推。
                        },
                        //  坐标轴轴线相关设置
                        axisLine: {
                            show: false,
                            lineStyle: {
                                type: 'dashed',
                                color: '#dedede',
                                // width: 0.5
                            }
                        },
                        axisLabel: {
                            //rotate: 12, //刻度旋转45度角
                            show: true,
                            interval: 0,
                            textStyle: {
                                color: "#8F9EA6",
                                fontSize: 12
                            }
                        },
                        // 坐标轴刻度相关设置
                        axisTick: {
                            lineStyle: {
                                opacity: '0',
                            }
                        }
                    }
                ],
                yAxis: {
                    type: 'value',
                    data: ['0','100','200','300','400','500'],
                    //axisTick: {length:2},//设置x轴之间的距离
                    axisLabel: {
                        //rotate: 12, //刻度旋转45度角
                        show: true,
                        interval: 0,
                        textStyle: {
                            color: "#8F9EA6",
                            fontSize: 12,
                            fontFamily: '锐字逼格青春体简2.0'
                        }
                    },
                    // 纵坐标透明度设为0，则不显示
                    axisLine: {
                        lineStyle: {
                            opacity: '0'
                        }
                    },
                    // 坐标轴刻度相关设置
                    axisTick: {
                        lineStyle: {
                            opacity: '0',
                        }
                    },
                    // y轴上的横向分割线
                    splitLine: {
                        show: true,
                        lineStyle: {
                            type: 'dashed',
                            color: '#8F9EA6',
                            width: 0.5
                        }
                    }
                },
                series: [
                    {
                        name: '学习时间',
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
                        data: arr
                    }]
            };
            left2.setOption(option2);
        },
        error: function (er) {

        }
    });
    
}
//============================  left3  option3==============
function GetEconomic(groupid)
{
    $.ajax({
        type: "GET",
        url: webApiDomain + "home/page/econ/money",
        data: { "groupid": groupid },
        dataType: "JSON",
        success: function (result) {
            if (result.issuccess){
                var left3 = echarts.init(document.getElementById('left3_canvas'), 'chalk');
                var arr = [];
                var indicatorData = [];
                for (var i = 0; i < result.data.length; i++) {
                    arr.push(result.data[i].sum);
                }
                for (var i = 0; i < result.data.length; i++) {
                    var item = {
                        "text": result.data[i].econ,
                        "max": result.data[i].sum + 1
                    };
                    indicatorData.push(item);
                }
                option3 = {
                    tooltip: {
                        show: true,
                    },
                    textStyle: {
                        color: "#8F9EA6",
                        fontSize: '12',
                        fontFamily: '锐字逼格青春体简2.0'
                    },
                    radar: [
                        {
                            indicator: indicatorData,
                            center: ['50%', '60%'],
                            radius: 60,
                            // y轴上的横向分割线
                            splitLine: {
                                show: true,
                                lineStyle: {
                                    type: 'dashed',
                                    color: '#eaeaea',
                                    width: 0.5
                                }
                            },
                            nameGap: 5, //指示器名称和指示器轴的距离。
                            //  坐标轴在 grid 区域中的分隔区域
                            splitArea: {
                                areaStyle: {
                                    opacity: 0
                                }
                            }
                        }
                    ],
                    series: [
                        {
                            type: 'radar',
                            itemStyle: { normal: { areaStyle: { type: 'default' } } },
                            data: [{ value:arr,name:'主要经济来源' }]
                        }
                    ]
                };
                left3.setOption(option3);
            }
        },
        error: function (er) {

        }
    });
    
}
