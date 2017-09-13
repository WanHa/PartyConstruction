//=======================   center3  option5   ====================
function getcount(id) {
    
    $.ajax({
        type: "GET",
        url: webApiDomain + "homepagedata/get/getpartymembercount/list",
        data: {"groupid":id},
        dataType: "JSON",
        success: function (result) {
            var center3 = echarts.init(document.getElementById('center3_canvas'));
            var arr = [];
            arr.push({ name: "正式党员", value: result.data.off_count }, { name: "预备党员", value: result.data.ready_count });

            option5 = {
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
                    name: '正式党员与预备党员比例',
                    type: 'pie',
                    selectedMode: 'single',
                    //selectedOffset: 16, //选中是扇区偏移量
                    clockwise: true,
                    startAngle: 90,
                    data: arr
                }]
            };
            //男女扇形的颜色
            var colorList2 = [
                '#f39800', '#9b1924'
            ]
            echarts.util.each(arr, function (item, index) {
                item.itemStyle = {
                    normal: {
                        color: colorList2[index]
                    }
                };
            });
            center3.setOption(option5);
        },
        error: function (er) {

        }
    });
}



function getmeetingcount(id) {
    //===============================  right1 option6  ============
    
    $.ajax({
        type: "GET",
        url: webApiDomain + "homepagedata/get/getmeetingcount/list",
        data: { "groupid": id },
        dataType: "JSON",
        success: function (result) {
            //if (result.issuccess){
            var right1 = echarts.init(document.getElementById('right1_canvas'));
            var arr = result.data;
                //for (var i = 0; i < result.data.length; i++) {
                //    arr.push(result.data[i].count);
                //}
            option6 = {
                    textStyle: {
                        fontFamily: '锐字逼格青春体简2.0',
                        color: "#8F9EA6"
                    },
                    //color: ['#9b1924','#f39800','#2495e8','#23b576'],
                    color: ['#3398DB'],
                    tooltip: {
                        show: true,  
                        trigger: 'axis',
                        axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                            type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                        }
                    },
                    //   坐标轴线的文字  可设置图表的大小
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
                                interval: 0,
                                textStyle: {
                                    color: "#8F9EA6",
                                    fontSize: 12,
                                    fontFamily: '锐字逼格青春体简2.0'
                                }
                            },
                            // 坐标轴刻度相关设置   把默认的灰线去掉
                            axisTick: {
                                lineStyle: {
                                    opacity: '0',
                                }
                            }
                        }
                    ],
                    yAxis: {
                        type: 'value',
                        data: arr,
                        //axisTick: {length:2},//设置x轴之间的距离
                        axisLabel: {
                            //rotate: 12, //刻度旋转45度角
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
                                color: '#dedede',
                                width: 0.5
                            }
                        }
                    },


                    series: [
                        {
                            name: '会议次数',
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
                                        formatter: '{b}月份\n{c}次'
                                    }
                                }
                            },
                            data: arr
                        }
                    ]
                };
                right1.setOption(option6);
            //}
        },
        error: function (er) {

        }
    });
}

