var check_users = [];
var setting = {
    check: {
        enable: true,
        chkboxType: { "Y": "s", "N": "ps" },
        chkStyle: "checkbox"
    },
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        onClick: zTreeOnClick,
        onCheck: GroupZTreeCheck
    }
};

var user_string = {
    check: {
        enable: true,
        chkboxType: { "Y": "s", "N": "p" },
        chkStyle: "checkbox"
    },
    data: {
        simpleData: {
            enable: true
        }
    },
    callback: {
        onCheck: CheckUser
    }
};

function InitTree() {
    if (action == 'Edit') {
        $.ajax({
            url: webApiDomain + "ztree/edit-groups",
            type: "GET",
            data: {"id":id},
            datatype: "json",
            success: function (result) {
                $.fn.zTree.init($("#grouptree"), setting, result.data);
            },
            error: function (msg) {
                alert(" 数据加载失败！" + msg);
            }
        });
        return;
    }

    $.ajax({
        url: webApiDomain + "ztree/groups",
        type: "GET",
        datatype: "json",
        success: function (result) {
            $.fn.zTree.init($("#grouptree"), setting, result.data);
        },
        error: function (msg) {
            alert(" 数据加载失败！" + msg);
        }
    });
} 

function zTreeOnClick(e, treeId, treeNode) {
    $("#group_name").val(treeNode.name);
    $.ajax({
        url: webApiDomain + "party_style/sty/member",
        data: {
            "id": treeNode.id,
            "check_user": check_users
        },
        type: "POST",
        datatype: "json",
        success: function (result) {

            $.fn.zTree.init($("#usertree"), user_string, result.data);
        },
        error: function (msg) {
            alert(" 数据加载失败！" + msg);
        }
    });
};

function GroupZTreeCheck(e, treeId, treeNode) {
    $("#group_name").val(treeNode.name);
    if (treeNode.checked) {
        $.ajax({
            url: webApiDomain + "party_style/sty/gourp/all-users",
            data: {
                "groupid": treeNode.id
            },
            type: "GET",
            datatype: "json",
            success: function (result) {
                //$.fn.zTree.init($("#usertree"), user_string, result.data);
                //var treeObj = $.fn.zTree.getZTreeObj("usertree");
                //treeObj.checkAllNodes(true);
                //var users = treeObj.getCheckedNodes(true);
                for (var i = 0; i < result.data.length; i++) {
                    check_users.push(result.data[i].id);
                }
            },
            error: function (msg) {
                alert(" 数据加载失败！" + msg);
            }
        });
        $.ajax({
            url: webApiDomain + "party_style/sty/member",
            data: {
                "id": treeNode.id,
                "check_user": check_users
            },
            type: "POST",
            datatype: "json",
            success: function (result) {
                $.fn.zTree.init($("#usertree"), user_string, result.data);
                var treeObj = $.fn.zTree.getZTreeObj("usertree");
                treeObj.checkAllNodes(true);
                var users = treeObj.getCheckedNodes(true);
                //for (var i = 0; i < users.length; i++) {
                //    check_users.push(users[i].id);
                //}
            },
            error: function (msg) {
                alert(" 数据加载失败！" + msg);
            }
        });
    } else {
        $.ajax({
            url: webApiDomain + "party_style/sty/gourp/all-users",
            data: {
                "groupid": treeNode.id
            },
            type: "GET",
            datatype: "json",
            success: function (result) {
                //$.fn.zTree.init($("#usertree"), user_string, result.data);
                //var treeObj = $.fn.zTree.getZTreeObj("usertree");
                //treeObj.checkAllNodes(false);
                //var users = treeObj.getCheckedNodes(false);
                //alert(result.data.length);
                for (var i = 0; i < result.data.length; i++) {
                    for (var j = 0; j < check_users.length; j++) {
                        if (check_users[j] == result.data[i].id) {
                            check_users.splice(j, 1);
                            j--;
                        }
                    }
                }
            },
            error: function (msg) {
                alert(" 数据加载失败！" + msg);
            }
        });
        $.ajax({
            url: webApiDomain + "party_style/sty/member",
            data: {
                "id": treeNode.id,
                "check_user": check_users
            },
            type: "POST",
            datatype: "json",
            success: function (result) {
                $.fn.zTree.init($("#usertree"), user_string, result.data);
                var treeObj = $.fn.zTree.getZTreeObj("usertree");
                treeObj.checkAllNodes(false);
                //var users = treeObj.getCheckedNodes(false);
                //for (var i = 0; i < users.length; i++) {
                //    for (var j = 0; j < check_users.length; j++) {
                //        if (check_users[j] == users[i].id) {
                //            check_users.splice(j, 1);
                //            j--;
                //        }
                //    }
                //}
            },
            error: function (msg) {
                alert(" 数据加载失败！" + msg);
            }
        });
        //var treeObj = $.fn.zTree.getZTreeObj("usertree");
        //treeObj.checkAllNodes(false);
        //var allNodes = treeObj.getNodes();
        //for (var i = 0; i < allNodes.length; i++) {
        //    for (var j = 0; j < check_users.length; j++) {
        //        if (check_users[j] == allNodes[i].id) {
        //            check_users.splice(j, 1);
        //            j--;
        //        }
        //    }
        //}

    }

}

function CheckUser(e, treeId, treeNode) {
    if (!treeNode.checked){
        var userTreeObj = $.fn.zTree.getZTreeObj(treeId);
        var allNodes = userTreeObj.getNodes();
        var checkNodes = userTreeObj.getCheckedNodes();
        if (allNodes.length - 1 == checkNodes.length) {
            $.ajax({
                type: "GET",
                url: webApiDomain + "ztree/user/groups",
                data: { "userid": treeNode.id },
                dataType: "JSON",
                success: function (result) {
                    if (result.data.length > 0) {
                        var treeObj = $.fn.zTree.getZTreeObj("grouptree");
                        for (var i = 0; i < result.data.length; i++) {
                            var data = treeObj.getNodeByParam("id", result.data[i], null);
                            treeObj.checkNode(data, false, false, false);
                        }
                    }
                },
                error: function () {

                }
            });
        }
    }

    if (treeNode.checked) {
        check_users.push(treeNode.id);
        return;
    }

    for (var i = 0; i < check_users.length; i++) {
        if (check_users[i] == treeNode.id) {
            check_users.splice(i, 1);
            i--;
        }
    }
}



