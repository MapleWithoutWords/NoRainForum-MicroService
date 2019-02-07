function FrontLoadAjax($, layer, url, data,fun) {
    data = data == null ? {} : data;
    var index = layer.load(1, {
        shade: [0.1, '#fff'] //0.1透明度的白色背景
    });
    $.ajax({
        url: url,
        data: data,
        dataType: "json",
        type: "post",
        success: function (res) {
            if (res.status == "ok") {
                fun(res.data);
            }
            else if (res.status == "error") {
                layer.msg(res.errorMsg, { icon: 2, time: 1000 });
            }
            else if (res.status == "redirect") {
                location.href = res.data;
            } else {
                layer.msg("未知错误", { icon: 2, time: 1000 });
            }
            layer.close(index);
        }, error: function () {
            layer.msg("网络错误", { icon: 2, time: 1000 });
            layer.close(index);
        }
    });
}