$(function () {
    var _hmt = _hmt || [];
    (function () {
        var hm = document.createElement("script");
        hm.src = "https://hm.baidu.com/hm.js?3b889148d3a97c2e11263168434b1788";
        var s = document.getElementsByTagName("script")[0];
        s.parentNode.insertBefore(hm, s);
    })();
    function autoIndent(html) {
        var indent;
        return $.map(html.split('\n'), function (line) {
            if (/^\s*$/.test(line)) {
                return line;
            }
            if (!indent) {
                indent = /^ */.exec(line)[0].length;
            }

            return line.substring(indent);
        }).join('\n');
    }

    var codeheader = $('<div class="sourceheader">').text('代码');
    var codeblock = $('<div>');

    $(document.body)
        .append(codeheader)
        .append(codeblock);

    CodeMirror(codeblock.get(0), {
        value: autoIndent($('script:not([src]):first').html()),
        lineNumbers: true,
        readOnly: true,
        matchBrackets: true,
        mode: 'text/javascript'
    }).setSize('auto', 'auto');

    var less = $('var[type=less]:first');
    if (!less.length) {
        return;
    }

    $.get(less.attr('src')).then(function (lesstext) {
        var styleheader = $('<div class="sourceheader">').text('Less 样式');
        var styleblock = $('<div>');

        $(document.body)
            .append(styleheader)
            .append(styleblock);

        CodeMirror(styleblock.get(0), {
            value: lesstext,
            lineNumbers: true,
            readOnly: true,
            matchBrackets: true,
            mode: 'text/x-less'
        }).setSize('auto', 'auto');
    });
});