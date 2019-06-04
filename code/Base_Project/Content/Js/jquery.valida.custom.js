(function (factory) {
    if (typeof define === "function" && define.amd) {
        define(["jquery", "../jquery.validate"], factory);
    } else {
        factory(jQuery);
    }
}(function ($) {
    $.extend($.validator.messages, {
        required: "必填",
        email: "請輸入有效的電子郵件",
        url: "請輸入有效的網址",
        date: "請輸入有效的日期",
        dateISO: "請輸入有效的日期 (YYYY-MM-DD)",
        number: "請輸入有效的數字",
        digits: "只能輸入數字",
        creditcard: "請輸入有效的信用卡號碼",
        equalTo: "你的輸入不相同",
        extension: "請輸入有效的副檔名",
        maxlength: $.validator.format("最多可以輸入 {0} 個字"),
        minlength: $.validator.format("最少要輸入 {0} 個字"),
        rangelength: $.validator.format("請輸入長度在 {0} 到 {1} 之間的字"),
        range: $.validator.format("請輸入範圍在 {0} 到 {1} 之間的數值"),
        max: $.validator.format("請輸入不大於 {0} 的數值"),
        min: $.validator.format("請輸入不小於 {0} 的數值")
    });
}));
$(function () {
    $("#Form").validate({
        highlight: function (element, errorClass, validClass) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass("is-invalid");
        },
        errorPlacement: function (error, element) {
            $(error).addClass("text-danger");
            error.insertAfter(element);
        }
    });
    $(".required").rules("add", {
        required: true
    }),
    $(".email").rules("add", {
        email: true
    }),
    $(".password").rules("add", {
        checkPwd: true
    }),
    $(".number").rules("add", {
        digits: true
    }),
    $.validator.addMethod("checkPwd", function (value, element, params) {
        var checkPwd = /^\w{6,16}$/g;
        return this.optional(element) || (checkPwd.test(value));
    }, "只允許6-16位英文字母、數字或者下底線");
});