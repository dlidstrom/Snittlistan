$(function () {
    $("input[data-datepicker]").each(function () {
        $(this).datepicker($.datepicker.regional["sv"]);
    });
});