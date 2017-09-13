﻿/**
 * TinySelect 灵活的下拉组件
 * @作者: hyjiacan
 * @源码: https://git.oschina.net/hyjiacan/TinySelect.git
 * @示例: http://hyjiacan.oschina.io/tinyselect
 * @许可协议: MIT
 * @依赖: jQuery 1.8.0及更高版本
 * @浏览器支持: 不支持IE8及更低版本
 * @QQ群: 187786345 (Javascript爱好者)
 *
 * @param {Window} win 窗口对象
 * @param {jQuery} $ jQuery别名
 */
(function (win, $) {
    // 来一波严格模式
    'use strict';

    /**
     * 存放所有的实例的集合，在每次通过某个DOM对象创建下拉组件前，会先在这里面找是否在对应的实例
     */
    var instanceSet = [];

    /**
     * 获取文档对象
     */
    var document = win.document;

    /**
     * 获取控制台对象
     */
    var console = win.console;

    /**
     * 保存一个 setTimeout 函数
     */
    var setTimeout = win.setTimeout;

    /**
     * 给值true一个别名
     */
    var TRUE = !0;

    /**
     * 给值false一个别名
     */
    var FALSE = !1;

    /**
     * 给 null值一个别名
     */
    var NULL = null;

    //-------------------CSS样式名称定义
    /**
     * 根样式 tinyselect，这个样式不存在，写这里只是用来拼接
     */
    var css_root = 'tinyselect';

    /**
     * 给创建下拉组件时传入的DOM对象添加的样式类名称： tinyselect-context
     */
    var css_context = css_root + '-context';

    /**
     * 给创建下拉组件时传入的DOM对象需要包含下拉指示器时的样式类名称： tinyselect-context-with-arrow
     */
    var css_contextWithArrow = css_context + '-with-arrow';

    /**
     * 给创建下拉组件时传入的DOM对象里面存放结果的元素添加的样式类名称： tinyselect-context-result
     */
    var css_contextResult = css_context + '-result';

    /**
     * 给创建下拉组件时传入的DOM对象里面指示下拉的元素添加的样式类名称： tinyselect-context-arrow
     */
    var css_contextArrow = css_context + '-arrow';

    /**
     * 设置为只读时结果框的样式 tinyselet-context-readonly
     */
    var css_readonly = css_context + '-readonly';

    /**
     * 下拉框容器的样式名称： tinyselect-container
     */
    var css_container = css_root + '-container';

    /**
     * 下拉框容器mask层的样式名称： tinyselect-container
     */
    var css_mask = css_root + '-mask';

    /**
     * 表格布局时，滚动的代理层  .tinyselect-table-proxy
     */
    var css_tableProxy = css_root + '-table-proxy';

    /**
     * 下拉项为空时，下拉框的样式 tinyselect-container-empty
     */
    var css_empty = css_container + '-empty';

    /**
     * 下拉头部元素的样式名称： tinyselect-header
     */
    var css_header = css_root + '-header';

    /**
     * 下拉底部元素的样式名称： tinyselect-footer
     */
    var css_footer = css_root + '-footer';

    /**
     * 底部左侧容器，在多选时存放全选控件 tinyselect-footer-left
     */
    var css_footerLeft = css_footer + '-left';

    /**
     * 底部右侧容器，存放下拉项的总数。在多选时还要存放选中数量 tinyselect-footer-right
     */
    var css_footerRight = css_footer + '-right';

    /**
     * 放在底部右侧容器中，显示显示下拉项的总数 tinyselect-count-total
     */
    var css_totalCount = css_root + '-count-total';

    /**
     * 多选时选中的项的数量 tinyselect-count-selected
     */
    var css_selectedCount = css_root + '-count-selected';

    /**
     * 存放下拉框每一项的容器元素的样式名称： tinyselect-box
     */
    var css_box = css_root + '-box';

    /**
     * 存放下拉框数据分组的样式名称： tinyselect-group
     */
    var css_group = css_root + '-group';

    /**
     * 下拉框每一项元素的样式名称： tinyselect-item
     */
    var css_item = css_root + '-item';

    /**
     * 下拉框每一项元素前的样式名称： tinyselect-item-before
     */
    var css_itemBefore = css_root + '-item-before';

    /**
     * 下拉框每一项元素文本的样式名称： tinyselect-item-text
     */
    var css_itemText = css_root + '-item-text';

    /**
     * 下拉框每一项元素后的样式名称： tinyselect-item-after
     */
    var css_itemAfter = css_root + '-item-after';

    /**
     * 下拉框每一项元素选中时添加的样式名称： tinyselect-item-selected
     */
    var css_selected = css_item + '-selected';

    /**
     * 下拉框每一项元素hover时添加的样式名称： tinyselect-item-hover
     */
    var css_itemHover = css_item + '-hover';

    /**
     * 下拉框的过滤输入框的样式名称： tinyselect-filter
     */
    var css_filter = css_root + '-filter';

    /**
     * 多选时;选中项会放入前面的context中;放入后每一项的样式名称  tinyselect-result-item
     */
    var css_result = css_root + '-result-item';

    /**
     * 多选时，选中的项会被添加到用来创建下拉组件下拉框的元素中，这个是存放选中文字的容器 tinyselect-result-item-text
     */
    var css_resultText = css_result + '-text';

    /**
     * 多选时，被添加到用来创建下拉组件下拉框的元素中的项的移除按钮，点击可以取消选中那一项 tinyselect-result-item-link
     */
    var css_resultLink = css_result + '-link';

    //-------------------事件类型定义

    /**
     * 事件类型 select
     */
    var evt_select = 'select';

    /**
     * 事件类型 unselect
     */
    var evt_unselect = 'unselect';

    /**
     * 事件类型 ready
     */
    var evt_ready = 'ready';

    /**
     * 支持的事件类型
     */
    var evt_supported = [evt_select, evt_unselect, evt_ready];

    /**
     * 在使用.on(evnetName, handler)时，传入了不支持的eventType时，在控制台输出此信息
     */
    var evt_notSupportedMsg = '不支持此事件类型';

    //-----------------使用jQuery选择器时用到的字符串
    /**
     * css 选择器用到的":visible"符号
     */
    var selector_visible = ':visible';

    //-------------------html标签称名称
    /**
     * 标签名称 span
     */
    var tag_span = 'span';

    /**
     * 创建input[type=checbox]标签的字符串  这个checkbox有一个样式类
     * tinyselect-selectall
     */
    var tag_checkbox = '<input type="checkbox" style="vertical-align:-2px;" class="' +
        css_root + '-selectall" />';

    /**
     * 通过jQuery存放每一项数据的属性名称，如： $(item).data('data'; itemdata);
     */
    var str_data = 'data';

    /**
     * 每一项的序号索引属性名称
     */
    var str_indexAttr = 'data-tiny-index';

    /**
     * 分组标记的属性名称
     */
    var str_groupAttr = 'data-tiny-group';

    /**
     * 字符串模板的占位符，这个在渲染下拉项数量时使用
     */
    var str_placeholder = '%s';

    /**
     * 显示模式-下拉
     */
    var mode_dropdown = 'dropdown';

    /**
     * 显示模式-列表
     */
    var mode_list = 'list';

    /**
     * 显示模式-弹出
     */
    var mode_popup = 'popup';

    /**
     * 支持的显示模式
     */
    var support_mode = [mode_dropdown, mode_list, mode_popup];

    /**
     * 布局模式-列表
     */
    var layout_list = 'list';

    /**
     * 布局模式-网格
     */
    var layout_grid = 'grid';

    /**
     * 布局模式-列表
     */
    var layout_table = 'table';

    /**
     * 支持的显示模式
     */
    var support_layout = [layout_list, layout_grid, layout_table];

    /**
     * 输入即时过滤的定时器句柄
     */
    var filter_handle;

    /**
     * 默认的创建下拉组件选项
     * 这里列出了所有可用的项
     * 这些项会被附加到 TinySelect上面,可以通过  TinySelect.defaults.xxx 来修改这些默认配置
     */
    var defaultOption = {
        // 组件是否是只读的
        readonly: FALSE,
        // 显示模式，可以设置的值为： dropdown(默认下拉模式), list(列表模式), popup(弹出模式)
        mode: mode_dropdown,
        // 是否支持键盘操作，默认为 true
        keyboard: TRUE,
        // 附加的样式类名称
        css: NULL,
        // 下拉框容器的样式
        style: {
            // 这个行高是必须的，覆盖这些样式时，需要注意，
            // 其值需要是可以转换成整数的类型，因为下拉的项会使用这个作为默认的行高
            // 例外的情况：设置了项的行高(即下面的 item.line-height)
            lineHeight: '28px'
        },
        // 下拉框的头部
        header: {
            // 头部渲染器，其this上下文就是其DOM对象，
            // 直接操作这个对象来改变头部
            // 这个是在执行完其初始化，添加到容器前调用的
            render: FALSE,
            // 过滤框
            filter: {
                // 触发过滤的动作，可以设置为 change 或 enter(默认)。
                // 为change时，输入有变化时执行
                // 为enter时，按下回车时执行
                trigger: 'enter',
                // 此值表示在输入框经过指定时间后没有键盘输入时，触发过滤动作
                // 单位是毫秒，默认值为 618
                // 当 trigger 为change时有效
                // 设置这个是为了防止使用亚洲输入法时（比如：中文，日文等）时，文字输入中将字母上屏导致的事件触发
                delay: 618,
                // 过滤框的提示文字
                placeholder: '输入后按回车过滤',
                // 附加的样式类名称
                css: NULL,
                // 过滤框的样式
                style: {}
            },
            // 附加的样式类名称
            css: NULL,
            // 头部样式
            style: {}
        },
        // 下拉项容器
        box: {
            // 下拉列表没有数据时显示的文字
            empty: '没有数据',
            // 数据项的布局方式
            // 可设置的值有： list(列表布局，默认值), grid(网格布局), table(表格布局)
            layout: layout_list,
            // 附加的样式类名称
            css: NULL,
            // 下拉项容器的样式
            style: {}
        },
        // 数据项分组设置
        group: {
            // 分组值字段
            // 设置此值时才会分组
            valueField: FALSE,
            // 分组文本字段，不设置时使用 valueField
            // 相同的 valueField 而 textField不同时，只会取第一个 textField的值
            textField: FALSE,
            // 数据项不包含指定的 valueField字段时的分组名称
            unknown: '未分组',
            // 分组的渲染器
            render: FALSE,
            // 附加的样式类名称
            css: NULL,
            // 下拉项容器的样式
            style: {}
        },
        // 下拉项
        item: {
            // 下拉项数据的数组，每一项需要对象结构的数据
            data: [],
            // 默认选中的项
            value: FALSE,
            // 数据对象的值字段，在获取/设置值时，会使用这个字段
            valueField: 'id',
            // 数据对象的文本字段，下拉项的显示文字
            textField: 'text',
            // 可见项的数量，数据数量多余此值时出现滚动条
            visible: 5,
            // 下拉项的渲染器，使用返回值设置项的内容
            // render: function(itemdata, index, alldata){}  this 指向即将渲染的网页元素对象。
            // itemdata:这一项的数据 
            // index: 这一项数据的索引
            // alldata:下拉的所有数据
            // 设置为false 禁用渲染器
            render: FALSE,
            // 是否在数据项比设定的 visible 多时使用异步渲染(true)，
            // 在数据较多时建议设置为true，以避免大量的dom操作阻塞页面执行
            async: TRUE,
            // 附加的样式类名称
            css: NULL,
            // 每一个下拉项的样式
            style: {}
        },
        // 下拉底部DOM
        footer: {
            // 底部渲染器，其this上下文就是其DOM对象，
            // 直接操作这个对象来改变底部
            // 需要注意的是，底部分了“左”、“右”两个区域，以放置不同的东西
            // 这个是在执行完其初始化，添加到容器前调用的
            render: FALSE,
            /**
             * 下拉项数量
             */
            totalTpl: '共' + str_placeholder + '项',
            /**
             * 选中的下拉项数据
             */
            selectedTpl: '选中' + str_placeholder + '项/',
            // 附加的样式类名称
            css: NULL,
            // 底部的样式
            style: {}
        },
        result: {
            // 是否启用多选模式
            multi: FALSE,
            // 渲染选中结果的渲染器，可以通过这个来改变选中结果的渲染
            render: FALSE,
            // 多选结果展示方式，可以设置为 0（显示选中的数量，默认值） 或者 1（显示 选中的项列表）
            // 这是一个预留配置项
            type: 0,
            // 附加的样式类名称
            css: NULL,
            // 多选结果的样式
            style: {}
        }
    };

    function Selector(css) {
        this.selector = [];
        this.css(css);
    }

    Selector.prototype = {
        constructor: Selector,
        css: function (css) {
            if (css) {
                this.selector.push('.' + css);
            }
            return this;
        },
        attr: function (key, val) {
            if (arguments.length === 1) {
                this.selector.push('[' + key + ']');
            } else {
                this.selector.push('[' + key + '=' + val + ']');
            }
            return this;
        },
        sub: function (css) {
            this.selector.push(' ');
            return this.css(css);
        },
        visible: function () {
            return this.addon(selector_visible)
                .done();
        },
        first: function () {
            return this.addon(':first')
                .done();
        },
        addon: function (addon) {
            if (addon) {
                this.selector.push(addon);
            }
            return this;
        },
        done: function (css) {
            this.css(css);
            return this.selector.join('');
        }
    };

    Selector.build = function (css) {
        return new Selector(css);
    };

    //---------------------工具函数定义

    /**
     * 判断对象是否包含某个属性，搞个短的名称
     * 通过  own(obj, name) 的方式调用
     */
    var own = function (obj, prop) {
        return !obj || obj.hasOwnProperty(prop);
    };

    /**
     * 使用异步调用，这是通过  setTimeout 来假装的
     *
     * @param {Function} fn 要异步调用的函数
     * @param {Array} args 函数参数的数组
     */
    var asyncCall = function (fn, args) {
        setTimeout(function () {
            fn.apply(NULL, args);
        }, 0);
    };

    /**
     * 对象深度复制工具，始终返回新的对象
     */
    var clone = function (obj1, obj2) {
        return $.extend(TRUE, $.isArray(obj1) ? [] : {}, obj1, obj2);
    };

    /**
     * 创建Div元素，附加class属性
     * @param {String} className 样式类列表
     * @param {String} [tagName='div'] 标签名称，默认为 div
     * @return {jQuery} 创建的元素
     */
    var createElement = function (className, tagName) {
        return $('<' + (tagName || 'div') + ' class="' + className + '">');
    };

    /**
     * 合并数据，后面的数组会被合并到前面的数组中
     * @param {Array} array 原始数组
     * @param {Array} dataArray 这个数组里面的项会被追加到 array 的最后
     */
    var mergeArray = function (array, dataArray) {
        Array.prototype.push.apply(array, dataArray);
    };

    /**
     * 根据上下文DOM元素初始化下拉框，如果已经初始化过了，那就返回对应的实例对象
     *
     * @param {jQuery|String|HtmlElement} selector 用来创建下拉组件的上下文DOM元素
     * @param {Object|Array} option 选项或数据
     * @param {Boolean} multi 是否可以多选，true为可多选，false为仅单选(默认);仅当option为数组时此参数有效
     * @return {TinySelect} 下拉实例
     */
    function TinySelect(selector, option, multi) {
        // 取第一个DOM对象
        var context = $(selector).get(0);

        // 取不到DOM对象，就放弃，不创建下拉组件了
        if (!context) {
            return;
        }

        // 声明一个对象来存放下拉组件的实例，不管实例是否存在，都会用到这个
        var instance;

        // 遍历实例集合，看看有这个context有没有对应的下拉组件
        for (var i = 0; i < instanceSet.length; i++) {
            instance = instanceSet[i];
            // 找到了通过这个context创建的下拉组件，返回这个下拉组件
            if (instance.context.get(0) === context) {
                return instance;
            }
        }
        // 创建下拉组件
        instance = new TinySelect.fn.init(context, option, multi);

        // 将创建的下拉组件放到实例集合中，以方便实例的查找
        // 查找：前面通过遍历这个集合，查找context对应的实例部分
        instanceSet.push(instance);

        // 返回实例
        return instance;
    }

    /**
     * 添加原型方法
     */
    TinySelect.fn = TinySelect.prototype = {
        constructor: TinySelect,

        /**
         * 初始化函数，用来创建创建下拉实例
         *
         * @param {HtmlElement} context 下拉的上下文DOM元素，下拉将在这个元素的上方或下方显示
         * @param  {Object|Array} option 选项或数据
         * @param {Boolean} multi 是否可以多选，true为可多选，false为仅单选(默认);仅当option为数组时此参数有效
         * @return {TinySelect} 新的实例
         */
        init: function (context, option, multi) {
            // 保存实例对象到变量里面
            var ts = this;

            // 如果传的是一个数组，那么就使用默认的选项，
            // 并且将这个数组设为下拉的数据源
            if ($.isArray(option)) {
                ts.option = clone(defaultOption, {
                    item: {
                        data: option
                    },
                    result: {
                        multi: multi
                    }
                });
            } else {
                // 传的是对象，那么合并选项参数
                ts.option = clone(defaultOption, option);

                // 显示模式
                var mode = ts.option.mode || mode_dropdown;
                if (support_mode.indexOf(mode) === -1) {
                    throw new Error('Render mode "' + mode + '" is not supported,\nhere is the valid modes:' + support_mode.join());
                }

                // 布局模式
                var layout = ts.option.layout || layout_list;
                if (support_layout.indexOf(layout) === -1) {
                    throw new Error('Item layout "' + layout + '" is not supported,\nhere is the valid modes:' + support_layout.join());
                }
            }

            // DOM上下文
            ts.context = $(context);

            // 渲染上下文DOM元素
            renderContext(ts);

            // 初始化事件集合
            ts.events = {};

            // 创建DOM结构
            createDOM(ts);

            // 绑定事件
            bindEvent(ts);

            // 渲染项
            ts.load(ts.option.item.data, function (data) {
                // 这里搞了个回调，以在所有项渲染完成后触发组件的ready 事件
                emitEvent(ts, evt_ready, {
                    data: data
                });
            });

            // 返回实例对象
            return ts;
        },

        /**
         * 绑定事件到TinySelect上，如果想要使用.off(eventType, handler)来解除绑定，
         * 那么就需要传入函数名，而不能使用匿名函数
         *
         * @param {String} eventType 事件类型
         * @param {Function} handler 事件处理函数，
         * @return {TinySelect} 当前实例
         */
        on: function (eventType, handler) {
            var ts = this;

            // 检查事件是否支持 不支持就在console提示，然后返回
            if (evt_supported.indexOf(eventType) === -1) {
                console.warn(evt_notSupportedMsg + ':' + eventType);
                return ts;
            }
            var events = ts.events;

            // 已经绑定过这个事件，则将新的处理函数追加到事件数组里面
            if (own(events, eventType)) {
                events[eventType].push(handler);
            } else {
                // 还没有绑定过这个事件，创建一个包含这个事件处理函数的数组
                events[eventType] = [handler];
            }

            return ts;
        },

        /**
         * 解除通过 .on(eventType, handler) 绑定的事件处理函数。
         * 注意：如果handler是匿名函数，那么此操作无效
         *
         * @param {String} eventType 事件类型
         * @param {Function} handler 要解除的事件处理函数
         * @return {TinySelect} 当前实例
         */
        off: function (eventType, handler) {
            var ts = this;

            // 检查事件是否支持 不支持就直接返回
            if (evt_supported.indexOf(eventType) === -1) {
                return ts;
            }

            var events = ts.events;
            // 如果没有绑定过这个事件，直接返回
            if (!own(events, eventType)) {
                return ts;
            }
            var event = events[eventType];

            // 这个函数是否存在
            var index = event.indexOf(handler);

            // 这个函数是绑定上的，干掉它！！
            if (index !== -1) {
                event.splice(index, 1);
            }

            return ts;
        },

        /**
         * 显示下拉框
         *
         * @param {Function} callback 显示完成后的回调函数
         * @return {TinySelect} 当前实例
         */
        show: function (callback) {
            var ts = this;
            var dom = ts.dom;
            var mode = ts.option.mode;

            // 列表模式调用无效
            if (mode === mode_list) {
                return ts;
            }
            if (mode === mode_popup) {
                // 弹出模式时，要显示mask
                ts.mask.show();
            }

            // 设置下拉框的显示位置
            fixPosition(ts.context, dom, ts.option);

            // 用fadein搞个动画
            dom.fadeIn('fast', function () {

                // 下拉框显示出来后，如果过滤框可见，则将焦点放到过滤框中
                dom.find(Selector.build(css_header).sub(css_filter).visible()).focus();

                fixSize(ts);

                // 显示后，调用回调函数
                if (callback) {
                    callback.call(ts);
                }
            });

            return ts;
        },
        /**
         * 隐藏下拉框
         *
         * @param {Function} callback 隐藏完成后的回调函数
         * @return {TinySelect} 当前实例
         */
        hide: function (callback) {
            var ts = this;
            var mode = ts.option.mode;

            // 列表模式调用无效
            if (mode === mode_list) {
                return ts;
            }
            // 弹出模式时，隐藏的是mask层
            var target = mode === mode_dropdown ? ts.dom : ts.mask;

            // 用fadeout搞个隐藏时候的动画
            target.fadeOut('fast', function () {
                if (callback) {
                    callback.call(ts);
                }
            });

            return ts;
        },

        /**
         * 过滤下拉项，指定要过滤的关键字或过滤函数
         *
         * @param {String|Function} keyOrFn 过滤的关键字或函数
         * @param {Boolean} toggle 是否隐藏未命中项，显示命中项
         * @return {Array} 筛选命中的项组成的数组
         */
        filter: function (keyOrFn, toggle) {
            var ts = this;
            var result = [];

            // 判断是通过关键字过滤还是传入了自定义的过滤器（函数）
            var isfn = $.isFunction(keyOrFn);

            var groupThem = !!ts.option.group.valueField;

            // 取到所有下拉项的DOM
            var items = getItemsFromDom(ts);

            // 所有的分组头
            var groups = ts.dom.find(Selector.build(css_group).done());

            // 逻辑说明：
            // 每处理一项，就找到这一项所在组，然后将对应项的已经处理记录加1，即  ++handled
            // 要是已经处理的项<分组中总项数  那么这一组就还没有处理完
            // 不小于年 时候，表示这组的项已经处理完了，可以去搞分组头的显示或隐藏了

            // 每组中已经过滤的项的数量
            var groupHandledCount = {};

            groups.each(function () {
                var id = $(this).attr(str_groupAttr);
                groupHandledCount[id] = {
                    total: items.filter(Selector.build(css_item).attr(str_groupAttr, id).done()).length,
                    handled: 0
                };
            });

            function groupProxy(item) {
                if (!groupThem) {
                    return;
                }

                var groupid = $(item).attr(str_groupAttr);

                var count = groupHandledCount[groupid];

                if ((++count.handled) < count.total) {
                    return;
                }

                setGroupVisible(groups, items, groupid);
            }
            // 遍历过滤
            items.each(function (index, item) {
                item = $(item);

                // 取到这一项的数据  数据是通过jQuery的 xx.data(str_data) 取到的
                var data = getData(item);

                // 如果传了过滤器，那就调用哇，这里会设置过滤器的this对象为这一项的DOM对象，同时会将这项的数据作为一个参数传入
                // 过滤器函数的返回值决定了这一项是否会被命中（true）
                if (isfn ? keyOrFn.call(this, data) :
                    // 传的是字符串，直接看项的显示文字里面有没有这个字符串
                    item.text().indexOf(keyOrFn.toString()) !== -1) {
                    result.push({
                        item: item,
                        data: data
                    });

                    // 如果要显示状态（根据过滤是否命中来显示和隐藏这一项）
                    if (toggle) {
                        item.slideDown(50, function () {
                            groupProxy(this);
                        });
                    }
                } else if (toggle) {
                    item.slideUp(50, function () {
                        groupProxy(this);
                    });
                }
            });

            return result;
        },
        /**
         * 设置或获取下拉的选中值
         *
         * @param {any} val 配置的item.valueField字段的值，可以是单个值(单选)或数组(多选)。不传时获取值
         * @param {Boolean} trigger 是否引发事件，默认为 false
         * @return {any} 返回值或实例
         */
        value: function (val, trigger) {
            var ts = this;

            // 没有传参数，这时候就是获取值
            if (arguments.length === 0) {
                return getValue(ts);
            }

            // 传了参数，这时候是设置值
            setValue(ts, val, trigger);

            return ts;
        },
        /**
         * 清除选中的项
         * @rerurn {TinySelect} 下拉组件实例
         */
        clear: function () {
            // 清除所有的选中项
            clearSelection(this);

            return this;
        },
        /**
         * 使用指定的数据重新渲染下拉项
         *
         * @param {Array} data 数据项
         * @param {Function} callback 渲染完成后的回调函数
         * @return {TinySelect} 下拉组件实例
         */
        load: function (data, callback) {
            var ts = this;

            // 将新的数据绑定到组件上
            // 为了保持数据的纯洁性，用clone创建数据的副本来玩
            ts.option.item.data = clone(data);

            // 渲染下拉项
            renderItems(ts, function () {
                fixSize(ts);
                if (callback) {
                    callback.call(ts, ts.option.item.data);
                }
            });

            return ts;
        },
        /**
         * 设置或获取下拉组件是否是只读的
         *
         * @param {Boolean} readonly 设置是否只读，若不传这个参数，那就是获取只读状态
         * @return {Boolean|TinySelect} 获取状态时返回是否只读的状态，设置值时返回组件实例
         */
        readonly: function (readonly) {
            var ts = this;

            if (arguments.length === 0) {
                return ts.option.readonly;
            }

            // 如果设置为只读，那么就先隐藏下拉框
            if (readonly) {
                ts.hide();
                // 添加只读样式
                ts.context.addClass(css_readonly);
            } else {
                ts.context.removeClass(css_readonly);
            }

            ts.option.readonly = readonly;

            return ts;
        }
    };

    TinySelect.fn.init.prototype = TinySelect.fn;

    /**
     * 默认配置项，可以在加载时修改
     */
    TinySelect.defaults = defaultOption;

    /**
     * 渲染上下文DOM元素里面的DOM，创建结果容器和下拉指示元素
     */
    function renderContext(ts) {
        // 不是列表模式时，总是渲染上下文
        if (ts.option.mode === mode_list) {
            return;
        }

        var context = ts.context;

        // 添加存放选中结果的容器
        context.addClass(css_context)
            .append(createElement(css_contextResult));

        // 初始化时如果设置了只读属性，那么给上下文元素添加只读的样式类
        if (ts.option.readonly) {
            context.addClass(css_readonly);
        }

        // 多选的话就不添加下拉指示器
        if (ts.option.result.multi) {
            return;
        }

        // 单选时添加下拉指示器
        context.addClass(css_contextWithArrow)
            .append(createElement(css_contextArrow));

        // 如果context是静态布局，那么修改为相对布局
        // 因为单选时要显示那个下拉指示器，这个指示器是用的绝对定位
        if (/static/i.test(context.css('position'))) {
            context.css('position', 'relative');
        }
    }

    /**
     * 创建下拉的所有DOM结构
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     */
    function createDOM(ts) {
        var option = ts.option;
        var context = ts.context;

        // 给下拉容器添加css类
        var container = ts.dom = createElement(css_container)
            .addClass(css_root + '-mode-' + option.mode);

        // 以列表模式显示
        switch (option.mode) {
            case mode_list:
                // 此时就不会再使用绝对定位，而是替换context的位置
                // 同时，也不会应用样式 width height position
                // 如果context的父级只有context一个元素，那就设置宽度为auto
                // 这样可以避免滚动条出现后挡住下拉组件右侧
                option.style.width = context.siblings().length ? context.width() :
                    'auto';
                option.style.height = context.height() || 'auto';
                var position = context.css('position');
                option.style.position = /static/i.test(position) ? 'relative' : position;

                context.append(container);
                break;
            case mode_dropdown:
                // 默认的下拉模式
                // 把下拉组件添加到 document.body
                $(document.body).append(container);
                break;
            case mode_popup:
                // 弹出模式时，添加mask层
                var mask = ts.mask = createElement(css_mask);
                $(document.body).append(mask.append(container));
                break;
        }

        // 给下拉框添加样式
        // 用户设置的优先级最高了
        container.addClass(option.css).css(option.style)
            // 创建下拉的头部元素
            .append(renderHeader(ts, option));

        var boxoption = option.box;
        // 创建下拉项的容器
        var box = ts.box = createElement(css_box)
            .addClass(css_box + '-layout-' + boxoption.layout)
            .addClass(boxoption.css)
            .css(boxoption.style);

        // 如果是表格布局，那么加一个滚动的代理层
        if (boxoption.layout === layout_table) {
            box.append(createElement(css_tableProxy));
        }

        container.append(box);

        // 创建下拉的底部元素
        container.append(renderFooter(ts, option));
    }

    /**
     * 渲染下拉框的头部DOM
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {any} option 选项，创建下拉组件时传入的参数与默认参数合并得到
     * @return {jQuery} header的jquery对象
     */
    function renderHeader(ts, option) {
        var headeroption = option.header;
        // 创建  header
        var header = ts.header = createElement(css_header)
            // 添加css
            .addClass(headeroption.css)
            // 设置头部的样式
            .css(headeroption.style)
            // 在头部添加一个过滤的输入框
            .append(renderFilter(ts, option));

        // 调用自定义的头部渲染函数
        if (headeroption.render) {
            headeroption.render.call(header, option.item.data);
        }

        return header;
    }

    /**
     * 当配置了过滤可见时，渲染过滤器
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {Object} option 选项，创建下拉组件时传入的参数与默认参数合并得到
     * @return {jQuery} 过滤框的jquery对象
     */
    function renderFilter(ts, option) {
        // 创建过滤
        var filteroption = option.header.filter;

        var filter = $('<input type="text"  placeholder="' +
            filteroption.placeholder + '" class="' + css_filter + '" />')
            .addClass(filteroption.css)
            .css(filteroption.style);

        filter.keyup(function (e) {
            var val = filter.val();

            // 只要按下了键，就先清除过滤的定时器
            if (filter_handle) {
                win.clearTimeout(filter_handle);
                filter_handle = 0;
            }

            if (/^change$/i.test(filteroption.trigger) ?
                // 按下非输入键 (不可见字符)不处理
                ($.trim(String.fromCharCode(e.keyCode || e.which)) !== '' &&
                    filter.data('last') === val) : e.keyCode !== 13) {
                return;
            }

            filter.data('last', val);

            // 设置过滤的定时器
            filter_handle = setTimeout(function () {
                ts.filter(val, TRUE);
            }, filteroption.delay);
        });

        return filter;
    }

    /**
     * 渲染下拉框的底部
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {Object} option 选项，创建下拉组件时传入的参数与默认参数合并得到
     * @return {jQuery} footer的jquery对象
     */
    function renderFooter(ts, option) {
        var footeroption = option.footer;
        // 创建下拉底部DOM元素
        var footer = ts.footer = createElement(css_footer)
            .addClass(footeroption.css)
            // 设置下拉底部的样式
            .css(footeroption.style);

        // 这里面创建左右两个容器，是为了方便底部数据分左右显示

        // 创建下拉底部左侧容器
        var left = createElement(css_footerLeft);

        // 创建下拉底部右侧容器
        var right = createElement(css_footerRight);

        // 添加一个数据项总量显示框
        // 内容根据字符串模板 option.footer.totalTpl 来的
        right.append(createElement(css_totalCount, tag_span)
            .html(footeroption.totalTpl.replace(str_placeholder, 0)));

        // 将左右两个容器添加到底总元素中
        footer.append(left).append(right);

        // 如果配置了多选，那么就添加一个用于全选的checkbox元素
        // 同时会添加一个表示已经选中项数量的元素
        if (option.result.multi) {
            renderMultiSelectFooter(ts, option, left, right);
        }

        // 如果定义了底部渲染器，现在是时候调用了
        if (footeroption.render) {
            footeroption.render.call(footer, option.data);
        }

        return footer;
    }

    /**
     * 多选时，使用这个函数来渲染底部的全选/选中项等显示
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {Object} option 创建下拉组件时传入的参数与默认参数合并得到
     * @param {jQuery} left footer左侧的容器的jQuery对象
     * @param {jQuery} right footer右侧的容器的jQuery对象
     */
    function renderMultiSelectFooter(ts, option, left, right) {
        // 创建一个 checkbox
        var checkbox = $(tag_checkbox);

        // 向底部左侧的容器添加元素，结构如下：
        // label>input[type=checkbox].tinyselect-selectall[text=全选]
        left.append($('<label>').append(checkbox).append('全选'));

        // 给checkbox绑定change事件，以在checkbox被点击，勾选状态改变后执行选中/取消选中
        // 项的选中状态，通过是否包含css类 tinyselect-item-selected 指示
        // 这里只会操作可见的下拉项，以适用于过滤后的数据项批量操作
        checkbox.change(function () {
            // 判断此时的checkbox是否是被勾选的
            var checked = checkbox.is(':checked');

            // 把所有可见的下拉项都弄出来，循环设置选中状态
            getItemsFromDom(ts, selector_visible).each(function (index, item) {
                item = $(item);

                // 如果此时checkbox是勾选的  那就是要设置可见的项为选中状态了
                if (checked) {

                    // 对已经选中的项，当然啥也不做
                    if (item.hasClass(css_selected)) {
                        return;
                    }

                    // 把还没有选中的项选中，并触发一个 select 事件
                    selectItem(ts, item, TRUE);

                    return;
                }

                // 代码执行到这里，就表示checkbox是没有被勾选的，这时候就要把可见的项设置为未选中状态

                // 当前这一项本来就没有被选中，那啥也不做
                if (!item.hasClass(css_selected)) {
                    return;
                }

                // 把选中的项取消选中，并触发一个 unselect 事件
                deselectItem(ts, item, TRUE);
            });
        });

        // 创建一个选中数量的显示框
        var el = createElement(css_selectedCount, tag_span);

        // 设置选中的初始值0
        setSelectedCount(option, el);

        // 选中数量显示框添加到底部右铡的容器中
        right.prepend(el);
    }

    /**
     * 解析分组数据，根据指定的字段对数据进行分组
     * @param {Array} data 原始数据
     * @param {Object} group 分组的选项
     * @return {Array} 处理后的数据
     */
    function resolveGroupData(data, group) {
        var valueField = group.valueField;
        // 没有设置 textField的话，就用 valueField 来作为 text
        var textField = group.textField || valueField;

        // 创建一个单独的数组用来作为未分组的项
        var unknown = [{ _group_item_: TRUE, text: group.unknown, id: 0 }];

        // 存放除了未分组的所有分组
        var groups = {};

        var groupid = 0;

        $.each(data, function () {
            var item = this;
            var hasValue = own(item, valueField);
            var hasText = own(item, textField);
            if (!hasValue) {
                item._group_id_ = 0;
                unknown.push(item);
                return;
            }

            var val = item[valueField];

            // 分组名称留空
            var text = hasText ? item[textField] : '';

            if (!own(groups, val)) {
                groups[val] = {
                    id: ++groupid,
                    text: text,
                    data: []
                };
            }

            item._group_id_ = groupid;

            groups[val].data.push(item);
        });

        // 重新弄成数组
        // 先取出所有的分组数据，将其搞成一条数组记录
        // 然后再将下面的项追加到数组后面
        // 重复 一直到结束
        // 最后再将 unknown 的项加进去
        var temp = [];

        $.each(groups, function (key, groupData) {
            temp.push({
                id: groupData.id,
                _group_item_: TRUE,
                key: key,
                text: groupData.text
            });
            mergeArray(temp, groupData.data);
        });

        // 有个默认的分组项  判断>1才表示真的有未分组的项
        if (unknown.length > 1) {
            mergeArray(temp, unknown);
        }
        return temp;
    }

    /**
     * 渲染下拉的项
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {Function} callback 所有项渲染完成后的回调函数
     */
    function renderItems(ts, callback) {
        // 先从下拉框里面找出存放下拉项的容器
        // 选择器  .tinyselect-item
        var box = ts.box;
        var option = ts.option;
        var itemoption = option.item;
        var data = itemoption.data;
        var group = option.group;

        // 可见项的数量
        var visibleCount = parseInt(itemoption.visible);

        // 如果可见项的数量不是数字或是负数，那么设置成0
        // 你要给我搞怪乱填，那我就按我的方式来处理了
        if (isNaN(visibleCount) || visibleCount < 0) {
            visibleCount = 0;
        }

        // 数据的长度
        // 防止没有设置这个data属性引发错误
        var length = !data ? 0 : data.length;

        // 清空下拉项容器
        box.height('auto');

        if (option.box.layout === layout_table) {
            box.find(Selector.build(css_tableProxy).first()).empty();
        } else {
            box.empty();
        }

        // 清除选中结果，这个放在box.empty()后面，可以在选中项很多时执行更快
        // 当然，这个快是我猜的
        clearSelection(ts);

        // 移除下拉框的没有数据的样式类名称
        // 如果真的没有数据，那么后面再重新加上
        ts.dom.removeClass(css_empty);

        // 获取到用来显示总数量项数的元素的对象
        var totalElement = getTotalCount(ts);

        // 设置元素数量显示值
        // 这一步只是把数量值绑定到元素上了：通过 $().data(xxx)的方式
        setData(totalElement, length);

        // 显示总数量
        setTotalCount(option, totalElement);

        // 没有数据
        if (!length) {
            // 显示设置的空数据表示文本
            box.append(option.box.empty);
            // 给下拉添加没有数据的样式类
            ts.dom.addClass(css_empty);

            // 没有数据也要调用一下渲染完成的回调函数，做人要有礼貌
            if (callback) {
                callback.call(ts, data);
            }
            return;
        }

        // 处理分组
        if (group.valueField) {
            data = resolveGroupData(data, group);
            length = data.length;
        }

        // 先渲染指定的数量 若visibleCount为0表示全部显示
        // 如果 visibleCount 为0，函数  renderSpecifiedItems 会渲染所有数据的
        // 您不用担心了
        var item = renderSpecifiedItems(ts, box, itemoption, data, callback, 0, visibleCount);

        // 如果可见项的数量大于等于数据项的数量，那么就让box的高度自己高兴吧
        // 当前  visibleCount为0也是这样
        if (visibleCount === 0 || visibleCount >= length) {
            return;
        }

        // 如果设置了container高度，就直接box高度了
        if (!isNaN(parseInt(ts.dom.get(0).style.height))) {
            var boxHeight = ts.dom.height();
            if (ts.header.is(selector_visible)) {
                boxHeight -= ts.header.height();
            }
            if (ts.footer.is(selector_visible)) {
                boxHeight -= ts.footer.height();
            }
            box.height(boxHeight);
        } else {
            // 看有没有设置  option.box.style.height
            // 如果没有设置，我就自作聪明，给计算一下
            // 我想让box的高度=第一个下拉项的高度*visibleCount

            // 根据第一项来计算容器的理论高度： 行高+上下padding
            var itemHeight = parseInt(item.css('lineHeight')) +
                parseInt(item.css('paddingTop')) +
                parseInt(item.css('paddingBottom'));

            // 数据项的数量大于可见项数量时，设置容器高度为可见项数量的高度+分组高度（如果有分组）
            var groupHeight = 0;
            // 有分组
            if (group.valueField) {
                groupHeight = box.find(Selector.build(css_group).first()).height();
            }

            box.height(visibleCount * itemHeight + groupHeight);
        }

        // 这里分两次渲染，假装考虑性能问题
        // 渲染剩下的项（只在 visibleCount>0 并且 visibleCount > length时会执行到这里
        if (itemoption.async) {
            // 异步渲染剩下的项
            asyncCall(renderSpecifiedItems, [ts, box, itemoption, data, callback, visibleCount]);
        } else {
            // 同步渲染剩下的项
            renderSpecifiedItems(ts, box, itemoption, data, callback, 0);
        }
    }

    /**
     * 渲染指定范围的下拉项
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {jQuery} box 下拉项容器的jQuery对象
     * @param {Object} itemoption ts.option.item 的配置
     * @param {Array} alldata 要渲染的所有数据，这些数据可能是经过分组处理的
     * @param {Function} callback 所有项渲染完成后的回调函数
     * @param {Number} start 开始渲染的索引
     * @param {Number} count 渲染的数量
     * @return  {jQuery} 渲染的第一项，这个返回给调用函数，调用函数根据这一项来计算每一项的高度
     */
    function renderSpecifiedItems(ts, box, itemoption, alldata, callback, start, count) {
        // 这个变量用来保存一个下拉项的DOM对象，最后会被作为这个函数的返回值，
        // renderItems函数会用到这个DOM对象，就在计算box高度那里
        var keep;

        // 如果是表格布局 那么就将元素添加到滚动代理层下
        if (ts.option.box.layout === layout_table) {
            box = box.find(Selector.build(css_tableProxy).first());
        }

        var groupOption = ts.option.group;

        // 所有的数据项对象
        var originData = itemoption.data;

        // 这次要渲染的数据数量，如果没有设置或为0，就用数据的总长度-起始位置
        // 同时这里就处理了 visibleCount 为0的情况
        count = count || (alldata.length - start);

        // 开啥玩笑？起始位置还比数据长度大？等于也不行啊，索引是从0开始的哇
        if (start >= alldata.length) {
            return;
        }

        // 结束渲染数据项的索引
        var end = start + count;

        // 如果结束渲染数据项的索引比数据的长度大，那就直接设置成数据的长度
        if (end > alldata.length) {
            end = alldata.length;
        }

        // 数据是不是需要分组
        var groupThem = groupOption.valueField;

        // 来哇，循环数据项，并在下拉选项容器中添加DOM元素
        for (var i = start; i < end; i++) {
            var data = alldata[i];

            // 是分组项
            if (groupThem && data._group_item_) {
                var group = createElement(css_group)
                    .addClass(groupOption.css)
                    .css(groupOption.style)
                    .html(data.text).attr(str_groupAttr, data.id);
                // 调用渲染器
                if (groupOption.render) {
                    group.append(groupOption.render.call(ts, group, data));
                }
                box.append(group);
                continue;
            }

            // 创建一个下拉项的元素对象，并且使用 $().data() 把这一项的数据绑定到元素上
            var item = setData(createElement(css_item), data);

            // 下拉项的元素分为3个部分
            // 1 前缀元素 .tinyselect-item-before
            // 2 文本元素 .tinyselect-item-text
            // 3 后缀元素 .tinyselect-item-after

            var before = createElement(css_itemBefore);
            var text = createElement(css_itemText);
            var after = createElement(css_itemAfter);

            // 给下拉项设置样式，并把三部分追加上
            item.addClass(itemoption.css).css(itemoption.style)
                .append(before).append(text).append(after);

            // 文本部分的渲染，如果有指定渲染器，那么就把渲染器的返回值作为文本的显示内容，
            // 如果没有指定渲染器，那么就把指定的 option.item.textField 指定的属性值作为文本内容
            text.append(itemoption.render ?
                itemoption.render.call(item, data, i, originData) : data[itemoption.textField]);

            // 给下拉项设置一个索引（添加属性 'data-tiny-index'）
            item.attr(str_indexAttr, i);
            if (groupThem) {
                // 添加分组属性
                item.attr(str_groupAttr, data._group_id_);
            }
            // 把新的下拉项追加到下拉项容器中
            box.append(item);

            // 只保存一个下拉项的DOM对象
            if (!keep) {
                keep = item;
            }
        }

        // 如果结束索引与数据项长度相同，表示所有的数据项都渲染完成了
        if (end !== alldata.length) {
            return keep;
        }

        // 所有下拉项渲染完成后要做的事

        // 根据配置设置默认的选中项
        if (itemoption.value) {
            ts.value(itemoption.value, true);
        }

        // 调用下拉项渲染完成的回调函数
        if (callback) {
            callback.call(ts, originData);
        }

        return keep;
    }

    /**
     * 给下拉绑定事件
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     */
    function bindEvent(ts) {
        // 列表模式不会显示和隐藏下拉组件
        if (ts.option.mode !== mode_list) {
            // 绑定下拉组件的显示事件
            // 这个是绑定到context上的，旨在点击context时显示下拉组件
            bindShowBoxEvent(ts);

            // 绑定下拉组件的隐藏事件
            // 这个是绑定到window对象上的，旨在点击context和下拉组件外的所有东西都隐藏下拉组件
            bindHideBoxEvent(ts);
        }

        // 绑定下拉项的点击事件，这个用于下拉项的选中和取消选中
        bindItemClickEvent(ts);

        // 绑定下拉组件的默认事件，这里绑定的是下拉组件定义的事件的默认处理函数
        // 比如选中的结果渲染等
        bindDefaultItemEvent(ts);

        if (ts.option.keyboard) {
            // 绑定键盘事件，这里主要是绑定一下方向键移动时高亮下拉项的事件
            bindKeyboardEvent(ts);
        }
        // 给下拉组件绑定window.resize事件，以在改变浏览器大小时，下拉组件可以停留在正确的位置上
        $(win).resize(function () {
            // 为了不那么影响性能，如果下拉组件没有显示出来，就啥也不做
            if (!ts.dom.is(selector_visible)) {
                return;
            }
            fixPosition(ts.context, ts.dom, ts.option);
            fixSize(ts);
        });
    }

    /**
     * 绑定事件：用来控制何时应该显示/隐藏下拉框
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     */
    function bindShowBoxEvent(ts) {
        var context = ts.context;

        var dom = ts.dom;

        context.click(function (e) {
            // 如果是只读的，就不显示出来
            if (ts.option.readonly) {
                return;
            }

            // 没有点上下文DOM元素或上下文DOM元素的子元素
            if (!context.is(e.target) && context.find(e.target).length === 0) {
                return;
            }

            // 下拉是关闭的
            if (!dom.is(selector_visible)) {
                // 就打开
                ts.show();
            }

            return;
        });
    }

    /**
     * 修正下拉框的位置，这个在初始化以及window大小变化时调用
     *
     * @param {jQuery} context 初始化下拉的上下文DOM元素的jQuery对象
     * @param {jQuery} dom 下拉框的jQuery对象
     */
    function fixPosition(context, dom, option) {
        // 用jquery获取到context的偏移量
        var pos = context.offset();
        var mode = option.mode;
        var domheight = dom.height();
        var winheight = $(win).height();

        if (mode === mode_dropdown) {
            // 下拉组件默认会出现在context的下方
            // 但是如果下方没有足够的空间放下这货
            // 就放到上方
            // 要是上方也没有足够的空间呢？  那就与我无关了
            // 这里的 +2  -2  是防止下拉组件与context的边框重叠
            // 重叠的话可能context就会被挡住一点，特别是边框，看起来会很怪
            if (winheight - pos.top - context.height() < domheight) {
                // 放到上方
                pos.top = pos.top - domheight - 2;
            } else {
                // 放在下方
                pos.top = pos.top + context.height() + 2;
            }

            // 设置下拉组件的显示位置
            dom.css({
                left: pos.left,
                top: pos.top,
                // 如果选项中设置了组件的宽度，就用设置的宽度
                // 如果没有设置，就让下拉组件与context宽度相同
                width: option.style.width || context.width()
            });
        } else if (mode === mode_popup) {
            // 弹出模式时，水平居中，垂直方向上，top为剩下空间的1/3
            // 这里通过设置 mask的padding来实现
            dom.parent().css({
                paddingTop: (winheight - domheight) / 3,
                paddingLeft: ($(win).width() - dom.width()) / 2
            });
        }
    }

    function fixSize(ts) {
        var dom = ts.dom;

        // 修正容器大小
        // 如果container的高度超出了父容器的高度，那么就将container的高度设置为与父容器一致
        var parentHeight = dom.parent().height();

        if (ts.option.mode === mode_list || dom.height() >= parentHeight) {
            dom.height(parentHeight);
        }
        // container 的原始高度，这里不能取 jQuery的计算高度
        var nativeHeight = dom.get(0).style.height;
        if (!nativeHeight || /auto/i.test(nativeHeight)) {
            return;
        }
        // 高度不是自动时，设置 box的滚动条
        // 让数据项出现滚动条
        ts.box.height(dom.height() - 8 -
            // header 高度
            (ts.header.is(selector_visible) ? ts.header.height() : 0) -
            // footer 高度
            (ts.footer.is(selector_visible) ? ts.footer.height() : 0));
    }

    /**
     * 向页面绑定隐藏下拉框的事件，这个事件会被绑定到 window 对象上
     * 当鼠标点击不在上下文DOM元素和下拉框，以及他们的子元素时，就会隐藏下拉框
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     */
    function bindHideBoxEvent(ts) {
        var context = ts.context;

        var dom = ts.dom;
        // 给window对象绑定点击事件，以关闭下拉组件
        $(win).click(function (e) {
            var target = e.target;

            // 如果是点击了 context 或者 点击了下拉组件，啥也不做
            if (context.is(target) || dom.is(target)) {
                return;
            }

            // 如果点击了 context的子元素或者 下拉组件的子元素，啥也不做
            if (context.find(target).length || dom.find(target).length) {
                return;
            }
            // 这时候就可以考虑隐藏下拉组件了

            // 但是，如果下拉组件是不可见的，那也啥都不做
            // 难道这个不能节约一点性能？
            if (!dom.is(selector_visible)) {
                return;
            }

            // 隐藏下拉组件了
            ts.hide();
        });
    }

    /**
     * 绑定下拉项的点击事件
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     */
    function bindItemClickEvent(ts) {
        // 给下拉组件的下拉项容器添加事件的委托 .tinyselect-box
        // 委托容器监听下拉项的点击事件  .tinyselect-item 
        ts.box.on('click', Selector.build(css_item).done(), function () {
            var item = $(this);

            // 下拉项被点击了，切换这个项的选中状态
            setItemValue(ts, item, TRUE, TRUE);

            // 如果是单选，就隐藏下拉组件，如果是多选，就啥也不做，即保持下拉组件的打开状态
            if (ts.option.result.multi) {
                return;
            }

            ts.hide();
        });
    }

    /**
     * 绑定下拉项的默认事件。这里绑定了默认的select和unselect事件，
     * 以实现选中结果的填充效果
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     */
    function bindDefaultItemEvent(ts) {
        // 找出存放选中结果的容器元素对象
        // .tinyselect-context-result:first
        var result = ts.context.find(Selector.build(css_contextResult).first());

        // 选中数量的元素对象
        var count = getSelectedCount(ts);

        var option = ts.option;
        var resultOption = option.result;

        // 是否多选
        var multi = resultOption.multi;

        var render = resultOption.render;

        // 绑定一下下拉组件的项选中事件 
        ts.on(evt_select, function (e) {
            // 根据配置  option.item.textField 属性取出数据项的显示文本  
            var text = e.data[option.item.textField];

            // 如果有定义选中结果的渲染器，那么调用渲染器
            // 并将渲染器的返回值作为选中结果项的显示文本
            text = !!render ? render.call(e.target, e.data) : text;

            // 如果是单选，直接将选中项的文本设置为结果的文本并返回
            if (!multi) {
                result.html(text);
                return;
            }

            //------------- 处理多选的结果项

            // 不是列表模式才渲染结果DOM
            if (option.mode !== mode_list) {
                // 添加一个结果项到结果容器中
                var item = renderMultiSelectResultItem(ts, text, e.index);
                result.append(item);

                // 滚动到最底部
                result.stop().animate({
                    scrollTop: result[0].scrollHeight
                });
            }
            // 设置多选的选中项数量
            setData(count, (getData(count) || 0) + 1);

            // 显示多选的选中项数量
            setSelectedCount(option, count);
        });

        // 如果是单选，就不绑定取消选中的事件了
        // 因为这里的绑定是用于改变选中结果的，单选的结果不需要复杂的改变
        if (!multi) {
            return;
        }

        // 绑定取消选中事件 
        ts.on(evt_unselect, function (e) {
            // 点击项后，如果需要取消选中这一项，那么就把已经选中的结果从结果容器中移除
            // 移除的依据是元素的 data-tiny-index 属性
            // .tinyselect-result-item[data-tiny-index=n]:first
            result.find(Selector.build(css_result).attr(str_indexAttr, e.index).first())
                .remove();

            // 设置多选的选中项数量
            setData(count, getData(count) - 1);

            // 显示多选的选中项数量
            setSelectedCount(option, count);
        });
    }

    /**
     * 绑定键盘事件，这里主要是绑定一下方向键移动时高亮下拉项的事件
     *
     * @param {TinySelect} ts 下拉组件实例
     */
    function bindKeyboardEvent(ts) {

        // 这里把键盘事件绑定到 window 对象上
        $(win).keydown(function (e) {
            // 如果下拉组件是隐藏的，就不处理这个
            if (!ts.dom.is(selector_visible)) {
                return;
            }

            // 找到一个已经具有高亮属性的元素
            var old = ts.dom.find(Selector.build(css_itemHover).first());
            // 保存当前要高亮的元素的变量
            var now;

            var keycode = e.keyCode || e.which;

            // 下方向键
            if (keycode === 40) {
                // 如果当前没有高亮的，就高亮第一项
                if (old.length === 0) {
                    now = getItemsFromDom(ts).eq(0);
                } else {
                    // 当前有高亮的项，就高亮当前项的后一项
                    now = old.next();
                }
            } else if (keycode === 38) {
                // 上方向键

                // 如果当前没有高亮的，就高亮第一项
                if (old.length === 0) {
                    now = getItemsFromDom(ts).eq(0);
                } else {
                    // 当前有高亮的项，就高亮当前项的前一项
                    now = old.prev();
                }
            } else {
                if (keycode === 32) {
                    // 按下空格  相当于选中这项
                    old.click();
                } else if (keycode === 27) {
                    // 按下 esc 关闭组件
                    ts.hide();
                }

                return;
            }

            if (!now.length) {
                now = old;
            }

            ts.header.find('.' + css_filter).blur();
            now.focus();

            // 给这个项添加高亮样式，并移除其它项的高亮样式
            old.removeClass(css_itemHover);
            now.addClass(css_itemHover);

            scrollToItem(now);
        });

        // 每一项绑定  mouseover事件
        // 通过这个来添加和移除键盘方向键绑定上的样式名
        ts.dom.on('mouseover', Selector.build(css_item).done(), function () {
            $(this).addClass(css_itemHover).siblings().removeClass(css_itemHover);
        });

    }

    /**
     * 把滚动条定位到指定的下拉项位置
     *
     * @param {jQuery} item 滚动到的下拉项的jquery对象
     */
    function scrollToItem(item) {
        var box = item.parent();
        if (!/auto/i.test(box.css('overflowY'))) {
            box = box.parent();
        }

        // 设置滚动条的位置 
        box.stop().animate({
            scrollTop: item.offset().top - box.offset().top + box.scrollTop()
        }, 100);
    }

    /**
     * 渲染多选时的结果项并返回新项
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {String} text 要在一个结果项显示的内容
     * @param {Number} index 这一项的索引
     * @return {jQuery} 新创建的项的jQuery对象
     */
    function renderMultiSelectResultItem(ts, text, index) {
        // 列表模式不渲染这个
        if (ts.option.mode === mode_list) {
            return;
        }

        // 创建元素
        return createElement(css_result)
            // 设置样式
            .css(ts.option.result.style)
            // 设置项的索引属性 data-tiny-index
            .attr(str_indexAttr, index)
            // 设置显示的文本
            .append(createElement(css_resultText, tag_span).html(text))
            // 设置结果上用来取消某项选中的元素，鼠标点一下就取消选中对应的
            // 取消选中的依据是元素的 data-tiny-index 属性
            // .tinyselect-item-selected[data-tiny-index]:first
            .append(createElement(css_resultLink, tag_span).click(function () {
                // 如果是只读的，就不能操作
                if (ts.option.readonly) {
                    return;
                }

                // 取消选中这一项，并触发取消选中的事件 
                deselectItem(ts, getItemsFromDom(ts, Selector.build().attr(str_indexAttr, index).first()), TRUE);

                return FALSE;
            }));
    }

    /**
     * 发出下拉项事件
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {String} eventType 事件类型
     * @param {jQuery} item 发生事件的下拉项的jQuery对象
     */
    function emitItemEvent(ts, eventType, item) {
        var target = item.get(0);

        emitEvent(ts, eventType, {
            target: target,
            data: getData(item),
            index: item.attr(str_indexAttr)
        });
    }

    /**
     *
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {String} eventType 事件类型
     * @param {Object} arg 附加的事件参数
     */
    function emitEvent(ts, eventType, arg) {
        // 如果没有绑定过这个事件的处理函数，则返回
        if (!own(ts.events, eventType)) {
            return;
        }

        // 设置事件类型属性
        arg.type = eventType;

        // 根据绑定顺序循环调用事件处理函数
        ts.events[eventType].forEach(function (fn) {
            fn.call(ts, arg);
        });
    }

    /**
     * 获取选中的项的值，值是根据  option.item.valueField 来取的
     * 单选返回一个值，多选返回数组
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @return {any} 选中的值
     */
    function getValue(ts) {
        // 表示数据项值的字段名称
        var valueField = ts.option.item.valueField;

        // 单选
        if (!ts.option.result.multi) {
            // 单选的时候找到第一个选中的项就行了，所以加个 first 限定符
            // 这里也是假装考虑一下查询的性能
            var item = getItemsFromDom(ts, Selector.build(css_selected).first());

            // 如果没有选中项就返回undefined，有选中项就返回选中项的 valueField 属性的值
            return item.length === 0 ? undefined : getData(item)[valueField];
        }

        // 多选  返回所有选中项的值组成的数组
        return getItemsFromDom(ts, Selector.build(css_selected).done()).map(function (index, item) {
            return getData($(item))[valueField];
        });
    }

    /**
     * 设置选中的值
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {any} value 要设置的选中的值，根据单选和多选的不同传入数组和非数组
     * @param {Boolean} trigger 是否触发事件
     */
    function setValue(ts, value, trigger) {
        var item;

        // 为了后面将值与项的数据进行比较的方便(使用indexOf判断值)，
        // 这里把传进来的值搞成数组
        var selectedValues = $.makeArray(value);

        // 遍历下拉项，在传入的值数组中查找下拉项的 data 数据的 valueField 属性的值（即每项的数据的值）
        // 如果传入的值中存在这一项的值（即命中），就说明要选中这一项
        // 在单选时，只要命中一次，这个函数就返回了；而多选 会遍历所有项
        for (var i = 0, items = getItemsFromDom(ts); i < items.length; i++) {
            item = $(items[i]);

            // 是否命中
            var hit = selectedValues.indexOf(
                getData(item)[ts.option.item.valueField]) !== -1;

            // 没有命中就比较下一项了
            if (!hit) {
                continue;
            }

            // 设置这一项为选中的状态
            setItemValue(ts, item, FALSE, trigger);

            // 如果是单选，就返回，不再检查后续的项了
            if (!ts.option.result.multi) {
                return;
            }
        }
    }

    /**
     * 清除选中的所有项
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     */
    function clearSelection(ts) {
        // 先取到所有的下拉项DOM对象
        var items = getItemsFromDom(ts);

        // 清空选中结果
        ts.context.find(Selector.build(css_contextResult).first()).empty();

        // 对多选选中项的清除
        if (ts.option.result.multi) {
            // 根据样式类取消选中项
            // .tinyselect-item-selected
            items.filter(Selector.build(css_selected).done()).each(function (index, item) {
                // 取消选中项并触发  unselect 事件
                deselectItem(ts, $(item), TRUE);
            });

            return;
        }

        // 对单选的处理

        // 单选的话，只有一项选中，所在使用 first 来过滤
        var item = items.filter(Selector.build(css_selected).first());

        // 没有选中项，返回吧
        if (!item.length) {
            return;
        }

        // 取消选中项并触发  unselect 事件
        deselectItem(ts, item, TRUE);

        return;
    }

    /**
     * 设置某一项的选中状态
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {jQuery} item 要设置状态的项
     * @param {Boolean} toggle 是否要切换选中的状态，默认为false
     * @param {Boolean} trigger 是否触发事件
     */
    function setItemValue(ts, item, toggle, trigger) {
        var multi = ts.option.result.multi;

        // 判断这一项是不是选中的状态
        var selected = item.hasClass(css_selected);

        /**
         * 切换状态，即：
         * 如果是选中，那么就切换成未选中；反之亦然。
         *
         * 那么，就能得到：
         * 1 单选时： 如果这个项已经选中了，无论是否需要切换状态，
         *  都直接返回，因为不能再次选中一个已经选中的项;
         *  而在未选中时，无论是否需要切换状态，都要把已经选中的项取消选中，
         *  再选中当前项。
         *
         * 2 多选时：如果这个项已经选中，并且不需要切换状态，那么就啥也不做，直接返回，
         *  如果需要切换状态，那么就取消选中这一项；
         *
         *
         */

        if (!multi) {
            // 此项是选中的，返回
            if (selected) {
                return;
            }

            // 此项未选中，那么
            // 获取上次选中的项，然后取消选中
            var lastSelected = item.siblings(Selector.build(css_selected).first());

            // 如果有选中的，那么先取消选中
            if (lastSelected.length) {
                // 取消选中上次选中的项
                deselectItem(ts, lastSelected, trigger);
            }

            // 选中当前的项
            selectItem(ts, item, trigger);

            return;
        }

        if (!selected) {
            // 此项未选中        那就选中这一项
            selectItem(ts, item, trigger);
            return;
        }

        // 如果此项已经是选中的，并且不需要切换状态，那么直接返回
        if (!toggle) {
            return;
        }

        // 需要切换状态  就是取消选中
        deselectItem(ts, item, trigger);
        return;
    }

    /**
     * 选中指定的项
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {jQuery} item 要选择的项
     * @param {Boolean} trigger 是否触发事件
     */
    function selectItem(ts, item, trigger) {
        // 给下拉项添加选中的样式 tinyselect-item-selected
        item.addClass(css_selected);

        // 是否需要触发事件
        if (trigger) {

            // 触发选中事件
            emitItemEvent(ts, evt_select, item);
        }
    }

    /**
     * 取消选中指定的项
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {jQuery} item 要取消选择的项
     * @param {Boolean} trigger 是否触发事件
     */
    function deselectItem(ts, item, trigger) {
        // 移除下拉项的选中样式
        item.removeClass(css_selected);

        // 是否需要触发事件
        if (trigger) {

            // 触发取消选中事件
            emitItemEvent(ts, evt_unselect, item);
        }
    }

    /**
     * 获取显示下拉项总数量的元素
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @return {jQuery} 根据选择器选择到的jQuery对象集合
     */
    function getTotalCount(ts) {
        // 构建选择器： .tinyselect-footer-right .tinyselect-count-total
        return ts.footer.find(Selector.build(css_footerRight).sub(css_totalCount).done());
    }

    /**
     * 获取显示选中项数量的元素
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @return {jQuery} 根据选择器选择到的jQuery对象集合
     */
    function getSelectedCount(ts) {
        // 构建选择器： .tinyselect-footer-right .tinyselect-count-selected
        return ts.footer.find(Selector.build(css_footerRight).sub(css_selectedCount).done());
    }

    /**
     * 通过jQuery从下拉框直接查找下拉的所有项
     *
     * @param {TinySelect} ts 当前的TinySelect实例
     * @param {String} addon 附加的样式
     * @return {jQuery} 根据选择器选择到的jQuery对象集合
     */
    function getItemsFromDom(ts, addon) {
        // 构建选择器: .tinyselect-box .tinyselect-item<addon>
        return ts.box.find(Selector.build(css_item).addon(addon).done());
    }

    /**
     * 设置下拉项的数量
     *
     * @param {Object} option 实例配置项
     * @param {jQuery} element 要显示下拉项数量的元素的jquery对象
     */
    function setTotalCount(option, element) {
        setCount(element, option.footer.totalTpl);
    }

    /**
     * 设置选中项的数量
     *
     * @param {Object} option 实例配置项
     * @param {jQuery} element 要显示选中数量的元素的jquery对象
     */
    function setSelectedCount(option, element) {
        setCount(element, option.footer.selectedTpl);
    }

    /**
     * 设置数量。设置下拉项总数和选中数都会调用这个函数
     *
     * @param {jQuery} element 要设置数量的元素的jquery对象
     * @param {String} tpl 使用的模板字符串
     */
    function setCount(element, tpl) {
        element.html(tpl.replace(str_placeholder, (getData(element) || 0)));
    }

    /**
     * 使用jQuery.fn.data获取数据
     *
     * @param {jQuery} element 要获取数据的元素的jquery对象
     * @return {Object} 保存的数据
     */
    function getData(element) {
        return element.data(str_data);
    }

    /**
     * 使用jQuery.fn.data保存数据
     *
     * @param {jQuery} element 要设置数据的元素的jquery对象
     * @param {Object} value 要保存的数据
     */
    function setData(element, value) {
        return element.data(str_data, value);
    }

    /**
     * 设置分组头是否可见
     * @param {jQuery} groups 所有分组头
     * @param {jQuery} items 所有数据项
     * @param {Strnig} groupid 分组id
     */
    function setGroupVisible(groups, items, groupid) {

        var group = groups.filter(Selector.build()
            .attr(str_groupAttr, groupid).done());

        if (items.filter(Selector.build()
            .attr(str_groupAttr, groupid).visible()).length === 0) {
            // groupid 分组下没有可见的项了，隐藏这个分组头
            group.hide();
        } else {
            // 有可见项，显示这个分组头
            group.show();
        }
    }

    /**
     * 设置TinySelect到window对象上面
     */
    win.tinyselect = TinySelect;
})(window, jQuery);