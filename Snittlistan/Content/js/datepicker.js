/// <reference path="jquery-1.6.4-vsdoc.js" />
/// <reference path="jquery-ui.js" />

$(function () {
    $("input[data-datepicker]").each(function () {
        $(this).datepicker($.datepicker.regional["sv"]);
    });
});