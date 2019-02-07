
layui.config({
    base: "js/"
}).use(['form', 'layer', 'jquery', 'laypage'], function () {
    var form = layui.form(),
        layer = parent.layer === undefined ? layui.layer : parent.layer,
        laypage = layui.laypage,
        $ = layui.jquery;

    //加载页面数据
    var pageData = '';

    //添加项
    //改变窗口大小时，重置弹窗的高度，防止超出可视区域（如F12调出debug的操作）
    $(window).one("resize", function () {
        $("." + appConfig.name + "Add_btn").click(function () {
            var index = layui.layer.open({
                title: "添加" + appConfig.title,
                type: 2,
                content: appConfig.addUrl,
                success: function (layero, index) {
                    setTimeout(function () {
                        layui.layer.tips('点击此处返回' + appConfig.title + '列表', '.layui-layer-setwin .layui-layer-close', {
                            tips: 3
                        });
                    }, 500);
                }
            });
            layui.layer.full(index);
        });
    }).resize();


    //批量删除
    $(".batchDel").click(function () {
        var $checkbox = $("." + appConfig.name + "_list tbody input[type='checkbox'][name='checked']");
        var $checked = $("." + appConfig.name + "_list tbody input[type='checkbox'][name='checked']:checked");
        if ($checkbox.is(":checked")) {
            layer.confirm('确定删除选中的' + appConfig.title + '？', { icon: 3, title: '提示信息' }, function (ind) {
                var index = layer.msg('删除中，请稍候', { icon: 16, time: false, shade: 0.8 });


                var data = [];

                $checked.each(function () {
                    //$(this).parents("tr").find("."+appConfig.name+"_del").attr("data-id");
                    //console.log($(this).parents("tr").find("." + appConfig.name + "_del").attr("data-id"));

                    data.push($(this).parents("tr").find("." + appConfig.name + "_del").attr("data-id"));
                });


                //删除的方法
                appConfig.batchDel($(data));

                form.render();
                layer.close(index);
                layer.msg("删除成功");
            });
        } else {
            layer.msg("请选择需要删除的" + appConfig.title);
        }
    })

    //全选
    form.on('checkbox(allChoose)', function (data) {
        var child = $(data.elem).parents('table').find('tbody input[type="checkbox"]:not([name="show"])');
        child.each(function (index, item) {
            item.checked = data.elem.checked;
        });
        form.render('checkbox');
    });

    //通过判断项是否全部选中来确定全选按钮是否选中
    form.on("checkbox(choose)", function (data) {
        var child = $(data.elem).parents('table').find('tbody input[type="checkbox"]:not([name="show"])');
        var childChecked = $(data.elem).parents('table').find('tbody input[type="checkbox"]:not([name="show"]):checked');
        if (childChecked.length == child.length) {
            $(data.elem).parents('table').find('thead input#allChoose').get(0).checked = true;
        } else {
            $(data.elem).parents('table').find('thead input#allChoose').get(0).checked = false;
        }
        form.render('checkbox');
    });

    //是否展示
    form.on('switch(isShow)', function (data) {
        var index = layer.msg('修改中，请稍候', { icon: 16, time: false, shade: 0.8 });
        setTimeout(function () {
            layer.close(index);
            layer.msg("展示状态修改成功！");
        }, 2000);
    });

    //操作
    $("body").on("click", "." + appConfig.name + "_edit", function () {  //编辑
        var id = $(this).attr("data-id");
        var index = layui.layer.open({
            title: "修改" + appConfig.title,
            type: 2,
            content: appConfig.editUrl + "?id=" + id,
            success: function (layero, index) {
                setTimeout(function () {
                    layui.layer.tips('点击此处返回' + appConfig.title + '列表', '.layui-layer-setwin .layui-layer-close', {
                        tips: 3
                    });
                }, 500);
            }
        });
        layui.layer.full(index);
    });

    $("body").on("click", "." + appConfig.name + "_del", function () {  //删除
        var _this = $(this);
        layer.confirm('确定删除此' + appConfig.title + '？', { icon: 3, title: '提示信息' }, function (index) {
            _this.parents("tr").remove();
            layer.close(index);
            //删除的方法
            //appConfig.del(_this.attr("data-id"),$,layer);
            var deleteIndex = layer.load(1, {
                shade: [0.1, '#fff'] //0.1透明度的白色背景
            });
            $.ajax({
                url: appConfig.delUrl,
                dataType: "json",
                data: { "id": _this.attr("data-id") },
                type: "post",
                success: function (res) {
                    if (res.status == "ok") {
                        location.reload();
                    }
                    else if (res.status == "error") {
                        layer.msg(res.errorMsg, { icon: 2, time: 1000 });
                    }
                    else if (res.status == "redirect") {
                        location.href = res.data;
                    } else {
                        layer.msg("未知错误", { icon: 2, time: 1000 });
                    }
                    layer.close(deleteIndex);
                },
                error: function () {
                    layer.msg("网络错误", { icon: 2, time: 1000 });
                    layer.close(deleteIndex);
                }
            });
        });
    });

    function pageList(that) {

        //分页
        var nums = 13; //每页出现的数据量
        if (that) {
            pageData = that;
        }
        laypage({
            cont: "page",
            pages: Math.ceil(pageData.length / nums),
            jump: function (obj) {
                $("." + appConfig.name + "_content").html(renderDate(pageData, obj.curr));
                $("." + appConfig.name + "_list thead input[type='checkbox']").prop("checked", false);
                form.render();
            }
        })
    }
});