//=======================  right2  option7  ==========
function getappealinfo(id) {
    
    $.ajax({
        type: "GET",
        url: webApiDomain + "homepagedata/get/getappeal/list",
        data: { "groupid": id },
        dataType: "JSON",
        success: function (result) {
            var right2 = echarts.init(document.getElementById('right2_canvas'), 'dark');
            option7 = {
                textStyle: {
                    fontFamily: '锐字逼格青春体简2.0'
                },
                //  是否出现纵向比较的线
                tooltip: {
                    show: true,  
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
                                fontSize: '10px'
                            }
                        },
                        {
                            name: '已处理',
                            textStyle: {
                                color: '#F39800',
                                fontSize: '10px'
                            }
                        },
                        {
                            name: '未处理',
                            textStyle: {
                                color: '#9B1924',
                                fontSize: '10px'
                            }
                        }
                    ],
                    x: '30',
                    y: '50'
                },
                //   坐标轴线的文字  可设置图表的大小
                grid: {
                    show: false,
                    top: '40%',
                    left: '3%',
                    bottom: '8%',
                    containLabel: true
                },
                xAxis: [
                    {
/*            show:false,
*/          type: 'category',
                        color: '#8F9EA6',
                        data: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '  /月'],
                        axisTick: {
                            alignWithLabel: true,
                            interval: 0//interval 是指间隔多少个类别画栅格，为 0 时则每个数据都画，为 1 时间隔 1 个画，以此类推。
                        },
                        //  坐标轴轴线相关设置
                        axisLine: {
                            lineStyle: {
                                type: 'dashed',
                                color: '#dedede',
                                width: 0.5
                            }
                        },
                        splitLine: {
                            show: false
                        },
                        axisLabel: {
                            //rotate: 12, //刻度旋转45度角
                            interval: 0,
                            textStyle: {
                                color: "#8F9EA6",
                                fontSize: 12,
                                fontFamily: '锐字逼格青春体简2.0'
                            }
                        },            // 坐标轴刻度相关设置
                        axisTick: {
                            lineStyle: {
                                opacity: '0',
                            }
                        }
                    }
                ],
                yAxis: {
                    type:'value',
                    //axisTick: {length:2},//设置x轴之间的距离
                    axisLabel: {
                        //rotate: 12, //刻度旋转45度角
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
                    //   y轴上的横向分割线
                    splitLine: {
                        show: true,
                        lineStyle: {
                            type: 'dashed',
                            color: '#dedede',
                            width: 0.5
                        }
                    },
                    // 坐标轴刻度相关设置   把默认的灰线去掉
                    axisTick: {
                        lineStyle: {
                            opacity: '0',
                        }
                    }

                },
                splitArea: { show: true },
                series: [
                    {
                        name: '提交诉求数',
                        type: 'line',
                        //stack: '总量',
                        data: result.data.all
                    },
                    {
                        name: '已处理',
                        type: 'line',
                        //stack: '总量',
                        data: result.data.processed
                    },
                    {
                        name: '未处理',
                        type: 'line',
                        //stack: '总量',
                        data: result.data.untreated
                    }
                ]
            };
            right2.setOption(option7);
        },
        error: function (er) {

        }
    });
}

//============================  right3  option3   ==============
function getpartypayinfo(id) {
    
    $.ajax({
        type: "GET",
        url: webApiDomain + "homepagedata/get/getpay/list",
        data: { "groupid": id },
        dataType: "JSON",
        success: function (result) {
            var submit = result.data.submit;
            var unsubmit = result.data.unsubmit;
            var right3 = echarts.init(document.getElementById('right3_canvas'), 'chalk');
            option8 = {
                color: ['#9b1924', '#f39800'],
                textStyle: {
                    fontFamily: '锐字逼格青春体简2.0'
                },
                tooltip: {
                    show: true,
                    
                },
                    //   图例组件
                legend: {
                    right: '20',
                    y: '30',
                    width: '20',
                    data: ['已提交', '未提交'],
                    textStyle: {
                        color: ['#9b1924', '#f39800'],//文本颜色
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
*/                           res.push({ text: i, max: result.data.submit[i-1] + result.data.unsubmit[i-1] +1});
                            }
                            return res;
                        })(),
                        center: ['50%', '60%'],
                        radius: 60,
                        //  坐标轴在 grid 区域中的分隔线
                        splitLine: {
                            lineStyle: {
                                type: 'dashed',
                                width: 0.5,
                                color: '#eaeaea'
                            }
                        },
                        //  坐标轴在 grid 区域中的分隔区域
                        splitArea: {
                            areaStyle: {
                                opacity: 0
                            }
                        },
                        textStyle: {
                            fontSize: '12',
                            fontFamily: '锐字逼格青春体简2.0'
                        },
                        nameGap: 5   //指示器名称和指示器轴的距离
                    }
                ],
                series: [
                    {
                        type: 'radar',
                        //radarIndex: 2,
                        itemStyle: { normal: { areaStyle: { type: 'default' } } },
                        data: [
                            {
                                name: '已提交',
                                value: submit,
                            },
                            {
                                name: '未提交',
                                value: unsubmit
                            }
                        ]
                    }
                ]
            };
            right3.setOption(option8);
        },
        error: function () {

        }
    });
}


// 使用刚指定的配置项和数据显示图表。
//center3.setOption(option5, true);
//right1.setOption(option6);
//right2.setOption(option7);
//right3.setOption(option8);
