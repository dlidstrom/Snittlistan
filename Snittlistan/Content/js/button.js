/// <reference path="jquery-1.6.4-vsdoc.js" />
/// <reference path="jquery-ui.js" />

$(function () {
    $("a[data-button]").each(function () {
        $(this).button();
    });
});